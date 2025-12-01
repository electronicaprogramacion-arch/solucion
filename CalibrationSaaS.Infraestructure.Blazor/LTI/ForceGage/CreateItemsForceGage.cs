using Bogus;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.ForceGage
{
    public class CreateItemsForceGage : ComponentBase, ICreateItems
    {
        public AppState AppState { get; set; }
        public IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
        public CreateModel Parameter { get; set; }

        public async Task<List<GenericCalibration>> CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId)
        {
            List < GenericCalibration > list= new List<GenericCalibration>();

            //////bool isiso = false;

            //////int subtipec = 30;

            //////if (subtipec == 4 || subtipec == 5 || subtipec == 8)
            //////{
            //////    isiso = true;
            //////}

            //////List<Domain.Aggregates.Entities.Force> listlienarity = new List<Domain.Aggregates.Entities.Force>();

            ////////CompresionResult r = new CompresionResult();

            //////Faker<ForceResult> fo = new Faker<ForceResult>();
            //////int cont = 1;

            //////fo.
            //////    RuleFor(r => r.FS, f => CalibrationSaaS.Domain.Aggregates.Shared.LTILogic.GetFS(cont))
            //////    .RuleFor(r => r.Nominal, f => Math.Round((CalibrationSaaS.Domain.Aggregates.Shared.LTILogic.GetFS(cont) / 100) * eq.PieceOfEquipment.Capacity))
            //////  .RuleFor(r => r.Resolution, f => eq.PieceOfEquipment.Resolution)
            //////  .RuleFor(r => r.Error, x => 0)
            //////  .RuleFor(r => r.ErrorPer, 0)
            //////  .RuleFor(r => r.DecimalNumber, (f, u) => eq.PieceOfEquipment.DecimalNumber)
            //////   .RuleFor(r => r.CalibrationSubTypeId, f => subtipec)
            //////   .RuleFor(r => r.WorkOrderDetailId, f => eq.WorkOrderDetailID)
            //////   .RuleFor(r => r.IsZeroReturn, f => IsZero(CalibrationSaaS.Domain.Aggregates.Shared.LTILogic.GetFS(cont)))
            //////   .RuleFor(r => r.Position, f => cont)
            //////  .RuleFor(r => r.SequenceID, f => cont++);

            ////////var a = fo.Generate(10);


            //////int cont2 = 1;
            //////Faker<Force> t = new Faker<Force>();

            //////t.RuleFor(r => r.BasicCalibrationResult, f => fo.Generate())
            //////    //.RuleFor(r => r.SequenceID, x => x.Random.Int())
            //////    .RuleFor(r => r.UnitOfMeasureId, f => eq.PieceOfEquipment.UnitOfMeasureID)
            //////    .RuleFor(r => r.CalibrationUncertaintyValueUnitOfMeasureId, f => eq.PieceOfEquipment.UnitOfMeasureID)
            //////    .RuleFor(r => r.CalibrationSubTypeId, f => subtipec)
            //////    .RuleFor(r => r.WorkOrderDetailId, f => eq.WorkOrderDetailID)
            //////    .RuleFor(r => r.SequenceID, f => cont2++)
            //////    .RuleFor(r => r.ISO, f => isiso)
            //////    ;

            //////var aa = t.Generate(12);

            //////foreach (var item in aa)
            //////{
            //////    item.BasicCalibrationResult.Resolution = eq.Resolution;
            //////    if (item.BasicCalibrationResult.FS != 0 && item.BasicCalibrationResult.Nominal < item.BasicCalibrationResult.Resolution * 200)
            //////    {
            //////        item.BasicCalibrationResult.Nominal = item.BasicCalibrationResult.Resolution * 200;
            //////        item.BasicCalibrationResult.FS = Math.Round((item.BasicCalibrationResult.Nominal * 100) / eq.PieceOfEquipment.Capacity, 3);

            //////    }
            //////    item.BasicCalibrationResult.NominalTemp = item.BasicCalibrationResult.Nominal;
            //////}

            int cont = 1;
            foreach (var item in GetFS((int)eq.PieceOfEquipment.Capacity))
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

                    gce.StandardForce = item;
                    //gce.Tol_HM = poe.ToleranceValue;
                    //gce.Tol_HV = poe.ToleranceHV;
                    //gce.Load_KFG = poe.LoadKGF;
                    //gce.UncertantyBlock = poe.UncertaintyValue;
                    //gce.HardnessValue = poe.Hardness;

                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(gce);

                    gc.BasicCalibrationResult.Object = json;

                    list.Add(gc);

                    cont++;


                


            }





            return list;

           
           


        }


        public static bool IsZero(double fs)
        {
            if (fs == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<int> GetFS(int  capacidad)
        {
           
            List<int> list = new List<int>();

            for(int i =0; i <= capacidad; i = i + 50)
            {
                list.Add(i);
            }

            return list;
        }

    }
}
