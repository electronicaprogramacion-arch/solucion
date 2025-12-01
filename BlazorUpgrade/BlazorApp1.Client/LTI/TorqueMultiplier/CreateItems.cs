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
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.TorqueMultiplier
{
    public class CreateTest : ComponentBase, ICreateItems<WorkOrderDetail, GenericCalibration2>
    {
        //public Application.Services.IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
        public Func<dynamic, Application.Services.IPieceOfEquipmentService<CallContext>> PoeServices { get; set; }
        public AppState AppState { get ; set ; }
        public CreateModel Parameter { get; set; }
        public async Task<List<GenericCalibration2>> CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId, string GroupName = null)

        {
            List<GenericCalibration2> list = new List<GenericCalibration2>();



            int cont = 1;
            foreach (var item in GetFS(eq.PieceOfEquipment.Capacity, eq.Resolution))
            {


                GenericCalibrationResult2 gcr = new GenericCalibrationResult2();


                dynamic gce = new ExpandoObject();

                gce.StandardInput = item;

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
