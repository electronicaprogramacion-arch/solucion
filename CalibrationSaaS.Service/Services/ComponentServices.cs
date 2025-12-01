using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Infraestructure.Grpc.Helpers;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using Grpc.Core;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using Component = Helpers.Controls.Component;

namespace CalibrationSaaS.Infraestructure.GrpcServices.Services
{
    public class ComponentServices :ServiceBase, IComponentServices<CallContext>
    {
        private readonly BasicsUseCases  basicsLogic;
        private readonly ValidatorHelper modelValidator;
        private readonly ILogger _logger;
        public ComponentServices(BasicsUseCases basicsLogic, ILogger<BasicsServices> logger, ValidatorHelper modelValidator)
        {
            this.basicsLogic = basicsLogic;
            this.modelValidator = modelValidator;
            this._logger = logger;


        }


        public async Task<Component> DeleteComponent(Component Component, CallContext context)
        {
            var result = await basicsLogic.DeleteComponent(Component);

            return result;
        }

        public async  Task<ResultSet<Component>> CreateComponents(ICollection<Component> Component, CallContext context)
        {
            var result = await basicsLogic.CreateComponents(Component);

            return result;
        }


        public async Task<ResultSet<CustomSequence>> CustomSequences(CallContext context)
        {
            var result = await basicsLogic.CustomSequences();

            return result;
        }

        public async Task<ResultSet<Component>> GetAllComponents(CallContext context)
        {
            var result = await basicsLogic.GetAllComponents();

            return result;
        }

        public async ValueTask<Component> GetComponentByRoute(string route)
        {
            var result = await basicsLogic.GetComponentByRoute(route);

            return result;
        }


    }
}
