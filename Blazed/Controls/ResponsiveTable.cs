
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Infraestructure.Blazor.Controls;
using DynamicExpressions;
using Helpers;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Blazed.Controls
{
    public partial class ResponsiveTable<TableItem> : ControlComponentBase, IDisposable where TableItem : class, new()
    {




        [Parameter]
        public Func<TableItem,Task> AddToListCheck { get; set; }

        bool allowRowSelectOnRowClick = false;
        public IEnumerable<TableItem> employees;
        public IList<TableItem> selectedEmployees;


        [Parameter]
        public bool HasCheck { get; set; }



        [Parameter]
        public bool UseRadzen { get; set; } = true;


        /// <summary>
        /// 1-grid, 2-datalist
        /// /// </summary>

        [Parameter]
        public int TypeRender { get; set; } = 1;


        [Parameter]
        public bool OnlySearch { get; set; }

        // Keep this for backward compatibility
        public RadzenResponsiveTable<TableItem> Grid { get; set; }




        public List<TableItem> ListMan = new List<TableItem>();

        // For the Radzen DataGrid
        public RadzenDataGrid<TableItem> dataGrid;
        public int totalCount;
        public bool isLoading = false;
        

        // Paging properties
        public int pageSize = 10;
        public int currentPage = 1;
        public IEnumerable<int> pageSizeOptions = new int[] { 5, 10, 20, 30, 50, 100 };

        [Parameter]
        public Func<List<TableItem>, List<TableItem>> OrderMethod{ get; set; }

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
                var pagination = new Pagination<TableItem>()
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
                if (!string.IsNullOrWhiteSpace(SearchTerm))
                {
                    // If we have a search term from the search box, use it as the primary filter
                    pagination.Filter = SearchTerm;
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

                await SearchFunction(null, pagination);

                var result = CurrentResult;

                if (result != null && result.List != null)
                {
                    ListMan = result.List.ToList();
                    totalCount = result.Count;
                    Console.WriteLine($"Loaded {ListMan.Count} manufacturers out of {totalCount} total");
                }
                else if(result?.List == null && PaginationFunction1 == null && Items != null)
                {
                    if(OrderMethod!= null )
                    {
                        Items = OrderMethod(Items);
                    }
                    totalCount = Items.Count;
                }
                else
                {


                    ListMan = new List<TableItem>();
                    totalCount = 0;
                    Console.WriteLine("No manufacturers loaded");
                }

                //await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading manufacturers: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                ListMan = new List<TableItem>();
                totalCount = 0;
            }
            finally
            {
                isLoading = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        public async Task Search()
        {
            if (dataGrid != null)
            {
                await dataGrid.Reload();
            }
        }
        public async Task OnPageSizeChange(int value)
        {
            pageSize = value;
            if (dataGrid != null)
            {
                // Force a reload of the grid with the new page size
                await dataGrid.Reload();
            }
        }

        //[Parameter]

        //public Func<GridColumn<TableItem>,Task> Search { get; set; }

        public void NavigateToEdit(TableItem manufacturer)
        {

        }


        [Parameter]
        public bool Scrollable { get; set; }

        //public  bool visible = false;
        public bool interactive = true;
        public bool interactiveExceptId = true;
        public bool fullScreen = true;
        public int counter = 0;

        protected void HandleOnClose()
        {

        }



        //public FluentDialog? _myFluentDialog;
        public bool _trapFocus = true;
        public bool _modal = true;
        public bool _preventScroll = true;
        public string? _status;
        public bool Hidden { get; set; } = true;


        [Parameter]
        public bool ReportHeader { get; set; }

        [Parameter]
        public bool ReportParams { get; set; }


        [Parameter]
        public bool ReportTitle { get; set; }


        [Parameter]
        public ReportView Report { get; set; }

        [Parameter]
        public bool ReportViewPag { get; set; }


        [Parameter]
        public bool PaginationView { get; set; } = true;

        [Parameter]
        public bool ReportView { get; set; }

        [Parameter]
        public bool ReportCSS { get; set; }

        [Parameter]
        public bool AutoGenerateColumn { get; set; }

        public void OnOpen()
        {
            _status = "Dialog opened with button click";
            //_myFluentDialog!.Show();

        }

        //public async Task OnClose()
        //{
        //    _status = $"Dialog dismissed with reason: Close button clicked";
        //    //_myFluentDialog!.Hide();
        //    await EnabledAdvancedSearch();
        //}

        //public async Task OnDismiss(DialogEventArgs args)
        //{
        //    if (args is not null && args.Reason is not null && args.Reason == "dismiss")
        //    {
        //        _status = $"Dialog dismissed with reason: Dismissed";
        //        //_myFluentDialog!.Hide();
        //        await EnabledAdvancedSearch();

        //    }
        //}
        public static IList<TableItem> CheckListTMP { get; set; }

        [Parameter]
        public IList<TableItem> CheckList { get; set; }


        [Parameter]
        public IList<TableItem> CheckListNew { get; set; }



        [Parameter]
        public string OverlayContainerID { get; set; } = "FullContainer";


        [Parameter]
        public string CSSWidth { get; set; }


        [Inject]
        public HttpClient Http { get; set; }



        [Parameter]
        public bool IsVisibleNew { get; set; } = true;










        [Parameter]
        public string ClientDefaultSortDirection { get; set; } = "Asc";

        [Parameter]
        public string ClientDefaultSortField { get; set; }

        public class SelectRow
        {
            public TableItem Entity { get; set; } = new TableItem();
            public int Position { get; set; }
        }

        //private Component _Component { get; set; } = new Component();

        //[Parameter]
        //public new Component Component { get
        //    {
        //        return base.Component;

        //    }
        //    set
        //    {
        //        base.Component = value;

        //    } }

        [Parameter]
        public string EditBehavior { get; set; } = "link";

        //[CascadingParameter]
        //private Task<AuthenticationState> stateTask { get; set; }

        private string infoString { get; set; }

        private async Task<bool> IsInRole()
        {
            var user = (await stateTask).User;

            return user.IsInRole(Roles);
        }

        private ElementReference searchE;

        public bool ShowSearch { get; set; }


        public string Show { get; set; } = "display: inline-block;";

        [Parameter]
        public bool ForceShow { get; set; }

        [Parameter]
        public string ID { get; set; } = "modalWindow";

        [Parameter]
        public bool DefaultHide { get; set; }


        public string CSSFull { get; set; }

        public string CSSClass { get; set; } = "modal fade bd-example-modal-lg";


        [Parameter]
        public bool Enabled { get; set; } = true;

        public bool IsDisable { get; set; }

        public int TotalRenderRow { get; set; }

        public int CurrentRenderIndexRow { get; set; }

        public TableItem CurrentRenderRow { get; set; } = new TableItem();

        public TableItem OriginalCurrentRenderRow { get; set; }


        [Parameter]
        public bool DisabledRowHover { get; set; }

        [Parameter]
        public bool ClientPagination { get; set; } = false;

        [Parameter]
        public bool HasCancelButton { get; set; } = true;


        [Parameter]
        public bool ForceNEwButton { get; set; }


        [Parameter]
        public Func<TableItem, TableItem> RowAfterRender { get; set; }

        [Parameter]
        public Func<TableItem, TableItem> SaveNewItemFunc { get; set; }

        [Parameter]
        public Func<TableItem, TableItem> RowExecute { get; set; }


        [Parameter]
        public Expression<Func<TableItem, bool>> RowCondition { get; set; }

        public string SortColumn { get; set; }

        public List<GridColumn<TableItem>> Columns { get; set; } = new List<GridColumn<TableItem>>();

        [Parameter]
        public RenderFragment ChildContent { get; set; }


        [Parameter]
        public Func<string, Task<IEnumerable<TableItem>>> FuncFilter { get; set; }


        [Parameter]
        public Func<TableItem, Task<IEnumerable<TableItem>>> FilterByObject { get; set; }

        [Parameter]
        public Func<Pagination<TableItem>, Task<Helpers.Controls.ValueObjects.ResultSet<TableItem>>> PaginationFunction1 { get; set; }


        public TableChanges<TableItem> TableChanges { get; set; } = new TableChanges<TableItem>();


        [Parameter]
        public Action<int, int> PaginationMethod { get; set; }

        [Parameter] public int TotalRows { get; set; } = 1;
        [Parameter] public int PaginaActual { get; set; } = 1;
        [Parameter] public int PaginasTotales { get; set; }

        public Helpers.Controls.ValueObjects.ResultSet<TableItem> CurrentResult { get; set; }
        //[Inject] 
        //public NavigationManager NavigationManager { get; set; }
        public string MessageResult { get; set; }

        [Parameter]
        public bool ShowHeader { get; set; } = true;

        [Parameter]
        public bool HasChange { get; set; }

        [Parameter]
        public bool ActionBeginRow { get; set; }


        [Inject]
        public IResponsiveTableService? Cache { get; set; }

        public bool EnableCache { get; set; } = true;


        [Inject]
        public IStateFacade? Facade { get; set; }


        public bool IsDragAndDrop { get; set; }


        [Inject]
        public DragDropService<TableItem> DragDropService { get; set; }


        [Parameter]
        public bool EnabledDrag { get; set; }


        public EditContext EditContextSearch { get; set; } = new EditContext(new TableItem());



        [Parameter]
        public string NewButtonText { get; set; } = "new";


        [Parameter]
        public EventCallback<ChangeEventArgs> OnUpdate { get; set; }


        [Parameter]
        public EventCallback<ChangeEventArgs> OnSave { get; set; }


        public string ModalName { get; set; } = "modalWindow";

        public bool EnableShowModal { get; set; } = false;

        [Parameter]
        public bool IsNew { get; set; } = false;

        public int CurrentEditIndex = -1;

        public int CurrentRowIndex { get; set; }

        public string SearchTerm { get; set; } = "";

        public string SearchTerm2 { get; set; } = "";

        private List<string> identifiers = new List<string> { "Button1", "Button2", "Button3" };

        private Dictionary<int, ElementReference> myComponents = new Dictionary<int, ElementReference>();


        List<TableItem> _ItemList = new List<TableItem>();



        [Parameter]
        public string Roles { get; set; }


        [Parameter]
        public bool SaveButtonSubmit { get; set; } = false;


        [Parameter]
        public bool EnabledModal { get; set; } = true;

        [Parameter]
        public bool EnabledValidation { get; set; } = true;

        [Parameter]
        public bool EnabledSearch { get; set; } = false;


        [Parameter]
        public string ActionText { get; set; } = "edit";

        [Parameter]
        public TableItem DefaultNew { get; set; }


        [Parameter]
        public bool HasAction { get; set; }





        [Parameter]
        public bool? HasSelect { get; set; }

        [Parameter]
        public bool? HasSave { get; set; }

        [Parameter]
        public bool? HasNew { get; set; }

        [Parameter]
        public bool? HasEdit { get; set; }

        [Parameter]
        public bool? HasDelete { get; set; }

        [Parameter]
        public bool? HasDuplicate { get; set; }

        [Parameter]
        public bool FootAction { get; set; } = false;


        [Parameter]
        public RenderFragment GridFooter { get; set; }


        [Parameter]
        public RenderFragment<TableItem> EditButton { get; set; }


        [Parameter]
        public RenderFragment NewButton { get; set; }


        [Parameter]
        public RenderFragment<TableItem> GridSelect { get; set; }


        [Parameter]
        public RenderFragment<dynamic> GridSelect2 { get; set; }

        [Parameter]
        public bool ShowProgress { get; set; }


        [Parameter]
        public RenderFragment<TableItem> GridEdit { get; set; }

        public FilterObject<TableItem> Filter { get; set; } = new FilterObject<TableItem>();

        [Parameter]
        public RenderFragment<dynamic> GridFilter { get; set; }


        [Parameter]
        public RenderFragment<dynamic> DynamicGridFilter { get; set; }


        [Parameter]
        public RenderFragment<TableItem> GridNew { get; set; }

        public System.Timers.Timer aTimer;

        public void OnUserFinish(System.Object source, ElapsedEventArgs e)
        {
            aTimer.Stop();
            InvokeAsync(async () =>
            {
               await  SearchFunction2();
            });
            
        }

        public ValidationMessageStore messageStore;

        public Pagination<TableItem> currentPagination { get; set; }

        public GridColumn<TableItem> currentColumn { get; set; }

        public async Task SearchFunction2()
        {
            await Search();
            //await SearchFunction();

        }


        public async Task SearchFunction(GridColumn<TableItem> _Column = null, Pagination<TableItem> pag = null)
        {
            try
            {
                //await ShowProgress();

                if (CheckList != null)
                {
                    CheckListTMP = CheckList;
                }

                int Page = 1;

                if (_Column != null)
                {
                    SortColumn = _Column.Field;
                    _Column.SortDescending = !_Column.SortDescending;
                    sortAscending = !_Column.SortDescending;
                    if (_Column.PageNumber > 0)
                    {
                        Page = _Column.PageNumber;
                    }


                }
                else
                {
                    if (currentColumn == null)
                    {
                        currentColumn = new GridColumn<TableItem>();
                    }

                    currentColumn.SortDescending = sortAscending;
                    currentColumn.Field = SortColumn;
                }





                if (PaginationFunction1 != null)
                {
                    if (pag is null)
                    {
                        pag = new Pagination<TableItem>()
                        {

                            Page = Page,
                            Show = PageSize,
                            Filter = SearchTerm,
                            ColumnName = SortColumn,
                            SortingAscending = sortAscending,
                            Object = Filter,
                            ReportView = Report




                        };
                    }


                    if (ActiveAdvancedSearch)
                    {
                        pag.Filter = "";
                        pag.Object.Advanced = true;
                    }

                    //Facade.LoadResult<TableItem>(PaginationFunction1,pag);


                    //CurrentPage = pag;
                    //var resultitems = await PaginationFunction1(pag);
                    //CurrentResult = default;

                    Helpers.Controls.ValueObjects.ResultSet<TableItem> resultitems = await ExecuteServiceWithBlock(PaginationFunction1, pag, Component);



                    if (CheckList != null)
                    {
                        CheckList = CheckListTMP;
                    }

                    await LoadData(PaginaActual, resultitems);

                    currentPagination = pag;

                }
                else
                {
                    ClientDefaultSortField = SortColumn;
                    if (sortAscending)
                    {
                        ClientDefaultSortDirection = "asc";
                    }
                    else
                    {
                        ClientDefaultSortDirection = "desc";
                    }

                    if (CheckList != null)
                    {
                        CheckList = CheckListTMP;
                    }
                    updateList(1);

                }


                if (_Column != null)
                {
                    if (currentColumn == null)
                    {
                        currentColumn = new GridColumn<TableItem>();
                    }

                    if (!sortAscending)
                    {
                        currentColumn.SortDescending = false;
                    }
                    else
                    {
                        currentColumn.SortDescending = true;
                    }
                    currentColumn.Field = SortColumn;
                }
                try
                {
                    if (EnabledSearch)
                    {
                        await searchE.FocusAsync();
                    }

                }
                catch (Exception ex)
                {

                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
            finally
            {
                
                //await CloseProgress();

            }

        }





        public async Task KeyEvent(string FieldName, object Field, KeyboardEventArgs arg)
        {



            string value = "";
            if (Field != null)
            {
                value = Field.ToString();
            }


            var kp = arg.Key; //Backspace

            ///if ((PaginationFunction1 != null && value.Length > 2 && kp != "Backspace") || PaginationFunction1 != null && kp == "Enter")
            if ((PaginationFunction1 != null) || PaginationFunction1 != null && kp == "Enter")
            {

                // remove previous one
                aTimer.Stop();

                // new timer
                aTimer.Start();

                //await SearchFunction();

                //Pagination pag = new Pagination()
                //{
                //    Page = 1,
                //    Show = PageSize,
                //    Filter = SearchTerm

                //};


                //var resultitems = await PaginationFunction1(pag);

                //await LoadData(PaginaActual, resultitems);

            }

            return;


            //if (FuncFilter != null)
            //{
            //    var lst = await FuncFilter(Field.ToString());

            //    if (lst != null)
            //    {

            //        ItemList = lst.Skip((curPage - 1) * PageSize).Take(PageSize);
            //        totalPages = (int)Math.Ceiling(Items.Count() / (decimal)PageSize);

            //        SetPagerSize("forward");
            //    }

            //}
        }



        public void onClick(MouseEventArgs arg)
        {


        }


        [Parameter]
        public Func<TableItem, Task<bool>> DeleteAction { get; set; }

        public int CurrentDeleteIndex { get; set; }

        public int CurrentSelectIndex { get; set; }

        public TableItem CurrentSelect { get; set; }


        private string sortColumn = string.Empty;
        private bool sortAscending = true;
        /// <summary>
        ///  Event occurs when the user clicks on a column header to sort.
        /// </summary>
        [Parameter]
        public EventCallback<SortEventArgs> OnSort { get; set; }


        /// <summary>
        /// Size of each page of BlazorGrid. This is a required field.
        /// </summary>
        [Parameter]
        public int PageSize { get; set; } = 10;

        [Parameter]
        public bool ModalFilter { get; set; }

        [Parameter]
        public string HeaderStyle { get; set; } = "\"font-weight: bold; background-color: #5A5C69 !important; color: white; font-size: small; padding: .0rem; height: 45px;\"";

        [Parameter]
        public string HeaderRowClass { get; set; } = "row headerRow bg-dark";


        [Parameter]
        public string GridClass { get; set; } = " gridContainer p-2 grid-striped ";


        [Parameter]
        public string RowClass { get; set; } = "row grid-row";

        [Parameter]
        public string RowEditClass { get; set; } = "row grid-row";

        [Parameter]
        public string RowNewClass { get; set; } = "row grid-row new";


        [Parameter]
        public string GridID { get; set; } = "gridID";

        [Parameter]
        public string AloneNewButtonCSS { get; set; } = "col-sm customcol";

        [Parameter]
        public string ColActionHeaderCSS { get; set; } = "col-sm-1 customcolAction";

        [Parameter]
        public string ColActionCSS { get; set; } = "col-sm-1 customcolAction";

        [Parameter]
        public string AlingActionCSS { get; set; } = "AlingLeft";


        [Parameter]
        public string ColButtonActionCSS { get; set; } = "col-sm-1 customcolAction";

        //// <summary>
        ///  new and cancel zone
        /// </summary>
        [Parameter]
        public string ColFormActionCSS { get; set; } = "col-sm customcolAction paddingzero";

        [Parameter]
        public string NewButtonCSS { get; set; } = "btn btn-sm btn-primary shadow-sm editButton";

        [Parameter]
        public string SaveButtonCSS { get; set; } = "btn btn-sm btn-info shadow-sm editButton";//"btn btn-sm btn-success shadow-sm editButton";


        [Parameter]
        public string EditButtonCSS { get; set; } = "btn btn-sm btn-primary shadow-sm btn-sm editButton";


        [Parameter]
        public string SaveFormButtonCSS { get; set; } = "btn btn-lg btn-primary shadow-lg btn-lg editButton";

        [Parameter]
        public string CancelFormButtonCSS { get; set; } = "btn btn-lg btn-primary shadow-lg btn-lg editButton";


        [Parameter]
        public string CancelButtonCSS { get; set; } = "d-sm-inline-block btn btn-sm btn-warning shadow-sm editButton";

        [Parameter]
        public string DeleteButtonCSS { get; set; } = "d-sm-inline-block btn btn-sm btn-danger shadow-sm editButton";



        [Parameter]
        public bool InLineEdit { get; set; } = true;

        [Parameter]
        public string DefaultView { get; set; } = "Grid";

        [Parameter]
        public EventCallback<EventArgs> OnChangeWindow { get; set; }

        [Parameter]
        public TableItem currentEdit2 { get; set; }

        [Parameter]
        public Func<TableItem, string, Task> AfterAction { get; set; }


        /// <summary>
        /// The list of item supplied to the BlazorGrid.
        /// </summary>
        [Parameter]
        public List<TableItem> ItemsDataSource { get { return _ItemsDataSource; } set { SetDataSource(value); } }

        public async Task<bool> DispatchSaveEvent(object ar)
        {

            if (OnSave.HasDelegate)
            {
                ChangeEventArgs arg = new ChangeEventArgs();

                arg.Value = ar;

                //IsSave = true;
                try
                {
                    await OnSave.InvokeAsync(arg);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }

                //IsSave = false;

            }
            else
            {

                return true;
            }



        }


        public async Task<bool> DispatchEditEvent(object ar)
        {

            if (OnUpdate.HasDelegate)
            {
                ChangeEventArgs arg = new ChangeEventArgs();

                arg.Value = ar;

                //IsSave = true;
                try
                {
                    await OnUpdate.InvokeAsync(arg);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }

                //IsSave = false;

            }
            else
            {

                return true;
            }



        }



        public async Task DispatchEditEvent2(object ar)
        {

            if (OnUpdate.HasDelegate)
            {
                ChangeEventArgs arg = new ChangeEventArgs();

                arg.Value = ar;

                //IsSave = true;

                await OnUpdate.InvokeAsync(arg);

                //IsSave = false;

            }

        }


        public async Task SaveNewItem(TableItem item, string SortDirecction = null, string SortField = null, bool? isUpdate = true, int Position = -1, bool isdelete = false)
        {
            if (item == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(SortDirecction))
            {
                ClientDefaultSortDirection = SortDirecction;
            }


            if (!string.IsNullOrEmpty(SortField))
            {
                ClientDefaultSortField = SortField;
            }



            List<TableItem> list = new List<TableItem>();

            TableItem ti = new TableItem();

            ti = item;

            //editContext.Model
            if (editContext != null && EnabledValidation)
            {

                //if (isdelete)
                //{
                //    editContext = new EditContext(item);
                //}


                messageStore = new ValidationMessageStore(editContext);

                var result = editContext.Validate();

                if (!result)
                {
                    var editContext2 = new EditContext(item);

                    editContext2.ShouldUseFieldIdentifiers = editContext2.ShouldUseFieldIdentifiers;

                    result = editContext2.Validate();

                }



                if (!result)
                {
                    FieldIdentifier _fieldIdentifier;

                    //Expression<Func<TableItem>> For;

                    //_fieldIdentifier = FieldIdentifier.Create();

                    List<string> properties = new List<string>();

                    properties = GetProperties();

                    foreach (var prop in properties)
                    {
                        var fieldIdentifier = new FieldIdentifier(editContext.Model, prop);

                        var mes = editContext.GetValidationMessages(fieldIdentifier);

                        string mesagge = "";
                        foreach (var msg in mes)
                        {
                            mesagge = msg;
                        }

                        ValidationMessages[prop] = mesagge;

                        if (string.IsNullOrEmpty(mesagge))
                        {
                            await ShowError("error in " + prop);
                        }
                        else
                        {
                            await ShowError(mesagge);
                        }
                    }

                    StateHasChanged();
                    return;
                }
            }

            var a = await DispatchSaveEvent(ti);


            if (SaveNewItemFunc != null)

            {
                ti = SaveNewItemFunc(ti);
            }



            if (a == false)
            {
                return;
            }


            //list = Items.ToList();
            if (Position == -1)
            {
                Items.Add(ti);
            }
            else
            {

                for (int iu = 0; iu <= Position; iu++)
                {
                    list.Add(Items[iu]);
                }
                list.Add(ti);
                for (int iu = Position + 1; iu < Items.Count; iu++)
                {
                    list.Add(Items[iu]);
                }
                Items = list;
            }



            IsNew = false;

            TableChanges.AddList.Add(ti);


            if (PaginationFunction1 != null)
            {
                if (TableChanges.AddList.Count >= PageSize)
                {
                    PageSize = TableChanges.AddList.Count + 1;
                }
            }

            if (!isUpdate.HasValue || isUpdate.Value)
            {
                updateList(curPage);
            }


            if (AfterAction != null)
            {
                await AfterAction(ti, "Save");
            }
            ////Items.Add(ti);
            //await CloseModal();

            HasChange = true;
        }



        public List<string> GetProperties()
        {
            if (ValidationMessages == null || ValidationMessages.Count() == 0)
            {

                if (currentEdit2 == null)
                {
                    currentEdit2 = new TableItem();
                }

                ValidationMessages = currentEdit2.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => "");

            }
            return ValidationMessages.Keys.ToList();

        }


        public async Task CancelNew(string item)
        {
            CurrentEditIndex = -1;
            IsNew = false;
            await CloseModal();

        }

        public async Task CloseModal()
        {
            //if (EnableShowModal)
            //{

            //clean mistakes

            EnableShowModal = false;

            List<string> properties = new List<string>();

            properties = ValidationMessages.Keys.ToList();

            foreach (var prop in properties)
            {

                ValidationMessages[prop] = "";

            }

            IsNew = false;

            await RemoveClass();

            StateHasChanged();
            //}
        }


        public async Task CancelItem(string item)
        {
            CurrentEditIndex = -1;
            //currentEdit = new TableItem();

            currentEdit.CopyPropertiesFrom(oldCurrentEditItem);

            //currentEdit=oldCurrentEditItem;

            ChangeWindow(item);

            EnableShowModal = false;

            //await ShowModal();


            await CloseModal();

        }


        //[Parameter]
        //public Func<TableItem, Task> EndExecute { get; set; }




        public async Task SaveItem(string item)
        {
            if (ExternalSave)
            {

                await ExcecuteButtom();

                return;

            }


            var ind = item.Split('-');

            int indice = Convert.ToInt32(ind[1]);

            TableItem iintem = ItemList.ElementAt<TableItem>(indice);

            await SaveItem(iintem);





        }


       


        public async Task SaveItem(TableItem iintem)
        {


            if (iintem == null)
            {
                return;
            }
            //editContext.Model
            if (editContext != null && EnabledValidation)
            {


                var result = editContext.Validate();

                if (!result)
                {
                    FieldIdentifier _fieldIdentifier;

                    //Expression<Func<TableItem>> For;

                    //_fieldIdentifier = FieldIdentifier.Create();

                    List<string> properties = new List<string>();

                    properties = GetProperties();

                    foreach (var prop in properties)
                    {
                        var fieldIdentifier = new FieldIdentifier(editContext.Model, prop);

                        var mes = editContext.GetValidationMessages(fieldIdentifier);

                        string mesagge = "";
                        foreach (var msg in mes)
                        {
                            mesagge = msg;
                        }
                        //if (!string.IsNullOrEmpty(mesagge))
                        //{

                        ValidationMessages[prop] = mesagge;

                        if (string.IsNullOrEmpty(mesagge))
                        {
                            //await ShowError("error in " + prop);
                        }
                        else
                        {
                            await ShowError("Error in Field " + prop + " " + mesagge);
                        }
                    }

                    StateHasChanged();
                    return;
                }
            }

            var res = await DispatchEditEvent(iintem);

            if (!res)
            {
                return;
            }
            //var ind = item.Split('-');

            //int indice = Convert.ToInt32(ind[1]);

            //TableItem iintem = ItemList.ElementAt<TableItem>(indice);

            List<TableItem> list = new List<TableItem>();

            list = Items.ToList();

            //list.Add(iintem);

            Items = list;

            IsNew = false;

            CurrentEditIndex = -1;

            ActionText = "edit";

            //EditItems.Add(iintem);

            updateList(curPage);


            await CloseModal();


        }

        [Parameter]
        public Func<TableItem> NewItemDefaultItem { get; set; }


        public async Task NewItem()
        {
            if (NewItemDefaultItem == null)
            {
                DefaultNew = new TableItem();
            }
            else
            {
                DefaultNew = NewItemDefaultItem();
            }
            if (DefaultNew == null)
            {

                await JSRuntime.InvokeVoidAsync("alert", "The new record could not be generated ");

                return;

            }


            await NewItem(DefaultNew);

            ////editContext = new EditContext(DefaultNew);
            ////editContext.AddDataAnnotationsValidation();
            ////editContext.OnFieldChanged += EditContext_OnFieldChanged;
            //if (!InLineEdit && EnabledModal)
            //{
            //    EnableShowModal = true;

            //    await ShowModal();
            //}

            //IsNew = true;

        }

        [Inject]
        public DialogService DialogService { get; set; }

        public async Task NewItem(TableItem item)
        {

            IsNew = true;
            DefaultNew = item;
            FirstRenderEdit = true;
            

            //if (!InLineEdit && EnabledModal)
            //{
            //    EnableShowModal = true;

            //    await ShowModal();
            //}

            await ShowInlineDialogNew(item);


            

            //StateHasChanged();


        }

        private void onclick(MouseEventArgs args)
        {
            var d = myComponents;
            var a = args;
        }





        public void SetPagerSize(string direction)
        {
            try
            {
                if (direction == "forward" && endPage < totalPages)
                {
                    startPage = endPage + 1;
                    if (endPage + pagerSize < totalPages)
                    {
                        endPage = startPage + pagerSize - 1;
                    }
                    else
                    {
                        endPage = totalPages;
                    }
                    this.StateHasChanged();
                }
                else if (direction == "back" && startPage > 1)
                {
                    endPage = startPage - 1;
                    startPage = startPage - pagerSize;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void NavigateToPage(string direction)
        {


            if (direction == "next")
            {
                if (curPage < totalPages)
                {
                    if (curPage == endPage)
                    {
                        SetPagerSize("forward");
                    }
                    curPage += 1;
                }
            }
            else if (direction == "previous")
            {
                if (curPage > 1)
                {
                    if (curPage == startPage)
                    {
                        SetPagerSize("back");
                    }
                    curPage -= 1;
                }
            }

            updateList(curPage);
        }

        protected async Task Submit(EditContext editContext)
        {

        }

        public string BoundValues(TableItem model, IEnumerable<string> Roles, string PropertyName)
        {
            var entity = editContext.Model.GetType();
            Type type = entity;
            PropertyInfo prop = type.GetProperty(PropertyName);

            FieldIdentifier ID = new FieldIdentifier(editContext.Model, PropertyName);

            if (Roles == null || Roles.Count() == 0)
            {

                prop.SetValue(editContext.Model, string.Empty, null);
                editContext.NotifyValidationStateChanged();
                editContext.NotifyFieldChanged(ID);

                return System.String.Empty;
            }


            var items = Roles;//Roles.Select(p => p.Value);
            var roles = string.Join(", ", items);
            if (model != null && !string.IsNullOrEmpty(roles))
            {
                prop.SetValue(model, roles, null);
                editContext.NotifyValidationStateChanged();
                editContext.NotifyFieldChanged(ID);
            }


            return "";
        }

        public string BoundValues(TableItem model, object Value, string PropertyName)
        {
            var entity = editContext.Model.GetType();
            Type type = entity;
            PropertyInfo prop = type.GetProperty(PropertyName);

            FieldIdentifier ID = new FieldIdentifier(editContext.Model, PropertyName);

            prop.SetValue(model, Value, null);
            editContext.NotifyValidationStateChanged();
            editContext.NotifyFieldChanged(ID);



            return "";
        }


        public int Height { get; set; }
        public int Width { get; set; }

        public async Task GetDimensions()
        {

            try
            {
                if (BrowserService != null)
                {
                    var dimension = await BrowserService.GetDimensions();
                    Height = dimension.Height;
                    Width = dimension.Width;
                }


            }
            catch (Exception ex)
            {



            }

        }








        public async Task ExcecuteButtom(string control = "executebutton")
        {
            if (JSRuntime != null)
            {
                await JSRuntime.InvokeVoidAsync("executeMethod", control);
            }
        }









        [Parameter]
        public bool CheckxBox { get; set; }

        public List<SwitchItem> SwitchList { get; set; }

        public void ChangeSwitch(ChangeEventArgs arg)
        {
            var a = SwitchList.FirstOrDefault(x => x.ID == ((SwitchItem)arg.Value).ID);

            a.Value = !((SwitchItem)arg.Value).Value;
        }


        public class SwitchItem
        {
            public int ID { get; set; }

            public string Label { get; set; }

            public bool Value { get; set; }


        }
        async Task Keyev()
        {
            //await  KeyEvent("Name", SearchTerm);
        }


        public Func<List<TableItem>, List<TableItem>> QueryList { get; set; }

        [Parameter]
        public string FilterColumn { get; set; }

        [Parameter]
        public object ValueFilter { get; set; }

        [Parameter]
        public DynamicExpressions.FilterOperator FilterOperator { get; set; }



        public int totalPages;
        public int curPage;
        public int pagerSize;

        public int startPage = 1;
        public int endPage;

        public string b = "back";

        public string p = "previous";

        public TableItem test;



        public List<TableItem> Items { get; set; } = new List<TableItem>();



        /// <summary>
        /// Header for BlazorGrid.
        /// </summary>
        ///

        [Parameter]
        public RenderFragment GridHeader { get; set; }

        /// <summary>
        /// Rows for BlazorGrid.
        /// </summary>
        [Parameter]
        public RenderFragment<TableItem> GridRow { get; set; }


        [Parameter]
        public RenderFragment<TableItem> TestRow { get; set; }

        [Parameter]
        public RenderFragment<TableItem> GridRadzenT { get; set; }


        [Parameter]
        public RenderFragment<TableItem> RadzenGridRow { get; set; }

        [Parameter]
        public RenderFragment<dynamic> GridRowDynamic { get; set; }

        [Parameter]
        public RenderFragment<TableItem> GridCard { get; set; }

        /// <summary>
        /// Rows for BlazorGrid.
        /// </summary>
        [Parameter]
        public RenderFragment<TableItem> GridHeader2 { get; set; }

        public List<TableItem> _ItemsDataSource;



        [Parameter]
        public TableItem currentEdit { get; set; } = new TableItem();


        [Parameter]
        public EditContext editContext { get; set; }

        public bool endload = false;


        public void Dispose()
        {

            if (this != null)
            {

                DragDropService.StateHasChanged -= ForceRender;
                //this.Items = null;

                //this.EditItems = null;
                //this.NewItems = null;
                //this.ItemList = null;


            }
        }

        public List<TableItem> Filter_List(string FilterExpression, object value, List<TableItem> data)
        {

            //      var predicate = new DynamicFilterBuilder<Product>()
            //.And("Enabled", FilterOperator.Equals, true)
            //.And(b => b.And("Brand", FilterOperator.Equals, "Nike").Or("Brand", FilterOperator.Equals, "Adidas"))
            //.And(b => b.And("Price", FilterOperator.GreaterThanOrEqual, 20).And("Price", FilterOperator.LessThanOrEqual, 100))
            //.Build();

            //      var products = _dbContext.Products.AsQueryable().Where(predicate).ToList();

            var predicate = DynamicExpressions.DynamicExpressions.GetPredicate<TableItem>(FilterExpression, DynamicExpressions.FilterOperator.Equals, value);



            // ^ can also be cached or compiled and used anywhere
            //var products = _dbContext.Products.AsQueryable().Where(predicate).ToList();
            // ^ or FirstByDefault, Any, etc...



            List<TableItem> data_sorted = new List<TableItem>();

            var a = data.Where(predicate.Compile()).ToList();

            //data_sorted = (from n in data
            //               where GetDynamicSortProperty(n, sortExpression)== value
            //               select n).ToList();


            return a;

        }




        public object GetDynamicSortProperty(object item, string propName)
        {
            try
            {
                string[] prop = propName.Split('.');

                //Use reflection to get order type
                int i = 0;
                while (i < prop.Count())
                {
                    item = item.GetType().GetProperty(prop[i]).GetValue(item, null);
                    i++;
                    if (item == null)
                    {
                        return null;
                    }
                }

                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


        /// <summary>
        /// Run an action once after the component is rendered
        /// </summary>
        /// <param name="action">Action to invoke after render</param>
        //protected void RunAfterRender(Action action) => IniCoordControl();

        [Parameter]
        public Func<List<TableItem>, List<TableItem>, int, Task> ChangePage { get; set; }

        [Parameter] public bool OnlyNextPrevious { get; set; }

        public bool GetShowControl()
        {
            return ShowControl;
        }


        public bool GetShowlabel()
        {
            return ShowLabel;
        }

        [Parameter]
        public bool LabelHeader { get; set; }

        [Parameter]
        public bool SmallRender { get; set; }


        public bool ShowControl { get; set; }

        public bool ShowLabel { get; set; } = true;


        [Parameter]
        public string Key { get; set; }



        public void ReplaceItemKey(TableItem item)
        {
            var a = GetItemKey(item, Items);

            Items.Remove(a);

            Items.Add(item);
        }



        public TableItem GetItemKey(TableItem item, List<TableItem> data)
        {

            var value = GetDynamicSortProperty(item, Key);

            return GetItem<TableItem>(Key, value, data);



        }


        public TableItem GetItem<TableItem>(string field, object value, List<TableItem> data)
        {

            var predicate = DynamicExpressions.DynamicExpressions.GetPredicate<TableItem>(field, DynamicExpressions.FilterOperator.Equals, value);

            var a = data.Where(predicate.Compile()).FirstOrDefault();

            return a;
        }


        private void HandleClick(string path)
        {
            NavigationManager.NavigateTo(path, true);
        }


        /// <summary>
        /// visible rows inthe grid
        /// </summary>
        public List<TableItem> ItemList
        {

            get
            {
                return _ItemList; // FilterList();
            }
            set
            {

                _ItemList = value;


            }
        }


        public Dictionary<string, string> ValidationMessages = new Dictionary<string, string>();

        private void EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            var entity = e.FieldIdentifier.Model.GetType();



            FieldIdentifier id = new FieldIdentifier(editContext.Model, e.FieldIdentifier.FieldName);

            var message = editContext.GetValidationMessages(id);
            string msg = "";
            foreach (var item in message)
            {
                msg += item + " ";
            }
            if (!string.IsNullOrEmpty(msg))
            {
                ValidationMessages[e.FieldIdentifier.FieldName] = msg;
            }
            else
            {
                ValidationMessages[e.FieldIdentifier.FieldName] = "";
            }

            return;
        }





        public RenderFragment GetErrorMessage(string PropertyName)
        {
            if (ValidationMessages.ContainsKey(PropertyName) && !string.IsNullOrEmpty(ValidationMessages[PropertyName]))
            {
                return builder =>
                {
                    builder.OpenElement(0, "div");
                    builder.AddAttribute(1, "class", "validation-message");
                    builder.AddAttribute(1, "style", "display:block !important");
                    builder.AddContent(2, ValidationMessages[PropertyName]);
                    builder.CloseElement();
                };


                //return "<div class='validation-message'>" + ValidationMessages[PropertyName] + "</div>";
            }
            else
            {
                return null;
            }
        }

        public void DeleteAll()
        {


            List<TableItem> list = new List<TableItem>();

            TableItem ti = new TableItem();

            list = Items.ToList();


            list.Clear();

            Items = list;

            IsNew = false;

            ItemList = list;
            //Items.Add(ti);
            if (Items.Count() == 0)
            {
                //Clear();
                updateList(curPage);
            }
            else
            {
                updateList(curPage);
            }



        }




        public TableItem oldCurrentEditItem { get; set; } = new TableItem();

        public async Task EditItem(string item)
        {
            FirstRenderEdit = true;

            var ind = item.Split('-');

            int indice = Convert.ToInt32(ind[1]);

            oldCurrentEditItem.CopyPropertiesFrom(ItemList.ElementAt(indice));

            CurrentEditIndex = indice;
            ActionText = "save";

            if (!InLineEdit && EnabledModal)
            {
                EnableShowModal = true;

                await ShowModal();
            }


            StateHasChanged();

        }
        [Parameter]
        public bool ExternalSave { get; set; } = false;

        public async Task SaveNewItem()
        {
            if (ExternalSave)
            {

                await ExcecuteButtom();

                return;

            }

            List<TableItem> list = new List<TableItem>();

            TableItem ti = new TableItem();

            ti = DefaultNew;


            //editContext.Model
            if (editContext != null && EnabledValidation)
            {
                messageStore = new ValidationMessageStore(editContext);

                var result = editContext.Validate();

                if (!result)
                {
                    //Console.WriteLine("SaveNewItem validation fail");

                    FieldIdentifier _fieldIdentifier;

                    //Expression<Func<TableItem>> For;

                    //_fieldIdentifier = FieldIdentifier.Create();

                    List<string> properties = new List<string>();

                    properties = GetProperties();

                    foreach (var prop in properties)
                    {
                        var fieldIdentifier = new FieldIdentifier(editContext.Model, prop);

                        var mes = editContext.GetValidationMessages(fieldIdentifier);

                        string mesagge = "";
                        foreach (var msg in mes)
                        {
                            mesagge = msg;
                        }
                        if (!string.IsNullOrEmpty(mesagge))
                        {

                            ValidationMessages[prop] = mesagge;

                            if (string.IsNullOrEmpty(mesagge))
                            {
                                await ShowError("error in " + prop);
                            }
                            else
                            {
                                await ShowError(mesagge);
                            }

                        }

                    }

                    StateHasChanged();
                    return;
                }
            }





            var res = await DispatchSaveEvent(ti);

            if (!res)
            {
                return;
            }

            list = Items.ToList();

            list.Add(ti);

            //NewItems.Add(ti);


            Items = list;

            IsNew = false;

            //ClientPagination = true;

            TableChanges.AddList.Add(ti);


            if (PaginationFunction1 != null)
            {
                if (TableChanges.AddList.Count >= PageSize)
                {
                    PageSize = TableChanges.AddList.Count + 1;
                }
            }

            updateList(curPage);
            //Items.Add(ti);
            if (AfterAction != null)
            {
                await AfterAction(ti, "Save");
            }

            await CloseModal();

        }

        public bool IsSave { get; set; }

        public void SetDataSource(List<TableItem> v)
        {
            if (InternalOperation)
            {
                return;
            }

            if (v == null)
            {
                //Console.WriteLine("Responsive SetDataSource Null Table");
                _ItemsDataSource = new List<TableItem>();
            }
            else
            {
                //Console.WriteLine("Responsive SetDataSource value Table " + v.ToString());
                _ItemsDataSource = v;
            }



        }
        public void Clear()
        {
            //Console.WriteLine("Clear");
            Items = new List<TableItem>();

            //ItemList= new List<TableItem>();

            ItemsDataSource = new List<TableItem>();

        }

        public void Clear2()
        {
            //Console.WriteLine("Clear2");
            Items = new List<TableItem>();

            ItemList = new List<TableItem>();

            ItemsDataSource = new List<TableItem>();

        }

        public bool ActiveAdvancedSearch { get; set; } = false;

        [Parameter]
        public bool VisibleSearch { get; set; } = true;

        public async Task EnabledAdvancedSearch()
        {
            if (ActiveAdvancedSearch)
            {
                ActiveAdvancedSearch = false;
                Filter = null;//new FilterObject<TableItem>();
                FilterByObject = null;
                StateHasChanged();
            }
            else
            {
                ActiveAdvancedSearch = true;
                Filter = new FilterObject<TableItem>();
                Filter.Advanced = true;
                if (ModalFilter)
                {
                    OnOpen();
                }

            }
            if (JSRuntime != null)
            {
                await JSRuntime.InvokeVoidAsync("removeValidClass");
            }


            StateHasChanged();
        }






        //Task<Task<ResultSet<TableItem>>>



        public async Task DeleteItem(string item)
        {

            //bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure do you want to delete this record?");
            //if (!confirmed)
            //{
            //    return;
            //}


            var ind = item.Split('-');

            int indice = Convert.ToInt32(ind[1]);

            var item2 = Items.ElementAtOrDefault(indice);

            CurrentDeleteIndex = indice;

            await DeleteItem(item2);



        }

        public bool InternalOperation;


        public async Task DuplicateItem(TableItem item, string Position)
        {
            TableItem item2 = new TableItem();

            //item2.CopyPropertiesFrom(item);

            item2 = (TableItem)item.CloneObject();


            Position = Position.Replace("Index-", "");

            await SaveNewItem(item2, null, null, true, Convert.ToInt32(Position));


        }


        public async Task DeleteItem(TableItem item)
        {
            //InternalOperation = true;
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure do you want to delete this record?");
            if (!confirmed)
            {
                return;
            }

            var item2 = item;

            if (DeleteAction != null)
            {
                try
                {
                    await ShowProgress();
                    var a = await DeleteAction(item2);

                    if (!a)
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                    return;
                }
                finally
                {
                    await CloseProgress();
                }
            }

            List<TableItem> list = new List<TableItem>();

            List<TableItem> list2 = new List<TableItem>();


            TableItem ti = new TableItem();

            list = Items.ToList();

            list2 = ItemList.ToList();

            var item3 = item2;



            IsNew = false;
            TableChanges.DeleteList.Add(item3);
            //Items.Add(ti);
            if (Items.Count() == 1)
            {
                Items.Clear();
                ItemList = null;// new List<TableItem>();
                ItemsDataSource.Clear();// = new List<TableItem>();
                //Console.WriteLine("delete item");
                //Console.WriteLine(ItemsDataSource.Count());
                updateList(curPage);
            }
            else
            {

                list.Remove(item3);

                //lock (this)
                //{
                //    Items.Remove(item);
                //    ItemList.Remove(item);
                //    ItemsDataSource.Remove(item);
                //}

                Items.Clear();
                ItemList = null;// new List<TableItem>();
                ItemsDataSource.Clear();// = new List<TableItem>();
                                        //Console.WriteLine("delete item");
                                        //Console.WriteLine(ItemsDataSource.Count());

                // Items = list;

                //ItemsDataSource = list;

                updateList(curPage);

                foreach (var item45 in list)
                {
                    SaveNewItem(item45, ClientDefaultSortDirection, ClientDefaultSortField, null, -1, true);
                }




                //updateList(curPage);
            }

            if (AfterAction != null)
            {
                await AfterAction(item3, "Delete");
            }

            InternalOperation = false;

            HasChange = true;
        }


        protected void ChangeWindow(object item)
        {
            OnChangeWindow.InvokeAsync(new EventArgs());
        }


        public async Task LoadData(int Pagina, Helpers.Controls.ValueObjects.ResultSet<TableItem> result)
        {
            MessageResult = result.Message;
            //Console.WriteLine("result.count " + result.Count);
            if (result.CurrentPage > result.PageTotal)
            {
                result.PageTotal = result.CurrentPage;
            }

            if (Pagina == 1 || Pagina == 0 || 1 == 1)
            {
                PaginasTotales = result.PageTotal;
                if (!HasTotalRows)
                {
                    TotalRows = result.Count;
                }
            }


            Clear();

            if (result.List == null)
            {
                ShowSearch = true;
                ItemList = new List<TableItem>();
                ItemsDataSource = new List<TableItem>();
                Items = new List<TableItem>();

            }
            else
            {
                ItemList = result.List;
                ItemsDataSource = result.List;
                Items=result.List;
            }

            foreach (var item in this.Columns)
            {
                if (result.ColumnName == item.Field)
                {
                    item.SortDescending = !result.SortingAscending;
                }
            }
            PaginaActual = result.CurrentPage;

            if (PaginaActual == 1)
            {
                PaginasTotales = result.PageTotal;
                if (!HasTotalRows)
                {
                    TotalRows = result.Count;
                }

                if (result.Shown > 0 && OnePagePagination)
                {
                    PageSize = result.Shown;
                }
                else
                {

                }
            }
            if (result.Shown > 0 && OnePagePagination)
            {
                PageSize = result.Shown;
            }
            else
            {

            }

            if (AutoGenerateColumn)
            {
                var pa = this.currentPagination;

                var properties = GetProperties();
                if (Report != null && Report.Columns.Count > 0)
                {
                    foreach (var item in Report.Columns) //properties)
                    {



                        //var camp = Report.Columns.Where(x => x.Field.ToLower() == item.ToLower()).FirstOrDefault();
                        //if (camp != null)
                        //{
                        GridColumn<TableItem> column = new GridColumn<TableItem>();

                        column.Field = item.Field;

                        column.Title = item.Title;

                        column.Format = item.Format;

                        this.Columns.Add(column);
                        //}


                    }
                }

            }

            CurrentResult = result;

            StateHasChanged();

        }



        public async Task ShowModal()
        {
            await JSRuntime.InvokeVoidAsync("showModal", ModalName);

        }

        public async Task RemoveClass()
        {
            await JSRuntime.InvokeVoidAsync("removeClass", "modal-backdrop");

        }

        protected string GetSortStyle(GridColumn<TableItem> _column)
        {

            string type = "";

            if (_column.SortDescending)
            {
                type = "desc";
            }
            else
            {
                type = "asc";
            }



            if (_column.Field == SortColumn)
            {

                return "sortable actual " + type;
            }
            else
            {
                return "sortable2 actual " + type; ;
            }
        }

        //protected string GetSortStyle(string columnName)
        //{
        //    return sortColumn == columnName ? (sortAscending ? "sorting_asc" : "sorting_desc") : string.Empty;
        //}

        protected void SortGrid(string columnName)
        {
            sortAscending = sortColumn == columnName ? !sortAscending : true;
            sortColumn = columnName;

            OnSort.InvokeAsync(new SortEventArgs { ColumnName = sortColumn, SortingAscending = sortAscending });
        }


        [Inject]
        IServiceProvider Services { get; set; }
        protected override async Task OnInitializedAsync()

        {


            timerconfig(timerref1);



            if (Services.GetService<DragDropService<TableItem>>() is { } bs)
            {
                DragDropService = bs;
            }






            IsDisable = !Enabled;
            aTimer = new System.Timers.Timer(1500);
            aTimer.Elapsed += OnUserFinish;
            aTimer.AutoReset = false;
            DragDropService.StateHasChanged += ForceRender;

            //if (AutoGenerateColumn)
            //{
            //    var pa = this.currentPagination;

            //    var properties = GetProperties();

            //    foreach (var item in properties)
            //    {
            //        GridColumn<TableItem> column = new GridColumn<TableItem>();

            //        column.Field = item;

            //        column.Title = item;

            //        this.Columns.Add(column);
            //    }              

            //}



        }



        protected override async Task OnParametersSetAsync()

        {
            IsDisable = !Enabled;
            //IntializedGrid();

        }

        public string GetField(string Field)
        {

            string ColumnNameTmp = "";
            if (!string.IsNullOrEmpty(Field))
            {
                var apos = Field.Split('.');

                ColumnNameTmp = apos[apos.Length - 1];
            }

            return ColumnNameTmp;

        }

        public bool FirstRenderEdit { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            int lineerror = 0;
            //await ShowProgress();

            //if (firstRender && _myFluentDialog != null)
            //    _myFluentDialog!.Hide();


            if (FirstRenderEdit)
            {
                lineerror = 1;
                await JSRuntime.InvokeVoidAsync("removeValidClass");
                FirstRenderEdit = false;
                lineerror = 2;
            }

            try
            {

                if (firstRender && PaginationFunction1 != null)
                {
                    lineerror = 3;
                    foreach (var item in Columns)
                    {
                        if (item.DefaultSort)
                        {
                            SortColumn = GetField(item.Field);
                        }
                    }
                    lineerror = 4;
                    Pagination<TableItem> pag = new Pagination<TableItem>()
                    {
                        Page = 1,
                        Show = PageSize,
                        ColumnName = SortColumn,
                        SortingAscending = true,
                        ReportView = Report

                    };
                    lineerror = 5;
                    PaginaActual = 1;



                    //var resultitems = await PaginationFunction1(pag);
                    var resultitems = await ExecuteServiceWithBlock(PaginationFunction1, pag, Component);
                    lineerror = 6;
                    CurrentResult = resultitems;
                    ClientPagination = resultitems.ClientPagination;
                    if (resultitems.ClientPagination)
                    {
                        lineerror = 7;
                        TotalRows = resultitems.List.Count;
                        PaginaActual = 1;
                        ItemsDataSource = resultitems.List;
                        lineerror = 8;
                    }
                    else
                    {
                        lineerror = 9;
                        await LoadData(1, resultitems);
                        lineerror = 10;
                    }



                    if (currentColumn != null)
                    {
                        lineerror = 11;
                        currentColumn.SortDescending = !sortAscending;
                        currentColumn.Field = SortColumn;

                    }

                    if (EnableCache && Cache != null && Cache?.ResultState?.Value != null)
                    {
                        lineerror = 12;
                        //Console.WriteLine("ISTORE 2222.......................................");
                        if (currentPagination != null && Cache != null && currentPagination.GetType() == Cache.ResultState.Value.CurrentPagination.GetType())
                        {
                            //Console.WriteLine("type cache");
                            //Console.WriteLine(currentPagination.GetType().Name);
                            //Console.WriteLine(Cache.ResultState.Value.CurrentPagination.GetType());
                            currentPagination = (Pagination<TableItem>)Cache.ResultState.Value.CurrentPagination;
                        }
                        if (currentPagination != null)
                        {
                            SearchTerm = currentPagination.Filter;
                            if (currentPagination?.Object != null)
                            {
                                Filter = currentPagination.Object;
                            }
                        }



                        lineerror = 13;
                    }




                    lineerror = 14;
                    //Grid.Items = Eq.PieceOfEquipments;

                    IntializedGrid();
                    lineerror = 15;
                    return;
                }

                if (EnabledSearch && !firstRender)
                {
                    try
                    {
                        await searchE.FocusAsync();
                    }
                    catch (Exception ex)
                    {

                    }

                }
                lineerror = 16;
                await GetDimensions();
                lineerror = 17;

                if (((Items == null || (Items?.Count() == 0 && ItemsDataSource?.Count() >= 0))) && IsSave == false)
                {

                    IntializedGrid();
                }

                if (!firstRender)
                {
                    return;
                }

                IsDisable = !Enabled;

                await base.OnAfterRenderAsync(firstRender);

                EditContextSearch = new EditContext(new TableItem());


            }

            catch (Exception ex)

            {
                await ShowError(ex.Message + "  " + lineerror);
            }
            finally
            {
                if (endload && !firstRender)
                {
                    //await IniCoordControl();
                }
                //await CloseProgress();
            }

        }



        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    int lineerror = 0;
        //    //await ShowProgress();

        //    if (FirstRenderEdit)
        //    {
        //        lineerror = 1;
        //        await JSRuntime.InvokeVoidAsync("removeValidClass");
        //        FirstRenderEdit = false;
        //        lineerror = 2;
        //    }

        //    try
        //    {

        //        if (firstRender)
        //        {
        //            await SearchFunction();
        //        }

        //        if (EnabledSearch && !firstRender)
        //        {
        //            try
        //            {
        //                await searchE.FocusAsync();
        //            }
        //            catch (Exception ex)
        //            {

        //            }

        //        }
        //        lineerror = 16;
        //        await GetDimensions();
        //        lineerror = 17;

        //        if (((Items == null || (Items?.Count() == 0 && ItemsDataSource?.Count() >= 0))) && IsSave == false)
        //        {

        //            IntializedGrid();
        //        }



        //        IsDisable = !Enabled;

        //        await base.OnAfterRenderAsync(firstRender);

        //        EditContextSearch = new EditContext(new TableItem());


        //    }

        //    catch (Exception ex)

        //    {
        //        await ShowError(ex.Message + "  " + lineerror);
        //    }
        //    finally
        //    {
        //        if (endload && !firstRender)
        //        {
        //            //await IniCoordControl();
        //        }
        //        //await CloseProgress();
        //    }

        //}

        public void IntializedGrid()
        {

            var entity = typeof(TableItem);
            //Logger.LogDebug(entity.Name);
            //Logger.LogDebug("OnInitializedAsync Table");

            pagerSize = 5;
            curPage = 1;

            //if (ItemsDataSource == null)
            //{
            //    throw new Exception("Error in ItemDatasource");


            //}

            //Console.WriteLine("OnInitializedAsync xxxxx");
            //Console.WriteLine(Items.Count);
            //Console.WriteLine(ItemsDataSource.Count());
            Items.Clear();
            //Console.WriteLine(Items.Count);
            ItemList = new List<TableItem>();
            if (ItemsDataSource != null)
            {
                foreach (var item in ItemsDataSource)
                {
                    //Logger.LogDebug("OnInitializedAsync Table7");
                    if (Items == null)
                    {
                        Items = new List<TableItem>();
                    }
                    //Logger.LogDebug("OnInitializedAsync Table8");
                    Items.Add(item);
                    //Logger.LogDebug("OnInitializedAsync Table9");
                    //Logger.LogDebug(Items.Count);

                }
            }


            //Console.WriteLine(Items.Count);
            if (!string.IsNullOrEmpty(FilterColumn) && ValueFilter != null)
            {
                Items = Filter_List(FilterColumn, ValueFilter, Items);
            }

            if (!string.IsNullOrEmpty(ClientDefaultSortField))
            {
                Items = Sort_List(ClientDefaultSortDirection, ClientDefaultSortField, Items);
            }



            //Logger.LogDebug("OnInitializedAsync Table3");

            ItemList = Items.Skip((curPage - 1) * PageSize).Take(PageSize).ToList();
            totalPages = (int)Math.Ceiling(Items.Count() / (decimal)PageSize);
            // Logger.LogDebug("OnInitializedAsync Table5");
            SetPagerSize("forward");

            try
            {

                if (currentEdit2 == null)
                {
                    currentEdit2 = new TableItem();
                }

                ValidationMessages = currentEdit2.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .ToDictionary(prop => prop.Name, prop => "");
            }

            catch (Exception ex)

            {

                //Logger.LogDebug(ex.Message);
            }
            //Logger.LogDebug("OnInitializedAsync Table6");



        }


        public object Format(object value)
        {
            try
            {
                if (value == null)
                {
                    return "";
                }

                return value ??= "";
            }

            catch (Exception ex)

            {

            }

            return "";
        }





        public string GetTypeName(Type type)
        {
            if (type.MemberType == MemberTypes.NestedType)
            {
                return string.Concat(GetTypeName(type.DeclaringType), ".", type.Name);
            }

            return type.Name;
        }

        public string GetPropertyName<T>(Expression<Func<T>> action)
        {
            //var s= action.Body.ToString();

            MemberExpression body = action.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)action.Body;
                body = ubody.Operand as MemberExpression;
            }

            var s = GetMemberPath(body);

            return s;
        }



        public List<T> Sort_List<T>(string sortDirection, string sortExpression, List<T> data)
        {

            List<T> data_sorted = new List<T>();



            if (sortDirection.ToLower() == "asc")
            {
                data_sorted = (from n in data
                               orderby GetDynamicSortProperty(n, sortExpression) ascending
                               select n).ToList();
            }
            else if (sortDirection.ToLower() == "desc")
            {
                data_sorted = (from n in data
                               orderby GetDynamicSortProperty(n, sortExpression) descending
                               select n).ToList();

                data_sorted = (from n in data
                               orderby GetDynamicSortProperty(n, sortExpression) descending
                               select n).ToList();

            }

            return data_sorted;

        }

        public string GetMemberPath(MemberExpression me)
        {
            var parts = new List<string>();

            while (me != null)
            {
                parts.Add(me.Member.Name);
                me = me.Expression as MemberExpression;
            }

            parts.Reverse();
            if (parts.Count > 1)
            {
                parts.RemoveAt(0);
            }

            return string.Join(".", parts);
        }


        private void OnDropItemOnSpacing(int newIndex)
        {
            if (!IsDropAllowed())
            {
                DragDropService.Reset();
                return;
            }

            var activeItem = DragDropService.ActiveItem;
            var oldIndex = ItemList.IndexOf(activeItem);
            var sameDropZone = false;
            if (oldIndex == -1) // item not present in target dropzone
            {
                if (CopyItem == null)
                {
                    DragDropService.Items.Remove(activeItem);
                }
            }
            else // same dropzone drop
            {
                sameDropZone = true;
                ItemList.RemoveAt(oldIndex);
                // the actual index could have shifted due to the removal
                if (newIndex > oldIndex)
                    newIndex--;
            }

            if (CopyItem == null)
            {
                ItemList.Insert(newIndex, activeItem);
            }
            else
            {
                // for the same zone - do not call CopyItem
                ItemList.Insert(newIndex, sameDropZone ? activeItem : CopyItem(activeItem));
            }

            //Operation is finished
            DragDropService.Reset();
            OnItemDrop.InvokeAsync(activeItem);
            IsDragAndDrop = true;
            //Console.WriteLine("swap");
        }

        private bool IsMaxItemLimitReached()
        {
            var activeItem = DragDropService.ActiveItem;
            return (!ItemList.Contains(activeItem) && MaxItems.HasValue && MaxItems == ItemList.Count());
        }

        private string IsItemDragable(TableItem item)
        {
            if (AllowsDrag == null)
                return "true";
            if (item == null)
                return "false";
            return AllowsDrag(item).ToString();
        }

        private bool IsItemAccepted(TableItem dragTargetItem)
        {
            if (Accepts == null)
                return true;
            return Accepts(DragDropService.ActiveItem, dragTargetItem);
        }

        private bool IsValidItem()
        {
            return DragDropService.ActiveItem != null;
        }

        protected override bool ShouldRender()
        {
            return DragDropService.ShouldRender;
        }

        private void ForceRender(object sender, EventArgs e)
        {
            StateHasChanged();
        }

        public string CheckIfDraggable(TableItem item)
        {
            if (AllowsDrag == null)
                return "";
            if (item == null)
                return "";
            if (AllowsDrag(item))
                return "";
            return "plk-dd-noselect";
        }

        public string CheckIfDragOperationIsInProgess()
        {
            var activeItem = DragDropService.ActiveItem;
            return activeItem == null ? "" : "plk-dd-inprogess";
        }

        public void OnDragEnd()
        {
            if (DragEnd != null)
            {
                DragEnd(DragDropService.ActiveItem);
            }

            DragDropService.Reset();
            //dragTargetItem = default;
        }

        public void OnDragEnter(TableItem item)
        {
            var activeItem = DragDropService.ActiveItem;
            if (item.Equals(activeItem))
                return;
            if (!IsValidItem())
                return;
            if (IsMaxItemLimitReached())
                return;
            if (!IsItemAccepted(item))
                return;
            DragDropService.DragTargetItem = item;
            if (InstantReplace)
            {
                Swap(DragDropService.DragTargetItem, activeItem);
            }

            DragDropService.ShouldRender = true;
            StateHasChanged();
            DragDropService.ShouldRender = false;
        }

        public void OnDragLeave()
        {
            DragDropService.DragTargetItem = default;
            DragDropService.ShouldRender = true;
            StateHasChanged();
            DragDropService.ShouldRender = false;
        }

        public void OnDragStart(TableItem item)
        {
            DragDropService.ShouldRender = true;
            DragDropService.ActiveItem = item;
            DragDropService.Items = ItemList;
            StateHasChanged();
            DragDropService.ShouldRender = false;
        }

        public string CheckIfItemIsInTransit(TableItem item)
        {
            return item.Equals(DragDropService.ActiveItem) ? "plk-dd-in-transit no-pointer-events" : "";
        }

        public string CheckIfItemIsDragTarget(TableItem item)
        {
            if (item.Equals(DragDropService.ActiveItem))
                return "";
            if (item.Equals(DragDropService.DragTargetItem))
            {
                return IsItemAccepted(DragDropService.DragTargetItem) ? "plk-dd-dragged-over" : "plk-dd-dragged-over-denied";
            }

            return "";
        }

        private string GetClassesForDraggable(TableItem item)
        {
            var builder = new StringBuilder();
            builder.Append("plk-dd-draggable");
            if (ItemWrapperClass != null)
            {
                var itemWrapperClass = ItemWrapperClass(item);
                builder.AppendLine(" " + itemWrapperClass);
            }

            return builder.ToString();
        }

        private string GetClassesForDropzone()
        {
            var builder = new StringBuilder();
            builder.Append("plk-dd-dropzone");
            if (!System.String.IsNullOrEmpty(Class))
            {
                builder.AppendLine(" " + Class);
            }

            return builder.ToString();
        }

        private string GetClassesForSpacing(int spacerId)
        {
            var builder = new StringBuilder();
            builder.Append("plk-dd-spacing");
            //if active space id and item is from another dropzone -> always create insert space
            if (DragDropService.ActiveSpacerId == spacerId && ItemList.IndexOf(DragDropService.ActiveItem) == -1)
            {
                builder.Append(" plk-dd-spacing-dragged-over");
            } // else -> check if active space id and that it is an item that needs space
            else if (DragDropService.ActiveSpacerId == spacerId && (spacerId != ItemList.IndexOf(DragDropService.ActiveItem)) && (spacerId != ItemList.IndexOf(DragDropService.ActiveItem) + 1))
            {
                builder.Append(" plk-dd-spacing-dragged-over");
            }

            return builder.ToString();
        }

        /// <summary>
        /// Allows to pass a delegate which executes if something is dropped and decides if the item is accepted
        /// </summary>
        [Parameter]
        public Func<TableItem, TableItem, bool> Accepts { get; set; }

        /// <summary>
        /// Allows to pass a delegate which executes if something is dropped and decides if the item is accepted
        /// </summary>
        [Parameter]
        public Func<TableItem, bool> AllowsDrag { get; set; }

        /// <summary>
        /// Allows to pass a delegate which executes if a drag operation ends
        /// </summary>
        [Parameter]
        public Action<TableItem> DragEnd { get; set; }

        /// <summary>
        /// Raises a callback with the dropped item as parameter in case the item can not be dropped due to the given Accept Delegate
        /// </summary>
        [Parameter]
        public EventCallback<TableItem> OnItemDropRejected { get; set; }

        /// <summary>
        /// Raises a callback with the dropped item as parameter
        /// </summary>
        [Parameter]
        public EventCallback<TableItem> OnItemDrop { get; set; }

        /// <summary>
        /// Raises a callback with the replaced item as parameter
        /// </summary>
        [Parameter]
        public EventCallback<TableItem> OnReplacedItemDrop { get; set; }

        /// <summary>
        /// If set to true, items will we be swapped/inserted instantly.
        /// </summary>
        [Parameter]
        public bool InstantReplace { get; set; }

        ///// <summary>
        ///// List of items for the dropzone
        ///// </summary>
        //[Parameter]
        //public IList<TableItem> Items { get; set; }

        /// <summary>
        /// Maximum Number of items which can be dropped in this dropzone. Defaults to null which means unlimited.
        /// </summary>
        [Parameter]
        public int? MaxItems { get; set; }

        /// <summary>
        /// Raises a callback with the dropped item as parameter in case the item can not be dropped due to item limit.
        /// </summary>
        [Parameter]
        public EventCallback<TableItem> OnItemDropRejectedByMaxItemLimit { get; set; }

        //[Parameter]
        //public RenderFragment<TableItem> ChildContent { get; set; }

        /// <summary>
        /// Specifies one or more classnames for the Dropzone element.
        /// </summary>
        [Parameter]
        public string Class { get; set; }

        /// <summary>
        /// Specifies the id for the Dropzone element.
        /// </summary>
        //[Parameter]
        //public string Id { get; set; }

        /// <summary>
        /// Allows to pass a delegate which specifies one or more classnames for the draggable div that wraps your elements.
        /// </summary>
        [Parameter]
        public Func<TableItem, string> ItemWrapperClass { get; set; }

        /// <summary>
        /// If set items dropped are copied to this dropzone and are not removed from their source.
        /// </summary>
        [Parameter]
        public Func<TableItem, TableItem> CopyItem { get; set; }

        private bool IsDropAllowed()
        {
            var activeItem = DragDropService.ActiveItem;
            if (!IsValidItem())
            {
                return false;
            }

            if (IsMaxItemLimitReached())
            {
                OnItemDropRejectedByMaxItemLimit.InvokeAsync(activeItem);
                return false;
            }

            if (!IsItemAccepted(DragDropService.DragTargetItem))
            {
                OnItemDropRejected.InvokeAsync(activeItem);
                return false;
            }

            return true;
        }

        private void OnDrop()
        {
            DragDropService.ShouldRender = true;
            if (!IsDropAllowed())
            {
                DragDropService.Reset();
                return;
            }

            var activeItem = DragDropService.ActiveItem;
            if (DragDropService.DragTargetItem == null) //no direct drag target
            {
                if (!Items.Contains(activeItem)) //if dragged to another dropzone
                {
                    if (CopyItem == null)
                    {
                        Items.Insert(Items.Count, activeItem); //insert item to new zone
                        DragDropService.Items.Remove(activeItem); //remove from old zone
                    }
                    else
                    {
                        Items.Insert(Items.Count, CopyItem(activeItem)); //insert item to new zone
                    }
                }
                else
                {
                    //what to do here?
                }
            }
            else // we have a direct target
            {
                if (!Items.Contains(activeItem)) // if dragged to another dropzone
                {
                    if (CopyItem == null)
                    {
                        if (!InstantReplace)
                        {
                            Swap(DragDropService.DragTargetItem, activeItem); //swap target with active item
                        }
                    }
                    else
                    {
                        if (!InstantReplace)
                        {
                            Swap(DragDropService.DragTargetItem, CopyItem(activeItem)); //swap target with a copy of active item

                        }
                    }
                }
                else
                {
                    // if dragged to the same dropzone
                    if (!InstantReplace)
                    {
                        Swap(DragDropService.DragTargetItem, activeItem); //swap target with active item

                    }
                }
            }

            DragDropService.Reset();
            StateHasChanged();
            OnItemDrop.InvokeAsync(activeItem);
        }

        private void Swap(TableItem draggedOverItem, TableItem activeItem)
        {
            var indexDraggedOverItem = Items.IndexOf(draggedOverItem);
            var indexActiveItem = Items.IndexOf(activeItem);
            if (indexActiveItem == -1) // item is new to the dropzone
            {
                //insert into new zone
                Items.Insert(indexDraggedOverItem + 1, activeItem);
                //remove from old zone
                DragDropService.Items.Remove(activeItem);
            }
            else if (InstantReplace) //swap the items
            {
                if (indexDraggedOverItem == indexActiveItem)
                    return;
                TableItem tmp = Items[indexDraggedOverItem];
                Items[indexDraggedOverItem] = Items[indexActiveItem];
                Items[indexActiveItem] = tmp;
                OnReplacedItemDrop.InvokeAsync(Items[indexActiveItem]);
            }
            else //no instant replace, just insert it after 
            {
                if (indexDraggedOverItem == indexActiveItem)
                    return;
                var tmp = Items[indexActiveItem];
                Items.RemoveAt(indexActiveItem);
                Items.Insert(indexDraggedOverItem, tmp);
            }
            IsDragAndDrop = true;
            //Console.WriteLine("swap");
        }

        public async Task<Helpers.Controls.ValueObjects.ResultSet<TableItem>> ExecuteServiceWithBlock(Func<Pagination<TableItem>, Task<Helpers.Controls.ValueObjects.ResultSet<TableItem>>> Method, Pagination<TableItem> Parameter, Helpers.Controls.Component component)
        {

            try
            {

               
                //await ShowProgress();


                //Console.WriteLine("ExecuteServiceWithBlock show");


                var result = await ExecuteService(Method, Parameter,    component);


                return result;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("ExecuteServiceWithBlock close");
                await ShowError(ex.Message);
                return new Helpers.Controls.ValueObjects.ResultSet<TableItem>();
            }
            finally
            {
                await CloseProgress();
            }


        }

        public async Task<Helpers.Controls.ValueObjects.ResultSet<TableItem>> ExecuteService(Func<Pagination<TableItem>, Task<Helpers.Controls.ValueObjects.ResultSet<TableItem>>> Method, Pagination<TableItem> Parameter, Helpers.Controls.Component component)
        {
            //Console.WriteLine(component.Route);
            //Console.WriteLine(component.IsModal);

            if (component.IsModal || (Parameter != null && !Parameter.SaveCache) || string.IsNullOrEmpty(component.Route))
            {
                if (Method == null || Parameter == null || component == null)
                {
                    return new Helpers.Controls.ValueObjects.ResultSet<TableItem>();
                }


                var resultitems2 = await Method(Parameter);

                return resultitems2;

            }
            else
            if (EnableCache && Cache != null && Cache?.ResultState?.Value != null && Cache.ResultState.Value.Url != component.Route
                && Cache.ResultState.Value._dicState != null && Cache.ResultState.Value._dicState.Count > 0)
            {
                //Console.WriteLine("ISTORE .......................................");

                var aa = Cache.ResultState.Value._dicState;

                //if (aa.ContainsKey(component.Route))
                //{
                //Logger.LogInformation("ContainsKey " + component.Route);

                //TodosState r;

                var r = GetPageState(component.Route, aa);

                if (r != null)
                {

                    dynamic entity = r;
                    Helpers.Controls.Component comp = entity.Component;
                    if (entity != null && comp.Route == component.Route)
                    {



                        int page = entity.Page;



                        string filter = entity.Filter;

                        //if (!string.IsNullOrEmpty(filter))




                        dynamic fo = entity.Object;



                        Parameter = entity;



                    }




                }
                else
                {

                }


                //parr = (Pagination<T>)r.CurrentPagination;

                //}


            }
            else
            {
                //Console.WriteLine("no if ");
                IPaginationBase par = Parameter;
            }

            //Console.WriteLine("methodo execute ");

            Parameter.Component = component;

            //Console.WriteLine("methodo execute 2");
            if (Method == null || Parameter == null || component == null)
            {
                return new Helpers.Controls.ValueObjects.ResultSet<TableItem>();
            }
            var resultitems = await Method(Parameter);

            //Console.WriteLine("methodo execute 3");
            //IPaginationBase par = Parameter;
            if (EnableCache)
            {
                Facade.LoadTodos2<TableItem>(resultitems, Parameter, component.Route);
            }



            return resultitems;


        }

        public async void updateList(int currentPage)
        {


            try
            {

                if (currentPage == 0)
                {
                    currentPage = 1;
                }

                if (ChangePage != null)
                {
                    await ChangePage(Items, ItemList, currentPage);
                }

                if (Items.Count == PageSize && currentPage > 1)
                {
                    currentPage = currentPage - 1;
                }

                if (!string.IsNullOrEmpty(FilterColumn) && ValueFilter != null && Items != null && Items.Count > 0)
                {
                    //Console.WriteLine("updateList 1");
                    Items = Filter_List(FilterColumn, ValueFilter, Items);
                }



                if (!string.IsNullOrEmpty(ClientDefaultSortField) && Items != null && Items.Count > 0)
                {
                    //Console.WriteLine("updateList 2");
                    Items = Sort_List(ClientDefaultSortDirection, ClientDefaultSortField, Items);
                    //Console.WriteLine(Items.Count);
                }




                if (Items != null && Items.Count > 0)
                {
                    ItemList = Items.Skip((currentPage - 1) * PageSize).Take(PageSize).ToList();
                    curPage = currentPage;
                    totalPages = (int)Math.Ceiling(Items.Count() / (decimal)PageSize);
                    //SetPagerSize("forward");
                    //Console.WriteLine("updateList 3");
                    //Console.WriteLine(Items.Count);

                }
                else
                if (Items != null && Items.Count == 0)
                {
                    //ItemList = new List<TableItem>();
                    //ItemsDataSource = new List<TableItem>();// Items;
                    //Items.Clear();
                    //CurrentRenderRow = null;

                    Clear2();


                }

                ///RADZEN
                _ItemsDataSource = Items;

                //await dataGrid.Reload();  
                ///////////////////////
                this.StateHasChanged();

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }

        }

        public System.Timers.Timer timerref1 = new System.Timers.Timer();


        void timerconfig(System.Timers.Timer timer1)
        {
            
           timer1.Elapsed += new ElapsedEventHandler(OnReloadEvent);
           timer1.Interval = 2000;
           timer1.Enabled = true;
           
        }

        public void OnReloadEvent(object source, ElapsedEventArgs e)
        {

            if (dataGrid != null)
            {

                timerref1.Stop();

                InvokeAsync(() =>
                {
                    dataGrid.Reload();
                });
            }
            


        }

        public void Reload()
        {
            if(dataGrid!= null)
            {
                isLoading = true;
            }
            
            timerref1.Start();  


        }

        public object GetPageState(string route, List<TodosStateDic> aa)
        {

            if (aa != null && aa.Count > 0)
            {
                //foreach (KeyValuePair<string, TodosState> item in aa)
                //{
                //    Logger.LogInformation("keyxxxx " + item.Key);
                //    Logger.LogInformation("key route " + route);


                //        dynamic ent = item.Value.CurrentPagination;
                //        string dd = ent.Component.Route;
                //        Logger.LogInformation("value from component " + dd + " routr " + route);
                //    if (route == dd)
                //    {
                //        return item.Value;


                //    }
                //}

                var a = aa.Where(x => x.Key == route).FirstOrDefault();
                if (a != null)
                {
                    return a.Value;
                }
                else
                {
                    return null;
                }


            }

            return null;
        }

        public static Type GetFieldType(string fieldName, Type classType)
        {
            if (string.IsNullOrEmpty(fieldName))
                throw new ArgumentException("Field name cannot be null or empty: " + fieldName, nameof(fieldName));

            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            var property = classType.GetProperty(fieldName);
            if (property != null)
                return property.PropertyType;

            var field = classType.GetField(fieldName);
            if (field != null)
                return field.FieldType;

            return null;
        }

        public static Type GetFieldType<T>(string fieldName)
        {
            return GetFieldType(fieldName, typeof(T));
        }

        IRadzenFormComponent editor;
        bool editorFocused;
        List<TableItem> ordersToUpdate = new List<TableItem>();
        List<KeyValuePair<int, string>> editedFields = new List<KeyValuePair<int, string>>();
        string columnEditing;

        /// <summary>
        /// Saves the changes from the Order to the database.
        /// </summary>
        /// <param name="order">The <see cref="Order" /> to save.</param>
        /// <remarks>
        /// Currently, this is called every time the Cell is changed. In a real in-cell edit scenario, you would likely either update
        /// on RowDeselect, or batch the changes using a "Save Changes" button in the header.
        /// </remarks>
        protected void OnUpdateRow(TableItem order)
        {
            //Reset(order);

            //dbContext.Update(order);

            //dbContext.SaveChanges();

            // If you were doing row-level edits and handling RowDeselect, you could use the line below to
            // clear edits for the current record.

            //editedFields = editedFields.Where(c => c.Key != order.OrderID).ToList();

            //RowExecute(order);  

        }

        protected async Task Update()
        {
            editorFocused = false;

            if (ordersToUpdate.Any())
            {
                await dataGrid.UpdateRow(ordersToUpdate.First());
            }
        }

        protected async Task EditRow(TableItem order)
        {
            Reset();

            ordersToUpdate.Add(order);

            await dataGrid.EditRow(order);
        }
        protected void Reset(TableItem order = null)
        {
            editorFocused = false;

            if (order != null)
            {
                ordersToUpdate.Remove(order);
            }
            else
            {
                ordersToUpdate.Clear();
            }
        }

        /// <summary>
        /// Handles the CellClick event of the RadzenDataGrid.
        /// </summary>
        /// <param name="args"></param>
        protected async Task OnCellClick(DataGridCellMouseEventArgs<TableItem> args)
        {
            if (!dataGrid.IsValid ||
                (ordersToUpdate.Contains(args.Data) && columnEditing == args.Column.Property)) return;

            // Record the previous edited field, if you're not using IRevertibleChangeTracking to track object changes
            if (ordersToUpdate.Any())
            {
                //editedFields.Add(new(ordersToUpdate.First().OrderID, columnEditing));
                await Update();
            }

            // This sets which column is currently being edited.
            columnEditing = args.Column.Property;

            // This sets the Item to be edited.
            await EditRow(args.Data);
        }


        /// <summary>
        /// Determines if the specified column is in edit mode for the specified order.
        /// </summary>
        /// <param name="columnName">The RadzenDataGridColumn.Property currently being rendered by the RadzenDataGrid.</param>
        /// <param name="order">The Order currently being rendered by the RadzenDataGrid.</param>
        /// <returns>True if the column should render the EditTemplate for the specified Order, otherwise false.</returns>
        bool IsEditing(string columnName, TableItem order)
        {
            // Comparing strings is quicker than checking the contents of a List, so let the property check fail first.
            return columnEditing == columnName && ordersToUpdate.Contains(order);
        }

        /// <summary>
        /// Determines if the specified column needs a custom CSS class based on the <typeparamref name="TItem">TItem's</typeparamref> state.
        /// </summary>
        /// <param name="column">The RadzenDataGridColumn.Property currently being rendered by the RadzenDataGrid.</param>
        /// <param name="order">The Order currently being rendered by the RadzenDataGrid.</param>
        /// <returns>A string containing the CssClass to add, or <see cref="String.Empty">.</returns>
        string IsEdited(RadzenDataGridColumn<TableItem> column, TableItem order)
        {
            // In a real scenario, you might use IRevertibleChangeTracking to check the current column
            //  against a list of the object's edited fields.

            return string.Empty;
            //return editedFields.Where(c => c.Key == order.OrderID && c.Value == column.Property).Any() ?
            //    "table-cell-edited" :
            //    string.Empty;
        }

    }

   


    public class BlazedControls
    {

        public int ResponsivetableTotalRows { get; set; }


        public int ResponsivetableTotalPage { get; set; }


        public string CurrentQuery { get; set; }


    }
}
