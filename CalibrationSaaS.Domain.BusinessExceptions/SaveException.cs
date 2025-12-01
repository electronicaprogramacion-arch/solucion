using CalibrationSaaS.Domain.Aggregates.Entities;
using System;

namespace CalibrationSaaS.Domain.BusinessExceptions
{
    [Serializable]
    public class BussinesException<T> : Exception
    {
        

        public string IdentityException { get; set; }

        public T Entity { get; set; }
        public BussinesException() { }
        public BussinesException(string message) : base(message)
        {

        }
        public BussinesException(string message, Exception inner) : base(message, inner)
        {

        }

        public  BussinesException(string message, Exception inner, T entity) : base(message, inner)
        {
            //this.Identity = entity.EquipmentTemplateID.ToString();
            Entity = entity;

        }
    }
}
