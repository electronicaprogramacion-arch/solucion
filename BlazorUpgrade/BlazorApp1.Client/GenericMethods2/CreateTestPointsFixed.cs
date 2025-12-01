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
    public class CreateTestPointsFixed : ComponentBase, ICreateItems<WorkOrderDetail,GenericCalibration2>
    {
        //public Application.Services.IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
        public Func<dynamic, Application.Services.IPieceOfEquipmentService<CallContext>> PoeServices { get; set; }
        public AppState AppState { get ; set ; }
        public CreateModel Parameter { get; set; }

        public async Task<List<GenericCalibration2>> CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId, string GroupName = null)
        {
            List<GenericCalibration2> list = new List<GenericCalibration2>();

            List<Double> ListStandarMagnification = new List<Double>();
            ListStandarMagnification.Add(0.00050);
            ListStandarMagnification.Add(0.00040);
            ListStandarMagnification.Add(0.00030);
            ListStandarMagnification.Add(0.00020);
            ListStandarMagnification.Add(0.00010);
            ListStandarMagnification.Add(0.00000);
            ListStandarMagnification.Add(-0.00010);
            ListStandarMagnification.Add(-0.00020);
            ListStandarMagnification.Add(-0.00030);
            ListStandarMagnification.Add(-0.00040);
            ListStandarMagnification.Add(-0.00050);


            List<Double> ListStandarTailstock = new List<Double>();
            ListStandarTailstock.Add(2);
            ListStandarTailstock.Add(8);
            ListStandarTailstock.Add(16);
            ListStandarTailstock.Add(24);
            ListStandarTailstock.Add(32);
            ListStandarTailstock.Add(40);


            List<Double> ListStandarLeadScrew = new List<Double>();
            ListStandarLeadScrew.Add(0.10500);
            ListStandarLeadScrew.Add(0.11000);
            ListStandarLeadScrew.Add(0.14000);
            ListStandarLeadScrew.Add(0.14500);
            ListStandarLeadScrew.Add(0.25000);
            ListStandarLeadScrew.Add(0.50000);
            ListStandarLeadScrew.Add(0.75000);

            List<string> ListAreaPlainPlug = new List<String>();
            ListAreaPlainPlug.Add("Front");
            ListAreaPlainPlug.Add("Center");
            ListAreaPlainPlug.Add("Back");

            List<Double> ListRadius = new List<Double>();
            ListRadius.Add(0.5625);
            ListRadius.Add(0.625);
            ListRadius.Add(0.6875);
            ListRadius.Add(0.75);
            ListRadius.Add(0.8125);
            ListRadius.Add(0.875);
            ListRadius.Add(0.9375);
            ListRadius.Add(1);

            
            if (CalibrationSubTypeId == 56 || CalibrationSubTypeId == 57)
            {

                int cont = 1;
                foreach (var item in ListStandarMagnification)
                {

                    GenericCalibrationResult2 gcr = new GenericCalibrationResult2();

                    dynamic gce = new ExpandoObject();


                    //gce.TorqueSettings = item;
                    gce.Standard = item;
                    gce.Result = 0;
                    gce.Tolerance = 0;

                    gcr.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce);


                    list.Add(gcr.GenericCalibration2);


                    cont++;



                }

                dynamic gce2 = new ExpandoObject();

                GenericCalibrationResult2 gcr2 = new GenericCalibrationResult2();

                gcr2.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce2);

                gcr2.Position = -1;

                list.Add(gcr2.GenericCalibration2);

            }
            else if (CalibrationSubTypeId == 58 || CalibrationSubTypeId == 59)
            {
                int cont = 1;
                foreach (var item in ListStandarTailstock)
                {

                    GenericCalibrationResult2 gcr = new GenericCalibrationResult2();



                    dynamic gce = new ExpandoObject();


                    //gce.TorqueSettings = item;
                    gce.Standard = item;
                    gce.Result = 0;
                    gce.Tolerance = 0;

                    gcr.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce);


                    list.Add(gcr.GenericCalibration2);


                    cont++;

                }

                dynamic gce2 = new ExpandoObject();

                GenericCalibrationResult2 gcr2 = new GenericCalibrationResult2();

                gcr2.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce2);

                gcr2.Position = -1;

                list.Add(gcr2.GenericCalibration2);


            }
            else if (CalibrationSubTypeId == 60 || CalibrationSubTypeId == 61)
            {
                int cont = 1;
                foreach (var item in ListStandarLeadScrew)
                {

                    GenericCalibrationResult2 gcr = new GenericCalibrationResult2();



                    dynamic gce = new ExpandoObject();


                    //gce.TorqueSettings = item;
                    gce.Standard = item;
                    gce.Result = 0;
                    gce.Tolerance = 0;

                    gcr.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce);


                    list.Add(gcr.GenericCalibration2);


                    cont++;

                }

                dynamic gce2 = new ExpandoObject();

                GenericCalibrationResult2 gcr2 = new GenericCalibrationResult2();

                gcr2.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce2);

                gcr2.Position = -1;

                list.Add(gcr2.GenericCalibration2);


            }
            else if (CalibrationSubTypeId >= 62 &&  CalibrationSubTypeId <= 71)
            {
                int cont = 1;
                foreach (var item in ListAreaPlainPlug)
                {

                    GenericCalibrationResult2 gcr = new GenericCalibrationResult2();



                    dynamic gce = new ExpandoObject();


                    //gce.TorqueSettings = item;
                    gce.Area = item;
                    gce.Function = "";
                    gce.NominalSize = "";
                    gce.Tolerance = "";
                    gce.AxisX = 0;
                    gce.AxisY = 0;

                    gcr.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce);


                    list.Add(gcr.GenericCalibration2);


                    cont++;

                }

                dynamic gce2 = new ExpandoObject();

                GenericCalibrationResult2 gcr2 = new GenericCalibrationResult2();

                gcr2.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce2);

                gcr2.Position = -1;

                list.Add(gcr2.GenericCalibration2);


            }
            else if (CalibrationSubTypeId ==74)
            {
                int cont = 1;
                foreach (var item in ListRadius)
                {

                    GenericCalibrationResult2 gcr = new GenericCalibrationResult2();



                    dynamic gce = new ExpandoObject();


                    //gce.TorqueSettings = item;
                    gce.Nominal = item;
                    gce.Id = "";
                    gce.Radius1 = 0;
                    gce.Radius2 = 0;
                    gce.Radius3 = 0;
                    gce.Radius4 = 0;
                    gce.Radius5 = 0;
                    gcr.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce);


                    list.Add(gcr.GenericCalibration2);


                    cont++;

                }

                dynamic gce2 = new ExpandoObject();

                GenericCalibrationResult2 gcr2 = new GenericCalibrationResult2();

                gcr2.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce2);

                gcr2.Position = -1;

                list.Add(gcr2.GenericCalibration2);


            }
            else if (CalibrationSubTypeId == 75)
            {

                var genericCalibrationResultsPoe = eq.PieceOfEquipment.TestPointResult.FirstOrDefault().Object;
                
                dynamic model = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(genericCalibrationResultsPoe);
                double InitialCapacity = Convert.ToDouble(model.InitialCapacity);
                List<Double> ListBoreMics = new List<Double>();
                
                ListBoreMics.Add(InitialCapacity);
                ListBoreMics.Add(eq.PieceOfEquipment.Capacity);
                int cont = 1;
                foreach (var item in ListBoreMics)
                {

                    GenericCalibrationResult2 gcr = new GenericCalibrationResult2();

                    dynamic gce = new ExpandoObject();

                    
                    //gce.TorqueSettings = item;
                    gce.Nominal = item;
                    gce.ObservedReadingsinInches = 0;
                    gcr.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce);


                    list.Add(gcr.GenericCalibration2);


                    cont++;

                }

                //dynamic gce2 = new ExpandoObject();

                //GenericCalibrationResult2 gcr2 = new GenericCalibrationResult2();

                //gcr2.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce2);

                //gcr2.Position = -1;

                //list.Add(gcr2.GenericCalibration2);


            }
            else if (CalibrationSubTypeId == 79) //HexPlug
            {
                List<string> ListHexPlug = new List<String>();
                ListHexPlug.Add("Width Across Flat");
                ListHexPlug.Add("Width Across Corners");
                ListHexPlug.Add("Length");
                ListHexPlug.Add("Usable Length");
                int cont = 1;

                foreach (var item in ListHexPlug)
                {

                    GenericCalibrationResult2 gcr = new GenericCalibrationResult2();



                    dynamic gce = new ExpandoObject();


                    //gce.TorqueSettings = item;
                    gce.Element = item;
                    gce.Nominal = 0;
                    gce.Low = 0;
                    gce.Max = 0;
                    gce.AsRecived = 0;
                    gce.Result1 = "Pass";
                    gce.AsLeft = 0;
                    gce.Result2 = "Pass";
                    gcr.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce);


                    list.Add(gcr.GenericCalibration2);


                    cont++;

                }

                //dynamic gce2 = new ExpandoObject();

                //GenericCalibrationResult2 gcr2 = new GenericCalibrationResult2();

                //gcr2.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce2);

                //gcr2.Position = -1;

                //list.Add(gcr2.GenericCalibration2);


            }
            else if (CalibrationSubTypeId == 80) //Height Gage
            {
                List<string> ListHeightGage = new List<String>();
                ListHeightGage.Add("Minimum Height");
                ListHeightGage.Add("Mid-Point Height");
                ListHeightGage.Add("Maximum Height");

                int cont = 1;

                foreach (var item in ListHeightGage)
                {

                    GenericCalibrationResult2 gcr = new GenericCalibrationResult2();



                    dynamic gce = new ExpandoObject();


                    //gce.TorqueSettings = item;
                    gce.Setting = item;
                    gce.ArmSurface = "";
                    gce.Nominal = 0;
                    gce.ToleranceWithin = 0;
                    gce.AsReceived = 0;
                    gce.ResultReceived = "Pass";
                    gce.AsLeft = 0;
                    gce.ResultAsLeft = "Pass";
                    gce.Low = 0;
                    gce.Max = 0;
                    gcr.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce);


                    list.Add(gcr.GenericCalibration2);


                    cont++;

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
