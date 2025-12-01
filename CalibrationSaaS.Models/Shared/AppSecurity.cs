using CalibrationSaaS.Domain.Aggregates.Entities;
using Helpers.Controls;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace CalibrationSaaS.Domain.Aggregates.Shared
{
    public class VersionApp
    {
        public const string Version = "VersionApp";

        public string Title { get; set; }
        //public string Name { get; set; }
    }


    public class AppSecurity
    {
        public AppSecurity()
        {
            WorkOrderDetailOptions.Add(1,"ISO 7500");
            WorkOrderDetailOptions.Add(2,"ASTM E4");
        }

        public bool IsOffline { get; set; }
        public bool AlreadyLoad { get; set; }

        public bool AlreadyLoadOff { get; set; }

        public ICollection<Rol> RolesList { get; set; }
        public ICollection<KeyValue> PageRoles { get; set; }

        public ICollection<KeyValue> Permissions { get; set; } = new List<KeyValue>();

        public ICollection<CustomSequence> CustomSequences{ get; set; }

        public ICollection<Component> Components { get; set; }

        public ICollection<User> UserList { get; set; }

        public ICollection<Certification> CertificationList { get; set; }

        public string Token { get; set; }


        public string AssemblyName { get; set; }

         public bool ShowCertification { get; set; }


         public
        ICollection<CalibrationType> CalibrtionTypeList { get; set; }

        public
       ICollection<EquipmentType> EquipmentTypeWODList
        { get; set; }

        public
        ICollection<EquipmentType> EquipmentTypeList
        { get; set; }

        public Dictionary<int, string> WorkOrderDetailOptions { get; set; } = new Dictionary<int, string>();

        public ICollection<Status> StatusList { get; set; }

        public IEnumerable<WOStatus> WOStatusList { get; set; }

        public bool IsNotGrpc { get; set; }

        public int? CurrentUserID { get; set; }

        public bool IsReport { get; set; }

        public ClaimsPrincipal Principal { get; set; }

    }
}
