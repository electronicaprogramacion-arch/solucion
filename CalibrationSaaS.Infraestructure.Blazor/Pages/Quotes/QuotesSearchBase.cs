using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Blazed.Controls;
using Blazed.Controls.Toast;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Quotes
{
    public class QuotesSearchBase : Base_Create<Quote, IQuoteService<CallContext>, AppStateCompany>
    {
        public Blazed.Controls.ResponsiveTable<Quote> Grid { get; set; } = new Blazed.Controls.ResponsiveTable<Quote>();

        [Inject] public ILogger<QuotesSearchBase> Logger { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IToastService ToastService { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] public IConfiguration Configuration { get; set; }
        [Inject] public Func<dynamic, CalibrationSaaS.Application.Services.IBasicsServices<CallContext>> BasicsServices { get; set; }

        public IEnumerable<Quote> ListQuotes { get; set; } = new List<Quote>();
        public Quote Filter { get; set; } = new Quote();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await base.OnInitializedAsync();

                // Set the component name and ensure permissions are set
                Component.Name = "QuotesSearch";
                if (string.IsNullOrEmpty(Component.Group))
                {
                    Component.Group = "admin,tech.HasEdit,tech.HasView,tech.HasNew,job.HasView,job.HasEdit,job.HasSave,job.HasNew";
                }

                // Debug logging
                //Logger.LogInformation($"QuotesSearch Component initialized - Name: {Component.Name}, Group: {Component.Group}");

                // Check user authentication and populate User property for authorization
                var authState = await stateTask;
                var user = authState.User;
                //Logger.LogInformation($"User authenticated: {user.Identity?.IsAuthenticated}, Name: {user.Identity?.Name}");

                if (user.Identity?.IsAuthenticated == true)
                {
                    var roles = user.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToList();
                    //Logger.LogInformation($"User roles: {string.Join(", ", roles)}");

                    // Populate User property for authorization
                    try
                    {
                        if (BasicsServices != null && !string.IsNullOrEmpty(user.Identity.Name))
                        {
                            var basicsService = new CalibrationSaaS.Infraestructure.Blazor.Services.BasicsServiceGRPC(BasicsServices, DbFactory);
                            var userDto = new User { UserName = user.Identity.Name };
                            var dbUser = await basicsService.GetUserById(userDto, new CallContext());

                            if (dbUser != null)
                            {
                                User = dbUser;
                                //Logger.LogInformation($"User loaded from database: {dbUser.UserName}, Roles: {string.Join(", ", dbUser.UserRoles?.Select(r => r.Rol?.Name) ?? new List<string>())}");
                            }
                            else
                            {
                                //Logger.LogWarning($"User {user.Identity.Name} not found in database");
                            }
                        }
                    }
                    catch (Exception userEx)
                    {
                        //Logger.LogError(userEx, "Error loading user from database");
                    }
                }

                await LoadInitialData();
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error initializing QuotesSearch page");
                ToastService.ShowError("Error loading quotes data");
            }
        }

        private async Task LoadInitialData()
        {
            try
            {
                //Logger.LogInformation("Starting to load initial quotes data");

                // First try to load real data
                var pagination = new Pagination<Quote>
                {
                    Page = 1,
                    Show = 100, // Increase to get more data
                    Filter = "",
                    Object = new Helpers.Controls.ValueObjects.FilterObject<Quote>()
                };

                //Logger.LogInformation($"Calling LoadData with pagination: Page={pagination.Page}, Show={pagination.Show}");
                var result = await LoadData(pagination);

                if (result != null)
                {
                    //Logger.LogInformation($"LoadData returned result with Count={result.Count}, List.Count={result.List?.Count ?? 0}");
                    ListQuotes = result.List ?? new List<Quote>();
                    TotalRows = result.Count;
                    paginasTotales = (int)Math.Ceiling((double)TotalRows / paginaSize);

                    //Logger.LogInformation($"Set ListQuotes with {ListQuotes.Count()} items, TotalRows={TotalRows}");
                }
                else
                {
                    //Logger.LogWarning("LoadData returned null result");
                    ListQuotes = new List<Quote>();
                    TotalRows = 0;
                    paginasTotales = 0;
                }

                // Force UI update
                StateHasChanged();
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error loading initial quotes data");
                ToastService.ShowError("Error loading quotes");
            }
        }

        public override async Task<ResultSet<Quote>> LoadData(Pagination<Quote> pagination)
        {
            try
            {
                //Logger.LogInformation("Calling Client.GetQuotes via gRPC");
                var result = await Client.GetQuotes(pagination, new CallContext());
                //Logger.LogInformation($"Client.GetQuotes returned: Count={result?.Count ?? 0}, List.Count={result?.List?.Count ?? 0}");

                if (result?.List != null && result.List.Any())
                {
                    //Logger.LogInformation($"First quote: ID={result.List.First().QuoteID}, Number={result.List.First().QuoteNumber}");
                }

                return result;
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error loading quotes data");
                ToastService.ShowError("Error loading quotes data");
                return new ResultSet<Quote> { List = new List<Quote>(), Count = 0 };
            }
        }

        public async Task<bool> Delete(Quote quote)
        {
            try
            {
                if (quote == null) return false;

                var result = await Client.DeleteQuote(quote, new CallContext());
                if (result != null)
                {
                    ToastService.ShowSuccess("Quote deleted successfully");
                    await LoadInitialData();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error deleting quote {QuoteId}", quote?.QuoteID);
                ToastService.ShowError("Error deleting quote");
                return false;
            }
        }

        public async Task Edit(Quote quote)
        {
            try
            {
                if (quote == null) return;
                NavigationManager.NavigateTo($"/QuoteEdit/{quote.QuoteID}");
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error navigating to edit quote {QuoteId}", quote?.QuoteID);
                ToastService.ShowError("Error opening quote for editing");
            }
        }

        public async Task New()
        {
            try
            {
                NavigationManager.NavigateTo("/QuoteCreate");
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error navigating to create new quote");
                ToastService.ShowError("Error creating new quote");
            }
        }

        public async Task ViewQuote(int quoteId)
        {
            try
            {
                NavigationManager.NavigateTo($"/QuoteEdit/{quoteId}");
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error navigating to quote details {QuoteId}", quoteId);
                ToastService.ShowError("Error viewing quote details");
            }
        }

        public async Task GenerateReport(int quoteId)
        {
            try
            {
                await ShowProgress();

                // Create the report URL and open it in a new tab
                var reportUrl = GetQuoteReportUrl(quoteId);
                await JSRuntime.InvokeVoidAsync("OpenWindow", reportUrl);

                ToastService.ShowSuccess("Quote report opened in new tab");
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error generating report for quote {QuoteId}", quoteId);
                ToastService.ShowError("Error generating quote report");
            }
            finally
            {
                await CloseProgress();
            }
        }

        public string GetQuoteReportUrl(int quoteId)
        {
            // Get the Reports API base URL from configuration
            var reportsBaseUrl = Configuration.GetSection("Reports")["Url"] ?? "http://localhost:44312";

            // Ensure no trailing slash
            reportsBaseUrl = reportsBaseUrl.TrimEnd('/');

            // Build the quote report URL with correct query parameter format
            return $"{reportsBaseUrl}/api/print/quote?id={quoteId}";
        }

        public async Task ChangeStatus(int quoteId, string newStatus)
        {
            try
            {
                var quote = new Quote { QuoteID = quoteId };
                var result = await Client.ChangeQuoteStatus(quote, newStatus, new CallContext());
                
                if (result != null)
                {
                    ToastService.ShowSuccess($"Quote status changed to {newStatus}");
                    await LoadInitialData();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error changing status for quote {QuoteId} to {Status}", quoteId, newStatus);
                ToastService.ShowError("Error changing quote status");
            }
        }

        public async Task ClearList()
        {
            try
            {
                ListQuotes = new List<Quote>();
                await LoadInitialData();
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error clearing quotes list");
                ToastService.ShowError("Error refreshing quotes list");
            }
        }

        public void ClearList(EventArgs args)
        {
            // Method called when window changes - can be used to clear filters or reset state
            //Logger.LogInformation("ClearList called");
        }

        public async Task Select(object args, Quote entity)
        {
            try
            {
                if (entity != null)
                {
                    await ViewQuote(entity.QuoteID);
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error selecting quote {QuoteId}", entity?.QuoteID);
                ToastService.ShowError("Error selecting quote");
            }
        }

        public async Task OnSave(Quote entity)
        {
            try
            {
                if (entity != null)
                {
                    var result = await Client.CreateQuote(entity, new CallContext());
                    if (result != null)
                    {
                        ToastService.ShowSuccess("Quote saved successfully");
                        await LoadInitialData();
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error saving quote");
                ToastService.ShowError("Error saving quote");
            }
        }

        public async Task OnUpdate(Quote entity)
        {
            try
            {
                if (entity != null)
                {
                    var result = await Client.UpdateQuote(entity, new CallContext());
                    if (result != null)
                    {
                        ToastService.ShowSuccess("Quote updated successfully");
                        await LoadInitialData();
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error updating quote");
                ToastService.ShowError("Error updating quote");
            }
        }
    }
}
