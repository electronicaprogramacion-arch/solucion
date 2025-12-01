
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using Blazor.Controls;

namespace Blazor.Controls
{
    public partial class ResponsiveTable<TableItem> : ControlComponentBase, IDisposable where TableItem : class, new()
    {

        //[Inject] 
        //public NavigationManager NavigationManager { get; set; }

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


        protected override async Task OnInitializedAsync()

        {
            IsDisable = !Enabled;
            aTimer = new System.Timers.Timer(1500);
            aTimer.Elapsed += OnUserFinish;
            aTimer.AutoReset = false;
            DragDropService.StateHasChanged += ForceRender;

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
                        SortingAscending = true

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
//                        Console.WriteLine("ISTORE 2222.......................................");
                        if (currentPagination != null && Cache != null && currentPagination.GetType() == Cache.ResultState.Value.CurrentPagination.GetType())
                        {
//                            Console.WriteLine("type cache");
//                            Console.WriteLine(currentPagination.GetType().Name);
//                            Console.WriteLine(Cache.ResultState.Value.CurrentPagination.GetType());
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
                //await CloseProgress();
            }

        }


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

//            Console.WriteLine("OnInitializedAsync xxxxx");
//            Console.WriteLine(Items.Count);
//            Console.WriteLine(ItemsDataSource.Count());
            Items.Clear();
//            Console.WriteLine(Items.Count);
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


//            Console.WriteLine(Items.Count);
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


        public static object GetPropertyValue(object src, string propName)
        {
            if (src == null)
            {

                return new object();
                // throw new ArgumentException("Value cannot be null.", "src");
            }

            if (propName == null)
            {

                throw new ArgumentException("Value cannot be null.", "propName");
            }


            if (propName.Contains("."))//complex type nested
            {
                var temp = propName.Split(new char[] { '.' }, 2);

                var og = GetPropertyValue(src, temp[0]);

                if (og == null)
                {
                    og = src;
                }

                return GetPropertyValue(og, temp[1]);
            }
            else
            {
                var prop = src.GetType().GetProperty(propName);
                return prop != null ? prop.GetValue(src, null) : null;
            }
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
            else if (sortDirection.ToLower() == "des")
            {
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
//            Console.WriteLine("swap");
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
            if (!String.IsNullOrEmpty(Class))
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
//            Console.WriteLine("swap");
        }

        public async Task<ResultSet<TableItem>> ExecuteServiceWithBlock(Func<Pagination<TableItem>, Task<ResultSet<TableItem>>> Method, Pagination<TableItem> Parameter, Component component)
        {

            try
            {
                await ShowProgress();
//                Console.WriteLine("ExecuteServiceWithBlock show");


                var result = await ExecuteService(Method, Parameter, component);


                return result;
            }
            catch (Exception ex)
            {
//                Console.WriteLine("ExecuteServiceWithBlock close");
                await ShowError(ex.Message);
                return new ResultSet<TableItem>();
            }
            finally
            {
                await CloseProgress();
            }


        }

        public async Task<ResultSet<TableItem>> ExecuteService(Func<Pagination<TableItem>, Task<ResultSet<TableItem>>> Method, Pagination<TableItem> Parameter, Component component)
        {
//            Console.WriteLine(component.Route);
//            Console.WriteLine(component.IsModal);

            if (component.IsModal || (Parameter != null && !Parameter.SaveCache) || string.IsNullOrEmpty(component.Route))
            {
                if (Method == null || Parameter == null || component == null)
                {
                    return new ResultSet<TableItem>();
                }


                var resultitems2 = await Method(Parameter);

                return resultitems2;

            }
            else
            if (EnableCache && Cache != null && Cache?.ResultState?.Value != null && Cache.ResultState.Value.Url != component.Route
                && Cache.ResultState.Value._dicState != null && Cache.ResultState.Value._dicState.Count > 0)
            {
//                Console.WriteLine("ISTORE .......................................");

                var aa = Cache.ResultState.Value._dicState;

                //if (aa.ContainsKey(component.Route))
                //{
                //Logger.LogInformation("ContainsKey " + component.Route);

                //TodosState r;

                var r = GetPageState(component.Route, aa);

                if (r != null)
                {

                    dynamic entity = r;
                    Component comp = entity.Component;
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
//                Console.WriteLine("no if ");
                IPaginationBase par = Parameter;
            }

//            Console.WriteLine("methodo execute ");

            Parameter.Component = component;

//            Console.WriteLine("methodo execute 2");
            if (Method == null || Parameter == null || component == null)
            {
                return new ResultSet<TableItem>();
            }
            var resultitems = await Method(Parameter);

//            Console.WriteLine("methodo execute 3");
            //IPaginationBase par = Parameter;
            if (EnableCache)
            {
                Facade.LoadTodos2<TableItem>(resultitems, Parameter, component.Route);
            }



            return resultitems;


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


    }


}
