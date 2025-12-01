using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls.ValueObjects;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.GrpcServices.Services
{
    public class EquipmentRecallService : ServiceBase, IEquipmentRecallService<CallContext>
    {
        private readonly EquipmentRecallUseCases equipmentRecallUseCases;
        private readonly ILogger<EquipmentRecallService> _logger;

        public EquipmentRecallService(
            EquipmentRecallUseCases equipmentRecallUseCases,
            ILogger<EquipmentRecallService> logger)
        {
            this.equipmentRecallUseCases = equipmentRecallUseCases;
            this._logger = logger;
        }

        public async ValueTask<EquipmentRecallCollectionResult> GetEquipmentRecalls(EquipmentRecallFilter filter, CallContext context)
        {
            try
            {
                var recalls = await equipmentRecallUseCases.GetEquipmentRecalls(filter);
                var recallsList = recalls.ToList();

                return new EquipmentRecallCollectionResult
                {
                    EquipmentRecalls = recallsList,
                    TotalCount = recallsList.Count,
                    OverdueCount = recallsList.Count(r => r.DueDate < System.DateTime.Now),
                    Success = true,
                    Message = ""
                };
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error getting equipment recalls");
                return new EquipmentRecallCollectionResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async ValueTask<ResultSet<EquipmentRecall>> GetEquipmentRecallsPaginated(Pagination<EquipmentRecall> pagination, CallContext context)
        {
            try
            {
                return await equipmentRecallUseCases.GetEquipmentRecallsPaginated(pagination);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error getting paginated equipment recalls");
                return new ResultSet<EquipmentRecall>
                {
                    List = new List<EquipmentRecall>(),
                    Count = 0
                };
            }
        }

        public async ValueTask<ExportResult> ExportEquipmentRecallsToExcel(EquipmentRecallFilter filter, CallContext context)
        {
            try
            {
                var excelData = await equipmentRecallUseCases.ExportEquipmentRecallsToExcel(filter);
                
                return new ExportResult
                {
                    Data = excelData,
                    FileName = $"EquipmentRecalls_{System.DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                    Success = true,
                    Message = ""
                };
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error exporting equipment recalls to Excel");
                return new ExportResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async ValueTask<CountResult> GetEquipmentRecallsCount(EquipmentRecallFilter filter, CallContext context)
        {
            try
            {
                var count = await equipmentRecallUseCases.GetEquipmentRecallsCount(filter);
                
                return new CountResult
                {
                    Count = count,
                    Success = true,
                    Message = ""
                };
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error getting equipment recalls count");
                return new CountResult
                {
                    Count = 0,
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async ValueTask<CountResult> GetOverdueEquipmentCount(CustomerFilterRequest request, CallContext context)
        {
            try
            {
                // Get all equipment recalls and count overdue ones
                var filter = new EquipmentRecallFilter();
                if (request.HasCustomerFilter)
                {
                    filter.CustomerID = request.CustomerID;
                }

                var recalls = await equipmentRecallUseCases.GetEquipmentRecalls(filter);
                var overdueCount = recalls.Count(r => r.DueDate < System.DateTime.Now);

                return new CountResult
                {
                    Count = overdueCount,
                    Success = true,
                    Message = ""
                };
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error getting overdue equipment count");
                return new CountResult
                {
                    Count = 0,
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
