using Bogus;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Infraestructure.Blazor.Helper;
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
using System.Linq;
using System.Threading.Tasks;
//Linearity Type
namespace CalibrationSaaS.Infraestructure.Blazor.GenericMethods.SelectItemsET
{
    public class SelectStandard : ComponentBase, ISelectItems
    {

        public Application.Services.IBasicsServices<CallContext> ETServices { get; set; }
       
        IPieceOfEquipmentService<CallContext> ISelectItems.PoeServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        
        public async Task<SelectList> SelectItems(WorkOrderDetail eq, int CalibrationSubTypeId, GenericCalibration item, int Position)
        {

           
            SelectList SelectList= new SelectList();

            return SelectList;
        }

        public Task<SelectList> SelectItemsET(EquipmentTemplate eq, int CalibrationSubTypeId, GenericCalibration item, int Position)
        {
            throw new NotImplementedException();
        }

        public Task StandardTask(WorkOrderDetail eq, WeightSetResult r, List<PieceOfEquipment> resultado, GenericCalibration itemParent, List<GenericCalibration> Items, int position)
        {
            throw new NotImplementedException();
        }
    }
}
