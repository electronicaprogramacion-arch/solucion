using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Models.ViewModels;
using Helpers.Controls.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Domain.Repositories
{
    public interface ICustomerRepository
    {
        Task<Address> GetAddressesByIDAsync(Address Address);
        Task<ICollection<Address>> GetAddressesAsync();


        #region "Customer"
        Task<Customer> InsertCustomer(Customer newCustomer,bool IsApi=false);

        Task<ResultSet<Customer>> GetCustomers(Pagination<Customer> Pagination);

        Task<Customer> GetCustomerByID(Customer id);

        Task<Customer> GetCustomerByName(string name,string customname=null);

        Task<Customer> DeleteCustomer(Customer customerDTO);

        Task<Customer> UpdateCustomer(Customer customerDTO, bool IsApi = false);
       
        Task<bool> Save();
        #endregion

        #region "User"
        void InsertUser(User userDTO);


        #endregion

        #region "Address"
        Task<ResultSet<Address>> GetAddress(Pagination<Address> Pagination);
        Task<ResultSet<AddressCustomerViewModel>> GetAddressCustomer(Pagination<AddressCustomerViewModel> Pagination);
        Task<ResultSet<AddressCustomerViewModel>> GetAddressCustomerOptimized(Pagination<AddressCustomerViewModel> Pagination);
        #endregion
        Task<string> ValidateCustomer(Customer customerDTO);

        Task<Address> DeleteAddress(Address DTO);
        Task<Contact> DeleteContact(Contact DTO);
        Task<PhoneNumber> DeletePhone(PhoneNumber DTO);
        Task<Social> DeleteSocial(Social DTO);

        Task<Address> InserAddress(Address DTO);

        Task Clear();

        Task<Customer> ReplaceCustomer(CustomerReplaced DTO);

        Task<IEnumerable<Contact>> GetContactsByCustomID(Customer customer);

        Task<Contact> GetContactByEmail(string email);
    }
}
