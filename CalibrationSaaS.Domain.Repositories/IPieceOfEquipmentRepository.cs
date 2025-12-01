using CalibrationSaaS.Domain.Aggregates.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls.ValueObjects;

namespace CalibrationSaaS.Domain.Repositories
{
    public interface IPieceOfEquipmentRepository
    {


        Task<PieceOfEquipment> UpdateChildPieceOfEquipment(PieceOfEquipment pieceOfEquipmentDTO);
        Task<ResultSet<PieceOfEquipment>> GetSelectPOEChildren(Pagination<PieceOfEquipment> pagination);
        Task<CalibrationType> CreateNormalConfiguration(CalibrationType DTO);

        Task<IEnumerable<PieceOfEquipment>> GetPoeOff();
        Task<CalibrationType> GetDynamicConfiguration(int CalibrationTypeID,string Component="");
        Task<CalibrationSubType> EnableConfiguration(CalibrationSubType DTO);
        Task<CalibrationSubType> DeleteConfiguration(CalibrationSubType DTO);
        Task<CalibrationType> CreateConfiguration(CalibrationType DTO);
        Task<IEnumerable<PieceOfEquipment>> GetResolutionByMass(IEnumerable<PieceOfEquipment> DTO);

        Task<IEnumerable<PieceOfEquipment>> GetResolutionByLenght(IEnumerable<PieceOfEquipment> DTO);
        Task<IEnumerable<PieceOfEquipment>> GetPieceOfEquipmentByScale(string id);

        Task<ResultSet<POE_Scale>> GetPOEScale(Pagination<POE_Scale> pagination);
        

        Task<ResultSet<PieceOfEquipment>> GetPOEByTestCodePag(Pagination<PieceOfEquipment> pagination);
        Task<ResultSet<WeightSet>> SaveWeights(ICollection<WeightSet> W);

        Task<ResultSet<PieceOfEquipment>> GetAllPeripheralsPag(Pagination<PieceOfEquipment> pagination);
        
        Task<ResultSet<PieceOfEquipment>> GetAllWeightSetsPag(Pagination<PieceOfEquipment> pagination,bool includeDueDate=false,bool includeAcred=false ,bool includeinclude=true);

        Task<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentBalanceByIndicator(Pagination<PieceOfEquipment> pagination);

         Task<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentBalanceByPer(Pagination<PieceOfEquipment> pagination);

        Task<PieceOfEquipment> InsertPieceOfEquipment(PieceOfEquipment newPieceOfEquipment, string Component = "", bool returnQuery = true,bool Validate=false);

        Task<ResultSet<PieceOfEquipment>> GetPieceOfEquipment(Pagination<PieceOfEquipment> pagination);

        Task<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentChildren(Pagination<PieceOfEquipment> pagination);

        Task<PieceOfEquipment> GetPieceOfEquipmentByIDHeader(string poe);

         Task<PieceOfEquipment> GetPieceOfEquipmentByID(string id, string user = "", string Component = "", bool IsIndicator = false,bool LoadDynamic=true);

        Task<PieceOfEquipment> GetPieceOfEquipmentBySerial(string serial,int EquipmentTemplateID);

        Task<PieceOfEquipment> GetPieceOfEquipmentBySerial1(string serial);

        Task<PieceOfEquipment> DeletePieceOfEquipment(PieceOfEquipment newPieceOfEquipment);

        Task<PieceOfEquipment> UpdatePieceOfEquipment(PieceOfEquipment newPieceOfEquipment, string Component = "", bool returnQuery = true);

        //Task<IEnumerable<PieceOfEquipment>> GetPiecesOfEquipmentXDueDate(Tenant tenantID);
        Task<IEnumerable<PieceOfEquipment>> GetAllWeightSets(PieceOfEquipment DTO);

        //YPPP
        Task<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentIndicator(Pagination<PieceOfEquipment> poe);

        //YPPP
        Task<IEnumerable<WorkOrderDetail>> GetPieceOfEquipmentHistory(string id);
        Task<bool> Save();

        Task<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentByCustomer(Pagination<PieceOfEquipment> DTO);

        Task<IEnumerable<PieceOfEquipment>> GetPieceOfEquipmentByET(int id);

        Task<IEnumerable<PieceOfEquipment>> GetPieceOfEquipmentByCustomer(int id);

        Task<PieceOfEquipment> UpdateIndicator(PieceOfEquipment newPieceOfEquipment);

        Task<Uncertainty> CreateUncertainty(TableChanges<Uncertainty> PieceOfEquipmentDTO);

        Task<ResultSet<Uncertainty>> GetUncertainty(Pagination<Uncertainty> Uncertainty);

        //YPPP

        Task InsertCalibrationResultContributor(CalibrationResultContributor calibrationContributor);
        //Task<CalibrationResultContributor> DeletetCalibrationResultContributor(string idCalibrationResult);

        Task<IEnumerable<Uncertainty>> GetUncertaintyByPoe(string id);
        Task<IEnumerable<Uncertainty>> GetUncertaintyByEt(int id);
        Task<IEnumerable<CalibrationSubType_Weight>> GetCalibrationByWod(int wodId, int calibrationSubtypeId);
        Task<WeightSet> GetWeigthSetById(int weightId);
        Task<IEnumerable<WeightSet>> GetWeigthSets();
        Task<IEnumerable<PieceOfEquipment>> GetTemperatureStandard();

        Task<IEnumerable<PieceOfEquipment>> GetPieceOfEquipmentChildrenAll(PieceOfEquipment poe);

    }
}
