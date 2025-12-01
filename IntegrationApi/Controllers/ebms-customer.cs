using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Domain.Aggregates.Entities;
using IntegrationApi.ViewModels;
using LitJson;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
namespace IntegrationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ebms_customerController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<ebms_work_orderController> _logger;

        private readonly AssetsUseCases WorkOrderUseCases;

        private readonly CustomerUseCases customerUseCases;

        
        public ebms_customerController(ILogger<ebms_work_orderController> logger,CalibrationSaaS.Application.UseCases.AssetsUseCases _WorkOrderUseCases,CustomerUseCases _CustomerUseCases )
        {
            _logger = logger;
            WorkOrderUseCases = _WorkOrderUseCases;
            customerUseCases = _CustomerUseCases;
        }

        //[HttpGet(Name = "Getebms_Customer")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}


        //[ApiExplorerSettings(IgnoreApi = true)]
        //public async Task<Customer> CreateCustomer(CustomerViewModel Customer)
        //{

        //    using (EventLog eventLog = new EventLog("Application"))
        //    {
        //        eventLog.Source = "Application";
        //        if (Customer != null)
        //        {
        //            var s = Newtonsoft.Json.JsonConvert.SerializeObject(Customer);


        //            eventLog.WriteEntry("Sent Customer: " + s + " - " + DateTime.Now.ToShortDateString(), EventLogEntryType.Information, 101, 1);

        //            eventLog.WriteEntry("Sent Customer: " + Customer.name.ToString(), EventLogEntryType.Information, 101, 1);


        //        }
        //        else
        //        {
        //            eventLog.WriteEntry("Customer Null ", EventLogEntryType.Error, 101, 1);
        //            throw new Exception ("Unable to create, Customer null");
        //        }
        //    }

        //    List<CalibrationSaaS.Domain.Aggregates.Entities.Address> lstad = new List<CalibrationSaaS.Domain.Aggregates.Entities.Address>();

        //    if (Customer.addresses != null)
        //    {
        //        foreach (var item in Customer.addresses)
        //        {
        //            CalibrationSaaS.Domain.Aggregates.Entities.Address add = new CalibrationSaaS.Domain.Aggregates.Entities.Address();

        //            add.StreetAddress1 = item.streetAddress1;
        //            add.StreetAddress2 = item.streetAddress2;

        //            if (item.streetAddress3 != null)
        //            {
        //                add.StreetAddress3 = item.streetAddress3;
        //            }


        //            add.City = item.city;
        //            add.CityID = item.city;
        //            add.StateID = item.state;
        //            add.ZipCode = item.zipCode;
        //            add.Country = item.country;
        //            add.CountryID = item.country;

        //            lstad.Add(add);
        //        }
        //    }
        //    else
        //    {
        //        CalibrationSaaS.Domain.Aggregates.Entities.Address add = new CalibrationSaaS.Domain.Aggregates.Entities.Address();

        //        add.StreetAddress1 = "NO ADDRESS";
        //        //add.StreetAddress2 = item.streetAddress2;

        //        //if (item.streetAddress3 != null)
        //        //{
        //        //    add.StreetAddress3 = item.streetAddress3;
        //        //}


        //        add.City = "NO CITY";
        //        add.CityID = "NO CITY";
        //        add.StateID = "NO STATE";
        //        add.ZipCode = "00000";
        //        add.Country = "USA";
        //        add.CountryID = "COUNTRY";

        //        lstad.Add(add);
        //    }

        //    List<CalibrationSaaS.Domain.Aggregates.Entities.Contact> lstcont = new List<CalibrationSaaS.Domain.Aggregates.Entities.Contact>();
        //    if (Customer.contacts != null)
        //    {
        //        foreach (var item in Customer.contacts)
        //        {
        //            CalibrationSaaS.Domain.Aggregates.Entities.Contact add = new CalibrationSaaS.Domain.Aggregates.Entities.Contact();

        //            add.LastName = item.lastName;
        //            add.Name = item.name;
        //            add.CellPhoneNumber = item.phone;
        //            add.Email = item.email;
        //            add.PhoneNumber = item.office;

        //            lstcont.Add(add);
        //        }
        //    }
        //    else
        //    {


        //        CalibrationSaaS.Domain.Aggregates.Entities.Contact add = new CalibrationSaaS.Domain.Aggregates.Entities.Contact();

        //        add.LastName = "NO CONTACT";
        //        add.Name = "NO CONTACT";
        //        add.CellPhoneNumber = "000000";
        //        add.Email = "NOEMAIL@email.com";
        //        add.PhoneNumber = "000000";

        //        lstcont.Add(add);



        //    }

        //    var DTO = new CalibrationSaaS.Domain.Aggregates.Entities.Customer();

        //    DTO.Name = Customer.name;

        //    DTO.CustomID = Customer.customerID;

        //    DTO.Aggregates = new List<CustomerAggregate>();

        //    CustomerAggregate lsy = new CustomerAggregate();

        //    lsy.Addresses = lstad;

        //    lsy.Contacts = lstcont;

        //    DTO.Aggregates.Add(lsy);

        //    CustomerDomain cd = new CustomerDomain(customerUseCases);

        //    var ass = await cd.CreateCustomer(DTO);


        //    return ass;


        //}




        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<string>> PostCustomer([FromBody] CustomerViewModel Customer)
        {
            //JsonData data = JsonMapper.ToObject(Customer);
            //var todoItem = new TodoItem
            //{
            //    IsComplete = todoItemDTO.IsComplete,
            //    Name = todoItemDTO.Name
            //};

            //_context.TodoItems.Add(todoItem);
            //await _context.SaveChangesAsync();

            //var id= data["workOrderId"];

            //var customerId= data["customerID"];

            //var name=data["name"];

            //var addresses=data["addresses"];

            //var Contacts=data["contacts"];
            try
            {
                //using (EventLog eventLog = new EventLog("Application"))
                //{
                //    eventLog.Source = "Application";
                //    if (Customer != null)
                //    {
                //        var s = Newtonsoft.Json.JsonConvert.SerializeObject(Customer);


                //        eventLog.WriteEntry("Sent Customer: " + s + " - " + DateTime.Now.ToShortDateString(), EventLogEntryType.Information, 101, 1);

                //        eventLog.WriteEntry("Sent Customer: " + Customer.name.ToString(), EventLogEntryType.Information, 101, 1);


                //    }
                //    else
                //    {
                //        eventLog.WriteEntry("Customer Null ", EventLogEntryType.Error, 101, 1);
                //        return BadRequest("Unable to create, Customer null");
                //    }
                //}

                //List<CalibrationSaaS.Domain.Aggregates.Entities.Address> lstad = new List<CalibrationSaaS.Domain.Aggregates.Entities.Address>();

                //if (Customer.addresses != null)
                //{
                //    foreach (var item in Customer.addresses)
                //    {
                //        CalibrationSaaS.Domain.Aggregates.Entities.Address add = new CalibrationSaaS.Domain.Aggregates.Entities.Address();

                //        add.StreetAddress1 = item.streetAddress1;
                //        add.StreetAddress2 = item.streetAddress2;

                //        if (item.streetAddress3 != null)
                //        {
                //            add.StreetAddress3 = item.streetAddress3;
                //        }


                //        add.City = item.city;
                //        add.CityID = item.city;
                //        add.StateID = item.state;
                //        add.ZipCode = item.zipCode;
                //        add.Country = item.country;
                //        add.CountryID = item.country;

                //        lstad.Add(add);
                //    }
                //}
                //else
                //{
                //    CalibrationSaaS.Domain.Aggregates.Entities.Address add = new CalibrationSaaS.Domain.Aggregates.Entities.Address();

                //    add.StreetAddress1 = "NO ADDRESS";
                //    //add.StreetAddress2 = item.streetAddress2;

                //    //if (item.streetAddress3 != null)
                //    //{
                //    //    add.StreetAddress3 = item.streetAddress3;
                //    //}


                //    add.City = "NO CITY";
                //    add.CityID = "NO CITY";
                //    add.StateID = "NO STATE";
                //    add.ZipCode = "00000";
                //    add.Country = "USA";
                //    add.CountryID = "COUNTRY";

                //    lstad.Add(add);
                //}

                //List<CalibrationSaaS.Domain.Aggregates.Entities.Contact> lstcont = new List<CalibrationSaaS.Domain.Aggregates.Entities.Contact>();
                //if (Customer.contacts != null)
                //{
                //    foreach (var item in Customer.contacts)
                //    {
                //        CalibrationSaaS.Domain.Aggregates.Entities.Contact add = new CalibrationSaaS.Domain.Aggregates.Entities.Contact();

                //        add.LastName = item.lastName;
                //        add.Name = item.name;
                //        add.CellPhoneNumber = item.phone;
                //        add.Email = item.email;
                //        add.PhoneNumber = item.office;

                //        lstcont.Add(add);
                //    }
                //}
                //else
                //{


                //    CalibrationSaaS.Domain.Aggregates.Entities.Contact add = new CalibrationSaaS.Domain.Aggregates.Entities.Contact();

                //    add.LastName = "NO CONTACT";
                //    add.Name = "NO CONTACT";
                //    add.CellPhoneNumber = "000000";
                //    add.Email = "NOEMAIL@email.com";
                //    add.PhoneNumber = "000000";

                //    lstcont.Add(add);



                //}

                //var DTO = new CalibrationSaaS.Domain.Aggregates.Entities.Customer();

                //DTO.Name = Customer.name;

                //DTO.CustomID = Customer.customerID;

                //DTO.Aggregates = new List<CustomerAggregate>();

                //CustomerAggregate lsy = new CustomerAggregate();

                //lsy.Addresses = lstad;

                //lsy.Contacts = lstcont;

                //DTO.Aggregates.Add(lsy);

                //var ass = await CustomerUseCases.CreateCustomer(DTO);


                CustomerDomain cd = new CustomerDomain(customerUseCases);


                var ass = await cd.CreateCustomer(Customer);


                if (ass != null)
                {
                    return CreatedAtAction("PostCustomer", ass.CustomerID);

                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "Application";
                        eventLog.WriteEntry("Customer created: " + ass.CustomerID, EventLogEntryType.Information, 101, 1);
                    }
                }
                else
                {
                    return BadRequest("Unable to create, check the information sent");
                }
            }
            catch (Exception ex)
            {
                if (Customer != null)
                {
                    if (Customer.name != null && Customer.name.Equals("Log"))
                    {
                        using (EventLog eventLog = new EventLog("Application"))
                        {
                            eventLog.Source = "Application";
                            eventLog.WriteEntry("Unable to create, check the information sent " + ex.Message + " - " + ex.StackTrace, EventLogEntryType.Information, 101, 1);
                        }
                        return BadRequest("Unable to create, check the information sent " + ex.Message + " - " + ex.StackTrace);
                    }
                    else
                    {
                        using (EventLog eventLog = new EventLog("Application"))
                        {
                            eventLog.Source = "Application";
                            eventLog.WriteEntry("Unable to create, check the information sent " + ex.Message + " - " + ex.StackTrace, EventLogEntryType.Information, 101, 1);
                        }
                        return BadRequest("Unable to create, check the information sent, Log Available ");
                    }
                }
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Unable to create, check the information sent, Customer null", EventLogEntryType.Information, 101, 1);
                }
                return BadRequest("Unable to create, check the information sent, Customer null");
            }
              
        }

    }
}