using CalibrationSaaS.Domain.Aggregates.Entities;
using System;

namespace CalibrationSaaS.Domain.BusinessExceptions.Customer
{
    [Serializable]
    public class ExistingEquipmentTemplateException : Exception
    {
        public string Identity;
        public ExistingEquipmentTemplateException() { }
        public ExistingEquipmentTemplateException(string message) : base(message)
        {

        }
        public ExistingEquipmentTemplateException(string message, Exception inner) : base(message, inner)
        {

        }

        public ExistingEquipmentTemplateException(string message, Exception inner, EquipmentTemplate entity) : base(message, inner)
        {
            this.Identity = entity.EquipmentTemplateID.ToString();
        }
    }
}
