using Microsoft.AspNetCore.Components;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blazed.Controls
{

    public class GridColumnBase : ComponentBase 
    {

        [Parameter]
        public string Key { get; set; }


        [Parameter]
        public string Field { get; set; }

        [Parameter]
        public string Title { get; set; }


        [Parameter]
        public string CSScol { get; set; } = "col-sm customcol text-truncate";

        [Parameter]
        public bool DefaultSort { get; set; } = false;


        [Parameter]
        public bool HasSorting { get; set; } = true;

        [Parameter]
        public object Value { get; set; }



        [Parameter]
        public string Format { get; set; }


        [Parameter]
        public bool IsVisble { get; set; } = true;


        [Parameter]
        public bool IsToolTip { get; set; }

        [Parameter]
        public string ToolTipField { get; set; }

        [Parameter]
        public string ControlType { get; set; }

        [Parameter]
        public string Width { get; set; }

        //Field="@item.Key" Title="@item.Value.Title" CSScol="@item.Value.CSScol" DefaultSort=


    }


    public class GridColumn<TableItem> : GridColumnBase where TableItem : class, new()
    {


     

        public int PageNumber { get; set; }

        public string SortColumName { get; set; }

        [Parameter]
        public bool SortDescending
        {
            get;
            set;
        } = true;


        [Parameter]
        public RenderFragment<dynamic> Template { get; set; }


        [Parameter]
        public RenderFragment<dynamic> FormatFunnction { get; set; }

        [Parameter]
        public RenderFragment<dynamic> ResponsiveTemplate { get; set; }

        [CascadingParameter()]
        public ResponsiveTable<TableItem> Grid { get; set; }



        [Parameter]
        public Expression<Func<TableItem, bool>> VisibleExpression { get; set; }


        [Parameter]
        public Func<GridColumn<TableItem>,object,object, RenderFragment> CustomColumn { get; set; }

        public object? PropertyValue { get; set; }

        protected override Task OnInitializedAsync()
        {
            Grid.Columns.Add(this);
            return base.OnInitializedAsync();
        }
    }





}