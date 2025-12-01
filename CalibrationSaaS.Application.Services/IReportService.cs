using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.Services
{
    [ServiceContract(Name = "CalibrationSaaS.Application.Services.ReportService")]
    public interface IReportService<T>
    {
        

        ValueTask<ReportResultSet> GetWorkOrderDetailXIdRepWithSave(WorkOrderDetail wo);
        
        ValueTask<ReportResultSet> GetWorkOrderDetailXIdRep(WorkOrderDetail wo);

        ValueTask<string> GetReportUncertaintyBudget(Linearity DTO, T context);
        ValueTask<string> GetReportUncertaintyBudgetComp(WorkOrderDetail wo, T context);
        ValueTask<string> GetSticker(WorkOrderDetail wo, T context);
        string GetUrlServer();
    }
}
