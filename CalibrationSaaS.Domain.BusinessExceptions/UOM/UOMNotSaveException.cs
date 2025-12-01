using CalibrationSaaS.Domain.Aggregates.Entities;
using System;
using System.Runtime.CompilerServices;

namespace CalibrationSaaS.Domain.BusinessExceptions
{
    [Serializable]
    public class UOMNotSaveException : BussinesException<UnitOfMeasure>
    {
       
        

        public UOMNotSaveException(string message):base(message)
        {
            
        }


    }
}
