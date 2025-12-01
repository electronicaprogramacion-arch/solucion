using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Controls
{
    public partial class RadzenResponsiveTable<TableItem> : ComponentBase, IDisposable where TableItem : class, new()
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public bool Scrollable { get; set; }

        [Parameter]
        public bool ReportHeader { get; set; }

        [Parameter]
        public bool ReportParams { get; set; }

        [Parameter]
        public bool ReportTitle { get; set; }

        [Parameter]
        public bool ReportView { get; set; }

        [Parameter]
        public bool ShowSearch { get; set; }

        [Parameter]
        public bool ForceShow { get; set; }

        [Parameter]
        public string ID { get; set; } = "modalWindow";

        [Parameter]
        public bool DefaultHide { get; set; }

        [Parameter]
        public bool Enabled { get; set; } = true;

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

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public RenderFragment<TableItem> GridRow { get; set; }

        [Parameter]
        public RenderFragment<TableItem> GridHeader { get; set; }

        [Parameter]
        public RenderFragment<TableItem> GridFooter { get; set; }

        [Parameter]
        public RenderFragment<dynamic> GridRowDynamic { get; set; }

        [Parameter]
        public RenderFragment<TableItem> GridCard { get; set; }

        [Parameter]
        public RenderFragment<TableItem> GridHeader2 { get; set; }

        [Parameter]
        public IEnumerable<TableItem> ItemsDataSource { get; set; }

        [Parameter]
        public TableItem currentEdit { get; set; } = new TableItem();

        [Parameter]
        public EditContext editContext { get; set; }

        [Parameter]
        public int PageSize { get; set; } = 10;

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
        public string Context { get; set; }

        [Parameter]
        public bool InLineEdit { get; set; }

        [Parameter]
        public string ColActionHeaderCSS { get; set; } = "col-sm-2";

        [Parameter]
        public bool EnabledSearch { get; set; }

        [Parameter]
        public string ClientDefaultSortField { get; set; }

        [Parameter]
        public string ClientDefaultSortDirection { get; set; } = "asc";

        [Parameter]
        public string Component { get; set; }

        [Parameter]
        public EventCallback<TableItem> AfterAction { get; set; }

        [Parameter]
        public string ColActionCSS { get; set; } = "btnWidth";

        [Parameter]
        public bool EnabledDrag { get; set; }

        [Parameter]
        public bool LabelHeader { get; set; }

        [Parameter]
        public string Key { get; set; }

        [Parameter]
        public bool ActionBeginRow { get; set; }

        [Parameter]
        public string ColButtonActionCSS { get; set; }

        [Parameter]
        public string AlingActionCSS { get; set; }

        [Parameter]
        public bool AutoGenerateColumn { get; set; }

        [Parameter]
        public object Report { get; set; }

        [Parameter]
        public bool ModalFilter { get; set; }

        [Parameter]
        public string GridID { get; set; }

        [Parameter]
        public string RowEditClass { get; set; }

        [Parameter]
        public string RowNewClass { get; set; }

        [Parameter]
        public bool EnabledModal { get; set; } = true;

        [Parameter]
        public bool SaveButtonSubmit { get; set; } = false;

        [Parameter]
        public EventCallback OnChangeWindow { get; set; }

        [Parameter]
        public bool SelectOnly { get; set; }

        [Parameter]
        public Func<int, Task<ResultSet<TableItem>>> PaginationFunction1 { get; set; }

        [Parameter]
        public int PaginasTotales { get; set; }

        [Parameter]
        public int TotalRows { get; set; }

        [Parameter]
        public Func<TableItem, Task> DeleteAction { get; set; }

        [Parameter]
        public object CheckList { get; set; }

        [Parameter]
        public Func<TableItem, bool> FilterByObject { get; set; }

        [Parameter]
        public EventCallback<TableItem> OnSave { get; set; }

        [Parameter]
        public EventCallback<TableItem> OnUpdate { get; set; }

        [Parameter]
        public TableItem NewItemDefaultItem { get; set; }

        [Parameter]
        public string NewButtonText { get; set; } = "Add New";

        // Radzen DataGrid reference
        public RadzenDataGrid<TableItem> DataGrid { get; set; }

        // List of columns
        public List<RadzenDataGridColumn<TableItem>> Columns { get; set; } = new List<RadzenDataGridColumn<TableItem>>();

        // Current edit item
        protected TableItem editItem;

        // This method is already defined below

        // Current selected item
        protected TableItem selectedItem;

        // Initial sort parameters
        protected bool initialSortSet = false;

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (!string.IsNullOrEmpty(ClientDefaultSortField) && !initialSortSet && DataGrid != null)
                {
                    // Set initial sort parameters
                    await DataGrid.Reload();
                    initialSortSet = true;
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        public async Task Reload()
        {
            if (DataGrid != null)
            {
                await DataGrid.Reload();
            }
        }

        public async Task InsertRow()
        {
            if (DataGrid != null)
            {
                var newItem = new TableItem();
                await DataGrid.InsertRow(newItem);
            }
        }

        public async Task EditRow(TableItem item)
        {
            if (DataGrid != null)
            {
                await DataGrid.EditRow(item);
            }
        }

        public void CancelEdit(TableItem item)
        {
            if (DataGrid != null)
            {
                DataGrid.CancelEditRow(item);
            }
        }

        public async Task SaveRow(TableItem item)
        {
            if (DataGrid != null)
            {
                await DataGrid.UpdateRow(item);
                if (OnSave.HasDelegate)
                {
                    await OnSave.InvokeAsync(item);
                }
            }
        }

        public async Task DeleteRow(TableItem item)
        {
            if (DataGrid != null && DeleteAction != null)
            {
                await DeleteAction.Invoke(item);
                await DataGrid.Reload();
            }
        }

        public async Task DuplicateRow(TableItem item)
        {
            if (DataGrid != null)
            {
                // Create a new instance with the same properties
                var newItem = Activator.CreateInstance<TableItem>();
                foreach (var prop in typeof(TableItem).GetProperties())
                {
                    if (prop.CanWrite && prop.CanRead)
                    {
                        prop.SetValue(newItem, prop.GetValue(item));
                    }
                }

                // Clear the primary key if it exists
                if (!string.IsNullOrEmpty(Key))
                {
                    var keyProp = typeof(TableItem).GetProperty(Key);
                    if (keyProp != null)
                    {
                        keyProp.SetValue(newItem, default);
                    }
                }

                await DataGrid.InsertRow(newItem);
            }
        }

        public async Task OnRowClick(DataGridRowMouseEventArgs<TableItem> args)
        {
            var item = args.Data;

            if (RowExecute != null)
            {
                RowExecute(item);
            }

            if (HasSelect.HasValue && HasSelect.Value)
            {
                selectedItem = item;
                await InvokeAsync(StateHasChanged);
            }
        }

        public void Dispose()
        {
            // Cleanup code
        }

        // Helper method to get property name
        public string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("Expression is not a member expression", nameof(expression));
            }
            return memberExpression.Member.Name;
        }
    }
}
