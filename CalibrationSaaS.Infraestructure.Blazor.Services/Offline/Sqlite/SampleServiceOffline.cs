using IndexedDB.Blazor;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using ProtoBuf.Grpc;
using System;
using System.Linq;
//using Blazored.LocalStorage;
using System.Threading.Tasks;
using Helpers.Controls.ValueObjects;

namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite
{
    public class SampleServiceOffline : Application.Services.ISampleService<CallContext>
    {
        //private readonly ILocalStorageService localStorageService;

        //public SampleServiceOffline(ILocalStorageService localStorageService)

        private readonly IIndexedDbFactory DbFactory;

        public SampleServiceOffline(IIndexedDbFactory dbFactory)
        {
            //this.localStorageService = localStorageService;
            this.DbFactory = dbFactory;
        }


        public async ValueTask<Customer> CreateCustomer(Customer customerDTO, CallContext context)
        {

            //Console.WriteLine("CreateCustomer Offline");

            //await localStorageService.SetItemAsync(customerDTO.Name.ToString(), customerDTO);

            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                db.Customer.Add(customerDTO);
                await db.SaveChanges();
                customerDTO = db.Customer.FirstOrDefault();
            }

            return await Task.FromResult(customerDTO);
        }

        public async ValueTask<CustomerResultSet> GetCustomers(TenantDTO tenantID, CallContext context)
        {
            //Console.WriteLine("GetCustomers Offline");
            CustomerResultSet resultSet = new CustomerResultSet();

            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                return await Task.FromResult(new CustomerResultSet { Customers = db.Customer.ToList() });

            }
        }

      

        public ValueTask<ResultSet<WorkOrderDetail>> GetWOD(Pagination<WorkOrderDetail> pag, CallContext context)
        {
            throw new NotImplementedException();
        }
    }
}
