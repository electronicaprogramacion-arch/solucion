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
    public class ebms_userController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<ebms_userController> _logger;

        private readonly BasicsUseCases basicsUseCases;

       

        
        public ebms_userController(ILogger<ebms_userController> logger,CalibrationSaaS.Application.UseCases.BasicsUseCases _BasicsUseCases )
        {
            _logger = logger;
            
            basicsUseCases = _BasicsUseCases;
        }

        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            UserViewModel user = new  UserViewModel();


            try
            {

                List<User> real = new List<User>();

                var a = await basicsUseCases.GetAllUserIdentity();

                var user1 = await basicsUseCases.GetAllUser();

                foreach (var item in user1)
                {
                    string name = item.UserName;
                    if (string.IsNullOrEmpty(item.UserName))
                    {
                        name = item.Name;
                    }          

                    var exi = a.Where(x => x.UserName == name).FirstOrDefault();

                    if (exi == null)
                    {
                        real.Add(item);
                    }
                }

                return Ok(real);
            }

        
            catch (Exception ex)
            {
                if (user != null)
                {
                    if (user.name != null && user.name.Equals("Log"))
                    {
                        using (EventLog eventLog = new EventLog("Application"))
                        {
                            eventLog.Source = "Application";
                            eventLog.WriteEntry("Unable to create, check the information sent " + ex.Message + " - " + ex.StackTrace, EventLogEntryType.Information, 101, 1);
//                            Console.WriteLine("Unable to create, check the information sent " + ex.Message + " - " + ex.StackTrace, EventLogEntryType.Information, 101, 1);
                        }
                        return BadRequest("Unable to create, check the information sent " + ex.Message + " - " + ex.StackTrace);
}
                    else
{
    using (EventLog eventLog = new EventLog("Application"))
    {
        eventLog.Source = "Application";
        eventLog.WriteEntry("Unable to create, check the information sent " + ex.Message + " - " + ex.StackTrace, EventLogEntryType.Information, 101, 1);
//                            Console.WriteLine("Unable to create, check the information sent " + ex.Message + " - " + ex.StackTrace, EventLogEntryType.Information, 101, 1);
                        }
    return BadRequest("Unable to create, check the information sent, Log Available ");
}
                }
                using (EventLog eventLog = new EventLog("Application"))
{
    eventLog.Source = "Application";
    eventLog.WriteEntry("Unable to create, check the information sent, user null", EventLogEntryType.Information, 101, 1);
}
return BadRequest("Unable to create, check the information sent, Customer null");
            }
        }


        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<string>> PostCustomer([FromBody] UserViewModel user)
        {
           
            try
            {


                User userAgg = new User();

                userAgg.UserName = user.name;

                var ass = await basicsUseCases.CreateUser(userAgg,true);


                if (ass != null)
                {
                    return Ok(ass.UserName);

                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "Application";
                        eventLog.WriteEntry("user created: " + ass.UserName, EventLogEntryType.Information, 101, 1);
                    }
                }
                else
                {
                    return BadRequest("Unable to create, check the information sent");
                }
            }
            catch (Exception ex)
            {
                if (user != null)
                {
                    if (user.name != null && user.name.Equals("Log"))
                    {
                        using (EventLog eventLog = new EventLog("Application"))
                        {
                            eventLog.Source = "Application";
                            eventLog.WriteEntry("Unable to create, check the information sent " + ex.Message + " - " + ex.StackTrace, EventLogEntryType.Information, 101, 1);
//                            Console.WriteLine("Unable to create, check the information sent " + ex.Message + " - " + ex.StackTrace, EventLogEntryType.Information, 101, 1);
                        }
                        return BadRequest("Unable to create, check the information sent " + ex.Message + " - " + ex.StackTrace);
                    }
                    else
                    {
                        using (EventLog eventLog = new EventLog("Application"))
                        {
                            eventLog.Source = "Application";
                            eventLog.WriteEntry("Unable to create, check the information sent " + ex.Message + " - " + ex.StackTrace, EventLogEntryType.Information, 101, 1);
//                            Console.WriteLine("Unable to create, check the information sent " + ex.Message + " - " + ex.StackTrace, EventLogEntryType.Information, 101, 1);
                        }
                        return BadRequest("Unable to create, check the information sent, Log Available ");
                    }
                }
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Unable to create, check the information sent, user null", EventLogEntryType.Information, 101, 1);
//                    Console.WriteLine("Unable to create, check the information sent, user null");

                }
                return BadRequest("Unable to create, check the information sent, Customer null");
            }
              
        }

    }
}