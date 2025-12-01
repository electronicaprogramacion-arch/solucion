using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{

    [DataContract]
    public  class Tenant
    {
        [DataMember(Order = 1)]
        public int TenantId { get; set; }
        [DataMember(Order = 2)]
        public string Name { get; set; }
    }
}
