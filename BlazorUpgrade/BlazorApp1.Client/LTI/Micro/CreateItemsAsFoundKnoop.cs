using Bogus;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.GenericMethods;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.Micro
{
    public class CreateItemsAsFoundKnoop : CreateBase<WorkOrderDetail, GenericCalibration2, GenericCalibrationResult2>, ICreateItems<WorkOrderDetail, GenericCalibration2>
    {
        public AppState AppState { get; set; }
        public IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
        public CreateModel Parameter { get; set; }

        public async  Task<List<GenericCalibration2>> CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId, string GroupName = null)
        {

            List<GenericCalibration2> list = new List<GenericCalibration2>();

            if (eq.WOD_Weights != null && eq.WOD_Weights.Count > 0)
            {
                int cont = 1;
                foreach (var item in eq.WOD_Weights)
                {

                    var poe = item.WeightSet.PieceOfEquipment;

                    if (poe.TypeMicro.HasValue && poe.TypeMicro.Value == 2)
                    {

                        GenericCalibration gc = new GenericCalibration();

                        GenericCalibrationResult2 gcr = new GenericCalibrationResult2();
                        dynamic gce = new ExpandoObject();

                        gce.UM = poe.MicronValue;
                        gce.Tol_HM = poe.Tolerance.ToleranceValue;
                        gce.Tol_HV = poe.ToleranceHV;
                        gce.Load_KFG = poe.LoadKGF;
                        gce.UncertantyBlock = poe.UncertaintyValue;
                        gce.HardnessValue = poe.Hardness;

                        gcr.CreateNew(cont, CalibrationSubTypeId, Parameter?.Component != null ? Parameter?.Component : "WorkOrderItem", eq.ComponentID,
                        eq.Resolution, Parameter.DefaultData);

                        gcr.Updated = DateTime.Now.Ticks;


                        //var gc = gcr.GenericCalibration2 as CalibrationItem;

                        //if (gc is null)
                        //{
                        //    throw new Exception("CalibrationItem no must be null");
                        //}

                        //list.Add(gc);

                        cont++;


                    }


                }

            }

            return list;
        }
    }
}
