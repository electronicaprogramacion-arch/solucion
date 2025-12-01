using System;
using System.Collections.Generic;
using System.Text;

namespace CalibrationSaaS.Application.Services
{
    public interface InterfaceService<T>
    {


         T Service { get; set; }

         T GetService(T service);

    }
}
