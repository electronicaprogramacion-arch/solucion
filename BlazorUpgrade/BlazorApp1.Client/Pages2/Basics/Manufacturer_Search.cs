using Blazored.Modal;
using Blazored.Modal.Services;

using CalibrationSaaS.Application.Services;
using Blazed.Controls;
using CalibrationSaaS.Infraestructure.Blazor.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using CalibrationSaaS.Infraestructure.Blazor.Shared; // Add this for MainLayout
using Grpc.Core;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web; // Add this for MouseEventArgs
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazed.Controls.Toast;
using Radzen;
using Radzen.Blazor;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using BlazorApp1.Blazor.Pages.Layout;

namespace BlazorApp1.Blazor.Pages.Basics.Test
{
    public class Manufacturer_SearchBase : Base_Create<Manufacturer, CalibrationSaaS.Application.Services.IBasicsServices<CallContext>,
        CalibrationSaaS.Domain.Aggregates.Shared.Basic.AppState>
    //ComponentBase,IPage<Manufacturer, Application.Services.IBasicsServices<CallContext>, AppState>
    {
        [CascadingParameter] BlazoredModalInstance BlazoredModal1 { get; set; }
        [CascadingParameter] public IModalService Modal { get; set; }




        public Search<Manufacturer, IBasicsServices<CallContext>, AppState> searchComponent
        { get; set; } = new Search<Manufacturer, IBasicsServices<CallContext>, AppState>();

        // Keep this for backward compatibility
        //public RadzenResponsiveTable<Manufacturer> Grid { get; set; }
        //public List<Manufacturer> ListMan = new List<Manufacturer>();

        //// For the Radzen DataGrid
        //public RadzenDataGrid<Manufacturer> dataGrid;
        //public int totalCount;
        //public bool isLoading = false;
        //public string searchTerm = string.Empty;

        //// Paging properties
        //public int pageSize = 10;
        //public int currentPage = 1;


        

        

       
        public override async Task<Helpers.Controls.ValueObjects.ResultSet<Manufacturer>> LoadData(Pagination<Manufacturer> pag)
        {
            BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);

            var Eq = (await basics.GetManufacturers(pag));

            if (Eq.List != null)
            {
                foreach (var item in Eq.List)
                {
                    var item2 = AppState.Manufacturers.Where(x => x.ManufacturerID == item.ManufacturerID).FirstOrDefault();

                    if (item2 == null)
                    {
                        AppState.AddManufacturer(item);
                    }

                }
            }


            return Eq;

        }


        // This method is already defined in the base class, so we're using OnAfterRenderAsync instead
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await base.OnAfterRenderAsync(firstRender);

                FormName = "Search Manufacturer";
                IsModal = IsModal;
                SelectOnly = SelectOnly;

                // Load initial data
                await RefreshData();

                // Set the SubMenu content (in case it wasn't set in OnInitializedAsync)
                if (MainLayout != null)
                {
                    //MainLayout.SetSubMenuContent(ManufacturerButtons);
                }
            }
        }



        public async Task<bool> Delete(Manufacturer DTO)
        {
            //await searchComponent.ShowModalAction();

            await Client.DeleteManufacturer(DTO, new CallOptions());


            return true;

            //searchComponent.ShowResult();
        }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public void NavigateToEdit(Manufacturer manufacturer)
        {
            NavigationManager.NavigateTo($"Manufacturer/{manufacturer.ManufacturerID}");
        }


        

        public async Task RefreshData()
        {
            try
            {
                // Reload the data from the server
                var pagination = new Pagination<Manufacturer>() { Page = 1, Show = 10 };
                var result = await LoadData(pagination);

                if (result != null && result.List != null)
                {
                    ListMan = result.List.ToList();
                    totalCount = result.Count;
                }
                else
                {
                    ListMan = new List<Manufacturer>();
                    totalCount = 0;
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading manufacturers: {ex.Message}");
                ListMan = new List<Manufacturer>();
                totalCount = 0;
                StateHasChanged();
            }
        }

        public async Task LoadDataForGrid(LoadDataArgs args)
        {
            try
            {
                isLoading = true;
                Console.WriteLine($"LoadDataForGrid called with filter: {args.Filter}");

                // Update currentPage based on args
                if (args.Skip.HasValue && args.Top.HasValue)
                {
                    currentPage = (args.Skip.Value / args.Top.Value) + 1;
                }

                // Create pagination object from LoadDataArgs
                var pagination = new Pagination<Manufacturer>()
                {
                    Page = currentPage,
                    Show = args.Top.HasValue ? args.Top.Value : pageSize // Use the value from args if available
                };

                // Handle sorting
                if (args.Sorts != null && args.Sorts.Any())
                {
                    var sort = args.Sorts.First();
                    pagination.ColumnName = sort.Property;
                    pagination.SortingAscending = sort.SortOrder == SortOrder.Ascending;
                    Console.WriteLine($"Sorting by {sort.Property}, Ascending: {sort.SortOrder == SortOrder.Ascending}");
                }

                // Handle filtering
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    // If we have a search term from the search box, use it as the primary filter
                    pagination.Filter = searchTerm;
                    Console.WriteLine($"Applied search term filter: {pagination.Filter}");
                }
                else if (args.Filters != null && args.Filters.Any())
                {
                    // Get all filter values and combine them
                    var filterValues = args.Filters
                        .Where(f => !string.IsNullOrEmpty(f.FilterValue?.ToString()))
                        .Select(f => f.FilterValue?.ToString())
                        .Where(v => !string.IsNullOrEmpty(v));

                    if (filterValues.Any())
                    {
                        pagination.Filter = string.Join(" ", filterValues);
                        Console.WriteLine($"Applied column filter: {pagination.Filter}");
                    }
                }
                else if (!string.IsNullOrEmpty(args.Filter))
                {
                    pagination.Filter = args.Filter;
                    Console.WriteLine($"Applied grid filter: {pagination.Filter}");
                }

                var result = await LoadData(pagination);

                if (result != null && result.List != null)
                {
                    ListMan = result.List.ToList();
                    totalCount = result.Count;
                    Console.WriteLine($"Loaded {ListMan.Count} manufacturers out of {totalCount} total");
                }
                else
                {
                    ListMan = new List<Manufacturer>();
                    totalCount = 0;
                    Console.WriteLine("No manufacturers loaded");
                }

                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading manufacturers: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                ListMan = new List<Manufacturer>();
                totalCount = 0;
            }
            finally
            {
                isLoading = false;
                await InvokeAsync(StateHasChanged);
            }
        }

