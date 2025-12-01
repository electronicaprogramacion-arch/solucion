
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
  

     [DataContract]
    public class TestCode :INote
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [DataMember(Order = 1)]
        public int TestCodeID { get; set; }

        [DataMember(Order = 2)]
        public string Code { get; set; }

        [DataMember(Order = 3)]
        public string  Description  { get; set; }

        [DataMember(Order = 4)]
        public Double RangeMIn { get; set; }

         [DataMember(Order = 5)]
        public Double RangeMax { get; set; }


        //[NotMapped]
        [DataMember(Order = 6)]
        public int? CalibrationTypeID { get; set; }



        [DataMember(Order = 8)]
        public bool Accredited { get; set; }


        [DataMember(Order = 9)]
        public int? UnitOfMeasureID { get; set; }

        private UnitOfMeasure _UnitOfMeasure;
        [DataMember(Order = 10)]
        public virtual UnitOfMeasure? UnitOfMeasure { get { if (NullableCollections) { return null; } else { return _UnitOfMeasure; } } set { _UnitOfMeasure = value; } }

        private CalibrationType _CalibrationType;
        [NotMapped]
         [DataMember(Order = 11)]
        public virtual CalibrationType? CalibrationType { get { if (NullableCollections) { return null; } else { return _CalibrationType; } } set { _CalibrationType = value; } }



        [DataMember(Order = 12)]
        public int? ProcedureID { get; set; }

        private Procedure _Procedure;
           [DataMember(Order = 13)]
        public virtual Procedure? Procedure { get { if (NullableCollections) { return null; } else { return _Procedure; } } set { _Procedure = value; } }


        private ICollection<Note> _Notes;
        [DataMember(Order = 14)]
        public virtual ICollection<Note> Notes { get { if (NullableCollections) { return null; } else { return _Notes; } } set { _Notes = value; } }


        [DataMember(Order = 15)]
        public int?  EquipmentTypeID { get; set; }

        //[DataMember(Order = 16)]
        //public EquipmentType EquipmentType { get; set; }


        [NotMapped]
        public bool NullableCollections { get; set; }


        [DataMember(Order = 16)]
        public int? EquipmentTypeGroupID { get; set; }
        


    }


}
