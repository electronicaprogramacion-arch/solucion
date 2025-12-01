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
using CalibrationSaaS.Domain.Aggregates.Querys;
namespace CalibrationSaaS.Infraestructure.Blazor.GenericMethods
{
    public class CreateCapacityBasedPOE : ComponentBase, ICreateItems<PieceOfEquipment, GenericCalibration2>
    {
        //publicCalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
        public Func<dynamic,CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>> PoeServices { get; set; }
        public AppState AppState { get ; set ; }
        public CreateModel Parameter { get; set; }
        public async Task<List<GenericCalibration2>> CreateItems(PieceOfEquipment eq, int CalibrationSubTypeId,string GropName=null)

        {
            List<GenericCalibration2> list = new List<GenericCalibration2>();
            
            var uom = eq.UnitOfMeasureID.GetUoM(AppState.UnitofMeasureList, eq.Capacity);


            int cont = 1;
            foreach (var item in GetFS(eq.Capacity, eq.Resolution))
            {

                if (string.IsNullOrEmpty(eq.PieceOfEquipmentID))
                {
                    throw new Exception("Please add ID Before, Configure Equipment Template");
                }
                GenericCalibrationResult2 gcr = new GenericCalibrationResult2();


                dynamic gce = new ExpandoObject();

                gce.Weight = item;
                gce.UoM = uom.Abbreviation;

                gcr.CreateNew(cont, CalibrationSubTypeId, "PieceOfEquipmentCreate", eq.PieceOfEquipmentID.ToString(), eq.Resolution, gce);


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
        public static List<double> GetFS(double capacity, double resol)
        {

            List<double> list = new List<double>();
            var NumberTestPoint = 5;

            int step = ((int)capacity / (int)NumberTestPoint);

            for (int i = 1; i < NumberTestPoint + 1; i++)
            {
                var s = ((i) * step); //+ (resol * i);      

                list.Add(s);

            }



            return list;
        }

    }
}
