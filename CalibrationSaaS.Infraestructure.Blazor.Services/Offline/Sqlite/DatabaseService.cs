using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite
{
    public class DatabaseService<T>
        where T : DbContext
    {
//#if RELEASE
        public static string FileName = "/database/app.db";
        private readonly IDbContextFactory<T> _dbContextFactory;
        private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
//#endif



//#if DEBUG
//        public DatabaseService()
//        {
//        }
//#else
        public DatabaseService(IJSRuntime jsRuntime
            , IDbContextFactory<T> dbContextFactory)
        {
            if (jsRuntime == null) throw new ArgumentNullException(nameof(jsRuntime));
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));

            _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./js/file5.js").AsTask());
        }
//#endif

        public async Task InitDatabaseAsync()
        {
            try
            {
//#if RELEASE
                var module = await _moduleTask.Value;
                await module.InvokeVoidAsync("mountAndInitializeDb");
                if (!File.Exists(FileName))
                {
                    File.Create(FileName).Close();
                }
                //Console.WriteLine("ini database");
                await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
                await dbContext.Database.EnsureCreatedAsync();
                //Console.WriteLine("end database");
//#endif
            }
            catch (Exception ex)
            {
                if(ex.InnerException != null)
                {
                     //Console.WriteLine(ex.GetType().Name, ex.InnerException.Message);
                }
                //Console.WriteLine(ex.GetType().Name, ex.Message);
                //Console.WriteLine("InitDatabaseAsync " + ex.Message);
            }
        }


        public async Task InitDatabaseAsync8()
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    //Console.WriteLine(ex.GetType().Name, ex.InnerException.Message);
                }
                //Console.WriteLine(ex.GetType().Name, ex.Message);
                //Console.WriteLine("InitDatabaseAsync " + ex.Message);
            }
        }

        public async Task InitDatabaseAsync2()
        {
            try
            {
//#if RELEASE
                var module = await _moduleTask.Value;
                //await module.InvokeVoidAsync("mountAndInitializeDb");
                if (!File.Exists(FileName))
                {
                    File.Create(FileName).Close();
                }
                //Console.WriteLine("ini database");
                await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
                await dbContext.Database.EnsureCreatedAsync();
                //Console.WriteLine("end database");
//#endif
            }
            catch (Exception ex)
            {
                if(ex.InnerException != null)
                {
                     //Console.WriteLine(ex.GetType().Name, ex.InnerException.Message);
                }
                //Console.WriteLine(ex.GetType().Name, ex.Message);
                //Console.WriteLine("InitDatabaseAsync " + ex.Message);
            }
        }

         public async Task deleteDatabaseAsync()
        {
            try
            {
//#if RELEASE
                var module = await _moduleTask.Value;
                await module.InvokeVoidAsync("deleteDb");
                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                }
                //Console.WriteLine("ini database");
               
                //Console.WriteLine("end database");
//#endif
            }
            catch (Exception ex)
            {
                if(ex.InnerException != null)
                {
                     //Console.WriteLine(ex.GetType().Name, ex.InnerException.Message);
                }
                //Console.WriteLine(ex.GetType().Name, ex.Message);
                //Console.WriteLine("deleteDatabaseAsync " + ex.Message);
            }
        }
    }
}
