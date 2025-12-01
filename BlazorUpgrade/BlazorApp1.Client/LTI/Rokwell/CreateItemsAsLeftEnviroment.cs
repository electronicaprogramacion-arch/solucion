using Bogus;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Order;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using Reports.Domain.ReportViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.Rockwell
{
    public class CreateItemsAsLeftEnviroment : ComponentBase, ICreateItems
    {

        public AppState AppState { get; set; }
        public IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
        public CreateModel Parameter { get; set; }
        public async Task<List<GenericCalibration>> CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId)
        {
            List<GenericCalibration> list = new List<GenericCalibration>();

            var cal = new GenericCalibration();

            cal.SequenceID = 1;
            cal.WorkOrderDetailId = eq.WorkOrderDetailID;
            cal.CalibrationSubTypeId = CalibrationSubTypeId;
            cal.BasicCalibrationResult = new GenericCalibrationResult();

            cal.BasicCalibrationResult.SequenceID = cal.SequenceID;
            cal.BasicCalibrationResult.WorkOrderDetailId = cal.WorkOrderDetailId;
            cal.BasicCalibrationResult.CalibrationSubTypeId = cal.CalibrationSubTypeId;
            cal.BasicCalibrationResult.Position = 1;




            list.Add(cal);


            return list;


        }
    }
}
