using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CalibrifyApp.Server.Services
{
    /// <summary>
    /// Service to handle component initialization with retry logic and error handling
    /// </summary>
    public class ComponentInitializationService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ILogger<ComponentInitializationService> _logger;
        private readonly Dictionary<string, int> _retryCounters = new();
        private const int MaxRetries = 3;
        private const int RetryDelayMs = 1000;

        public ComponentInitializationService(IJSRuntime jsRuntime, ILogger<ComponentInitializationService> logger)
        {
            _jsRuntime = jsRuntime;
            _logger = logger;
        }

        /// <summary>
        /// Initialize a component with retry logic
        /// </summary>
        public async Task<bool> InitializeComponentAsync(string componentName, Func<Task> initializationAction)
        {
            var retryKey = $"{componentName}_{DateTime.Now:yyyyMMdd}";
            var retryCount = _retryCounters.GetValueOrDefault(retryKey, 0);

            try
            {
                _logger.LogInformation("[COMPONENT INIT] Initializing {ComponentName} (attempt {RetryCount})", 
                    componentName, retryCount + 1);

                await initializationAction();

                _logger.LogInformation("[COMPONENT INIT] Successfully initialized {ComponentName}", componentName);
                
                // Reset retry counter on success
                _retryCounters.Remove(retryKey);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[COMPONENT INIT] Failed to initialize {ComponentName}: {Error}", 
                    componentName, ex.Message);

                retryCount++;
                _retryCounters[retryKey] = retryCount;

                if (retryCount < MaxRetries)
                {
                    _logger.LogInformation("[COMPONENT INIT] Retrying {ComponentName} in {Delay}ms (attempt {RetryCount}/{MaxRetries})", 
                        componentName, RetryDelayMs, retryCount + 1, MaxRetries);

                    await Task.Delay(RetryDelayMs);
                    return await InitializeComponentAsync(componentName, initializationAction);
                }
                else
                {
                    _logger.LogError("[COMPONENT INIT] Max retries reached for {ComponentName}. Initialization failed.", 
                        componentName);
                    return false;
                }
            }
        }

        /// <summary>
        /// Initialize JavaScript interop with error handling
        /// </summary>
        public async Task<bool> InitializeJavaScriptAsync(string scriptName, string functionName, params object[] args)
        {
            return await InitializeComponentAsync($"JS_{scriptName}_{functionName}", async () =>
            {
                await _jsRuntime.InvokeVoidAsync(functionName, args);
            });
        }

        /// <summary>
        /// Initialize a service with dependency injection
        /// </summary>
        public async Task<T?> InitializeServiceAsync<T>(string serviceName, Func<Task<T>> serviceFactory) where T : class
        {
            var success = await InitializeComponentAsync($"Service_{serviceName}", async () =>
            {
                var service = await serviceFactory();
                if (service == null)
                {
                    throw new InvalidOperationException($"Service {serviceName} returned null");
                }
            });

            if (success)
            {
                return await serviceFactory();
            }

            return null;
        }

        /// <summary>
        /// Initialize Blazor component with state management
        /// </summary>
        public async Task<bool> InitializeBlazorComponentAsync(ComponentBase component, string componentName)
        {
            return await InitializeComponentAsync($"Blazor_{componentName}", async () =>
            {
                // Ensure component is properly initialized
                if (component == null)
                {
                    throw new ArgumentNullException(nameof(component), "Component cannot be null");
                }

                // Check if component has required services
                await ValidateComponentDependencies(component);

                // Initialize component state
                await InitializeComponentState(component);
            });
        }

        /// <summary>
        /// Validate that component has all required dependencies
        /// </summary>
        private async Task ValidateComponentDependencies(ComponentBase component)
        {
            // Check for common required services
            var componentType = component.GetType();
            var properties = componentType.GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(InjectAttribute), false).Any());

            foreach (var property in properties)
            {
                var value = property.GetValue(component);
                if (value == null)
                {
                    throw new InvalidOperationException($"Required dependency {property.Name} is not injected in {componentType.Name}");
                }
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Initialize component state with error handling
        /// </summary>
        private async Task InitializeComponentState(ComponentBase component)
        {
            try
            {
                // Trigger component initialization if it has OnInitializedAsync
                var onInitMethod = component.GetType().GetMethod("OnInitializedAsync");
                if (onInitMethod != null)
                {
                    var result = onInitMethod.Invoke(component, null);
                    if (result is Task task)
                    {
                        await task;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[COMPONENT INIT] Error initializing component state for {ComponentType}", 
                    component.GetType().Name);
                throw;
            }
        }

        /// <summary>
        /// Get initialization statistics
        /// </summary>
        public Dictionary<string, int> GetInitializationStats()
        {
            return new Dictionary<string, int>(_retryCounters);
        }

        /// <summary>
        /// Clear retry counters (useful for testing or reset scenarios)
        /// </summary>
        public void ClearRetryCounters()
        {
            _retryCounters.Clear();
            _logger.LogInformation("[COMPONENT INIT] Retry counters cleared");
        }

        /// <summary>
        /// Check if a component has failed initialization multiple times
        /// </summary>
        public bool HasRepeatedFailures(string componentName)
        {
            var retryKey = $"{componentName}_{DateTime.Now:yyyyMMdd}";
            return _retryCounters.GetValueOrDefault(retryKey, 0) >= MaxRetries;
        }
    }

    /// <summary>
    /// Extension methods for easier component initialization
    /// </summary>
    public static class ComponentInitializationExtensions
    {
        public static async Task<bool> InitializeWithRetryAsync(this ComponentBase component, 
            ComponentInitializationService initService, string componentName)
        {
            return await initService.InitializeBlazorComponentAsync(component, componentName);
        }
    }
}
