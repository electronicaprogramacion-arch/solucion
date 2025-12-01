using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
     [DataContract]
    public class Blog
    {


        [DataMember(Order = 1)]
        public string Author { get; set; }

        [DataMember(Order = 2)]
        public string Commnet { get; set; }

        [DataMember(Order = 3)]
        public DateTime Date { get; set; }


    }
}
