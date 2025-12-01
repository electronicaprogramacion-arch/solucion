using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.EntityFramework.DataAccess
{
    public class RepositoryEF<TContext> where TContext : DbContext
    {
        //private readonly IDbContextFactory<TContext> DbFactory;
        public async Task SaveOffline(IDbContextFactory<TContext> DbFactory)
        {

            if (DbFactory == null)
            {
                return;
            }
            var ty1 = DbFactory.GetType();
            var ty2 =   typeof(SqliteWasmDbContextFactory<TContext>);

            if (ty1 == ty2)
            {
//                Console.WriteLine("MakeBackup");
                await using var context = await DbFactory.CreateDbContextAsync();

                var fac = DbFactory as ISqliteWasmDbContextFactory<TContext>;
                await fac.MakeBackup(context);

            }

        }


    }
}
