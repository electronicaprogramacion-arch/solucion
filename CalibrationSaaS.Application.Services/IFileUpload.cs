using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace CalibrationSaaS.Application.Services
{

    [ServiceContract(Name = "CalibrationSaaS.Application.Services.FileUpload")]
    public interface IFileUpload
    {
        Task UploadAsync(FileInfo[] file);
        string DownloadAsync(string fileName);

        Task<VersionBlazor> GetLatestBlazorVersion();
    }
}
