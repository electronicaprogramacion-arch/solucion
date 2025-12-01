using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class User_Rol 
    {
        [DataMember(Order = 1)]
        public int UserID { get; set; }

        [DataMember(Order = 2)]
        public int RolID { get; set; }       

        [DataMember(Order = 3)]
        public virtual User User { get; set; }

        [DataMember(Order = 4)]
        public virtual Rol Rol { get; set; }

        [DataMember(Order = 5)]
        public string Permissions { get; set; }

        

    }
}
