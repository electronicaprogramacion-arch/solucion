//using CalibrationSaaS.Data.EntityFramework;
//using CalibrationSaaS.Domain.Aggregates.Entities;
//using CalibrationSaaS.Domain.Repositories;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CalibrationSaaS.Infraestructure.EntityFramework.DataAccess
//{
//    public class CustomerAddressRepositoryEF : ICustomerAddressRepository, IDisposable
//    {
//        private CalibrationSaaSDBContext context;

//        public CustomerAddressRepositoryEF(CalibrationSaaSDBContext context)
//        {
//            this.context = context;
//        }

//        public async Task<CustomerAddress> DeleteCustomerAddress(int id, int tenat)
//        {
//            CustomerAddress customerAddress = new CustomerAddress();//await context.CustomerAddress.FirstOrDefaultAsync(sc => sc.TenantId == tenat && sc.CustomerAddressId == id);
//            //context.CustomerAddress.Remove(customerAddress);
//            return customerAddress;
//        }
      
//        //public async Task<CustomerAddress> GetCustomerAddressByID(int id, int tenat)
//        //{
//        //    return await context.CustomerAddress.FirstOrDefaultAsync(sc => sc.CustomerAddressId == id);
//        //}

//        //public async Task<IEnumerable<CustomerAddress>> GetCustomersAddress(int id)
//        //{
//        //    return await context.CustomerAddress.Where(sc => sc.CustomerId == id).ToListAsync();
//        //}

//        //public void InsertCustomerAddress(CustomerAddress newCustomerAddress)
//        //{
//        //    context.CustomerAddress.Add(newCustomerAddress);
//        //}

//        //public void UpdateCustomerAddress(CustomerAddress newCustomerAddress)
//        //{
//        //    context.Entry(newCustomerAddress).State = EntityState.Modified;
//        //}

//        public async Task<bool> Save()
//        {
//            return (await context.SaveChangesAsync()) > 0;
//        }

       

//        #region Dispose
//        private bool disposed = false;

//        protected virtual void Dispose(bool disposing)
//        {
//            if (!this.disposed)
//            {
//                if (disposing)
//                {
//                    context.Dispose();
//                }
//            }
//            this.disposed = true;
//        }

//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }
        
//        #endregion
//    }
//}
