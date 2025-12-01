using BlazorInputFile;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using FileInfo = CalibrationSaaS.Domain.Aggregates.Entities.FileInfo;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using Grpc.Core;
using System.Web;
using Microsoft.Azure.Storage.Blob;

using Microsoft.Azure.Storage;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Azure.Core;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace CalibrationSaaS.Infraestructure.GrpcServices.Services
{
    public class FileUpload : IFileUpload
    {
      
       private readonly IConfiguration Configuration;
       public static string storageconnstring = "DefaultEndpointsProtocol=https;AccountName=archiveslti;AccountKey=budUvxPMvjY2HoypiKgQnQbatihqV8AxLAPZJ+xI5+KoBkiBdLL4VzgLmIgmZmGKy+EWGeksW9Gd+AStaq+lOA==;EndpointSuffix=core.windows.net";
       private static string containerName = "certificates";
      
       private static string fileToUpload = "azurelogo.png";
      
       private static BlobServiceClient blobServiceClient = new BlobServiceClient(storageconnstring);
       private static BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        private IFormFile formFile;
        private readonly string _contentRootPath;

        public FileUpload(IHostEnvironment env, IWebHostEnvironment enviroment, IConfiguration configuration)
        {
            
            Configuration = configuration;
            _contentRootPath = env.ContentRootPath;
        }

        public async Task UploadAsync(FileInfo[] fileEntry)
        {
           

            await UploadCertificate(fileEntry);
            

        }
        public string DownloadAsync(string fileName)
        {
            
            
            var path = Configuration.GetSection("Reports")["LocalPath"];

            var file =  Directory.GetFiles(path, $"{fileName}.*").FirstOrDefault();
            if (file != null)
            {
                Byte[] bytes = File.ReadAllBytes(file);
                String base64String = Convert.ToBase64String(bytes);
                return base64String;
            }
            return null;


        }
        public async Task UploadCertificate(FileInfo[] fileEntry)
        {

            string name ;

            try
            {
                
                foreach (var file in fileEntry)
                {

                    byte[] myByteArray = file.Data;
                    Stream streamForm = new MemoryStream(myByteArray);
                    formFile = new FormFile(streamForm, 0, file.Data.Length, file.Name, file.Name);
                    name = @"\" + file.Name;

                    var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

                    if (string.IsNullOrEmpty(name))
                    {
                        name = Guid.NewGuid().ToString();
                    }

                    var issub = Configuration.GetSection("Reports")["EnableSubscription"];

                    if (!string.IsNullOrEmpty(issub) && Convert.ToBoolean(issub))
                    {
                        String strorageconn = ConfigurationExtensions.GetConnectionString(this.Configuration, "fileShareConnectionString");
                        CloudStorageAccount storageacc = CloudStorageAccount.Parse(strorageconn);

                        CloudBlobClient blobClient = storageacc.CreateCloudBlobClient();

                      
                        CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                        container.CreateIfNotExists();

                   
                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);

                        var blob = container.GetBlockBlobReference(file.Name);
                        await using (var stream = formFile.OpenReadStream())
                        {
                            await blob.UploadFromStreamAsync(stream);
                        }


                    }
                    else
                    {
                            
                        
                        foreach (var fileentry in fileEntry)
                        {

                            if (!fileentry.Name.ToLower().Contains(".pdf"))
                            {
                                fileentry.Name = fileentry.Name + ".pdf";
                            }
                         
                            var path = Path.Combine(Configuration.GetSection("Reports")["LocalPath"], "", fileentry.Name); //Path.Combine(_enviroment.WebRootPath, "", fileentry.Name);
                            var ms = new MemoryStream(fileentry.Data);



                            using (FileStream file1 = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                            {
                                ms.WriteTo(file1);
                            }

                        }

                    }



                }

            }
            catch (Exception ex)
            {

            }

        }

        private static async Task UploadBlob()
        {
            try
            {
                BlobClient blobClient = containerClient.
                                        GetBlobClient(fileToUpload);
                var result = await blobClient.UploadAsync("https://archivesgrpclti.blob.core.windows.net/archivesgrpc/" + fileToUpload, true);

                if (result.GetRawResponse().Status == 201)
                {
//                    Console.WriteLine("File uploaded sucessfully!");
                    return;
                }

//                Console.WriteLine("Error to upload file!");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void ListBlobs()
        {
            try
            {
//                Console.WriteLine($"List of all items in blob {containerName} container");
//                Console.WriteLine("------------------------------");
             
                foreach (BlobItem blob in containerClient.GetBlobs())
                {
//                    Console.WriteLine(blob.Name);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        

        public async Task<VersionBlazor> GetLatestBlazorVersion()
        {
            try
            {

                var path = Path.Combine(_contentRootPath, "Version");
                var fileName = "CurrentVersion";
                var file = Directory.GetFiles(path, $"{fileName}.*").FirstOrDefault();
                long numberVersion = 0;
                VersionBlazor version = new VersionBlazor();
                if (file != null)
                {

                    using (var reader = new StreamReader(file, Encoding.UTF8))  // Aseguramos la codificación UTF-8
                    {
                        var line = reader.ReadLine();
//                        Console.WriteLine($"Contenido del archivo: '{line}'");  // Mostrar contenido exacto

                        // Imprimir cada carácter y su código ASCII para verificar caracteres invisibles
                        foreach (var c in line)
                        {
//                            Console.WriteLine($"Carácter: '{c}', Código ASCII: {(int)c}");
                        }

                        if (long.TryParse(line?.Trim(), out numberVersion))
                        {
                            version.Version = numberVersion;
                        }
                        else
                        {
//                            Console.WriteLine("La línea no se pudo convertir a un número. Verifica posibles caracteres ocultos.");
                        }
                    }
                }


                return version;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }




}
