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

namespace CalibrationSaaS.Infraestructure.Blazor.GenericMethods
{
    public class NewDefaultValue : ComponentBase, INewItem<WorkOrderDetail,GenericCalibration2>
    {
        public Application.Services.IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
        public CreateModel Parameter { get ; set ; }
        public AppState AppState { get; set; }
        public async Task<List<GenericCalibration2>> DefaultNewItem(WorkOrderDetail eq, int CalibrationSubTypeId)
        {
            //eq.PieceOfEquipment.UnitOfMeasureID

            List<GenericCalibration2> list = new List<GenericCalibration2>();

            List<GenericCalibrationResult2> res = null;

            
            if (eq.TestPointResult != null && eq.TestPointResult.Count > 0)
            {
                res = eq.TestPointResult.Where(x => x.CalibrationSubTypeId == CalibrationSubTypeId).ToList();
            }


            if(res == null || res.Count == 0)
            {

                res = new List<GenericCalibrationResult2>();

                

                GenericCalibrationResult2 gcr2 = new GenericCalibrationResult2();

                CalibrationSaaS.Infraestructure.Blazor.GenericMethods.CreateBase<WorkOrderDetail, GenericCalibration2, GenericCalibrationResult2>
                c = new CalibrationSaaS.Infraestructure.Blazor.GenericMethods.CreateBase<WorkOrderDetail, GenericCalibration2, GenericCalibrationResult2>();

                Parameter.NumberTestpoints = 1.ToString();

                c.Parameter = Parameter;
                var result = await c.CreateItems(eq, CalibrationSubTypeId);


                res.AddRange(result.ElementAtOrDefault(0).TestPointResult);


            }


            if(Parameter?.DefaultData == null)
            {
                throw new Exception("No items to create");
            }


            GenericCalibrationResult2 gcr = new GenericCalibrationResult2();

            var res2 = res.ElementAtOrDefault(0);

            res2.GenericCalibration2 = null;

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
