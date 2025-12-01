using Bogus;
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
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.OpticalComparator
{
    public class CreateItemsAsFoundVerificationX : ComponentBase, ICreateItems<WorkOrderDetail, GenericCalibration2>
    {
       // public Application.Services.IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
       public Func<dynamic, Application.Services.IPieceOfEquipmentService<CallContext>> PoeServices { get; set; }
        public AppState AppState { get; set; }
        public CreateModel Parameter { get; set; }
        public async Task<List<GenericCalibration2>> CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId, string GroupName = null)

        {
            List<GenericCalibration2> list = new List<GenericCalibration2>();



            int cont = 1;
            for (int i = 0; i < 10; i++)
            {


                GenericCalibrationResult2 gcr = new GenericCalibrationResult2();



                dynamic gce = new ExpandoObject();


                //gce.TorqueSettings = item;
                gce.Standard = 0;
                gce.Result = 0;
                gce.Tolerance = 0;
                gcr.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce);


                list.Add(gcr.GenericCalibration2);


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
