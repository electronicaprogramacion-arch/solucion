using Bogus;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.GenericMethods;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.Micro
{
    public class CreateItemsMicro : CreateBase<WorkOrderDetail, GenericCalibration2, GenericCalibrationResult2>, ICreateItems<WorkOrderDetail, GenericCalibration2>
    {
        public AppState AppState { get; set; }
        public IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
        public CreateModel Parameter { get; set; }

        public async Task<List<GenericCalibration2>> CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId, string GroupName = null)
        {
            List < GenericCalibration2> list= new List<GenericCalibration2>();   

            if (eq.WOD_Weights != null && eq.WOD_Weights.Count > 0) 
            {
                int cont = 1;
                //foreach (var item in eq.WOD_Weights)
                //{
                //    GenericCalibration2 gc = new GenericCalibration2();
                //    gc. = new GenericCalibrationResult2();

                //    gc.WorkOrderDetailId = eq.WorkOrderDetailID;
                //    gc.CalibrationSubTypeId = CalibrationSubTypeId;
                //    gc.SequenceID = 1;

                //    gc.BasicCalibrationResult.WorkOrderDetailId = eq.WorkOrderDetailID;
                //    gc.BasicCalibrationResult.CalibrationSubTypeId = CalibrationSubTypeId;
                //    gc.BasicCalibrationResult.SequenceID = 1;


                //    list.Add(gc);
                //    cont++;
                //    GenericCalibration gc1 = new GenericCalibration();
                //    gc1.BasicCalibrationResult = new GenericCalibrationResult();

                //    gc1.WorkOrderDetailId = eq.WorkOrderDetailID;
                //    gc1.CalibrationSubTypeId = CalibrationSubTypeId;
                //    gc1.SequenceID = 2;

                //    gc1.BasicCalibrationResult.WorkOrderDetailId = eq.WorkOrderDetailID;
                //    gc1.BasicCalibrationResult.CalibrationSubTypeId = CalibrationSubTypeId;
                //    gc1.BasicCalibrationResult.SequenceID = 2;


                //    list.Add(gc1);
                //    cont++;

              //  }


            }

            return list;

            //Faker<GenericCalibrationResult> fo = new Faker<GenericCalibrationResult>();
            //int cont = 1;

            //fo
               
            //  .RuleFor(r => r.Resolution, f => eq.PieceOfEquipment.Resolution)
             
             
            //  .RuleFor(r => r.DecimalNumber, (f, u) => eq.PieceOfEquipment.DecimalNumber)
            //   .RuleFor(r => r.CalibrationSubTypeId, f => CalibrationSubTypeId)
            //   .RuleFor(r => r.WorkOrderDetailId, f => eq.WorkOrderDetailID)
             
            //   .RuleFor(r => r.Position, f => cont)
            //  .RuleFor(r => r.SequenceID, f => cont++);

            ////var a = fo.Generate(10);


            //    int cont2 = 1;
                
            //    Faker<GenericCalibration> t = new Faker<GenericCalibration>();

            //    t.RuleFor(r => r.BasicCalibrationResult, f => fo.Generate())
                
            //    .RuleFor(r => r.CalibrationSubTypeId, f => CalibrationSubTypeId)
            //    .RuleFor(r => r.WorkOrderDetailId, f => eq.WorkOrderDetailID)
            //    .RuleFor(r => r.SequenceID, f => cont2++)
               
            //    ;

            //var aa = t.Generate(2);

            //return aa;



           


        }
    }
}
