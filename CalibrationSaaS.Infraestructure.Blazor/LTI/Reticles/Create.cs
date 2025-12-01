using Bogus;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.GenericMethods;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Helpers.Models;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Reticles
{
    public class Create : CreateBase<WorkOrderDetail, GenericCalibration2, GenericCalibrationResult2>, ICreateItems<WorkOrderDetail, GenericCalibration2>
    {
        //public AppState AppState { get; set; }
        //public IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
        //public CreateModel Parameter { get; set; }

        public async Task<List<GenericCalibration2>> CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId, string GroupName = null)

        {
            List<GenericCalibration2> list = new List<GenericCalibration2>();

            if(Parameter  == null || Parameter?.Data == null)
            {
                return list;
            }
            int cont = 1;

            //string json = Newtonsoft.Json.JsonConvert.SerializeObject(Parameter.Data);

            //'[{"Category":"Category","LW":"L_W","Nominal":"1","NomDecimals":"2","RunDecimals":"3","Run1":"4","Run2":"5","Run3":"6\r"},{"Category":"Category2","LW":"L_W2","Nominal":"1","NomDecimals":"2","RunDecimals":"3","Run1":"4","Run2":"5","Run3":"6\r"}]'


            //'[{"Grade":"FLATNESS ≤:","AA":"0.000150","A":"0.000300","B":"0.000600"},{"Grade":"REPEAT ≤:","AA":"0.00045","A":"0.00070","B":"0.00120"}]'

            for (int i = 0; i < Parameter.Data.Count; i++)
            {

                GenericCalibrationResult2 gcr = new GenericCalibrationResult2();



                dynamic gce = new ExpandoObject();

                
                //gce.TorqueSettings = item;
                //gce.Standard = 0;
                //gce.Result = 0;
                //gce.AccuracySpecification = 0;
                gcr.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(),
                    eq.Resolution, Parameter.Data.ElementAtOrDefault(i));

                gcr.Updated = DateTime.Now.Ticks;

                list.Add(gcr.GenericCalibration2);


                cont++;



            }


            //////////////////////////            HEADER
            //dynamic gce2 = new ExpandoObject();

            //GenericCalibrationResult2 gcr2 = new GenericCalibrationResult2();

            //gcr2.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce2);

            //gcr2.Updated = DateTime.Now.Ticks;
            //gcr2.Position = -1;

            //list.Add(gcr2.GenericCalibration2);


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

        public static List<int> GetFS(int capacidad)
        {

            List<int> list = new List<int>();

            for (int i = 0; i <= capacidad; i = i + 50)
            {
                list.Add(i);
            }

            return list;
        }

    }
}
