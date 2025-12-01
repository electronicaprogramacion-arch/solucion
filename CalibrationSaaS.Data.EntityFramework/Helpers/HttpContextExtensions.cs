using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Helpers
{
    public static class HttpContextExtensions
    {

        public static async Task InsertPametersPage<T>(this HttpContext context,
           IQueryable<T> querable, int countReg)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            double conteo = await querable.CountAsync();
            double pageTotal = Math.Ceiling(conteo / countReg);
            context.Response.Headers.Add("pageTotal", pageTotal.ToString());
        }
    }
}
