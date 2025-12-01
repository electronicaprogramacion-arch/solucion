using Bogus;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Helpers.Models;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;

namespace CalibrationSaaS.Infraestructure.Blazor.GenericMethods.ET
{
    public class NewDefaultValue : ComponentBase, INewItem<EquipmentTemplate,GenericCalibration2>
    {
       
        public CreateModel Parameter { get ; set ; }
        public AppState AppState { get; set; }
        public IPieceOfEquipmentService<CallContext> PoeServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async Task<List<GenericCalibration2>> DefaultNewItem(EquipmentTemplate eq, int CalibrationSubTypeId)
        {
            //eq.PieceOfEquipment.UnitOfMeasureID

            List<GenericCalibration2> list = new List<GenericCalibration2>();

            var res= eq.TestPointResult.Where(x => x.CalibrationSubTypeId == CalibrationSubTypeId).ToList();


            if(Parameter?.DefaultData == null)
            {
                throw new Exception("No items to create");
            }


            GenericCalibrationResult2 gcr = new GenericCalibrationResult2();

            //var res2 = res.ElementAtOrDefault(0);

            //res2.GenericCalibration2 = null;

            gcr.CreateNew(res.Count +1, CalibrationSubTypeId,
                          Parameter?.Component != null ? Parameter?.Component : "WorkOrderItem", eq.ComponentID,eq.Resolution,Parameter.DefaultData);

            gcr.Updated = DateTime.Now.Ticks;


            var gc = gcr.GenericCalibration2;

            if (gc is null)
            {
                throw new Exception("CalibrationItem no must be null");
            }

            list.Add(gc);


            return list;



        }

        
    }
}
