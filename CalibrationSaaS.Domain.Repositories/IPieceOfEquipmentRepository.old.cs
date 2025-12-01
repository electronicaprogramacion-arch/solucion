using CalibrationSaaS.Domain.Aggregates.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Domain.Repositories
{
    public interface IPieceOfEquipmentRepository
    {
        Task<PieceOfEquipment> InsertPieceOfEquipment(PieceOfEquipment newPieceOfEquipment);

        Task<IEnumerable<PieceOfEquipment>> GetPieceOfEquipment();

        Task<PieceOfEquipment> GetPieceOfEquipmentByID(PieceOfEquipment newPieceOfEquipment);

        Task<PieceOfEquipment> GetPieceOfEquipmentBySerial(string serial);

        Task<PieceOfEquipment> DeletePieceOfEquipment(PieceOfEquipment newPieceOfEquipment);

        Task<PieceOfEquipment> UpdatePieceOfEquipment(PieceOfEquipment newPieceOfEquipment);

        //Task<IEnumerable<PieceOfEquipment>> GetPiecesOfEquipmentXDueDate(Tenant tenantID);

        Task<bool> Save();
    }
}
