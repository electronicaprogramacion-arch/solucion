using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazed.Controls.Toast
{
    public  static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddToast(this IServiceCollection services)
        {

            return services.AddScoped<IToastService, ToastService>();
            //return services.AddScoped(typeof(ToastService));
        }
    }
}
