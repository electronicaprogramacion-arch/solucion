using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.BusinessExceptions;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Models.ViewModels;
using Helpers;
using Helpers.Controls.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.UseCases
{
    public class CustomerUseCases
    {
        private readonly ICustomerRepository customerRepository;



        public CustomerUseCases(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;

        }

        public List<Address> DefaultAddress()
        {
            List<CalibrationSaaS.Domain.Aggregates.Entities.Address> lstad = new List<CalibrationSaaS.Domain.Aggregates.Entities.Address>();
            CalibrationSaaS.Domain.Aggregates.Entities.Address add = new CalibrationSaaS.Domain.Aggregates.Entities.Address();
            add.AddressId = NumericExtensions.GetUniqueID(add.AddressId);
            add.StreetAddress1 = "NO ADDRESS";
            add.City = "NO CITY";
            add.CityID = "NO CITY";
            add.StateID = "NO STATE";
            add.ZipCode = "00000";
            add.Country = "USA";
            add.CountryID = "COUNTRY";

            lstad.Add(add);

            return lstad;
        }

        public List<Contact> DefaultContact()
        {
            List<CalibrationSaaS.Domain.Aggregates.Entities.Contact> lstcont = new List<CalibrationSaaS.Domain.Aggregates.Entities.Contact>();
            CalibrationSaaS.Domain.Aggregates.Entities.Contact add = new CalibrationSaaS.Domain.Aggregates.Entities.Contact();
            add.ContactID = NumericExtensions.GetUniqueID(add.ContactID);
            add.LastName = "NO CONTACT";
            add.Name = "NO CONTACT";
            add.CellPhoneNumber = "000000";
            add.Email = "NOEMAIL@email.com";
            add.PhoneNumber = "000000";

            lstcont.Add(add);

            return lstcont;
        }


        //A DTO is also a Value Object, however a DTO has an integrity meaning you should not modify its properties while in a Value Object it is accepted.
        public async Task<Customer> CreateCustomer(Customer customerDTO, bool isApi = false)
        {

            Customer doesCustomerExist = null;

            //if (isApi)
            //{

            //    doesCustomerExist = await customerRepository.GetCustomerByName("", customerDTO.CustomID);


            //}

            doesCustomerExist = await this.customerRepository.GetCustomerByID(customerDTO);



            if (doesCustomerExist == null && isApi)
            {
                bool find = false;
                doesCustomerExist = await customerRepository.GetCustomerByName(customerDTO.Name, customerDTO.CustomID);
                if (doesCustomerExist == null)
                {
                    doesCustomerExist = await customerRepository.GetCustomerByName("", customerDTO.CustomID);

                    if (doesCustomerExist != null)
                    {
                        customerDTO.CustomerID = doesCustomerExist.CustomerID;



                    }
                    doesCustomerExist = await customerRepository.GetCustomerByName(customerDTO.Name, "");
                    if (doesCustomerExist != null && string.IsNullOrEmpty(doesCustomerExist.CustomID))
                    {
                        //throw new ExistingException("This Customer is already in");
                        customerDTO.CustomerID = doesCustomerExist.CustomerID;

                    }
                }
                else
                {
                    customerDTO.CustomerID = doesCustomerExist.CustomerID;

                }


                if (doesCustomerExist != null && customerDTO.CustomerID > 0
                    && (customerDTO?.Aggregates?.FirstOrDefault()?.Addresses == null || customerDTO?.Aggregates?.FirstOrDefault()?.Addresses?.Count == 0))
                {
                    customerDTO.Aggregates.FirstOrDefault().Addresses = DefaultAddress();
                    customerDTO.Aggregates.FirstOrDefault().Contacts = DefaultContact();

                    var addff = await this.customerRepository.GetCustomerByID(customerDTO);

                    if (addff?.Aggregates?.FirstOrDefault()?.Addresses != null && addff?.Aggregates?.FirstOrDefault()?.Addresses?.Count > 0)
                    {
                        customerDTO.Aggregates.FirstOrDefault().Addresses = addff?.Aggregates?.FirstOrDefault()?.Addresses;
                    }
                    if (addff?.Aggregates?.FirstOrDefault()?.Contacts != null && addff?.Aggregates?.FirstOrDefault()?.Contacts?.Count > 0)
                    {
                        customerDTO.Aggregates.FirstOrDefault().Contacts = addff?.Aggregates?.FirstOrDefault()?.Contacts;
                    }
                    if (addff?.Aggregates?.FirstOrDefault()?.PhoneNumbers != null && addff?.Aggregates?.FirstOrDefault()?.PhoneNumbers?.Count > 0)
                    {
                        customerDTO.Aggregates.FirstOrDefault().PhoneNumbers = addff?.Aggregates?.FirstOrDefault()?.PhoneNumbers;
                    }
                    if (addff?.Aggregates?.FirstOrDefault()?.Socials != null && addff?.Aggregates?.FirstOrDefault()?.Socials?.Count > 0)
                    {
                        customerDTO.Aggregates.FirstOrDefault().Socials = addff?.Aggregates?.FirstOrDefault()?.Socials;
                    }

                }

                if ( (customerDTO.CustomerID > 0 && !isApi) && customerDTO?.Aggregates?.Count > 0 || (customerDTO.CustomerID == 0 && isApi) &&  customerDTO?.Aggregates?.Count > 0)
                {
                    if (customerDTO?.Aggregates?.FirstOrDefault()?.Addresses == null|| customerDTO?.Aggregates?.FirstOrDefault()?.Addresses?.Count ==0)
                    {
                        customerDTO.Aggregates.FirstOrDefault().Addresses = DefaultAddress();
                    }

                    if (customerDTO?.Aggregates?.FirstOrDefault()?.Contacts == null || customerDTO?.Aggregates?.FirstOrDefault()?.Contacts?.Count == 0)
                    {
                        customerDTO.Aggregates.FirstOrDefault().Contacts = DefaultContact();
                    }


                    return await this.customerRepository.InsertCustomer(customerDTO,isApi);
                }
                else
                {
                    if (customerDTO?.Aggregates?.FirstOrDefault()?.Addresses == null || customerDTO?.Aggregates?.FirstOrDefault()?.Addresses?.Count == 0)
                    {
                        customerDTO.Aggregates.FirstOrDefault().Addresses = DefaultAddress();
                    }

                    if (customerDTO?.Aggregates?.FirstOrDefault()?.Contacts == null || customerDTO?.Aggregates?.FirstOrDefault()?.Contacts?.Count == 0)
                    {
                        customerDTO.Aggregates.FirstOrDefault().Contacts = DefaultContact();
                    }

                    return await this.customerRepository.InsertCustomer(customerDTO,isApi);
                }

            }

            if (doesCustomerExist == null && !string.IsNullOrEmpty(customerDTO.CustomID))
            {
                doesCustomerExist = await customerRepository.GetCustomerByName(customerDTO.Name, customerDTO.CustomID);
            }

            if (doesCustomerExist == null)
            {

                if (customerDTO.Aggregates != null)
                {

                    var msg = await customerRepository.ValidateCustomer(customerDTO);



                    if (msg != "")
                    {
                        throw new ExistingException(msg);

                    }



                }




            }
            else
            {


                customerDTO.CustomerID = doesCustomerExist.CustomerID;

            }



            customerDTO = await this.customerRepository.InsertCustomer(customerDTO,isApi);


            return customerDTO;
        }

        public async Task<ResultSet<Customer>> GetCustomer(Pagination<Customer> Pagination)
        {
            return await customerRepository.GetCustomers(Pagination);
        }

        public async Task<Customer> DeleteCustomer(Customer customerDTO)
        {
            var result = await customerRepository.DeleteCustomer(customerDTO);
            return result;
        }

        public async Task<Contact> DeleteContact(Contact DTO)
        {
            var result = await customerRepository.DeleteContact(DTO);
            return result;
        }



        public async Task<Address> DeleteAddress(Address DTO)
        {
            var result = await customerRepository.DeleteAddress(DTO);
            return result;
        }

        public async Task<PhoneNumber> DeletePhoneNumber(PhoneNumber DTO)
        {
            var result = await customerRepository.DeletePhone(DTO);
            return result;
        }
        public async Task<Social> DeleteSocial(Social DTO)
        {
            var result = await customerRepository.DeleteSocial(DTO);
            return result;
        }

        public async Task<Customer> GetCustomerById(Customer id)
        {
            var result = await customerRepository.GetCustomerByID(id);
            return result;
        }

        public async Task<Customer> GetCustomerByName(string Name, string customname = null)
        {
            var result = await customerRepository.GetCustomerByName(Name, customname);
            return result;
        }

        public async Task<Customer> UpdateCustomer(Customer customerDTO)
        {
            await customerRepository.UpdateCustomer(customerDTO);

            await this.customerRepository.Save();
            return customerDTO;
        }
        public async Task<ICollection<Address>> GetAddressesAsync()
        {
            var a = await customerRepository.GetAddressesAsync();


            return a;

        }

        public async Task<Address> GetAddressesByIDAsync(Address Address)
        {
            var a = await customerRepository.GetAddressesByIDAsync(Address);


            return a;

        }

        public async Task<ResultSet<Address>> GetAddress(Pagination<Address> Pagination)
        {
            return await customerRepository.GetAddress(Pagination);
        }

        public async Task<ResultSet<AddressCustomerViewModel>> GetAddressCustomer(Pagination<AddressCustomerViewModel> Pagination)
        {
            return await customerRepository.GetAddressCustomer(Pagination);
        }

        public async Task<ResultSet<AddressCustomerViewModel>> GetAddressCustomerOptimized(Pagination<AddressCustomerViewModel> Pagination)
        {
            return await customerRepository.GetAddressCustomerOptimized(Pagination);
        }

        public async Task<Customer> ReplaceCustomer(CustomerReplaced DTO)
        {
            var a = await customerRepository.ReplaceCustomer(DTO);

            return a;
        }
            
        public async Task<IEnumerable<Contact>> GetContactsByCustomID(Customer customer)
        {
            var result = await customerRepository.GetContactsByCustomID(customer);
            return result;
        }

        public async Task<Contact> GetContactByEmail(string email)
        {
            var result = await customerRepository.GetContactByEmail(email);
            return result;
        }
    }
}
