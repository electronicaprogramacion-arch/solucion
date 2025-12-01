using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.Services
{

    [ServiceContract(Name = "CalibrationSaaS.Application.Services.WorkOrderDetailServices")]
    public interface IWorkOrderDetailServices<T>
    {

        ValueTask<IEnumerable<ChildrenView>> GetChildrenView(WorkOrderDetail workOrderDetail, T context);

        ValueTask<IEnumerable<WorkOrderDetail>> GetWorkOrderDetailChildren(WorkOrderDetail workOrderDetail, T context);

        ValueTask<ResultSet<WorkOrderDetail>> GetWods(Pagination<WorkOrderDetail> pag, T context);

        ValueTask<ResultSet<WorkOrderDetail>> GetWodsFromQuery(Pagination<WorkOrderDetail> pag, T context);

        ValueTask<TestCode>GetTestCodeByID(TestCode item, T context);


        ValueTask<TestCode>DeleteTestCode(TestCode item, T context);


        ValueTask<ResultSet<TestCode>> GetTestCodes(Pagination<TestCode> pagination, T context);

        ValueTask<TestCode> CreateTestCode(TestCode item, T context);
        Task<TestCode> GetTestCodeXName(TestCode DTO, T context);

        ValueTask<ResultSet<WorkOrderDetail>> GetByTechnicianPag(Pagination<WorkOrderDetail> pagination, T context);

        ValueTask<WorkOrderDetailResultSet> GetAll( T context);

        ValueTask<WorkOrderDetail> Create(WorkOrderDetail DTO, T context);

        ValueTask<WorkOrderDetail> Reset(WorkOrderDetail DTO, T context);
        ValueTask<WorkOrderDetail> Delete(WorkOrderDetail DTO, T context);

        ValueTask<WorkOrderDetail> GetByID(WorkOrderDetail DTO, T context);

        ValueTask<ICollection<WorkOrderDetail>> GetByTechnician(User DTO, T context);

        ValueTask<WorkOrderDetailHistoryResultSet> GetHistory(WorkOrderDetail DTO, T context);
           

        ValueTask<WorkOrderDetail> ChangeStatus(WorkOrderDetail DTO,  T context);

        ValueTask<WorkOrderDetailResultSet> GetWorkOrderDetailXWorkOrder(WorkOrder DTO, T context);
       
        ValueTask<WorkOrderDetailResultSet> GetAllEnabled(T context);

        ValueTask<IEnumerable<Status>> GetStatus(T context);

        ValueTask<WorkOrderDetail> GetWorkOrderDetailByID(WorkOrderDetail DTO, T context);

        ValueTask<WorkOrderDetail> SaveWod(WorkOrderDetail DTO, T context);

        ValueTask<WorkOrderDetail> GetConfiguredWeights(WorkOrderDetail DTO, T context);
        
        ValueTask<WorkDetailHistory> ChangeStatusComplete(WorkDetailHistory dto, T context);

        ValueTask<Certificate> CreateCertificate(Certificate DTO, T context);

        ValueTask<Certificate> GetCertificate(WorkOrderDetail DTO, T context);

        Task<IEnumerable<int>> GetChartPie(T context);

        Task<IEnumerable<int>> GetTotals(T context);

        Task<IEnumerable<KeyValueDate>> GetWODCountPerDay(T context);

        Task<IEnumerable<KeyValueDate>> GetWOCountPerDay(T context);

        Task<List<CalibrationSubType>> GetCalibrationSubtype(T context);

        ValueTask<WorkOrderDetail> SaveWodOff(WorkOrderDetail DTO, T context);



        //YPP

        Task<InstrumentThread> GetInstrumentThread(WorkOrderDetail DTO, T context);

        Task<List<GenericCalibrationResult2>> GetResultsTable(WorkOrderDetail DTO, T context);

        Task<WOD_ParametersTable> GetWOD_Parameter(WOD_ParametersTable DTO, T context);

        //Task<WorkOrderDetail> CalculateValuesByID(WorkOrderDetail DTO, T context);

        //9503
        ValueTask<WorkOrderDetail> GetByIDPreviousCalibration(WorkOrderDetail DTO,  T context);




    }
}
