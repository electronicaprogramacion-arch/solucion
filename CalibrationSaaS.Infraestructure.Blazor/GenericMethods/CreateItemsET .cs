using Bogus;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Querys;
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

namespace CalibrationSaaS.Infraestructure.Blazor.GenericMethods
{
    public class CreateItemsET : CreateBase<EquipmentTemplate, GenericCalibration2, GenericCalibrationResult2>, ICreateItems<EquipmentTemplate, GenericCalibration2>//ComponentBase, ICreateItems<PieceOfEquipment,GenericCalibration2>
    {

        public async Task<List<GenericCalibration2>> CreateItems(EquipmentTemplate eq, int CalibrationSubTypeId, string GroupName = null)
        {
            

            return await base.CreateItems(eq, CalibrationSubTypeId);



           

        }

    }
}
