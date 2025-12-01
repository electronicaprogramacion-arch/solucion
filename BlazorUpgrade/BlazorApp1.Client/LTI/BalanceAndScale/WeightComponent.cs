using Microsoft.AspNetCore.Components;
using System;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
namespace CalibrationSaaS.Infraestructure.Blazor.LTI.Scale
{


    public partial class WeightComponent<Target> : ComponentBase, IDisposable where Target : new()
    {
        [Inject]
        IConfiguration Configuration { get; set; }
        public string url { get; set; }

       
        public void Dispose()
        {



        }
    }
}
