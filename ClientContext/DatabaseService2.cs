using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using SqliteWasmHelper;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite
{
    public class DatabaseService2<T>
        where T : DbContext
    {
        //#if RELEASE
        public static string FileName = "things.db";  //"things2.sqlite3";

        private readonly ISqliteWasmDbContextFactory<T> Factory;


        public async Task GetRoles()
        {
            try
            {
                
                await using var dbContext = await Factory.CreateDbContextAsync();
               
                //#endif
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
//                    Console.WriteLine(ex.GetType().Name, ex.InnerException.Message);
                }
//                Console.WriteLine(ex.GetType().Name, ex.Message);
//                Console.WriteLine("InitDatabaseAsync " + ex.Message);
            }
        }


        //#endif



        //#if DEBUG
        //        public DatabaseService()
        //        {
        //        }
        //#else
        public DatabaseService2(ISqliteWasmDbContextFactory<T> factory)
        {
            Factory = factory;
        }
//#endif

        public async Task InitDatabaseAsync()
        {
            try
            {
//#if RELEASE
                
                if(Factory != null)
                {
                    Console.WriteLine("ini database");
                    await using var dbContext = await Factory.CreateDbContextAsync();
                    await dbContext.Database.EnsureCreatedAsync();
                    //                Console.WriteLine("end database");
                }
                //                
                //#endif
            }
            catch (Exception ex)
            {
                if(ex.InnerException != null)
                {
//                     Console.WriteLine(ex.GetType().Name, ex.InnerException.Message);
                }
//                Console.WriteLine(ex.GetType().Name, ex.Message);
//                Console.WriteLine("InitDatabaseAsync " + ex.Message);
            }
        }

   

         public async Task deleteDatabaseAsync()
        {
            try
            {
//#if RELEASE
              
//#endif
            }
            catch (Exception ex)
            {
                if(ex.InnerException != null)
                {
//                     Console.WriteLine(ex.GetType().Name, ex.InnerException.Message);
                }
//                Console.WriteLine(ex.GetType().Name, ex.Message);
//                Console.WriteLine("deleteDatabaseAsync " + ex.Message);
            }
        }
    }
}
