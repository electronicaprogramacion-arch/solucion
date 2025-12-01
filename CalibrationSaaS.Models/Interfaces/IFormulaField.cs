using CalibrationSaaS.Domain.Aggregates.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Domain.Aggregates.Interfaces
{
    public interface IFormula
    {

        Task<object> FormulaMethod(dynamic gcrObject, PieceOfEquipment poe, CalibrationType calibrationType);



    }
}