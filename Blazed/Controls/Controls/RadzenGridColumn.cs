using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Controls
{
    public partial class RadzenGridColumn<TableItem> : RadzenDataGridColumn<TableItem> where TableItem : class, new()
    {
        [CascadingParameter]
        public RadzenResponsiveTable<TableItem> Grid { get; set; }

        [Parameter]
        public string Field { get; set; }

        [Parameter]
        public string CSScol { get; set; }

        [Parameter]
        public bool IsToolTip { get; set; }

        [Parameter]
        public string ToolTipField { get; set; }

        [Parameter]
        public Expression<Func<TableItem, bool>> VisibleExpression { get; set; }

        [Parameter]
        public string ControlType { get; set; }

        [Parameter]
        public string Format {
            get => FormatString;
            set => FormatString = value;
        }

        protected override Task OnInitializedAsync()
        {
            // If Field is provided but Property is not, use Field for Property
            if (string.IsNullOrEmpty(Property) && !string.IsNullOrEmpty(Field))
            {
                Property = Field;
            }

            // If CSScol is provided but Width is not, extract width from CSScol
            if (!string.IsNullOrEmpty(CSScol) && Width == "auto")
            {
                // Try to extract width from CSScol if it contains col-sm-X
                if (CSScol.Contains("col-sm-"))
                {
                    var parts = CSScol.Split(' ');
                    foreach (var part in parts)
                    {
                        if (part.StartsWith("col-sm-"))
                        {
                            var colWidth = part.Replace("col-sm-", "");
                            if (int.TryParse(colWidth, out int width))
                            {
                                // Convert Bootstrap column width to percentage
                                Width = $"{width * 8.33}%";
                                break;
                            }
                        }
                    }
                }
            }

            // Add this column to the Grid's columns collection
            Grid.Columns.Add(this);

            return base.OnInitializedAsync();
        }
    }
}
