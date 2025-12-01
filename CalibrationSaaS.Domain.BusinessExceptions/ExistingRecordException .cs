using CalibrationSaaS.Domain.Aggregates.Entities;
using System;

namespace CalibrationSaaS.Domain.BusinessExceptions
{
    [Serializable]
    public class ExistingRecordException<T> : Exception
    {
        

        public string IdentityException { get; set; }

        public T Entity { get; set; }
        public ExistingRecordException() { }
        public ExistingRecordException(string message) : base(message)
        {

        }
        public ExistingRecordException(string message, Exception inner) : base(message, inner)
        {

        }

        public  ExistingRecordException (string message, Exception inner, T entity) : base(message, inner)
        {
            //this.Identity = entity.EquipmentTemplateID.ToString();
            Entity = entity;

        }
    }
}
