using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls.ValueObjects;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.Services
{
    [ServiceContract(Name = "CalibrationSaaS.Application.Services.PieceOfEquipmentService")]
    public interface IPieceOfEquipmentService<T>
    {

        ValueTask<PieceOfEquipment> UpdateChildPieceOfEquipment(PieceOfEquipment pieceOfEquipmentDTO, T context = default);

        ValueTask<ResultSet<PieceOfEquipment>> GetSelectPOEChildren(Pagination<PieceOfEquipment> pagination, T context = default);
        //ValueTask<CalibrationType> GetConfiguration(CalibrationType DTO, T context = default);
        ValueTask<CalibrationType> GetDynamicConfiguration(CalibrationType CalibrationType, T context = default);
        ValueTask<CalibrationSubType> EnableConfiguration(CalibrationSubType DTO, T context = default);
        ValueTask<CalibrationSubType> DeleteConfiguration(CalibrationSubType DTO, T context = default);
        ValueTask<CalibrationType> CreateConfiguration(CalibrationType DTO, T context = default);
        ValueTask<IEnumerable<PieceOfEquipment>> GetResolutionByMass(IEnumerable<PieceOfEquipment> DTO, T context = default);

        ValueTask<IEnumerable<PieceOfEquipment>> GetResolutionByLenght(IEnumerable<PieceOfEquipment> DTO, T context = default);

        ValueTask<IEnumerable<PieceOfEquipment>> GetPieceOfEquipmentByScale(string id, T context = default);

        ValueTask<ResultSet<POE_Scale>> GetPOEScale(Pagination<POE_Scale> pagination, T context = default);
        ValueTask<ResultSet<PieceOfEquipment>> GetPOEByTestCodePag(Pagination<PieceOfEquipment> pagination,T context = default);
         ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentBalanceByPer(Pagination<PieceOfEquipment> DTO, T context = default);
        Task<ResultSet<WeightSet>> SaveWeights(ICollection<WeightSet> W, T context = default);
        Task<ResultSet<PieceOfEquipment>> GetAllPeripheralsPag(Pagination<PieceOfEquipment> pagination, T context = default);
        Task<ResultSet<PieceOfEquipment>> GetAllWeightSetsPag(Pagination<PieceOfEquipment> pagination, T context = default);
        ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentBalanceByIndicator(Pagination<PieceOfEquipment> DTO, T context = default);
        ValueTask<PieceOfEquipment> PieceOfEquipmentCreate(PieceOfEquipment PieceOfEquipmentDTO, T context);
        ValueTask<ResultSet<PieceOfEquipment>> GetPiecesOfEquipmentXDueDate(Pagination<PieceOfEquipment> pagination, T context);

        ValueTask<ResultSet<PieceOfEquipment>> GetPiecesOfEquipmentChildren(Pagination<PieceOfEquipment> pagination, T context);
        ValueTask<PieceOfEquipmentResultSet> GetPieceOfEquipment (Pagination<PieceOfEquipment> pagination, T context);

      
        ValueTask<PieceOfEquipment> GetPieceOfEquipmentXId(PieceOfEquipment PieceOfEquipmentDTO, T context);
        ValueTask<PieceOfEquipment> DeletePieceOfEquipment(PieceOfEquipment PieceOfEquipmentDTO, T context);
        ValueTask<PieceOfEquipment> UpdatePieceOfEquipment(PieceOfEquipment PieceOfEquipmentDTO,  T context);
       ValueTask<PieceOfEquipmentResultSet> GetAllWeightSets(PieceOfEquipment DTO, T context);

       
        ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentIndicator(Pagination<PieceOfEquipment> DTO, T context);
       
        ValueTask<WorkOrderDetailResultSet> GetPieceOfEquipmentHistory(PieceOfEquipment DTO, T context);

        ValueTask<PieceOfEquipmentResultSet> GetPieceOfEquipmentByET(EquipmentTemplate DTO, T context);

       
        ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentByCustomer(Pagination<PieceOfEquipment> Pagination, T context);

        ValueTask<PieceOfEquipmentResultSet> GetPieceOfEquipmentByCustomerId(Customer DTO, T context);

        ValueTask<PieceOfEquipment> UpdateIndicator(PieceOfEquipment PieceOfEquipmentDTO, T context);

        ValueTask<Uncertainty> CreateUncertainty(TableChanges<Uncertainty> PieceOfEquipmentDTO, T context);

        ValueTask<ResultSet<Uncertainty>> GetUncertainty(Pagination<Uncertainty> Uncertainty, T context);
       
        ValueTask<ICollection<Force>> GetCalculatesForISOandASTM(ISOandASTM ISOandASTM, T context);

        ValueTask<ICollection<PieceOfEquipment>> GetTemperatureStandard( T context = default);

        ValueTask<ICollection<Force>> CalculateUncertainty(List<Force> forces, int iso, T context);

        ValueTask<ICollection<PieceOfEquipment>>GetPieceOfEquipmentChildrenAll(PieceOfEquipment PieceOfEquipmentDTO, T context = default); 

    }
}
