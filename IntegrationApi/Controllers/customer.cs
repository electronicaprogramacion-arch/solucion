using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Domain.Aggregates.Entities;
using IntegrationApi.ViewModels;
using LitJson;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
namespace IntegrationApi.Controllers
{
   
    public class CustomerDomain
    {
       

        //private readonly ILogger<ebms_work_orderController> _logger;

        private readonly AssetsUseCases WorkOrderUseCases;

        private readonly CustomerUseCases CustomerUseCases;

        
        public CustomerDomain(CustomerUseCases _CustomerUseCases )
        {
            //_logger = logger;
            //WorkOrderUseCases = _WorkOrderUseCases;
            CustomerUseCases = _CustomerUseCases;
        }

     

     
        public async Task<Customer> CreateCustomer(CustomerViewModel Customer)
        {

            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                if (Customer != null)
                {
                    var s = Newtonsoft.Json.JsonConvert.SerializeObject(Customer);


                    eventLog.WriteEntry("Sent Customer: " + s + " - " + DateTime.Now.ToShortDateString(), EventLogEntryType.Information, 101, 1);

                    eventLog.WriteEntry("Sent Customer: " + Customer.name.ToString(), EventLogEntryType.Information, 101, 1);


                }
                else
                {
                    eventLog.WriteEntry("Customer Null ", EventLogEntryType.Error, 101, 1);
                    throw new Exception ("Unable to create, Customer null");
                }
            }

            List<CalibrationSaaS.Domain.Aggregates.Entities.Address> lstad = new List<CalibrationSaaS.Domain.Aggregates.Entities.Address>();

            if (Customer.addresses != null)
            {
                foreach (var item in Customer.addresses)
                {
                    CalibrationSaaS.Domain.Aggregates.Entities.Address add = new CalibrationSaaS.Domain.Aggregates.Entities.Address();

                    add.StreetAddress1 = item.streetAddress1;
                    add.StreetAddress2 = item.streetAddress2;

                    if (item.streetAddress3 != null)
                    {
                        add.StreetAddress3 = item.streetAddress3;
                    }


                    add.City = item.city;
                    add.CityID = item.city;
                    add.StateID = item.state;
                    add.ZipCode = item.zipCode;
                    add.Country = item.country;
                    add.CountryID = item.country;

                    lstad.Add(add);
                }
            }
            else
            {
                //CalibrationSaaS.Domain.Aggregates.Entities.Address add = new CalibrationSaaS.Domain.Aggregates.Entities.Address();

                //add.StreetAddress1 = "NO ADDRESS";
                //add.City = "NO CITY";
                //add.CityID = "NO CITY";
                //add.StateID = "NO STATE";
                //add.ZipCode = "00000";
                //add.Country = "USA";
                //add.CountryID = "COUNTRY";

                //lstad.Add(add);
            }

            List<CalibrationSaaS.Domain.Aggregates.Entities.Contact> lstcont = new List<CalibrationSaaS.Domain.Aggregates.Entities.Contact>();
            if (Customer.contacts != null)
            {
                foreach (var item in Customer.contacts)
                {
                    CalibrationSaaS.Domain.Aggregates.Entities.Contact add = new CalibrationSaaS.Domain.Aggregates.Entities.Contact();

                    add.LastName = item.lastName;
                    add.Name = item.name;
                    add.CellPhoneNumber = item.phone;
                    add.Email = item.email;
                    add.PhoneNumber = item.office;

                    lstcont.Add(add);
                }
            }
            else
            {


                //CalibrationSaaS.Domain.Aggregates.Entities.Contact add = new CalibrationSaaS.Domain.Aggregates.Entities.Contact();

                //add.LastName = "NO CONTACT";
                //add.Name = "NO CONTACT";
                //add.CellPhoneNumber = "000000";
                //add.Email = "NOEMAIL@email.com";
                //add.PhoneNumber = "000000";

                //lstcont.Add(add);



            }

            var DTO = new CalibrationSaaS.Domain.Aggregates.Entities.Customer();

            DTO.Name = Customer.name;

            DTO.CustomID = Customer.customerID;

            DTO.Aggregates = new List<CustomerAggregate>();

            CustomerAggregate lsy = new CustomerAggregate();

            lsy.Addresses = lstad;

            lsy.Contacts = lstcont;

            DTO.Aggregates.Add(lsy);

            var ass = await CustomerUseCases.CreateCustomer(DTO,true);


            return ass;


        }




      

    }
}