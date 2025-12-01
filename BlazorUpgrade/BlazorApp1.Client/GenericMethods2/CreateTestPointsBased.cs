using Bogus;
using CalibrationSaaS.Application.Services;
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

namespace CalibrationSaaS.Infraestructure.Blazor.GenericMethods
{

    public class CreateTestPointsBased : ComponentBase, ICreateItems<WorkOrderDetail,GenericCalibration2>
    {
       //public Application.Services.IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
       public Func<dynamic, Application.Services.IPieceOfEquipmentService<CallContext>> PoeServices { get; set; }
        public AppState AppState { get ; set ; }
        public CreateModel Parameter { get; set; }

        public async Task<List<GenericCalibration2>> CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId, string GroupName = null)
        {
            List<GenericCalibration2> list = new List<GenericCalibration2>();

            if (eq.WOD_Weights != null && eq.WOD_Weights.Count > 0)
            {
                int cont = 1;
                foreach (var item in eq.WOD_Weights)
                {

                    var poe = item.WeightSet.PieceOfEquipment;

                    if (poe != null && poe.TestPointResult != null)
                    {
                        foreach (var poeitem in poe.TestPointResult)
                        {
                            GenericCalibration2 gc = new GenericCalibration2();
                            gc.TestPointResult = new List<GenericCalibrationResult2>();

                            gc.TestPointResult.Add(new GenericCalibrationResult2());

                            gc.WorkOrderDetailId = eq.WorkOrderDetailID;
                            gc.CalibrationSubTypeId = CalibrationSubTypeId;
                            gc.SequenceID = cont;
                            gc.ComponentID = eq.WorkOrderDetailID.ToString();

                            gc.TestPointResult[0].WorkOrderDetailId = eq.WorkOrderDetailID;
                            gc.TestPointResult[0].CalibrationSubTypeId = CalibrationSubTypeId;
                            gc.TestPointResult[0].SequenceID = cont;
                            gc.TestPointResult[0].Resolution = eq.Resolution;
                            gc.TestPointResult[0].ComponentID = gc.ComponentID;

                            var poedyn = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(poeitem.Object);


                            dynamic gce = new ExpandoObject();

                            gce.Nominal = poedyn.StandardNominal;

                            gce.CalibratedDiameterAverage = poedyn.CalibratedDiameterAverage;
                           

                            string json = Newtonsoft.Json.JsonConvert.SerializeObject(gce);

                            gc.TestPointResult[0].Object = json;

                            list.Add(gc);

                            cont++;
                        }


                    }


                }


            }

            return list;



        }

        private static List<GenericCalibrationResult2> NewMethod(WorkOrderDetail eq,int CalibrationSubTypeId)
        {
            var list = new List<GenericCalibrationResult2>();

            var gen = new GenericCalibrationResult2();

            gen.SequenceID = 1;

            gen.ComponentID = eq.WorkOrderDetailID.ToString();

            gen.CalibrationSubTypeId= CalibrationSubTypeId;

            gen.Position = 1;
            dynamic gce = new ExpandoObject();

            gce.DiamX1 = 0;
            gce.DiamY1 = 0;
            
           

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(gce);

            gen.Object = json;


            var s = gen as IResult2;

            list.Add(gen);

            return list ;
        }
    }
}
