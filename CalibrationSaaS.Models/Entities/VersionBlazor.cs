using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
     [DataContract]
    public class VersionBlazor
    {


        [NotMapped()]
        [DataMember(Order = 1)]
        public double Version { get; set; }



    }
}
