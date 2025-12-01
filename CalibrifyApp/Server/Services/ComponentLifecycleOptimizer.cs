using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Concurrent;

namespace CalibrifyApp.Server.Services
{
    /// <summary>
    /// Service to optimize component lifecycle and prevent double-loading issues
    /// </summary>
    public class ComponentLifecycleOptimizer
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ILogger<ComponentLifecycleOptimizer> _logger;
        private readonly ConcurrentDictionary<string, ComponentState> _componentStates = new();
        private readonly ConcurrentDictionary<string, DateTime> _lastInitialization = new();
        private const int MinInitializationIntervalMs = 1000; // Prevent rapid re-initialization

        public ComponentLifecycleOptimizer(IJSRuntime jsRuntime, ILogger<ComponentLifecycleOptimizer> logger)
        {
            _jsRuntime = jsRuntime;
            _logger = logger;
        }

        /// <summary>
        /// Check if a component should be initialized or if it's too soon after last initialization
        /// </summary>
        public bool ShouldInitialize(string componentId)
        {
            var now = DateTime.UtcNow;
            var lastInit = _lastInitialization.GetValueOrDefault(componentId, DateTime.MinValue);
            var timeSinceLastInit = (now - lastInit).TotalMilliseconds;

            if (timeSinceLastInit < MinInitializationIntervalMs)
            {
                _logger.LogDebug("[LIFECYCLE] Skipping initialization of {ComponentId} - too soon after last init ({TimeSinceLastInit}ms)", 
                    componentId, timeSinceLastInit);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Mark a component as initialized
        /// </summary>
        public void MarkInitialized(string componentId, ComponentBase component)
        {
            var now = DateTime.UtcNow;
            _lastInitialization[componentId] = now;
            
            _componentStates[componentId] = new ComponentState
            {
                Component = new WeakReference(component),
                InitializedAt = now,
                State = ComponentLifecycleState.Initialized
            };

            _logger.LogDebug("[LIFECYCLE] Component {ComponentId} marked as initialized", componentId);
        }

        /// <summary>
        /// Mark a component as loading to prevent double initialization
        /// </summary>
        public bool TryMarkLoading(string componentId)
        {
            var state = _componentStates.GetOrAdd(componentId, _ => new ComponentState
            {
                State = ComponentLifecycleState.Loading,
                InitializedAt = DateTime.UtcNow
            });

            if (state.State == ComponentLifecycleState.Loading)
            {
                _logger.LogDebug("[LIFECYCLE] Component {ComponentId} already loading", componentId);
                return false; // Already loading
            }

            state.State = ComponentLifecycleState.Loading;
            state.InitializedAt = DateTime.UtcNow;
            
            _logger.LogDebug("[LIFECYCLE] Component {ComponentId} marked as loading", componentId);
            return true; // Can proceed with loading
        }

        /// <summary>
        /// Mark a component as failed and determine if retry is appropriate
        /// </summary>
        public bool ShouldRetry(string componentId, Exception exception)
        {
            var state = _componentStates.GetOrAdd(componentId, _ => new ComponentState());
            state.FailureCount++;
            state.LastException = exception;
            state.State = ComponentLifecycleState.Failed;

            _logger.LogWarning("[LIFECYCLE] Component {ComponentId} failed (attempt {FailureCount}): {Error}", 
                componentId, state.FailureCount, exception.Message);

            // Allow retry if failure count is reasonable and not too frequent
            const int maxRetries = 3;
            const int retryDelayMs = 2000;

            if (state.FailureCount >= maxRetries)
            {
                _logger.LogError("[LIFECYCLE] Component {ComponentId} exceeded max retries ({MaxRetries})", 
                    componentId, maxRetries);
                return false;
            }

            var timeSinceLastFailure = (DateTime.UtcNow - state.InitializedAt).TotalMilliseconds;
            if (timeSinceLastFailure < retryDelayMs)
            {
                _logger.LogDebug("[LIFECYCLE] Component {ComponentId} retry too soon ({TimeSinceLastFailure}ms < {RetryDelayMs}ms)", 
                    componentId, timeSinceLastFailure, retryDelayMs);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Optimize component rendering by checking if data has changed
        /// </summary>
        public bool ShouldRender(string componentId, object? currentData, object? previousData)
        {
            // Simple equality check - can be enhanced with deep comparison if needed
            if (ReferenceEquals(currentData, previousData))
            {
                _logger.LogDebug("[LIFECYCLE] Component {ComponentId} data unchanged - skipping render", componentId);
                return false;
            }

            if (currentData?.Equals(previousData) == true)
            {
                _logger.LogDebug("[LIFECYCLE] Component {ComponentId} data equal - skipping render", componentId);
                return false;
            }

            _logger.LogDebug("[LIFECYCLE] Component {ComponentId} data changed - allowing render", componentId);
            return true;
        }

        /// <summary>
        /// Prevent double data loading by checking if data is already being loaded
        /// </summary>
        public async Task<T?> LoadDataOnceAsync<T>(string dataKey, Func<Task<T>> dataLoader) where T : class
        {
            var loadingKey = $"data_{dataKey}";
            
            if (!TryMarkLoading(loadingKey))
            {
                // Already loading, wait for completion
                return await WaitForDataLoad<T>(loadingKey);
            }

            try
            {
                _logger.LogDebug("[LIFECYCLE] Loading data for key {DataKey}", dataKey);
                var data = await dataLoader();
                
                // Store the loaded data
                _componentStates[loadingKey] = new ComponentState
                {
                    State = ComponentLifecycleState.Initialized,
                    Data = data,
                    InitializedAt = DateTime.UtcNow
                };

                _logger.LogDebug("[LIFECYCLE] Data loaded successfully for key {DataKey}", dataKey);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[LIFECYCLE] Failed to load data for key {DataKey}", dataKey);
                ShouldRetry(loadingKey, ex);
                throw;
            }
        }

        /// <summary>
        /// Wait for data to be loaded by another component
        /// </summary>
        private async Task<T?> WaitForDataLoad<T>(string loadingKey) where T : class
        {
            const int maxWaitMs = 10000; // 10 seconds max wait
            const int checkIntervalMs = 100;
            var waited = 0;

            while (waited < maxWaitMs)
            {
                if (_componentStates.TryGetValue(loadingKey, out var state))
                {
                    if (state.State == ComponentLifecycleState.Initialized && state.Data is T data)
                    {
                        _logger.LogDebug("[LIFECYCLE] Data found after waiting {WaitedMs}ms for key {LoadingKey}", 
                            waited, loadingKey);
                        return data;
                    }
                    
                    if (state.State == ComponentLifecycleState.Failed)
                    {
                        _logger.LogWarning("[LIFECYCLE] Data loading failed for key {LoadingKey}", loadingKey);
                        return null;
                    }
                }

                await Task.Delay(checkIntervalMs);
                waited += checkIntervalMs;
            }

            _logger.LogWarning("[LIFECYCLE] Timeout waiting for data load for key {LoadingKey}", loadingKey);
            return null;
        }

        /// <summary>
        /// Get component statistics for debugging
        /// </summary>
        public Dictionary<string, object> GetComponentStats()
        {
            var stats = new Dictionary<string, object>();
            
            var totalComponents = _componentStates.Count;
            var initializedComponents = _componentStates.Values.Count(s => s.State == ComponentLifecycleState.Initialized);
            var loadingComponents = _componentStates.Values.Count(s => s.State == ComponentLifecycleState.Loading);
            var failedComponents = _componentStates.Values.Count(s => s.State == ComponentLifecycleState.Failed);

            stats["TotalComponents"] = totalComponents;
            stats["InitializedComponents"] = initializedComponents;
            stats["LoadingComponents"] = loadingComponents;
            stats["FailedComponents"] = failedComponents;
            stats["ComponentDetails"] = _componentStates.ToDictionary(
                kvp => kvp.Key,
                kvp => new
                {
                    State = kvp.Value.State.ToString(),
                    InitializedAt = kvp.Value.InitializedAt,
                    FailureCount = kvp.Value.FailureCount,
                    HasData = kvp.Value.Data != null
                }
            );

            return stats;
        }

        /// <summary>
        /// Clean up old component states to prevent memory leaks
        /// </summary>
        public void CleanupOldStates(TimeSpan maxAge)
        {
            var cutoff = DateTime.UtcNow - maxAge;
            var keysToRemove = _componentStates
                .Where(kvp => kvp.Value.InitializedAt < cutoff)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in keysToRemove)
            {
                _componentStates.TryRemove(key, out _);
                _lastInitialization.TryRemove(key, out _);
            }

            if (keysToRemove.Count > 0)
            {
                _logger.LogDebug("[LIFECYCLE] Cleaned up {Count} old component states", keysToRemove.Count);
            }
        }
    }

    /// <summary>
    /// Component state tracking
    /// </summary>
    public class ComponentState
    {
        public WeakReference? Component { get; set; }
        public ComponentLifecycleState State { get; set; } = ComponentLifecycleState.NotInitialized;
        public DateTime InitializedAt { get; set; } = DateTime.UtcNow;
        public int FailureCount { get; set; } = 0;
        public Exception? LastException { get; set; }
        public object? Data { get; set; }
    }

    /// <summary>
    /// Component lifecycle states
    /// </summary>
    public enum ComponentLifecycleState
    {
        NotInitialized,
        Loading,
        Initialized,
        Failed
    }
}
