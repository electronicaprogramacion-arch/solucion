using Blazed.Controls.Route.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazed.Controls
{
    public  static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBrowserService(this IServiceCollection services)
        {
            return services.AddScoped<BrowserService>();
            //return services.AddScoped<IToastService, ToastService>();
            //return services.AddScoped(typeof(ToastService));
        }

         public static IServiceCollection AddRouter(this IServiceCollection services)
        {
            return services.AddScoped<RouterSessionService>();
            //return services.AddScoped<IToastService, ToastService>();
            //return services.AddScoped(typeof(ToastService));
        }





    }
}
