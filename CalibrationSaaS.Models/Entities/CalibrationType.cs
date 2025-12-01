    using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public partial class CalibrationType: CalibrationSaaS.Domain.Aggregates.Interfaces.ICalibrationType, IGeneric
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [DataMember(Order = 1)]
        public int CalibrationTypeId { get; set; }
        
        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public string Description { get ; set ; }

        [NotMapped]
        [System.ComponentModel.DataAnnotations.Editable(false)]
        [IgnoreDataMember]
        public virtual ICollection<WorkOrderDetail> WorkOrderDetails { get; set; }

        [System.ComponentModel.DataAnnotations.Editable(false)]
        [DataMember(Order = 4)]
         public virtual ICollection<CalibrationSubType> CalibrationSubTypes { get; set; }

        [System.ComponentModel.DataAnnotations.Editable(false)]
        [DataMember(Order = 5)]
        public virtual ICollection<ToleranceType> ToleranceTypes { get; set; }


        [DataMember(Order = 6)]
        public bool? ShowType { get; set; }


        [DataMember(Order = 7)]
        public string UrlReport { get; set; }

        [DataMember(Order = 8)]
        public bool HasNew { get ; set ; }

        [NotMapped]
        [DataMember(Order = 9)]
        public virtual ICollection<CMCValues> CMCValues { get; set; }


        [DataMember(Order = 10)]
        public double? MaxHumidity { get; set; }

        [DataMember(Order = 11)]
        public double? MinHumidity { get; set; }

        [DataMember(Order = 12)]
        public string? UoMHum { get; set; }

        [DataMember(Order = 13)]
        public double? MaxTemperature { get; set; }

        [DataMember(Order = 14)]
        public double? MinTemperature { get; set; }

        [DataMember(Order = 15)]
        public string? UoMTemperature { get; set; }

        [NotMapped]
        [DataMember(Order = 16)]
        public string Component { get; set; }

        [DataMember(Order = 18)]
        public bool? IsNivel1 { get; set; }


        //[DataMember(Order = 16)]
        //public bool? UsePreviousCalibrationData { get; set; }

        [DataMember(Order = 19)]
        public bool? UsePreviousCalibrationData { get; set; }

        [DataMember(Order = 20)]
        public string? JsonUncertaintyConfiguration { get; set; }

        [DataMember(Order = 21)]
        public bool? ShowComposite { get; set; }

        [DataMember(Order = 22)]
        public string? CustomerComposite { get; set; }
    }
}
