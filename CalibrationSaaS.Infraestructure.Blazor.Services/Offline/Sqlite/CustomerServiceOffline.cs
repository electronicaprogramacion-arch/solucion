using IndexedDB.Blazor;

using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using ProtoBuf.Grpc;
using System;
using System.Linq;
//using Blazored.LocalStorage;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CalibrationSaaS.Infraestructure.EntityFramework.Helpers;
using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Infraestructure.EntityFramework.DataAccess;
using Helpers.Controls.ValueObjects;
using SqliteWasmHelper;
using CalibrationSaaS.Models.ViewModels;

namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite
{
    public class CustomerServiceOffline<TContext> : Application.Services.ICustomerService<CallContext>
        where TContext:DbContext, ICalibrationSaaSDBContextBase, ICalibrationSaaSDBContextBaseOff
    {
        //private readonly ILocalStorageService localStorageService;

        //public SampleServiceOffline(ILocalStorageService localStorageService)

        private readonly ISqliteWasmDbContextFactory<TContext> DbFactory;

        public CustomerServiceOffline(ISqliteWasmDbContextFactory<TContext> dbFactory)
        {
            //this.localStorageService = localStorageService;
            this.DbFactory = dbFactory;
        }

        public ValueTask<Customer> CreateCustomer(Customer customerDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Address> DeleteAddress(Address DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Contact> DeleteContact(Contact DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Customer> DeleteCustomer(Customer customerDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<PhoneNumber> DeletePhone(PhoneNumber DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Social> DeleteSocial(Social DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<Address>> GetAddress(Pagination<Address> Pagination, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<AddressCustomerViewModel>> GetAddressCustomer(Pagination<AddressCustomerViewModel> Pagination, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<AddressCustomerViewModel>> GetAddressCustomerOptimized(Pagination<AddressCustomerViewModel> Pagination, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ICollection<Address>> GetAddressesAsync(CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<Address> GetAddressesByIDAsync(Address Address, CallContext context)
        {
            ICustomerRepository wod = new CustomerRepositoryEF<TContext>(DbFactory);

            return await wod.GetAddressesByIDAsync(Address);
            
        }

        public async ValueTask<IEnumerable<Contact>> GetContactsByCustomID(string customID, Pagination<Contact> pagination, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<IEnumerable<Contact>> GetContactsByCustomID(Customer customer, Pagination<Contact> pagination, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<Contact>> GetContactsByCustomID(Customer customer, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<ResultSet<Customer>> GetCustomers(Pagination<Customer> Pagination, CallContext context)
        {
            // var DTO = Pagination.Entity;

            //var filterQuery = Querys.CustomerFilter(Pagination.Filter);

            //using (var db = await this.DbFactory.CreateDbContextAsync())
            //{
            //    var queriable = db.Customer.AsQueryable();
            //    //context.PieceOfEquipment.AsNoTracking().Include(x => x.WeightSets).ThenInclude(x => x.UnitOfMeasure)
            //    //.Include(x => x.UnitOfMeasure).OrderBy(c => c.DueDate).AsQueryable();

            //    var simplequery = db.Customer.AsQueryable(); ;

            //    //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
            //    var result = await queriable.PaginationAndFilterQuery<Customer>(Pagination, simplequery, filterQuery);

            //    return result;
            //}
            ICustomerRepository wod = new CustomerRepositoryEF<TContext>(DbFactory);

            return await wod.GetCustomers(Pagination);



        }

        public async ValueTask<Customer> GetCustomersByID(Customer customerDTO, CallContext context)
        {
            ICustomerRepository wod = new CustomerRepositoryEF<TContext>(DbFactory);

            return await wod.GetCustomerByID(customerDTO);
        }

        public ValueTask<Customer> ReplaceCustomer(CustomerReplaced DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Customer> UpdateCustomer(Customer customerDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Contact> GetContactByEmail(string email, CallContext context)
        {
            throw new NotImplementedException();
        }
    }
}
