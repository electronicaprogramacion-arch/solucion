using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.Controls
{
    public static partial class  ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazorDragDrop(this IServiceCollection services)
        {
            return services.AddScoped(typeof(DragDropService<>));
        }
    }
}
