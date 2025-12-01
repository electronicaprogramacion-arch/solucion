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

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.Microscopes
{
    public class Create : ComponentBase , ICreateItems<WorkOrderDetail, GenericCalibration2>
    {
        //public Application.Services.IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
       public Func<dynamic, Application.Services.IPieceOfEquipmentService<CallContext>> PoeServices { get; set; }
        public AppState AppState { get ; set ; }
        public CreateModel Parameter { get; set; }
        public async Task<List<GenericCalibration2>> CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId, string GroupName = null)
        
        {
            List <GenericCalibration2> list= new List<GenericCalibration2>();

           

            int cont = 1;
            foreach (var item in GetFS(eq))
            {


                    GenericCalibrationResult2 gcr = new GenericCalibrationResult2();


                    dynamic gce = new ExpandoObject();

                    //gce.StandardTorque = item;

                    gcr.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution,gce);

                
                    list.Add(gcr.GenericCalibration2);


                    cont++;
            }

            dynamic gce2 = new ExpandoObject();

            GenericCalibrationResult2 gcr2 = new GenericCalibrationResult2();

            gcr2.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce2);

            gcr2.Position = -1;

            list.Add(gcr2.GenericCalibration2);


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

        public static List<double> GetFS(WorkOrderDetail eq)
        {

            List<double> list = new List<double>();


            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);
            list.Add(6);





            return list;
            }

    }
}
