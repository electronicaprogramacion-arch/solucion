using System;

namespace CalibrationSaaS.Domain.BusinessExceptions.Customer
{
    [Serializable]
    public class ExistingPieceOfEquipmentException : Exception
    {
        public string serial;
        public ExistingPieceOfEquipmentException() { }
        public ExistingPieceOfEquipmentException(string message) : base(message)
        {

        }
        public ExistingPieceOfEquipmentException(string message, Exception inner) : base(message, inner)
        {

        }

        public ExistingPieceOfEquipmentException(string message, Exception inner, CalibrationSaaS.Domain.Aggregates.Entities.PieceOfEquipment pieceOfEquipment) : base(message, inner)
        {
            this.serial = pieceOfEquipment.SerialNumber;
        }
    }
}
