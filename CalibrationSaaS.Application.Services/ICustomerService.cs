using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Models.ViewModels;
using Helpers.Controls.ValueObjects;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.Services
{


    [ServiceContract(Name = "CalibrationSaaS.Application.Services.Customer")]
    public interface ICustomerService<T>
    {

        ValueTask<Address> GetAddressesByIDAsync(Address Address, T context);
        ValueTask<ICollection<Address>> GetAddressesAsync(T context);
        ValueTask<ResultSet<Address>> GetAddress(Pagination<Address> Pagination, T context);
        ValueTask<ResultSet<AddressCustomerViewModel>> GetAddressCustomer(Pagination<AddressCustomerViewModel> Pagination, T context);
        ValueTask<ResultSet<AddressCustomerViewModel>> GetAddressCustomerOptimized(Pagination<AddressCustomerViewModel> Pagination, T context);

        ValueTask<Customer> CreateCustomer(Customer customerDTO, T context);

        ValueTask<ResultSet<Customer>> GetCustomers(Pagination<Customer> Pagination,T context);
        //ValueTask<CustomerResultSet> GetCustomers();

        ValueTask<Customer> GetCustomersByID(Customer customerDTO, T context);

        ValueTask<Customer> DeleteCustomer(Customer customerDTO, T context);
        ValueTask<Address> DeleteAddress(Address DTO, T context);
        ValueTask<Contact> DeleteContact(Contact DTO, T context);
        ValueTask<PhoneNumber> DeletePhone(PhoneNumber DTO, T context);
        ValueTask<Social> DeleteSocial(Social DTO, T context);

        ValueTask<Customer> UpdateCustomer(Customer customerDTO, T context);

        ValueTask<Customer> ReplaceCustomer(CustomerReplaced DTO, T context);

        ValueTask<IEnumerable<Contact>> GetContactsByCustomID(Customer customer, T context);

        ValueTask<Contact> GetContactByEmail(string email, T context);


    }
}
