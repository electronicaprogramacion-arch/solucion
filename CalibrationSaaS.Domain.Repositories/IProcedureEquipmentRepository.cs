using CalibrationSaaS.Domain.Aggregates.Entities;
using Helpers.Controls.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Domain.Repositories
{
    public interface IProcedureEquipmentRepository
    {
        // ProcedureEquipment CRUD operations
        Task<ProcedureEquipment> CreateProcedureEquipment(ProcedureEquipment procedureEquipment);
        Task<ResultSet<ProcedureEquipment>> GetProcedureEquipments(Pagination<ProcedureEquipment> pagination);
        Task<ProcedureEquipment> GetProcedureEquipmentByID(int id);
        Task<ProcedureEquipment> UpdateProcedureEquipment(ProcedureEquipment procedureEquipment);
        Task<ProcedureEquipment> DeleteProcedureEquipment(ProcedureEquipment procedureEquipment);

        // Association-specific methods
        Task<IEnumerable<ProcedureEquipment>> GetProceduresByEquipment(string pieceOfEquipmentId);
        Task<IEnumerable<ProcedureEquipment>> GetEquipmentByProcedure(int procedureId);
        Task<bool> IsProcedureAssociatedWithEquipment(int procedureId, string pieceOfEquipmentId);
        Task<ProcedureEquipment> GetProcedureEquipmentAssociation(int procedureId, string pieceOfEquipmentId);

        // Bulk operations
        Task<IEnumerable<ProcedureEquipment>> CreateMultipleProcedureEquipments(IEnumerable<ProcedureEquipment> procedureEquipments);
        Task<bool> DeleteProcedureEquipmentsByEquipment(string pieceOfEquipmentId);
        Task<bool> DeleteProcedureEquipmentsByProcedure(int procedureId);

        // Business logic methods
        Task<IEnumerable<Procedure>> GetAvailableProceduresForEquipment(string pieceOfEquipmentId);
        Task<IEnumerable<PieceOfEquipment>> GetAvailableEquipmentForProcedure(int procedureId);

        Task<bool> Save();
    }
}