#pragma warning disable CS0108 // 'Manufacturer_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<Manufacturer, IBasicsServices<CallContext>, AppState>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        public void Dispose()
#pragma warning restore CS0108 // 'Manufacturer_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<Manufacturer, IBasicsServices<CallContext>, AppState>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        {

        }

        protected async Task FormSubmitted(EditContext editContext)
        {
            CurrentEditContext = new EditContext(Grid.currentEdit);

            bool re = await ContextValidation(true);

            var msg = ValidationMessages;
            //if (  re  || CustomValidation(eq))
            if (1 == 1)
            {

                try
                {

                    BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);


                    Result = (await basics.CreateManufacturer(eq));


                    await ShowToast("The information has been saved successfully.", ToastLevel.Success);

                    await CloseModal(Result);

                }
                catch (Exception ex)
                {
                    await ShowError("Error Save " + ex.Message);

                }

            }
        }

        protected async Task Submitted(ChangeEventArgs arg)
        {




            var aa = (Manufacturer)arg.Value;


            //CurrentEditContext = new EditContext(Grid.currentEdit);

            //bool re = await ContextValidation(true);


            //if (  re  || CustomValidation(eq))
            if (1 == 1)
            {


                try
                {

                    BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);


                    Result = (await basics.CreateManufacturer(aa));


                    await ShowToast("The information has been saved successfully.", ToastLevel.Success);


                    await CloseModal(Result);


                }

                catch (Exception ex)
                {


                    await ShowError("Error Save " + ex.Message);

                    throw ex;

                }

            }
        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected async Task SubmittedUP(ChangeEventArgs arg)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            var aa = (Manufacturer)arg.Value;



        }

        [CascadingParameter]
        public MainLayout MainLayout { get; set; }

        // Define RenderFragment for buttons to pass to SubMenu
        private RenderFragment ManufacturerButtons => builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "manufacturer-buttons");

            // Add New Manufacturer button
            builder.OpenComponent<RadzenButton>(2);
            builder.AddAttribute(3, "Text", "Add New Manufacturer");
            builder.AddAttribute(4, "Icon", "add_business");
            builder.AddAttribute(5, "ButtonStyle", ButtonStyle.Primary);
            builder.AddAttribute(6, "Click", EventCallback.Factory.Create<MouseEventArgs>(this, () => NavigationManager.NavigateTo("Manufacturer/0")));
            builder.AddAttribute(7, "class", "action-button");
            builder.CloseComponent();

            builder.CloseElement();
        };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            // Set the SubMenu content
            if (MainLayout != null)
            {
                //BlazorApp1.Blazor.Pages.Layout.MainLayout.SetSubMenuContent(ManufacturerButtons);
            }
        }

        // Search functionality
        public async Task Search()
        {
            if (dataGrid != null)
            {
                await dataGrid.Reload();
            }
        }

        public async Task SearchKeyPress(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                await Search();
            }
        }
    }
}
