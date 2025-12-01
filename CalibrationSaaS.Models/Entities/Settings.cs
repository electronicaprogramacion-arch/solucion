using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
   


    [DataContract]
    public  class Settings: IGeneric
    {
        //public CustomerAddress()
        //{
        //    WorkOrder = new HashSet<WorkOrder>();
        //}
        [Key]
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int SettingsID { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 2)]
        public int Key { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 3)]
        public string Value { get; set; }
     
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 4)]
        public string Type { get; set; }

      

    }

    //[DataContract]
    //public class StatusPossibilities
    //{

    //    [Required(ErrorMessage = "Required")]
    //    [DataMember(Order = 1)]
    //    public int StatusPossibilitiesID { get; set; }


       



    //}



    }
