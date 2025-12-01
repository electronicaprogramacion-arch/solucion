using CalibrationSaaS.Domain.Aggregates.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.GrpcServices.Identity
{
    public class Identity
    {
        public IIdentity identity { get; set; }

        public Identity ()
        {


            //WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();

            //identity = windowsIdentity;



        }
    }

}
