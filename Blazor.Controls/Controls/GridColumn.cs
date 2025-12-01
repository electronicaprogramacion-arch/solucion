using Microsoft.AspNetCore.Components;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blazor.Controls
{
    public class GridColumn<TableItem> : ComponentBase where TableItem : class, new()
    {

        public int PageNumber { get; set; }

        public string SortColumName { get; set; }

        public bool SortDescending
        {
            get;
            set;
        } = true;

        [Parameter]
        public string CSScol { get; set; } = "col-sm customcol text-truncate";

        [Parameter]
        public bool DefaultSort { get; set; } = false;

        [Parameter]
        public bool HasSorting { get; set; } = true;

        [Parameter]
        public object Value { get; set; }

        [Parameter]
        public string Field { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Format { get; set; }

        [Parameter]
        public RenderFragment<dynamic> Template { get; set; }

        [Parameter]
        public RenderFragment<dynamic> ResponsiveTemplate { get; set; }

        [CascadingParameter()]
        public ResponsiveTable<TableItem> Grid { get; set; }

        [Parameter]
        public bool IsVisble { get; set; } = true;


        [Parameter]
        public bool IsToolTip { get; set; }

        [Parameter]
        public string ToolTipField { get; set; }

        [Parameter]
        public Expression<Func<TableItem, bool>> VisibleExpression { get; set; }

        [Parameter]
        public string ControlType { get; set; }

        protected override Task OnInitializedAsync()
        {
            Grid.Columns.Add(this);
            return base.OnInitializedAsync();
        }
    }
}