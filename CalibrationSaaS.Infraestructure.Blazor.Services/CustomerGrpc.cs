using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Models.ViewModels;
using Grpc.Core;
using Helpers.Controls.ValueObjects;
using ProtoBuf.Grpc;
//using Reports.Domain.ReportViewModels;
using System;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Services
{
    public class CustomerGRPC : IDisposable, ICustomerService<CallContext>
    {

        private CallContext _context;

#pragma warning disable CS0169 // El campo 'CustomerGRPC.customer' nunca se usa
        private readonly ICustomerService<CallContext> customer;
#pragma warning restore CS0169 // El campo 'CustomerGRPC.customer' nunca se usa
        


        private readonly dynamic DbFactory;

        private readonly ICustomerService<CallContext> service;


        public CustomerGRPC(Func<dynamic, Application.Services.ICustomerService<CallContext>> _service, dynamic _DbFactory)
        {
            DbFactory = _DbFactory;
            service = _service(DbFactory);
            _context = new CallOptions();
        }


        public CustomerGRPC(Func<dynamic, Application.Services.ICustomerService<CallContext>> _service, dynamic _DbFactory, CallContext opt)
        {
            DbFactory = _DbFactory;
            service = _service(DbFactory);
            _context = opt;
        }


        public CustomerGRPC(ICustomerService<CallContext> _customer)
        {
            _context = new CallOptions();
            service = _customer;
        }

        //public ICustomerService<CallContext> Client2 { get; set; }

        public async ValueTask<Customer> CreateCustomer(Customer customerDTO, CallContext context = default)
        {
            //if (Client2 != null)
            //{
                var result = await service.CreateCustomer(customerDTO, _context);
                //var a = new Customer { Name = "custome", Description = "hhhhh" };

                return result;

            //}
            //else
            //{

            //    throw new Exception("Error");
            //}


        }

        public async ValueTask<Address> CreateAddress(Address addressDTO, CallContext context = default)
        {
            // Note: This would need to be implemented in the service layer
            // For now, returning the input as placeholder
            return addressDTO;
        }


        public void Dispose()
        {
            //Client2 = null;
        }

        public ValueTask<ResultSet<Customer>> GetCustomers(Pagination<Customer> Pagination, CallContext context = default)
        {
            if (service != null)
            {

                return service.GetCustomers(Pagination, _context);
            }
            else
            {
                return new ValueTask<ResultSet<Customer>>();

            }
        }

        public async ValueTask<Customer> GetCustomersByID(Customer customerDTO, CallContext context = default)
        {
            var result = await service.GetCustomersByID(customerDTO, _context);
            return result;
        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async ValueTask<Customer> DeleteCustomer(Customer customerDTO, CallContext context)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            var result = service.DeleteCustomer(customerDTO, _context);
            return customerDTO;
        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async ValueTask<Address> DeleteAddress(Address DTO, CallContext context)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            var result = service.DeleteAddress(DTO, _context);
            return DTO;
        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async ValueTask<Contact> DeleteContact(Contact DTO, CallContext context)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            var result = service.DeleteContact(DTO, _context);
            return DTO;
        }

        public async ValueTask<PhoneNumber> DeletePhone(PhoneNumber DTO, CallContext context = default)
        {
            var result = await service.DeletePhone(DTO, _context);
            return DTO;
        }
        public async ValueTask<Social> DeleteSocial(Social DTO, CallContext context = default)
        {
            var result = await service.DeleteSocial(DTO, _context);
            return DTO;
        }

        public async ValueTask<Customer> UpdateCustomer(Customer customerDTO, CallContext context)
        {
            var result = await service.UpdateCustomer(customerDTO, _context);
            return customerDTO;
        }


        public async ValueTask<ICollection<Address>> GetAddressesAsync(CallContext context = default)
        {
            var result = await service.GetAddressesAsync(_context);
            return result;
        }

        public async ValueTask<Address> GetAddressesByIDAsync(Address Address, CallContext context = default)
        {
            var result = await service.GetAddressesByIDAsync(Address, _context);
            return result;
        }

        public ValueTask<ResultSet<Address>> GetAddress(Pagination<Address> Pagination, CallContext context = default)
        {
            if (service != null)
            {
                return service.GetAddress(Pagination, _context);
            }
            else
            {
                return new ValueTask<ResultSet<Address>>();
            }
        }

        public ValueTask<ResultSet<AddressCustomerViewModel>> GetAddressCustomer(Pagination<AddressCustomerViewModel> Pagination, CallContext context = default)
        {
            if (service != null)
            {
                return service.GetAddressCustomer(Pagination, _context);
            }
            else
            {
                return new ValueTask<ResultSet<AddressCustomerViewModel>>();
            }
        }

        public ValueTask<ResultSet<AddressCustomerViewModel>> GetAddressCustomerOptimized(Pagination<AddressCustomerViewModel> Pagination, CallContext context = default)
        {
            if (service != null)
            {
                return service.GetAddressCustomerOptimized(Pagination, _context);
            }
            else
            {
                return new ValueTask<ResultSet<AddressCustomerViewModel>>();
            }
        }

        public async ValueTask<Domain.Aggregates.Entities.Customer> ReplaceCustomer(CustomerReplaced DTO, CallContext context)
        {
            var result = await service.ReplaceCustomer(DTO, _context);
            return result;
        }

        public async ValueTask<IEnumerable<Contact>> GetContactsByCustomID(Domain.Aggregates.Entities.Customer customer,  CallContext context = default)
        {
            var result = await service.GetContactsByCustomID(customer,  _context);
            return result;
        }

        public async ValueTask<Contact> GetContactByEmail(string email, CallContext context = default)
        {
            var result = await service.GetContactByEmail(email, _context);
            return result;
        }

    }
}
