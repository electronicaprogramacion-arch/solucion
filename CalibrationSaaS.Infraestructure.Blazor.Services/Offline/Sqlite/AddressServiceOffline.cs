using IndexedDB.Blazor;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
//using Blazored.LocalStorage;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CalibrationSaaS.Infraestructure.EntityFramework.Helpers;
using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Infraestructure.EntityFramework.DataAccess;
using SqliteWasmHelper;


namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite
{
    public class AddressServiceOffline<TContext> : Application.Services.IAddressServices
        where TContext:DbContext, ICalibrationSaaSDBContextBase, ICalibrationSaaSDBContextBaseOff
    {
        //private readonly ILocalStorageService localStorageService;

        //public SampleServiceOffline(ILocalStorageService localStorageService)

        private readonly ISqliteWasmDbContextFactory<TContext> DbFactory;

        public AddressServiceOffline(ISqliteWasmDbContextFactory<TContext> dbFactory)
        {
            //this.localStorageService = localStorageService;
            this.DbFactory = dbFactory;
        }

        public ValueTask<Address> Create(Address DTO)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Address> Delete(Address DTO)
        {
            throw new NotImplementedException();
        }

        public ValueTask<AddressResultSet> GetAll()
        {
            throw new NotImplementedException();
        }

        public ValueTask<AddressResultSet> GetAllByCustomer(Customer DTO)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Address> GetByCustomer(Customer DTO)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Address> GetByID(Address DTO)
        {
            throw new NotImplementedException();
        }

        public ValueTask<AddressResultSet> GetByType()
        {
            throw new NotImplementedException();
        }

        public ValueTask<PhoneNumbersResultSet> GetPhoneByCustomer(Customer DTO)
        {
            throw new NotImplementedException();
        }
    }
}
