using Bogus;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite;
using CalibrationSaaS.Infraestructure.Blazor.Shared;
using Helpers.Models;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using SqliteWasmHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.LengthStandard
{
    public class Create : ComponentBase, ICreateItems<WorkOrderDetail, GenericCalibration2>
    {
       //  public Application.Services.IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
        public Func<dynamic, Application.Services.IPieceOfEquipmentService<CallContext>> PoeServices { get; set; }
        public CreateModel Parameter { get; set; }

        //[Inject]
        public Domain.Aggregates.Shared.Basic.AppState AppState { get; set; }

        [Inject]
        public ISqliteWasmDbContextFactory<CalibrationSaaSDBContextOff2> DbFactory { get; set; }

        public async Task<List<GenericCalibration2>> CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId, string GroupName = null)
        {
            List<GenericCalibration2> list = new List<GenericCalibration2>();

            //PoeServices = null; 
            var _poeGrpc =  new PieceOfEquipmentGRPC(PoeServices, DbFactory);

          
            if (eq.PieceOfEquipment.TestPointResult == null)
            {
                throw new Exception("Rods Not Configured");
            }

            var sr = eq.PieceOfEquipment.TestPointResult.Where(x => x.CalibrationSubTypeId == (378070648));

            if (sr == null)
            {
                throw new Exception("Rods Not Configured");
            }


            List<PieceOfEquipment> pieces = new List<PieceOfEquipment>();


            foreach (var item123 in sr)
            {

                if (!string.IsNullOrEmpty(item123.Object))
                {
                    var poedyn = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(item123.Object);

                    PieceOfEquipment pieceOfEquipment   = new PieceOfEquipment();

                    //pieceOfEquipment.Capacity= poedyn.Mass;

                    //pieceOfEquipment.UnitOfMeasureID = poedyn.UoM;

                    //pieceOfEquipment.Class = eq.PieceOfEquipment.Class;

                    poedyn.CertificationID = eq.CertificationID;

                    pieceOfEquipment.TestPointResult = new List<GenericCalibrationResult2>();


                    var poedynstr = Newtonsoft.Json.JsonConvert.SerializeObject(poedyn);

                    item123.Object = poedynstr;

                    pieceOfEquipment.TestPointResult.Add(item123);

                    pieces.Add(pieceOfEquipment);   


                }
            }

            if(pieces.Count> 0)
            {
                var resultado = await _poeGrpc.GetResolutionByLenght(pieces);

                //var resultado = pieces; //await _poeGrpc.GetResolutionByMass(pieces);



                int cont = 1;
                foreach (var item in resultado)
                {
                    foreach (var piece in item.TestPointResult) {


                        var poedyn = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(piece.Object);

                        GenericCalibrationResult2 gcr = new GenericCalibrationResult2();


                        dynamic gce = new ExpandoObject();

                        gce.ReferenceNumber = poedyn.ReferenceNumber;
                        gce.NominalSize= poedyn.NominalSize;    
                        gce.UoM = poedyn.UoM;
                        gce.Tolerance = item.Tolerance.ToleranceValue;
                        //gce.ToleranceUoM = item.ToleranceUoM;
                        //gce.Identification = item.Capacity + " " + item.UnitOfMeasureID.GetUoM(AppState.UnitofMeasureList).Abbreviation;
                        //gce.MPE = item.ToleranceValue + " " + item.ToleranceUoM.GetUoM(AppState.UnitofMeasureList).Abbreviation;

                        gcr.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce);
                        list.Add(gcr.GenericCalibration2);
                        cont++;

                    }

                   
                
                
                
                
                }

            }

            return list;


        }

        public static List<double> GetFS(double capacidad, double resol)
        {

            List<double> list = new List<double>();
            var NumberTestPoint = 10;

            int step = (int)(capacidad / NumberTestPoint);

            for (int i = 1; i < NumberTestPoint + 1; i++)
            {
                var s = ((i) * step) + (resol * i);

                list.Add(s);

            }



            return list;
        }
    }
}
