using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Domain.Repositories
{
    public interface IWorkOrderDetailRepository
    {

        Task<IEnumerable<ChildrenView>> GetChildrenView(int workOrderId);
        Task<IEnumerable<WorkOrderDetail>> GetWorkOrderDetailChildren(int workOrderId);

        Task SaveConfiguredWeights(int WorkOrderDetailID, BalanceAndScaleCalibration bc);
        Task<TestCode> DeleteTestCode(TestCode item);

        Task<TestCode>GetTestCodeByID(int item);

        Task<TestCode> GetTestCodeXName(TestCode DTO);


        Task<ResultSet<TestCode>> GetTestCodes(Pagination<TestCode> pagination);

        Task<TestCode> CreateTestCode(TestCode item);

        Task<ResultSet<WorkOrderDetail>> GetByTechnicianPag(Pagination<WorkOrderDetail> pagination);

        Task<WorkOrderDetail> GetWorkOrderDetailByID(int workOrderDetailId, bool IsGeneric=false, int? calibrationtype=null);

        Task<WorkOrderDetail> UpdateWorkOrderDetail(WorkOrderDetail workOrderDetail);

        Task AttachWorkOrderDetail(WorkOrderDetail workOrderDetail);

        Task<IEnumerable<WorkOrderDetail>> GetWorkOrderDetailByWorkOrderID(int workOrderDetailId);

        Task Save();

        Task<ResultSet<WorkOrderDetail>> GetWods(Pagination<WorkOrderDetail> pag);

        Task<ResultSet<WorkOrderDetail>> GetWodsFromQuery(Pagination<WorkOrderDetail> pagination);

        Task<IEnumerable<WorkOrderDetail>> GetAll();

        Task<WorkOrderDetail> Create(WorkOrderDetail DTO);

        Task<WorkOrderDetail> Delete(WorkOrderDetail DTO,bool Reset=false);

        Task<WorkOrderDetail> GetByID(WorkOrderDetail DTO);

        Task<IEnumerable<WorkDetailHistory>> GetHistory(WorkOrderDetail DTO);


        Task<WorkOrderDetail> GetHeaderById(WorkOrderDetail DTO);

        Task<WorkOrderDetail> ChangeStatus(WorkOrderDetail DTO,string Component="", bool forceUpdate = false, bool CustomId = false);


        Task<IEnumerable<WorkOrderDetail>> GetWorkOrderDetailXWorkOrder(WorkOrder DTO);
        //ValueTask<WorkOrderDetailResultSet> GetByCustomer(Customer DTO);
        Task<IEnumerable<WorkOrderDetail>> GetAllEnabled();

        Task<IEnumerable<Status>> GetStatus();

        Task<Status> SaveStatus(Status sta);

        Task<bool> CreateHistory(WorkDetailHistory DTO);

        Task<BalanceAndScaleCalibration> GetConfiguredWeights(int WorkOrderDetailID,  BalanceAndScaleCalibration bc);

        Task<ICollection<WorkOrderDetail>> GetByTechnician(User DTO);

        //YPPP
        Task<WorkDetailHistory> ChangeStatusComplete(WorkDetailHistory dto);

        Task<Certificate> CreateCertificate(Certificate DTO);

        Task<Certificate> GetCertificate(WorkOrderDetail DTO);

        Task<IEnumerable<int>> GetChartPie();

        Task<IEnumerable<int>> GetTotals();


        Task<IEnumerable<KeyValueDate>> GetWODCountPerDay();

        Task<IEnumerable<KeyValueDate>> GetWOCountPerDay();

        Task<NoteWOD> SaveNotes(NoteWOD DTO);
        Task<List<NoteWOD>> GetNotes(int Id, int Source = 1);
        Task<bool> RemoveNotes<TSource>(int Id, TSource DTO) where TSource : INoteWOD;
        Task<ICollection<CalibrationSubType_Standard>> GetCalibrationSubType_StandardByWodI(int id);
        Task<List<CalibrationSubType>> GetCalibrationSubType();

        Task DeleteDatabase();
        
        //YPP 
        Task<InstrumentThread> GetInstrumentThread(WorkOrderDetail DTO);

        Task<ResultsTable> GetResultsTable(WorkOrderDetail DTO);

        Task<WOD_ParametersTable> GetWOD_Parameter(WOD_ParametersTable DTO);

        Task<WOD_ParametersTable> SaveWOD_Parameter(WOD_ParametersTable DTO);

        Task<WorkOrderDetail> CalculateValuesByID(WorkOrderDetail DTO);
    
     	Task<WorkOrderDetail> UpdateOfflineID(WorkOrderDetail DTO);

        //9503
        Task<WorkOrderDetail> GetByIDPreviousCalibration(WorkOrderDetail  DTO);

    }
     
}
