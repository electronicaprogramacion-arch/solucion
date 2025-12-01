using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.GrpcServices.Services
{
    public class AddressServices : IAddressServices
    {
        public AddressServices()
        {
        }

        public async  ValueTask<Address> Create(Address DTO)
        {
            return DTO;
        }

        public async ValueTask<Address> Delete(Address userDTO)
        {
            return userDTO;
        }





        public async  ValueTask<AddressResultSet> GetAll()
        {
            List<Address> result = new List<Address>
            {
                new Address
                {
                   AddressId = 1,
                   StreetAddress1 = "Anaheim West Tower 201 S. Anaheim Blvd",
                   City = "Anaheim",
                   State = "California",
                   ZipCode = "92800",

                },

                new Address
                {
                   AddressId = 2,
                   StreetAddress1 = "201 E Broadway, Anaheim",
                   City = "Anaheim",
                   State = "California",
                   ZipCode = "92899"
                },
                new Address
                {
                   AddressId = 3,
                   StreetAddress1 = "1313 Dr, Anaheim",
                   City = "Anaheim",
                   State = "California",
                   ZipCode = "92802"

                }
            };

            return new AddressResultSet { Addresses = result };
        }



        public async ValueTask<PhoneNumbersResultSet> GetPhoneByCustomer(Customer DTO)
        {
            List<PhoneNumber> result = new List<PhoneNumber>
            {
                new PhoneNumber
                {
                   PhoneNumberID=1,
                    CountryID="1",
                     Number="221233344",
                     TypeID="1",

                },

                new PhoneNumber
                {
                   PhoneNumberID=2,
                    CountryID="1",
                     Number="32124563344",
                     TypeID="1",
                },
                new PhoneNumber
                {
                  PhoneNumberID=3,
                    CountryID="1",
                     Number="32133456",
                     TypeID="1",

                }
            };

            return new PhoneNumbersResultSet { PhoneNumbers = result };
        }





        public ValueTask<Address> GetByCustomer(Customer DTO)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<Address> GetByID(Address DTO)
        {
            return  new Address()
            {
                AddressId = 1,
                StreetAddress1 = "Anaheim West Tower 201 S. Anaheim Blvd",
                City = "Anaheim",
                State = "California",
                ZipCode = "92800",

            };

        }

        public ValueTask<AddressResultSet> GetByType()
        {
            throw new NotImplementedException();
        }


        public async ValueTask<AddressResultSet> GetAllByCustomer(Customer DTO)
        {
            List<Address> result = new List<Address>
            {
                new Address
                {
                   AddressId = 1,
                   StreetAddress1 = "Anaheim West Tower 201 S. Anaheim Blvd",
                   City = "Anaheim",
                   State = "California",
                   ZipCode = "92800",

                },

                new Address
                {
                   AddressId = 2,
                   StreetAddress1 = "201 E Broadway, Anaheim",
                   City = "Anaheim",
                   State = "California",
                   ZipCode = "92899"
                },
                new Address
                {
                   AddressId = 3,
                   StreetAddress1 = "1313 Dr, Anaheim",
                   City = "Anaheim",
                   State = "California",
                   ZipCode = "92802"

                }
            };

            return new AddressResultSet { Addresses = result };
        }

    }
}
