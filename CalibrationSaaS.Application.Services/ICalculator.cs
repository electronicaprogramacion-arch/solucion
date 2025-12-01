using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.Services
{
    [ServiceContract(Name= "CalibrationSaaS.Service.Calculator")]
    public interface ICalculator
    {
        ValueTask<MultiplyResult> MultiplyAsync(MultiplyRequest request); 
    }

    [DataContract]
    public class MultiplyRequest
    {
        [DataMember(Order =1)]
        public int X { get; set; }
        [DataMember(Order = 2)]
        public int Y { get; set; }
    }

    [DataContract]
    public class MultiplyResult
    {
        [DataMember(Order = 1)]
        public int Result { get; set; }
    }
}
