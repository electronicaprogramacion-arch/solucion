using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Reports.Infraestructure.APIRep.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
namespace IntegrationApi.Controllers
{
   
    public class UserDomain
    {
       

      

        private readonly BasicsUseCases BasicsUseCases;

        
        public UserDomain(BasicsUseCases _BasicsUseCases)
        {
            //_logger = logger;
            //WorkOrderUseCases = _WorkOrderUseCases;
            BasicsUseCases = _BasicsUseCases;
        }

     

     
        public async Task<User> CreateUser(UserViewModel user)
        {


            User userAgg = new User();

            userAgg.UserName = user.name;

            var ass = await BasicsUseCases.CreateUser(userAgg, true);

            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                if (user != null)
                {
                    var s = Newtonsoft.Json.JsonConvert.SerializeObject(user);


                    eventLog.WriteEntry("Sent Customer: " + s + " - " + DateTime.Now.ToShortDateString(), EventLogEntryType.Information, 101, 1);

                    eventLog.WriteEntry("Sent Customer: " + user.name.ToString(), EventLogEntryType.Information, 101, 1);


                }
                else
                {
                    eventLog.WriteEntry("Customer Null ", EventLogEntryType.Error, 101, 1);
                    throw new Exception ("Unable to create, Customer null");
                }
            }


            return ass;


        }




      

    }
}