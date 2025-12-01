using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Helpers.Controls
{
    [DataContract]
    public class CustomSequence 
    {

        [Key]
        [Required]
        [DataMember(Order = 1)]
        public int CustomSequenceID { get; set; }

        [Required]
        [DataMember(Order = 2)]
        public int SequenceID { get; set; }


       
        [DataMember(Order = 3)]
        public int? NextSequenceID { get; set; }



        [DataMember(Order = 4)]
        public int? Min { get; set; }

        [DataMember(Order = 5)]
        public int? Max { get; set; }


        [Required]
        [DataMember(Order = 6)]
        public string Component { get; set; }

        [Required]
        [DataMember(Order = 7)]
        public string Tenant { get; set; }


       
        [DataMember(Order = 8)]
        public string? Prefix { get; set; }


        [DataMember(Order = 9)]
        public int Type { get; set; }


    }
}
