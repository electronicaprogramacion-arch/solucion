using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Domain.BusinessExceptions;
using IntegrationApi.ViewModels;
using LitJson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
namespace IntegrationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ebms_work_orderController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ebms_work_orderController> _logger;

        private readonly AssetsUseCases WorkOrderUseCases;

        private readonly CustomerUseCases CustomerUseCases;

        private readonly BasicsUseCases basicsLogic;

        

        private readonly IConfiguration Configuration;

        private IHttpClientFactory _httpClientFactory;

        //public void BasicModel(IHttpClientFactory httpClientFactory) =>
        //_httpClientFactory = httpClientFactory;



        public ebms_work_orderController(ILogger<ebms_work_orderController> logger,
            CalibrationSaaS.Application.UseCases.AssetsUseCases _WorkOrderUseCases,
            CustomerUseCases _CustomerUseCases
            , IConfiguration _Configuration,
            BasicsUseCases basicsLogic)
        {
            _logger = logger;
            WorkOrderUseCases = _WorkOrderUseCases;
            CustomerUseCases = _CustomerUseCases;
            Configuration = _Configuration;
            this.basicsLogic = basicsLogic;
        }

        //[HttpGet(Name = "Getebms_work_order")]
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


        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754


        private async Task<ActionResult<string>> ConsumeService(CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder DTO)
        {
            try
            {
                var urlc = Configuration.GetConnectionString("EBMSURl");

                var urllocal = Configuration.GetConnectionString("localUrl");
                var dol = "\"";
                string cotent = "{\"KWO_STATUS\":\"Open\",\"KWO_LINK\":\"" + urllocal + "/" + DTO.WorkOrderId + dol + "}";

                //string cotent = @"{"KWO_STATUS":"Open", "KWO_LINK":" + urllocal + "/" + wo.WorkOrderId + "}";
                //TODO: Change to appsettings
                //var httpRequestMessage = new HttpRequestMessage(
                //    HttpMethod.Patch,
                //    urlc + "(INVOICE='" + DTO.Invoice + "')")
                //{
                //    Headers =
                //    {
                //        { HeaderNames.ContentType, "application/json" },
                //        { HeaderNames.UserAgent, "Kavoku" }
                //    },
                //    Content = new StringContent(cotent, Encoding.UTF8, "application/json")
                //};

                //var httpClient = _httpClientFactory.CreateClient();

                //var byteArray = Encoding.ASCII.GetBytes("EBMSAPI:EBMSAPI.21");
                //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));


                //var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);


                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(urlc);
                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, urlc + "(INVOICE='" + DTO.Invoice + "')");
                urlc = urlc + "(INVOICE='" + DTO.Invoice + "')";
                //
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, urlc);
                request.Content = new StringContent(cotent,
                                                    Encoding.UTF8,
                                                    "application/json");//CONTENT-TYPE header

                var byteArray = Encoding.ASCII.GetBytes("EBMSAPI:EBMSAPI.21");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));


                var httpResponseMessage = await client.SendAsync(request);
                //await client.SendAsync(request)
                //      .ContinueWith(responseTask =>
                //      {
                //          Console.WriteLine("Response: {0}", responseTask.Result);
                //      });



                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream =
                        await httpResponseMessage.Content.ReadAsStreamAsync();
                    return null;
                }
                else
                {
                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "Application";
                        eventLog.WriteEntry("Work Order Updated but error contacting EBMS, " + httpResponseMessage.StatusCode + " " + httpResponseMessage.ReasonPhrase, EventLogEntryType.Information, 101, 1);
                    }
                    return Ok("Work Order Updated but error contacting EBMS, " + httpResponseMessage.StatusCode + " " + httpResponseMessage.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                if (DTO != null)
                {
                    if (DTO.Invoice != null && DTO.Invoice.Equals("Log"))
                    {
                        using (EventLog eventLog = new EventLog("Application"))
                        {
                            eventLog.Source = "Application";
                            eventLog.WriteEntry("Work Order Updated but error contacting EBMS, " + ex.Message + " - " + ex.StackTrace, EventLogEntryType.Information, 101, 1);
                        }
                        return Ok("Work Order Updated but error contacting EBMS, " + ex.Message + " - " + ex.StackTrace);
                    }
                    else
                    {
                        using (EventLog eventLog = new EventLog("Application"))
                        {
                            eventLog.Source = "Application";
                            eventLog.WriteEntry("Work Order Updated but error contacting EBM,S " + ex.Message + " - " + ex.StackTrace, EventLogEntryType.Information, 101, 1);
                        }
                        return Ok("Work Order Updated but error contacting EBMS, Log Available");
                    }
                }
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Work Order Updated but error contacting EBMS, Work Order null", EventLogEntryType.Information, 101, 1);
                }
                return Ok("Work Order Updated but error contacting EBMS, Work Order null");
            }



        }


        [HttpPost]
        public async Task<ActionResult<string>> PostWorkOrder([FromBody] WorkOrderViewModel WorkOrder)
        {
            //JsonData data = JsonMapper.ToObject(WorkOrder);
            //var todoItem = new TodoItem
            //{
            //    IsComplete = todoItemDTO.IsComplete,
            //    Name = todoItemDTO.Name
            //};

            //_context.TodoItems.Add(todoItem);
            //await _context.SaveChangesAsync();

            //var id= data["workOrderId"];

            //var customerId= data["customerId"];

            //var date = (string)data["workOrderDate"];
            //var workOrderDate= Convert.ToDateTime(date);
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                if (WorkOrder != null)
                {
                    var s = Newtonsoft.Json.JsonConvert.SerializeObject(WorkOrder);


                    eventLog.WriteEntry("Sent WorkOrder: " + s + " - " + DateTime.Now.ToShortDateString(), EventLogEntryType.Information, 101, 1);

                    eventLog.WriteEntry("Sent WorkOrder: " + WorkOrder.workOrderId.ToString(), EventLogEntryType.Information, 101, 1);


                }
                else
                {
                    eventLog.WriteEntry("WorkOrder Null ", EventLogEntryType.Error, 101, 1);
                    return BadRequest("Unable to create, WorkOrder null");
                }
            }




            var DTO = new CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder();

            //Console.WriteLine(WorkOrder.workOrderId);

            DTO.Invoice = WorkOrder.workOrderId;
            DTO.CustomerInvoice = WorkOrder.customerId;


            var ass = await CustomerUseCases.GetCustomerByName("", DTO.CustomerInvoice);

            if(ass == null) 
            {

                CustomerDomain customercontroller= new CustomerDomain(CustomerUseCases);


                CustomerViewModel customer = new CustomerViewModel();

                customer.customerID = DTO.CustomerInvoice;

                customer.name = "NO CUSTOMER CONFIGURED - " + DTO.CustomerInvoice;

                var resp = await customercontroller.CreateCustomer(customer);

                if(resp == null)
                {

                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "Application";
                        eventLog.WriteEntry("Check customer " + WorkOrder.customerId, EventLogEntryType.Information, 101, 1);
                    }
                   
                    return BadRequest("Error in customer " + DTO.CustomerInvoice);
                }
                else if(resp.CustomID != DTO.CustomerInvoice)
                {
                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "Application";
                        eventLog.WriteEntry("Check customer trucate" + WorkOrder.customerId, EventLogEntryType.Information, 101, 1);
                    }
                }
                else
                {

                }


            }

            if (WorkOrder.technicians ==null)
            {

                var result = await basicsLogic.GetAllUser();

                List<TechnicianViewModel> Techs = new List<TechnicianViewModel>();   

                foreach (var user in result)
                {
                    var item = new TechnicianViewModel();

                    if (user != null && !string.IsNullOrEmpty(user.UserName)) 
                    {
                        item.name= user.UserName;

                        Techs.Add(item);
                    }
                }

                WorkOrder.technicians = Techs;

            }

                try
            {
                if (!String.IsNullOrEmpty(WorkOrder.workOrderDate))
                {
                    int year = int.Parse(WorkOrder.workOrderDate.Substring(0, 4));
                    int month = int.Parse(WorkOrder.workOrderDate.Substring(4, 2));
                    int day = int.Parse(WorkOrder.workOrderDate.Substring(6, 2));
                    DTO.WorkOrderDate = new DateTime(year, month, day);
                }
                else
                {
                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "Application";
                        eventLog.WriteEntry("Check Work Order Date " + WorkOrder.workOrderId, EventLogEntryType.Information, 101, 1);
                    }
                    return BadRequest("Check Work Order Date");
                }
            }
            catch (Exception)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Invalid date format for" + WorkOrder.workOrderId, EventLogEntryType.Information, 101, 1);
                }
                return BadRequest("Invalid date format for " + WorkOrder.workOrderDate);
            }

            


            try
            {

               



                var wo = await WorkOrderUseCases.CreateWorkOrder(DTO, "service");

                DTO.WorkOrderId = wo.WorkOrderId;

                var a = await ConsumeService(DTO);

                if (a == null)
                {
                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "Application";
                        eventLog.WriteEntry("Work Order Created: " + wo.WorkOrderId, EventLogEntryType.Information, 101, 1);
                    }
                    return CreatedAtAction("PostWorkOrder", wo.WorkOrderId);
                }
                else
                {
                    return a;
                }
                

            }
            catch (AlreadyInUseException ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Work Order for that invoice already exists " + DTO.Invoice, EventLogEntryType.Information, 101, 1);
                }
                return Ok("Work Order for that invoice already exists");
            }
            catch (Exception ex)
            {
                if (WorkOrder != null)
                {
                    if (WorkOrder.workOrderId != null && WorkOrder.workOrderId.Equals("Log"))
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
                    eventLog.WriteEntry("Unable to create, check the information sent, Work Order null", EventLogEntryType.Information, 101, 1);
                }
                return BadRequest("Unable to create, check the information sent, Work Order null");
            }
        }

    }


}