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

namespace CalibrationSaaS.Infraestructure.Blazor.GenericMethods
{
    public class CreateCSV : CreateBase<PieceOfEquipment, GenericCalibration2,GenericCalibrationResult2>, ICreateItems<PieceOfEquipment, GenericCalibration2>
    {
        //public Application.Services.IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
        //public AppState AppState { get; set; }
        //public CreateModel Parameter { get; set; }
        public override async Task<List<GenericCalibration2>> CreateItems(PieceOfEquipment eq, int CalibrationSubTypeId, string GroupName = null)

        {
            List<GenericCalibration2> list = new List<GenericCalibration2>();


            int cont = 1;
            for (int i = 0; i < 5; i++)
            {

                GenericCalibrationResult2 gcr = new GenericCalibrationResult2();



                dynamic gce = new ExpandoObject();


                //gce.TorqueSettings = item;
                gce.Standard = 0;
                gce.Result = 0;
                gce.AccuracySpecification = 0;
                gcr.CreateNew(cont, CalibrationSubTypeId, "PieceOfEquipmentCreate", eq?.PieceOfEquipmentID?.ToString(), eq.Resolution, gce);
                

                list.Add(gcr.GenericCalibration2);


                cont++;



            }

            //dynamic gce2 = new ExpandoObject();

            //GenericCalibrationResult2 gcr2 = new GenericCalibrationResult2();

            //gcr2.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce2);

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
