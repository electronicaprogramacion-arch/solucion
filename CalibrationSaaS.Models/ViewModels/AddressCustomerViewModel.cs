using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using CalibrationSaaS.Domain.Aggregates.Entities;

namespace CalibrationSaaS.Models.ViewModels
{
    [DataContract]
    public class AddressCustomerViewModel : IGeneric
    {
        // Address properties
        [DataMember(Order = 1)]
        public int AddressId { get; set; }

        [DataMember(Order = 2)]
        public string StreetAddress1 { get; set; }

        [DataMember(Order = 3)]
        public string StreetAddress2 { get; set; }

        [DataMember(Order = 4)]
        public string StreetAddress3 { get; set; }

        [DataMember(Order = 5)]
        public string CityID { get; set; }

        [DataMember(Order = 6)]
        public string City { get; set; }

        [DataMember(Order = 7)]
        public string StateID { get; set; }

        [DataMember(Order = 8)]
        public string State { get; set; }

        [DataMember(Order = 9)]
        public string ZipCode { get; set; }

        [DataMember(Order = 10)]
        public string CountryID { get; set; }

        [DataMember(Order = 11)]
        public string Country { get; set; }

        [DataMember(Order = 12)]
        public string Description { get; set; }

        [DataMember(Order = 13)]
        public int AggregateID { get; set; }

        [DataMember(Order = 14)]
        public bool IsDefault { get; set; }

        [DataMember(Order = 15)]
        public bool IsEnable { get; set; }

        [DataMember(Order = 16)]
        public string County { get; set; }

        [DataMember(Order = 17)]
        public bool IsDelete { get; set; }

        [DataMember(Order = 18)]
        public string Name { get; set; }

        // Customer properties
        [DataMember(Order = 19)]
        public int CustomerID { get; set; }

        [DataMember(Order = 20)]
        public string CustomerName { get; set; }

        [DataMember(Order = 21)]
        public string CustomID { get; set; }

        [DataMember(Order = 22)]
        public string CustomerDescription { get; set; }

        // Constructor
        public AddressCustomerViewModel()
        {
        }

        // Constructor from Address and Customer
        public AddressCustomerViewModel(Address address, Customer customer)
        {
            if (address != null)
            {
                AddressId = address.AddressId;
                StreetAddress1 = address.StreetAddress1;
                StreetAddress2 = address.StreetAddress2;
                StreetAddress3 = address.StreetAddress3;
                CityID = address.CityID;
                City = address.City;
                StateID = address.StateID;
                State = address.State;
                ZipCode = address.ZipCode;
                CountryID = address.CountryID;
                Country = address.Country;
                Description = address.Description;
                AggregateID = address.AggregateID;
                IsDefault = address.IsDefault;
                IsEnable = address.IsEnable;
                County = address.County;
                IsDelete = address.IsDelete;
                Name = address.Name;
            }

            if (customer != null)
            {
                CustomerID = customer.CustomerID;
                CustomerName = customer.Name;
                CustomID = customer.CustomID;
                CustomerDescription = customer.Description;
            }
        }

        // Method to convert back to Address
        public Address ToAddress()
        {
            return new Address
            {
                AddressId = this.AddressId,
                StreetAddress1 = this.StreetAddress1,
                StreetAddress2 = this.StreetAddress2,
                StreetAddress3 = this.StreetAddress3,
                CityID = this.CityID,
                City = this.City,
                StateID = this.StateID,
                State = this.State,
                ZipCode = this.ZipCode,
                CountryID = this.CountryID,
                Country = this.Country,
                Description = this.Description,
                AggregateID = this.AggregateID,
                IsDefault = this.IsDefault,
                IsEnable = this.IsEnable,
                County = this.County,
                IsDelete = this.IsDelete,
                Name = this.Name
            };
        }
    }
}
