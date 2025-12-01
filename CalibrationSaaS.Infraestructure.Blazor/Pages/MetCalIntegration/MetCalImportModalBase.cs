using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.MetCalIntegration
{
    public class MetCalImportModalBase : ComponentBase
    {
        [Inject] protected IMetCalIntegrationService<CallContext> MetCalService { get; set; } = default!;
        [Inject] protected IWorkOrderDetailServices<CallContext> WorkOrderDetailService { get; set; } = default!;
        [Inject] protected IConfiguration Configuration { get; set; } = default!;
        [Inject] protected ILogger<MetCalImportModalBase> Logger { get; set; } = default!;

        // Parameters
        [Parameter] public int WorkOrderDetailId { get; set; }
        [Parameter] public bool IsVisible { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }
        [Parameter] public EventCallback OnImportComplete { get; set; }

        // Connection Properties
        protected string ServerUrl { get; set; } = string.Empty;
        protected string Username { get; set; } = string.Empty;
        protected string Password { get; set; } = string.Empty;
        protected MetCalConnectionResult? ConnectionStatus { get; set; }

        // Procedure Properties
        protected List<MetCalProcedure>? AvailableProcedures { get; set; }
        protected string SelectedProcedureName { get; set; } = string.Empty;

        // Import Properties
        protected bool OverwriteExisting { get; set; }
        protected bool HasExistingTestPoints { get; set; }
        protected MetCalImportResult? ImportResult { get; set; }

        // UI State Properties
        protected bool IsLoading { get; set; }
        protected string LoadingMessage { get; set; } = string.Empty;
        protected bool ShowProgressBar { get; set; }
        protected int ProgressPercentage { get; set; }
        protected CancellationTokenSource? CancellationTokenSource { get; set; }

        // Computed Properties
        protected bool CanStartImport => ConnectionStatus?.Success == true && 
                                        !string.IsNullOrEmpty(SelectedProcedureName) && 
                                        (!HasExistingTestPoints || OverwriteExisting) &&
                                        !IsLoading;

        protected override async Task OnInitializedAsync()
        {
            // Load default settings from configuration
            ServerUrl = Configuration["MetCal:DefaultServerUrl"] ?? "http://localhost:8080";
            Username = Configuration["MetCal:DefaultUsername"] ?? "";
            
            await CheckExistingTestPoints();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (IsVisible && WorkOrderDetailId > 0)
            {
                await CheckExistingTestPoints();
                ImportResult = null;
                ConnectionStatus = null;
                AvailableProcedures = null;
                SelectedProcedureName = string.Empty;
            }
        }

        protected async Task TestConnection()
        {
            if (string.IsNullOrEmpty(ServerUrl) || string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ConnectionStatus = new MetCalConnectionResult
                {
                    Success = false,
                    ErrorMessage = "Please provide server URL, username, and password"
                };
                StateHasChanged();
                return;
            }

            IsLoading = true;
            LoadingMessage = "Testing connection to MET/CAL server...";
            StateHasChanged();

            try
            {
                var request = new MetCalConnectionRequest
                {
                    MetCalServerUrl = ServerUrl,
                    Username = Username,
                    Password = Password
                };

                ConnectionStatus = await MetCalService.TestConnection(request, new CallContext());

                if (ConnectionStatus.Success)
                {
                    LoadingMessage = "Loading available procedures...";
                    StateHasChanged();
                    await LoadProcedures();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error testing MET/CAL connection");
                ConnectionStatus = new MetCalConnectionResult
                {
                    Success = false,
                    ErrorMessage = $"Connection test failed: {ex.Message}"
                };
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        protected async Task LoadProcedures()
        {
            if (ConnectionStatus?.Success != true) return;

            try
            {
                var request = new MetCalConnectionRequest
                {
                    MetCalServerUrl = ServerUrl,
                    Username = Username,
                    Password = Password
                };

                var result = await MetCalService.GetAvailableProcedures(request, new CallContext());
                
                if (result.Success)
                {
                    AvailableProcedures = result.Procedures?.Where(p => p.IsElectrical).ToList() ?? new List<MetCalProcedure>();
                }
                else
                {
                    //Logger.LogWarning("Failed to load procedures: {ErrorMessage}", result.ErrorMessage);
                    AvailableProcedures = new List<MetCalProcedure>();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error loading MET/CAL procedures");
                AvailableProcedures = new List<MetCalProcedure>();
            }

            StateHasChanged();
        }

        protected async Task StartImport()
        {
            if (!CanStartImport) return;

            IsLoading = true;
            ShowProgressBar = true;
            ProgressPercentage = 0;
            LoadingMessage = "Initializing MET/CAL import...";
            CancellationTokenSource = new CancellationTokenSource();
            StateHasChanged();

            try
            {
                var request = new MetCalImportRequest
                {
                    WorkOrderDetailId = WorkOrderDetailId,
                    ProcedureName = SelectedProcedureName,
                    MetCalServerUrl = ServerUrl,
                    Username = Username,
                    Password = Password,
                    OverwriteExisting = OverwriteExisting
                };

                // Simulate progress updates
                await UpdateProgress(25, "Connecting to MET/CAL server...");
                await Task.Delay(500, CancellationTokenSource.Token);

                await UpdateProgress(50, "Executing calibration procedure...");
                await Task.Delay(500, CancellationTokenSource.Token);

                await UpdateProgress(75, "Retrieving calibration results...");
                await Task.Delay(500, CancellationTokenSource.Token);

                await UpdateProgress(90, "Mapping test point data...");

                ImportResult = await MetCalService.ImportElectricalCalibrationData(request, new CallContext());

                await UpdateProgress(100, "Import complete!");

                if (ImportResult.Success)
                {
                    await CheckExistingTestPoints(); // Refresh the existing test points status
                    await OnImportComplete.InvokeAsync(); // Notify parent component
                }
            }
            catch (OperationCanceledException)
            {
                ImportResult = new MetCalImportResult
                {
                    Success = false,
                    ErrorMessage = "Import was cancelled by user"
                };
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error during MET/CAL import");
                ImportResult = new MetCalImportResult
                {
                    Success = false,
                    ErrorMessage = $"Import failed: {ex.Message}"
                };
            }
            finally
            {
                IsLoading = false;
                ShowProgressBar = false;
                CancellationTokenSource?.Dispose();
                CancellationTokenSource = null;
                StateHasChanged();
            }
        }

        protected async Task UpdateProgress(int percentage, string message)
        {
            ProgressPercentage = percentage;
            LoadingMessage = message;
            StateHasChanged();
            await Task.Delay(100); // Small delay to show progress
        }

        protected void CancelImport()
        {
            CancellationTokenSource?.Cancel();
        }

        protected async Task CloseModal()
        {
            CancelImport();
            await OnClose.InvokeAsync();
        }

        protected async Task CheckExistingTestPoints()
        {
            try
            {
                if (WorkOrderDetailId > 0)
                {
                    var workOrderDetail = new WorkOrderDetail { WorkOrderDetailID = WorkOrderDetailId };
                    var existingResults = await WorkOrderDetailService.GetResultsTable(workOrderDetail, new CallContext());
                    HasExistingTestPoints = existingResults?.Any() == true;
                }
                else
                {
                    HasExistingTestPoints = false;
                }
            }
            catch (Exception ex)
            {
                //Logger.LogWarning(ex, "Error checking existing test points for WorkOrderDetail {WorkOrderDetailId}", WorkOrderDetailId);
                HasExistingTestPoints = false;
            }
        }
    }
}
