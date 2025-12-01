using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazed.Controls
{
    public static partial class  ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazorDragDrop(this IServiceCollection services)
        {
            return services.AddScoped(typeof(DragDropService<>));
        }
    }
}
