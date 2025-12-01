using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.Controls
{
    public  static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBrowserService(this IServiceCollection services)
        {
            return services.AddScoped<BrowserService>();
            //return services.AddScoped<IToastService, ToastService>();
            //return services.AddScoped(typeof(ToastService));
        }





    }
}
