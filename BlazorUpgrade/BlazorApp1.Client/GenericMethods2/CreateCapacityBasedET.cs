using Bogus;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Helpers.Models;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Application.Services;

namespace CalibrationSaaS.Infraestructure.Blazor.GenericMethods
{
    public class CreateCapacityBasedET : ComponentBase, ICreateItems<EquipmentTemplate, GenericCalibration2>
    {
        public AppState AppState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Func<dynamic, IPieceOfEquipmentService<CallContext>> PoeServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public CreateModel Parameter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task<List<GenericCalibration2>> CreateItems(EquipmentTemplate eq, int CalibrationSubTypeId, string GroupName = null)
        {
            throw new NotImplementedException();
        }
    }
}
