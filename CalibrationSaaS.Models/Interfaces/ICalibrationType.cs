using CalibrationSaaS.Domain.Aggregates.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalibrationSaaS.Domain.Aggregates.Interfaces
{
    public interface ICalibrationType
    {

        int CalibrationTypeId { get; set; }

        //CalibrationType CalibrationType { get; set; }


        ICollection<CalibrationSubType> CalibrationSubTypes { get; set; }



        //string UsersAccess { get; set; }
        
        string Name { get; set; }


        bool? ShowType { get; set; }

        public string UrlReport { get; set; }

        bool HasNew { get; set; }

       

    }
}
