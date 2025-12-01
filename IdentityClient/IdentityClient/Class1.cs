using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

using System.Net.Http.Headers;

using Microsoft.AspNetCore.Identity;


using System.Linq;

using System.Security.Claims;
using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace IdentityClient
{
    public class IdentityClient: IIdentityRepository
    {
        public static HttpClient webClient = new HttpClient();

        public static string UrlBase = "https://localhost:5005";

        private readonly IConfiguration configuration;

        public IdentityClient(IConfiguration _configuration)
        {
            configuration = _configuration;
            UrlBase = configuration.GetSection("Security").GetValue<string>("Url");
        }

        public IdentityClient(string Url)
        {
            UrlBase = Url;
        }

        public static string GetUrl()
        {
            var builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.Bitterman.json", optional: true)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile("appsettings.PreProduction.json", optional: true, reloadOnChange: true)
          .AddJsonFile("appsettings.Production.json", optional: true)
          .AddJsonFile("appsettings.Staging.json", optional: true)
          .AddEnvironmentVariables();

            var Configuration = builder.Build();
            

            var a = Configuration.GetSection("Security").GetValue<string>("Url");

            return a;

        }

        public  async Task<User> ShowRecords(string name)
        {
             System.Net.ServicePointManager.Expect100Continue = false;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;

            using (HttpResponseMessage webResponse = await webClient.GetAsync(UrlBase + "/api/Manager/" + name))
            {
                 var a = webResponse.Content.ReadFromJsonAsync<User>().Result;


                if (string.IsNullOrEmpty(a.UserName))
                {
                    return null;
                }
            //foreach (var productModel in products)
            //{
            //    Console.WriteLine(productModel);
            //}
            return a;
            }
               
        }


        public async Task<IEnumerable<User>> ShowRecords()
        {
            System.Net.ServicePointManager.Expect100Continue = false;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;

            using (HttpResponseMessage webResponse = await webClient.GetAsync(UrlBase + "/api/Manager/All"))
            {
                var a = webResponse.Content.ReadFromJsonAsync<IEnumerable<User>>().Result;
                //foreach (var productModel in products)
                //{
                //    Console.WriteLine(productModel);
                //}
                return a;
            }

        }

        public async Task<User> DeleteRecordByID(string name)
        {
            System.Net.ServicePointManager.Expect100Continue = false;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            try
            {
                using (HttpResponseMessage webResponse = await webClient.GetAsync(UrlBase + "/api/Manager/DEL/" + name))
                {
                    var a = webResponse.Content.ReadFromJsonAsync<User>().Result;
                  
                    return a;
                }
            }
            catch
            {

                return null;
            }
           

        }


        public  async Task<User> CreateRecord(User user)
        {
            System.Net.ServicePointManager.Expect100Continue = false;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 ;
            //User user1 = new User();
            //user1.UserName = user.UserName;
            user.RolesList = null;

            string json2 = Newtonsoft.Json.JsonConvert.SerializeObject(user, Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
            using (HttpResponseMessage httpResponseMessage = await webClient.PostAsJsonAsync(UrlBase + "/api/Manager/", json2))
            {
                 var a = httpResponseMessage.Content.ReadFromJsonAsync<User>().Result;
                return a;
            }               

        }

        private async Task DeleteRecordByName(string name)
        {

            int id = Convert.ToInt32(Console.ReadLine());
            HttpResponseMessage httpResponse = await webClient.DeleteAsync(UrlBase + "/api/Manager/" + id);


        }

        private async Task EditRecord(User user)
        {

            HttpResponseMessage httpResponseMessage = await webClient.PutAsJsonAsync("http://localhost:90/api/ProductMasters/" + user.UserName, user);
        }

        public async Task<User> DeleteRecord(User user)
        {
             System.Net.ServicePointManager.Expect100Continue = false;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;

            int id = Convert.ToInt32(Console.ReadLine());
            HttpResponseMessage httpResponse = await webClient.DeleteAsync(UrlBase + "/api/Manager/" + id);

            var a = httpResponse.Content.ReadFromJsonAsync<User>().Result;

            return a;
        }

        public async Task<Rol> DeleteRol(Rol rol)
        {
             System.Net.ServicePointManager.Expect100Continue = false;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            using (HttpResponseMessage httpResponse = await webClient.DeleteAsync(UrlBase + "/api/Manager/DeleteRol/" + rol.Name))
            {
                  return null;
            }

              
        }



      

        public async Task<bool> Reset(string name)
        {
            System.Net.ServicePointManager.Expect100Continue = false;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 ;

            using (HttpResponseMessage webResponse = await webClient.GetAsync(UrlBase + "/api/Manager/reset/" + name))
            {
                var a = webResponse.Content.ReadFromJsonAsync<bool>().Result;
                //foreach (var productModel in products)
                //{
                //    Console.WriteLine(productModel);
                //}
                return a;
            }

        }

        public async Task<bool> Enable(string name)
        {
            System.Net.ServicePointManager.Expect100Continue = false;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13  ;

            using (HttpResponseMessage webResponse = await webClient.GetAsync(UrlBase + "/api/Manager/disable/" + name))
            {
                var a = webResponse.Content.ReadFromJsonAsync<bool>().Result;
                //foreach (var productModel in products)
                //{
                //    Console.WriteLine(productModel);
                //}
                return a;
            }
        }


        //        public async Task<string> CallApi()
        //{

        //    var accessToken = await HttpContext.GetTokenAsync("access_token");

        //    var client = new HttpClient();
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        //    var content = await client.GetStringAsync("https://localhost:6001/identity");

        //    //ViewBag.Json = JArray.Parse(content).ToString();
        //    return content;
        //}
    }
}
