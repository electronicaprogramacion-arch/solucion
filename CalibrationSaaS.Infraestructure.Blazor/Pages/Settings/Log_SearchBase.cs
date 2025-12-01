using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Blazed.Controls;
using Radzen.Blazor;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Domain.Aggregates.Shared;
using Radzen;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Settings;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using System.Text.Json;
using Microsoft.JSInterop;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using AppAuditLogEntry = CalibrationSaaS.Application.Services.AuditLogEntry;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Settings
{
    public class Log_SearchBase : KavokuComponentBase2, IDisposable
    {
        [Inject] protected ILogger<Log_SearchBase> Logger { get; set; }
        [Inject] protected DialogService DialogService { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IAuditLogService<ProtoBuf.Grpc.CallContext> AuditLogService { get; set; }
        [Inject] protected IEntityTypeProvider EntityTypeProvider { get; set; }

        // Search parameters
        protected string? SelectedEntityType { get; set; }
        protected string? EntityId { get; set; }
        protected string? UserName { get; set; }
        protected string? SelectedActionType { get; set; }
        protected DateTime? FromDate { get; set; }
        protected DateTime? ToDate { get; set; }

        // Extended audit search parameters
        protected string? ClientIpAddress { get; set; }

        // UI state
        protected bool IsLoading { get; set; } = false;
        protected bool HasSearched { get; set; } = false;
        protected bool HasError { get; set; } = false;
        protected string ErrorMessage { get; set; } = string.Empty;
        protected string ErrorType { get; set; } = string.Empty;

        // Data
        protected List<AuditLogEntry> AuditLogs { get; set; } = new List<AuditLogEntry>();
        protected List<EntityTypeOption> EntityTypes { get; set; } = new List<EntityTypeOption>();
        protected List<EntityTypeOption> ActionTypes { get; set; } = new List<EntityTypeOption>();

        // Grid reference
        protected RadzenDataGrid<AuditLogEntry> AuditLogGrid { get; set; }

        // Edit context for form validation
        protected EditContext CurrentEditContext { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //Logger.LogInformation("OnInitializedAsync called - Current DateTime.Now: {Now}", DateTime.Now);

            await base.OnInitializedAsync();

            // Set up authorization component
            LoadComponent();

            // Set default permissions for audit log access
            if (string.IsNullOrEmpty(Component.Group))
            {
                Component.Group = "admin";
            }

            Policy = "HasAccess";

            await InitializeEntityTypesAsync();
            InitializeActionTypes();

            //Logger.LogInformation("About to call InitializeDateFilters");
            InitializeDateFilters();
            //Logger.LogInformation("After InitializeDateFilters - FromDate: {FromDate}, ToDate: {ToDate}", FromDate, ToDate);

            // Initialize edit context for form validation
            CurrentEditContext = new EditContext(this);
        }

        public virtual void LoadComponent()
        {
            var BaseUrl = NavigationManager.BaseUri;
            var CurrUrl = NavigationManager.Uri;
            CurrUrl = CurrUrl.Replace(BaseUrl, "");

            Policy = "HasAccess";
            Component.Route = CurrUrl + "_AuditLog";

            var ass = CurrUrl.Split('/');
            string componenttmp = "";
            if (ass.Length > 0)
            {
                componenttmp = ass[0];
            }
            else
            {
                componenttmp = CurrUrl;
            }

            if (AppSecurity != null && AppSecurity?.Components?.Count > 0)
            {
                var assto = AppSecurity.Components.Where(x => x.Route != null && x.Route.ToLower() == componenttmp.ToLower()).FirstOrDefault();

                if (assto != null)
                {
                    if (!string.IsNullOrEmpty(assto.Group) && string.IsNullOrEmpty(Component.Group))
                    {
                        Component.Group = assto.Group;
                    }
                    componenttmp = assto.Route;
                }
            }

            Component.Name = componenttmp;
        }

        private async Task InitializeEntityTypesAsync()
        {
            // FINAL FIX: Hardcode all 5 entity types to ensure they all show up
            EntityTypes = new List<EntityTypeOption>
            {
                new EntityTypeOption { Value = "Manufacturer", Text = "Manufacturer" },
                new EntityTypeOption { Value = "EquipmentTemplate", Text = "Equipment Template" },
                new EntityTypeOption { Value = "PieceOfEquipment", Text = "Piece Of Equipment" },
                new EntityTypeOption { Value = "WorkOrder", Text = "Work Order" },
                new EntityTypeOption { Value = "WorkOrderDetail", Text = "Work Order Detail" }
            };

            //Logger.LogInformation("AUDIT DEBUG: Set all 5 entity types directly");
            //Console.WriteLine($"AUDIT DEBUG CONSOLE: Set all 5 entity types: {string.Join(", ", EntityTypes.Select(et => et.Text))}");

            await Task.CompletedTask;
        }

        private void InitializeActionTypes()
        {
            ActionTypes = new List<EntityTypeOption>
            {
                new EntityTypeOption { Value = "Create", Text = "Create" },
                new EntityTypeOption { Value = "Update", Text = "Update" },
                new EntityTypeOption { Value = "Delete", Text = "Delete" },
                new EntityTypeOption { Value = "Insert", Text = "Insert" }
            };
        }

        private void InitializeDateFilters()
        {
            // Default to last 30 days - ensure we're using current year
            var now = DateTime.Now;

            // Force current date and time
            ToDate = DateTime.Now;
            FromDate = DateTime.Now.AddDays(-30);

            //Logger.LogInformation("InitializeDateFilters called - Current DateTime.Now: {Now}", now);
            //Logger.LogInformation("Set ToDate to: {ToDate}", ToDate);
            //Logger.LogInformation("Set FromDate to: {FromDate}", FromDate);
            //Logger.LogInformation("Initialized date filters: FromDate={FromDate}, ToDate={ToDate}, Current Year={Year}",
                //                FromDate, ToDate, now.Year);
        }

        private void HandleError(Exception ex, string operation)
        {
            HasError = true;
            //Logger.LogError(ex, "Error occurred during {Operation}", operation);

            // Determine error type and provide user-friendly message
            switch (ex)
            {
                case TimeoutException:
                    ErrorType = "Timeout";
                    ErrorMessage = "The request timed out. Please try again or contact support if the problem persists.";
                    break;

                case HttpRequestException httpEx:
                    ErrorType = "Connection";
                    ErrorMessage = "Unable to connect to the audit service. Please check your internet connection and try again.";
                    break;

                case UnauthorizedAccessException:
                    ErrorType = "Authorization";
                    ErrorMessage = "You don't have permission to access audit logs. Please contact your administrator.";
                    break;

                case ArgumentException argEx:
                    ErrorType = "Validation";
                    ErrorMessage = $"Invalid search parameters: {argEx.Message}. Please check your filters and try again.";
                    //Logger.LogError(argEx, "ArgumentException details: {Message}, StackTrace: {StackTrace}", argEx.Message, argEx.StackTrace);
                    break;

                case InvalidOperationException:
                    ErrorType = "Service";
                    ErrorMessage = "The audit service is currently unavailable. Please try again later.";
                    break;

                case TaskCanceledException:
                    ErrorType = "Cancelled";
                    ErrorMessage = "The request was cancelled. Please try again.";
                    break;

                default:
                    ErrorType = "General";
                    ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                    break;
            }

            StateHasChanged();
        }

        private void ClearError()
        {
            HasError = false;
            ErrorMessage = string.Empty;
            ErrorType = string.Empty;
        }

        private CancellationToken CreateTimeoutToken(int timeoutSeconds = 30)
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));
            return cts.Token;
        }

        private bool ValidateSearchParameters()
        {
            //Logger.LogInformation("Validating search parameters: FromDate={FromDate}, ToDate={ToDate}, EntityId={EntityId}, UserName={UserName}",
                //                FromDate, ToDate, EntityId, UserName);

            // Validate date range
            if (FromDate.HasValue && ToDate.HasValue && FromDate.Value > ToDate.Value)
            {
                //Logger.LogWarning("Date validation failed: FromDate {FromDate} is later than ToDate {ToDate}", FromDate.Value, ToDate.Value);
                HandleError(new ArgumentException("From date cannot be later than To date"), "validating search parameters");
                return false;
            }

            // Validate date range is not too large (prevent performance issues)
            if (FromDate.HasValue && ToDate.HasValue)
            {
                var daysDifference = (ToDate.Value - FromDate.Value).TotalDays;
                //Logger.LogInformation("Date range validation: {DaysDifference} days between {FromDate} and {ToDate}",
                //                    daysDifference, FromDate.Value, ToDate.Value);

                if (daysDifference > 365) // More than 1 year
                {
                    //Logger.LogWarning("Date range validation failed: {DaysDifference} days exceeds 365 day limit", daysDifference);
                    HandleError(new ArgumentException($"Date range cannot exceed 365 days (current range: {daysDifference:F0} days)"), "validating search parameters");
                    return false;
                }
            }

            // Validate Entity ID format if provided
            if (!string.IsNullOrEmpty(EntityId) && EntityId.Length > 100)
            {
                //Logger.LogWarning("Entity ID validation failed: length {Length} exceeds 100 characters", EntityId.Length);
                HandleError(new ArgumentException("Entity ID cannot exceed 100 characters"), "validating search parameters");
                return false;
            }

            // Validate User Name format if provided
            if (!string.IsNullOrEmpty(UserName) && UserName.Length > 100)
            {
                //Logger.LogWarning("User name validation failed: length {Length} exceeds 100 characters", UserName.Length);
                HandleError(new ArgumentException("User name cannot exceed 100 characters"), "validating search parameters");
                return false;
            }

            //Logger.LogInformation("Search parameters validation passed");
            return true;
        }

        public virtual async Task<ResultSet<AuditLogEntry>> LoadData(Pagination<AuditLogEntry> pag)
        {
            try
            {
                // Only return data if a search has been performed
                if (!HasSearched)
                {
                    return new ResultSet<AuditLogEntry>
                    {
                        List = new List<AuditLogEntry>(),
                        Count = 0,
                        PageTotal = 0,
                        CurrentPage = 1,
                        Message = "Please click Search to view audit logs",
                        ClientPagination = false
                    };
                }

                // Validate search parameters before making the service call
                if (!ValidateSearchParameters())
                {
                    return new ResultSet<AuditLogEntry>
                    {
                        List = new List<AuditLogEntry>(),
                        Count = 0,
                        PageTotal = 0,
                        CurrentPage = 1,
                        Message = ErrorMessage,
                        ClientPagination = false
                    };
                }

                // Create search request based on current filters
                var searchRequest = new AuditLogSearchRequest
                {
                    EntityType = !string.IsNullOrEmpty(SelectedEntityType) ? SelectedEntityType : null,
                    EntityId = !string.IsNullOrEmpty(EntityId) ? EntityId : null,
                    UserName = !string.IsNullOrEmpty(UserName) ? UserName : null,
                    ActionType = !string.IsNullOrEmpty(SelectedActionType) ? SelectedActionType : null,
                    FromDate = FromDate,
                    ToDate = ToDate,
                    Page = pag.Page,
                    PageSize = pag.Show > 0 ? pag.Show : 20, // Use Show property instead of PageSize
                    SortColumn = pag.ColumnName ?? "Timestamp",
                    SortDescending = !pag.SortingAscending,

                    // Extended audit search parameters
                    ClientIpAddress = !string.IsNullOrEmpty(ClientIpAddress) ? ClientIpAddress : null
                };

                //Logger.LogInformation("About to call SearchAuditLogs with EntityType='{EntityType}', EntityId='{EntityId}', FromDate={FromDate}, ToDate={ToDate}",
                //                    searchRequest.EntityType, searchRequest.EntityId, searchRequest.FromDate, searchRequest.ToDate);

                //Logger.LogInformation("Created search request: EntityType={EntityType}, EntityId={EntityId}, FromDate={FromDate}, ToDate={ToDate}, Page={Page}, PageSize={PageSize}",
                //                    searchRequest.EntityType, searchRequest.EntityId, searchRequest.FromDate, searchRequest.ToDate, searchRequest.Page, searchRequest.PageSize);

                // Call the real audit log service with timeout
                var timeoutToken = CreateTimeoutToken(30); // 30 second timeout
                var serviceResult = await AuditLogService.SearchAuditLogs(searchRequest, timeoutToken);

                //Logger.LogInformation("SearchAuditLogs returned: serviceResult is {ServiceResult}",
                //                    serviceResult != null ? "NotNull" : "NULL");

                // Check for null results and handle gracefully
                if (serviceResult?.List == null)
                {
                    //Logger.LogWarning("SearchAuditLogs returned null result or null List for EntityType='{EntityType}'", searchRequest.EntityType);
                    return new ResultSet<AuditLogEntry>
                    {
                        List = new List<AuditLogEntry>(),
                        Count = 0,
                        PageTotal = 0,
                        CurrentPage = pag.Page,
                        Message = "No audit logs found",
                        ClientPagination = false
                    };
                }

                // Convert from Application.Services.AuditLogEntry to local AuditLogEntry
                var convertedList = serviceResult.List.Select(entry => new AuditLogEntry
                {
                    Id = entry.Id,
                    Timestamp = entry.Timestamp,
                    UserName = entry.UserName ?? string.Empty,
                    EntityType = entry.EntityType ?? string.Empty,
                    EntityId = entry.EntityId ?? string.Empty,
                    ActionType = entry.ActionType ?? string.Empty,
                    PreviousState = entry.PreviousState,
                    CurrentState = entry.CurrentState,
                    AdditionalData = entry.AdditionalData,
                    CreatedDate = entry.CreatedDate,

                    // Extended audit fields
                    ApplicationName = entry.ApplicationName,
                    UserId = entry.UserId,
                    TenantId = entry.TenantId,
                    TenantName = entry.TenantName,
                    ExecutionDuration = entry.ExecutionDuration,
                    ClientIpAddress = entry.ClientIpAddress,
                    CorrelationId = entry.CorrelationId,
                    BrowserInfo = entry.BrowserInfo,
                    HttpMethod = entry.HttpMethod,
                    HttpStatusCode = entry.HttpStatusCode,
                    Url = entry.Url,
                    ExceptionDetails = entry.ExceptionDetails,
                    Comments = entry.Comments,
                    AuditData = entry.AuditData
                }).ToList();

                return new ResultSet<AuditLogEntry>
                {
                    List = convertedList,
                    Count = serviceResult.Count,
                    PageTotal = serviceResult.PageTotal,
                    CurrentPage = serviceResult.CurrentPage,
                    Message = serviceResult.Message,
                    ClientPagination = serviceResult.ClientPagination
                };
            }
            catch (Exception ex)
            {
                HandleError(ex, "loading audit logs");
                return new ResultSet<AuditLogEntry>
                {
                    List = new List<AuditLogEntry>(),
                    Count = 0,
                    PageTotal = 0,
                    CurrentPage = 1,
                    Message = ErrorMessage,
                    ClientPagination = false
                };
            }
        }

        protected async Task SearchAuditLogs()
        {
            try
            {
                ClearError(); // Clear any previous errors
                IsLoading = true;
                HasSearched = true;
                StateHasChanged();

                // Load data directly for Radzen grid
                await LoadAuditLogData();

                //Logger.LogInformation("Search completed - found {Count} audit log entries", AuditLogs.Count);
            }
            catch (Exception ex)
            {
                HandleError(ex, "searching audit logs");
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        private async Task LoadAuditLogData()
        {
            try
            {
                // Create search request based on current filters
                var searchRequest = new AuditLogSearchRequest
                {
                    EntityType = !string.IsNullOrEmpty(SelectedEntityType) ? SelectedEntityType : null,
                    EntityId = !string.IsNullOrEmpty(EntityId) ? EntityId : null,
                    UserName = !string.IsNullOrEmpty(UserName) ? UserName : null,
                    ActionType = !string.IsNullOrEmpty(SelectedActionType) ? SelectedActionType : null,
                    FromDate = FromDate,
                    ToDate = ToDate,
                    Page = 1,
                    PageSize = 1000, // Load more records for client-side pagination
                    ClientIpAddress = !string.IsNullOrEmpty(ClientIpAddress) ? ClientIpAddress : null
                };

                //Logger.LogInformation("Loading audit log data with EntityType='{EntityType}', EntityId='{EntityId}'",
                //                //    searchRequest.EntityType, searchRequest.EntityId);

                // Call the audit log service
                var timeoutToken = CreateTimeoutToken(30);
                var serviceResult = await AuditLogService.SearchAuditLogs(searchRequest, timeoutToken);

                //Logger.LogInformation("SearchAuditLogs returned: serviceResult is {ServiceResult}",
                //                //    serviceResult != null ? "NotNull" : "NULL");

                // Check for null results and handle gracefully
                if (serviceResult?.List == null)
                {
                    //Logger.LogWarning("SearchAuditLogs returned null result or null List for EntityType='{EntityType}'", searchRequest.EntityType);
                    AuditLogs.Clear();
                    return;
                }

                // Convert from Application.Services.AuditLogEntry to local AuditLogEntry
                var convertedList = serviceResult.List.Select(entry => new AuditLogEntry
                {
                    Id = entry.Id,
                    Timestamp = entry.Timestamp,
                    UserName = entry.UserName ?? "Unknown",
                    EntityType = entry.EntityType ?? "Unknown",
                    EntityId = entry.EntityId ?? "Unknown",
                    ActionType = entry.ActionType ?? "Unknown",
                    PreviousState = entry.PreviousState,
                    CurrentState = entry.CurrentState,
                    ExecutionDuration = entry.ExecutionDuration,
                    ClientIpAddress = entry.ClientIpAddress
                }).ToList();

                AuditLogs.Clear();
                AuditLogs.AddRange(convertedList);

                //Logger.LogInformation("Successfully loaded {Count} audit log entries", AuditLogs.Count);
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error loading audit log data");
                AuditLogs.Clear();
                throw;
            }
        }

        protected void ClearFilters()
        {
            //Logger.LogInformation("ClearFilters called - Current DateTime.Now: {Now}", DateTime.Now);

            SelectedEntityType = null;
            EntityId = null;
            UserName = null;
            SelectedActionType = null;

            // Force current date and time
            ToDate = DateTime.Now;
            FromDate = DateTime.Now.AddDays(-30);

            //Logger.LogInformation("ClearFilters - Set ToDate to: {ToDate}", ToDate);
            //Logger.LogInformation("ClearFilters - Set FromDate to: {FromDate}", FromDate);

            // Clear extended audit search parameters
            ClientIpAddress = null;

            AuditLogs.Clear();
            HasSearched = false;
            ClearError(); // Clear any error messages
            StateHasChanged();
        }





        protected async Task ViewCurrentState(AuditLogEntry entry)
        {
            var jsonData = entry.CurrentState ?? "{}";
            await ShowJsonViewer($"Audit Data - {entry.EntityType} {entry.EntityId}", jsonData);
        }

        /// <summary>
        /// Truncates JSON data for display in the grid
        /// </summary>
        /// <param name="json">The JSON string to truncate</param>
        /// <param name="maxLength">Maximum length to display</param>
        /// <returns>Truncated JSON string</returns>
        protected string TruncateJson(string json, int maxLength)
        {
            if (string.IsNullOrEmpty(json))
                return "No data";

            // Remove extra whitespace and newlines for compact display
            var compactJson = json.Replace("\n", "").Replace("\r", "").Replace("  ", " ").Trim();

            if (compactJson.Length <= maxLength)
                return compactJson;

            return compactJson.Substring(0, maxLength) + "...";
        }

        protected async Task ViewAuditDetails(AuditLogEntry entry)
        {
            // Temporarily show JSON viewer until AuditDetailsModal is properly integrated
            var allData = $@"Audit Log Details
=================
Timestamp: {entry.Timestamp:MM/dd/yyyy HH:mm:ss.fff}
User: {entry.UserName ?? "System"}
Entity Type: {entry.EntityType}
Entity ID: {entry.EntityId}
Action: {entry.ActionType}
Application: {entry.ApplicationName ?? "CalibrationSaaS"}
Tenant ID: {entry.TenantId?.ToString() ?? "N/A"}
Execution Duration: {(entry.ExecutionDuration > 0 ? $"{entry.ExecutionDuration} ms" : "N/A")}
IP Address: {entry.ClientIpAddress ?? "N/A"}
Correlation ID: {entry.CorrelationId ?? "N/A"}

Previous State:
{(string.IsNullOrEmpty(entry.PreviousState) ? "N/A" : entry.PreviousState)}

Current State:
{(string.IsNullOrEmpty(entry.CurrentState) ? "N/A" : entry.CurrentState)}";

            await ShowJsonViewer($"Audit Details - {entry.EntityType} {entry.EntityId}", allData);
        }





        private async Task ShowJsonViewer(string title, string jsonData)
        {
            var parameters = new Dictionary<string, object>
            {
                { "Title", title },
                { "JsonData", jsonData }
            };

            await DialogService.OpenAsync<JsonViewerModal>(title, parameters, new DialogOptions
            {
                Width = "800px",
                Height = "600px",
                Resizable = false, // Disable resizing to prevent drag issues
                Draggable = false, // Disable dragging to prevent close button issues
                CloseDialogOnOverlayClick = false,
                CloseDialogOnEsc = true,
                CssClass = "json-viewer-modal",
                ShowClose = true // Ensure close button is shown
            });
        }







        public void Dispose()
        {
            // Cleanup if needed
        }
    }

    [System.Runtime.Serialization.DataContract]
    public class AuditLogEntry : CalibrationSaaS.Domain.Aggregates.Entities.IGeneric
    {
        [System.Runtime.Serialization.DataMember(Order = 1)]
        public Guid Id { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 2)]
        public DateTime Timestamp { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 3)]
        public string UserName { get; set; } = string.Empty;

        [System.Runtime.Serialization.DataMember(Order = 4)]
        public string EntityType { get; set; } = string.Empty;

        [System.Runtime.Serialization.DataMember(Order = 5)]
        public string EntityId { get; set; } = string.Empty;

        [System.Runtime.Serialization.DataMember(Order = 6)]
        public string ActionType { get; set; } = string.Empty;

        [System.Runtime.Serialization.DataMember(Order = 7)]
        public string? PreviousState { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 8)]
        public string? CurrentState { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 9)]
        public string? AdditionalData { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 10)]
        public DateTime CreatedDate { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 11)]
        public string? ApplicationName { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 12)]
        public string? UserId { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 13)]
        public int? TenantId { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 14)]
        public string? TenantName { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 15)]
        public int ExecutionDuration { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 16)]
        public string? ClientIpAddress { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 17)]
        public string? CorrelationId { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 18)]
        public string? BrowserInfo { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 19)]
        public string? HttpMethod { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 20)]
        public int? HttpStatusCode { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 21)]
        public string? Url { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 22)]
        public string? ExceptionDetails { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 23)]
        public string? Comments { get; set; }

        [System.Runtime.Serialization.DataMember(Order = 24)]
        public string? AuditData { get; set; }
    }

    public class EntityTypeOption
    {
        public string Value { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}
