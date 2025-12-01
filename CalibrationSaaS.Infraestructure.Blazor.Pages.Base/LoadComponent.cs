using Grpc.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Base
{
    public class LoadComponentBase : ComponentBase //LayoutComponentBase
    {

        [Inject]
        public dynamic DbFactory { get; set; }


    }




}
