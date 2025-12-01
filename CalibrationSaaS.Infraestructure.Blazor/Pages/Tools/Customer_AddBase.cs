using Blazor.IndexedDB.Framework;
using Bogus;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Infraestructure.Blazor.Shared;
using Grpc.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blazed.Controls.Toast;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Tools
{
    public class Customer_AddBase : ComponentBase
    {
        //[Inject] Application.Services.ISampleService<CallContext> Client { get; set; }

         [Inject] public IToastService toastService { get; set; }

        [Inject] public ILogger<Customer_AddBase> Logger { get; set; }

        protected Domain.Aggregates.Entities.Customer customer ;
        protected Domain.Aggregates.Entities.CustomerAddress customerAddress = new Domain.Aggregates.Entities.CustomerAddress();
        private string Error;
        //demo
        [Inject] System.Net.Http.HttpClient Http { get; set; }
        

        [Inject] IIndexedDbFactory DbFactory { get; set; }

        [Inject]
        Func<dynamic, Application.Services.ISampleService<CallContext>> Client { get; set; }

        protected StandardModal child;
        protected async Task CreateCustomer()
        {

            //Console.WriteLine(customer.Name);
            //Console.WriteLine(customer.Description);
            //Console.WriteLine(customer.CustomerID);

            return;
            //https://docs.microsoft.com/en-us/dotnet/architecture/grpc-for-wcf-developers/error-handling
            try
            {
                var result = await Client(DbFactory).CreateCustomer(this.customer, new CallOptions());
                child.ShowResult("The customer was created: " + this.customer.Name, "Customer Id: " + result.CustomerID, "OrderCreate/" + result.CustomerID, "Create Work Order");
            }
            catch (RpcException rpcException) when (rpcException.StatusCode == StatusCode.Cancelled)
            {
                // Ignore exception from cancellation
                Error = rpcException.Message;
            }
            catch (RpcException rpcException) when (rpcException.StatusCode == StatusCode.Unauthenticated)
            {
                Error = rpcException.Message;
            }
            catch (RpcException rpcException) when (rpcException.StatusCode == StatusCode.AlreadyExists)
            {
                Error = rpcException.Message;
            }
            catch (System.Exception exception)
            {
                Error = exception.Message;
            }

            if (!string.IsNullOrEmpty(Error))
            {
                //Logger.LogError(Error);
                toastService.ShowError(Error);
            }
        }

        private async Task AddItem(string name)
        {
            //https://docs.microsoft.com/en-us/aspnet/core/blazor/call-web-api?view=aspnetcore-3.1#:~:text=Blazor%20WebAssembly%20apps%20call%20web,to%20the%20server%20of%20origin.
            var addItem = new { name = name };
            var content = new System.Net.Http.FormUrlEncodedContent(new[]
            {
                new System.Collections.Generic.KeyValuePair<string, string>("name", name)
            });
            Http.BaseAddress = new System.Uri("https://localhost:44340");
            await Http.PostAsync("/Home/AddCustomer", content);
            //await Http.PostAsJsonAsync("https://localhost:44340/Home/AddCustomer", addItem);
        }

        public Faker<Domain.Aggregates.Entities.Customer> exa = new Faker<Domain.Aggregates.Entities.Customer>();

        public async  Task<Domain.Aggregates.Entities.Customer> testasync()
        {
            customer = new Domain.Aggregates.Entities.Customer();
            customer.Name = "este";
            await Task.Delay(100);
            return customer;
        }


        public bool FirstGenerate;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            //exa.CustomInstantiatorAsync(f => testasync())
            //    .RuleFor(x => x.Name, notesexample)
            //    .RuleFor2(x => x.Description, notesexample2x,x=>x.Name.Contains("este"))

            //    ;


            exa.CustomInstantiatorAsync(f => testasync())
               .RuleSet("test",
               (set) =>
               {
                   set.StrictMode(true);
                   set.RuleFor(x => x.Name, notesexample);
                   set.RuleFor2(x => x.Description, notesexample2x, x => x.Name.Contains("este"),notesexample2x);
               });
                


            //.RuleFor(x => x.CustomerID,"notesexample3", notesexample3, notesexample4);

            customer = await exa.GenerateAsync("test");

            FirstGenerate = true;

        }

        async Task<ViewProperty<dynamic>> notesexample(Faker f, Domain.Aggregates.Entities.Customer poe)
        {

            ViewProperty<dynamic> viewPropertyt = new ViewProperty<dynamic>();

            viewPropertyt.Value = poe.Name + "xxxx";

            viewPropertyt.ControlType = Types.text;

            await Task.Delay(1000);
            //poe.Description = "hola mundo" + poe.Name;

            return viewPropertyt;


        }

        async Task<ViewProperty<string>> notesexample2x(Faker f, Domain.Aggregates.Entities.Customer poe)
        {

            ViewProperty<string> viewPropertyt = new ViewProperty<string>();

            viewPropertyt.ControlType = Types.text;

            viewPropertyt.ReGenerate = true;

            viewPropertyt.Display = "Description";

            if (poe.Description == "testy")
            {
                viewPropertyt.IsValid = false;

                viewPropertyt.ErrorMesage = "error testy";

                poe.Description = "";

                viewPropertyt.Value = poe.Description;

            }
            if( string.IsNullOrEmpty(poe.Description) )
            {
                viewPropertyt.Value = "cbbcbcbcb";
            }
            else
            {
                viewPropertyt.Value = poe.Description;
            }
            

            return viewPropertyt;


        }

        async Task<ViewProperty<int>> notesexample3(Faker f, Domain.Aggregates.Entities.Customer poe)
        {

            ViewProperty<int> viewPropertyt = new ViewProperty<int>();

            viewPropertyt.ControlType = Types.select;

            viewPropertyt.ReGenerate = true;

            viewPropertyt.Display = "Description";

            Dictionary<int, string> dic = new Dictionary<int, string>();

            dic.Add(1, 1.ToString());
            dic.Add(2, 2.ToString());
            dic.Add(3, 3.ToString());

            viewPropertyt.Options = dic;
            if (poe.CustomerID.SelectSingle(RuleTestPrperty()))
            {
                viewPropertyt.Value = poe.CustomerID;
            }
            

            poe.SelectSingle(RuleTest());

            return viewPropertyt;


        }

         async Task<ViewProperty<int>> notesexample4(Faker f, Domain.Aggregates.Entities.Customer poe)
        {

            ViewProperty<int> viewPropertyt = new ViewProperty<int>();

            viewPropertyt.ControlType = Types.select;

            viewPropertyt.ReGenerate = true;

            viewPropertyt.Display = "Description";

            Dictionary<int, string> dic = new Dictionary<int, string>();

            dic.Add(1, 1.ToString());
            dic.Add(2, 2.ToString());
            dic.Add(3, 3.ToString());

            viewPropertyt.Options = dic;
            if (poe.CustomerID.SelectSingle(RuleTestPrperty()))
            {
                viewPropertyt.Value = poe.CustomerID;
            }


            poe.SelectSingle(RuleTest());

            return viewPropertyt;


        }

        public static Expression<Func<Domain.Aggregates.Entities.Customer, bool>> RuleTest()
        {
            Expression<Func<Domain.Aggregates.Entities.Customer, bool>> exprTree = x => x.CustomerID == 2;
            return exprTree;
        }


        public static Expression<Func<int, bool>> RuleTestPrperty()
        {
            Expression<Func<int, bool>> exprTree = x => x.Equals(2);
            return exprTree;
        }

        public static Expression<Func<int, bool>> RuleTestPrpertyx()
        {
            Expression<Func<int, bool>> exprTree = x => x.Equals(2);
            return exprTree;
        }

        //public static RuleTestProperty<T>(Expression<Func<Func<Faker, T, ViewProperty<TProperty>>, bool>> prop)
        //    {
        //        Expression<Func<int, bool>> exprTree = x => x.Equals(2);
        //        return null;
        //    }

       public static  Func<Faker, T, ViewProperty<TProperty>> RuleTestProperty2<T,TProperty>(params Func<Faker, T, ViewProperty<TProperty>>[] prop)
       {
            return null;
       }



    }
}
