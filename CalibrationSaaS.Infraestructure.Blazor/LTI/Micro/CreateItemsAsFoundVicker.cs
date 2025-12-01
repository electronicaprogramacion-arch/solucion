using Bogus;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.Micro
{
    public class CreateItemsAsFoundVicker : ComponentBase, ICreateItems
    {
        public AppState AppState { get; set; }
        public IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
        public CreateModel Parameter { get; set; }

        public async Task<List<GenericCalibration>> CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId)
        {

            List<GenericCalibration> list = new List<GenericCalibration>();

            if (eq.WOD_Weights != null && eq.WOD_Weights.Count > 0)
            {
                int cont = 1;
                foreach (var item in eq.WOD_Weights)
                {

                    var poe = item.WeightSet.PieceOfEquipment;

                    if (poe.TypeMicro.HasValue && poe.TypeMicro.Value == 1) //Vicker Hard
                    {

                        GenericCalibration gc = new GenericCalibration();
                        gc.BasicCalibrationResult = new GenericCalibrationResult();

                        gc.WorkOrderDetailId = eq.WorkOrderDetailID;
                        gc.CalibrationSubTypeId = CalibrationSubTypeId;
                        gc.SequenceID = cont;

                        gc.BasicCalibrationResult.WorkOrderDetailId = eq.WorkOrderDetailID;
                        gc.BasicCalibrationResult.CalibrationSubTypeId = CalibrationSubTypeId;
                        gc.BasicCalibrationResult.SequenceID = cont;
                        gc.BasicCalibrationResult.Resolution = eq.Resolution;



                        dynamic gce = new ExpandoObject();

                        gce.UM = poe.MicronValue;
                        gce.Tol_HM = poe.Tolerance.ToleranceValue;
                        gce.Tol_HV = poe.ToleranceHV;
                        gce.Load_KFG = poe.LoadKGF;
                        gce.UncertantyBlock = poe.UncertaintyValue;
                        gce.HardnessValue = poe.Hardness;

                        string json = Newtonsoft.Json.JsonConvert.SerializeObject(gce);

                        gc.BasicCalibrationResult.Object = json;

                        list.Add(gc);

                        cont++;

                    }


                }


            }

            return list;




        }
    }
}
