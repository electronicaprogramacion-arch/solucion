
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using CalibrationSaaS.Data.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace CalibrationSaaS.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {

                           

            var app = CreateHostBuilder(args).Build();

            ThreadProc();
            
            app.Run();


        }
        private static void DisplayThreadInfo()
        {

            Thread.CurrentThread.CurrentUICulture= new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator = ".";

//            Console.WriteLine("\nCurrent Thread Name: '{0}'",
//                              Thread.CurrentThread.Name);
//            Console.WriteLine("Current Thread Culture/UI Culture: {0}/{1}",
//                              Thread.CurrentThread.CurrentCulture.Name,
//                              Thread.CurrentThread.CurrentUICulture.Name);
        }

        private static void ThreadProc()
        {
            DisplayThreadInfo();
            
        }
        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>

           //  var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false).Build();




        Host.CreateDefaultBuilder(args)
             .UseContentRoot(Directory.GetCurrentDirectory())
                 //.ConfigureAppConfiguration((builderContext, config) =>
                 //{
                 //    config.AddJsonFile("appsettings.json", optional: false);
                 //})
                 .ConfigureLogging(logging =>
                 {
                     logging.AddConsole();
                     logging.AddFilter("Grpc", LogLevel.Debug);
                 })
                 .ConfigureAppConfiguration((hostContext, builder) =>
                 {
                    
                     builder.AddUserSecrets<Program>();
                 })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //webBuilder.UseStaticWebAssets();
                
                #if DEBUG
                    webBuilder.UseKestrel(options => options.ConfigureEndpoints());
                    webBuilder.UseIISIntegration();

                #else
                //webBuilder.UseKestrel(options => options.ConfigureEndpoints());
                    webBuilder.UseIISIntegration();
                #endif

                });
              
    }


    public static class KestrelServerOptionsExtensions
{
    public static void ConfigureEndpoints(this KestrelServerOptions options)
    {
        var configuration = options.ApplicationServices.GetRequiredService<IConfiguration>();
        var environment = options.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

        var endpoints = configuration.GetSection("HttpServer:Endpoints")
            .GetChildren()
            .ToDictionary(section => section.Key, section =>
            {
                var endpoint = new EndpointConfiguration(); 
                section.Bind(endpoint); 
                return endpoint;
            });

        foreach (var endpoint in endpoints)
        {
            var config = endpoint.Value;
            var port = config.Port ?? (config.Scheme == "https" ? 443 : 80);

            var ipAddresses = new List<IPAddress>();
            if (config.Host == "127.0.0.1")
            {
                ipAddresses.Add(IPAddress.IPv6Loopback);
                ipAddresses.Add(IPAddress.Loopback);
                //ipAddresses.Add(IPAddress.Any);
                //ipAddresses.Add(IPAddress.Loopback);

            }
            else if (IPAddress.TryParse(config.Host, out var address))
            {
                ipAddresses.Add(address);
            }
            else
            {
                ipAddresses.Add(IPAddress.IPv6Any);
            }

            foreach (var address in ipAddresses)
            {
                options.Listen(address, port,
                    listenOptions =>
                    {
                        if (config.Scheme == "https")
                        {
                            var certificate = LoadCertificate(config, environment);
                            listenOptions.UseHttps(certificate);
                        }
                    });
            }
        }
    }

    private static X509Certificate2 LoadCertificate(EndpointConfiguration config, IWebHostEnvironment environment)
    {
        if (config.StoreName != null && config.StoreLocation != null)
        {
            using (var store = new X509Store(config.StoreName, Enum.Parse<StoreLocation>(config.StoreLocation)))
            {
                store.Open(OpenFlags.ReadOnly);
                var certificate = store.Certificates.Find(
                    X509FindType.FindBySubjectName,
                    //config.Host,
                    "localhost",
                    validOnly: !environment.IsDevelopment());

                if (certificate.Count == 0)
                {
                    throw new InvalidOperationException($"Certificate not found for {config.Host}.");
                }

                return certificate[0];
            }
        }

        if (config.FilePath != null && config.Password != null)
        {
            return new X509Certificate2(config.FilePath, config.Password);
        }

        throw new InvalidOperationException("No valid certificate configuration found for the current endpoint.");
    }
}

public class EndpointConfiguration
{
    public string Host { get; set; }
    public int? Port { get; set; }
    public string Scheme { get; set; }
    public string StoreName { get; set; }
    public string StoreLocation { get; set; }
    public string FilePath { get; set; }
    public string Password { get; set; }
}
}
