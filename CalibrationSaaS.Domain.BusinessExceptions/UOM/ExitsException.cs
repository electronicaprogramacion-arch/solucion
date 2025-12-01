using CalibrationSaaS.Domain.Aggregates.Entities;
using System;

namespace CalibrationSaaS.Domain.BusinessExceptions
{
    [Serializable]
    public class UOMExistingException : Exception
    {
        

        public string IdentityException { get; set; }

        public UnitOfMeasure Entity { get; set; }
        public UOMExistingException() { }
        public UOMExistingException(string message) : base(message)
        {

        }
        public UOMExistingException(string message, Exception inner) : base(message, inner)
        {

        }

        public UOMExistingException(string message, Exception inner, UnitOfMeasure entity) : base(message, inner)
        {
            //this.Identity = entity.EquipmentTemplateID.ToString();
            Entity = entity;
            IdentityException = "Name" + " or " + "abbreviature exists";
        }
    }
}
