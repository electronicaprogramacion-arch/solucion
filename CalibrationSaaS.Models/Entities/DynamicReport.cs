using Bogus;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{

    [DataContract]
    public class DynamicReport : ICalibrationType
    {
        //[DataMember(Order = 1)]
        [NotMapped]
        public int CalibrationTypeId { get; set; }

        [Key]
        [DataMember(Order = 1)]
        public int DynamicReportID { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public string Description { get; set; }


        [DataMember(Order = 4)]
        public bool? ShowType { get; set; }

        [DataMember(Order = 5)]
        public string UrlReport { get; set; }


        [DataMember(Order = 6)]
        public bool HasNew { get; set; }

        [DataMember(Order = 7)]
        public bool? UsePreviousCalibrationData { get; set; }

        [NotMapped]
        [DataMember(Order = 8)]
        public virtual ICollection<CalibrationSubType> CalibrationSubTypes { get; set; }

    }


    [DataContract]
    public class Report_SubType
    {
        [Key]
        [DataMember(Order = 1)]
        public int DynamicReportID { get; set; }


        [DataMember(Order = 2)]
        public int SubTypeID { get; set; }


        [DataMember(Order = 3)]
        public int SubTypeView { get; set; }

      


    }

    [DataContract]
    public class SubType_DynamicProperty
    {
        [Key]
        [DataMember(Order = 1)]
        public int SubTypeID { get; set; }


        [DataMember(Order = 2)]
        public int DynamicPropertieID { get; set; }


        [DataMember(Order = 3)]
        public int ViewPropertyBaseID { get; set; }

        //[DataMember(Order = 4)]
        //public virtual ICollection<DynamicProperty> DynamicPropertiesSchema { get; set; }


    }
}
