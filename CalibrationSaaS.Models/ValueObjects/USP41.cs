using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Views;
using System.Linq;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace CalibrationSaaS.Domain.Aggregates.ValueObjects
{
    public class USP41
    {
      

        public string RepeatabilityPass { get; set; }

        public string AsLeftStandardStdev { get; set; }
        public string Resolution { get; set; }
        public string MaxValue { get; set; }
        public string MinWeight{ get; set; }
    }

}
