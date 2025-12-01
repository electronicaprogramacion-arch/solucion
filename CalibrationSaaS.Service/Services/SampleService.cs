using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using ProtoBuf.Grpc;
using System;
using System.Threading.Tasks;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
namespace CalibrationSaaS.Infraestructure.GrpcServices.Services
{
    public class SampleService : ISampleService<CallContext>
    {
        // private readonly SampleUseCases customerLogic;
        //private readonly ILogger _logger;
        //private readonly ValidatorHelper modelValidator;
        //public System.Net.Http.HttpClient Http { get; set; }
        //IConfiguration Configuration { get; set; }

        // public SampleService(SampleUseCases customerLogic, ILogger<SampleService> logger, ValidatorHelper modelValidator)
        public SampleService()
        {
            // this.customerLogic = customerLogic;
            //this._logger = logger;
            //this.modelValidator = modelValidator;
        }

        public ValueTask<Domain.Aggregates.Entities.Customer> CreateCustomer(Domain.Aggregates.Entities.Customer customerDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<CustomerResultSet> GetCustomers(TenantDTO tenantID, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<WorkOrderDetail>> GetWOD(Pagination<WorkOrderDetail> pag, CallContext context)
        {
            throw new NotImplementedException();
        }
    }
}