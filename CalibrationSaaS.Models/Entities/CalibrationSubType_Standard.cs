using CalibrationSaaS.Domain.Aggregates.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class CalibrationSubType_Standard 
    {
        [DataMember(Order = 1)]
        public int? WorkOrderDetailID{ get; set; }

        [DataMember(Order = 2)]
        public string PieceOfEquipmentID { get; set; }


        [DataMember(Order = 3)]
        public int CalibrationSubTypeID { get; set; }


        [DataMember(Order = 4)]
        public int? SecuenceID { get; set; }


        [IgnoreDataMember]
        public virtual CalibrationSubType CalibrationSubType { get; set; }

        [DataMember(Order = 7)]
        public virtual PieceOfEquipment Standard { get; set; }


        private string componentID;
        [DataMember(Order = 5)]
        public string ComponentID
        {
            get
            {

                if (string.IsNullOrEmpty(componentID))
                {
                    return WorkOrderDetailID.ToString();
                }
                return this.componentID;

            }
            set
            {

                componentID = value;

            }

        }


        [DataMember(Order = 6)]
        public  string Component { get; set; }

    }
}
