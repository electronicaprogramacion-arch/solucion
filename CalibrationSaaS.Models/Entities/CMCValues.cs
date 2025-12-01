using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using static System.Net.Mime.MediaTypeNames;


namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public partial class CMCValues : IGeneric
    {
        [Key]
        //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [DataMember(Order = 1)]
        public int CMCValueId { get; set; }

        [DataMember(Order = 2)]
        public int CalibrationTypeId { get; set; }

        [DataMember(Order = 3)]
        public bool IncludeEqualsMin { get; set; }


        [DataMember(Order = 4)]
        public double MinRange { get; set; }

        [DataMember(Order = 5)]
        public double MaxRange { get; set; }

        [DataMember(Order = 6)]
        public bool IncludeEqualsMax { get; set; }

        [DataMember(Order = 7)]
        public int UnitofMeasureid { get; set; }

        [DataMember(Order = 8)]
        public double CMC { get; set; }

        [DataMember(Order = 9)]
        public double CMCUoM { get; set; }

        [DataMember(Order = 10)]
        public int Position { get; set; }

    }
}
