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

namespace CalibrationSaaS.Infraestructure.Blazor.GenericMethods.POE
{
    public class Create : CreateBase<PieceOfEquipment, GenericCalibration2, GenericCalibrationResult2>, ICreateItems<PieceOfEquipment, GenericCalibration2>//ComponentBase, ICreateItems<PieceOfEquipment,GenericCalibration2>
    {

        public async Task<List<GenericCalibration2>> CreateItems(PieceOfEquipment eq, int CalibrationSubTypeId, string GroupName = null)
        {

            return await base.CreateItems(eq, CalibrationSubTypeId);
            

            //List<GenericCalibration2> list = new List<GenericCalibration2>();

            //if (Parameter == null || (Parameter?.Data == null && !Parameter.NumberTestpoints.HasValue))
            //{
            //    return list;
            //}
            //if (Parameter != null && Parameter?.Data?.Count > 0)
            //{

            //    int cont = 1;

            //    for (int i = 0; i < Parameter.Data.Count; i++)
            //    {

            //        GenericCalibrationResult2 gcr = new GenericCalibrationResult2();



            //        dynamic gce = new ExpandoObject();


            //        //gce.TorqueSettings = item;
            //        //gce.Standard = 0;
            //        //gce.Result = 0;
            //        //gce.AccuracySpecification = 0;
            //        gcr.CreateNew(cont, CalibrationSubTypeId, "PieceOfEquipmentCreate", eq.PieceOfEquipmentID.ToString(),
            //            eq.Resolution, Parameter.Data.ElementAtOrDefault(i));


            //        gcr.Updated = DateTime.Now.Ticks;

            //        list.Add(gcr.GenericCalibration2);


            //        cont++;
            //    }


            //}
            //else
            //   if (Parameter != null && Parameter.NumberTestpoints.HasValue && Parameter.NumberTestpoints.Value > 0 && Parameter.DefaultData != null)

            //{
            //    int cont = 1;

            //    for (int i = 0; i < Parameter.NumberTestpoints.Value; i++)
            //    {

            //        GenericCalibrationResult2 gcr = new GenericCalibrationResult2();



            //        dynamic gce = new ExpandoObject();


            //        //gce.TorqueSettings = item;
            //        //gce.Standard = 0;
            //        //gce.Result = 0;
            //        //gce.AccuracySpecification = 0;
            //        gcr.CreateNew(cont, CalibrationSubTypeId, "PieceOfEquipmentCreate", eq.PieceOfEquipmentID.ToString(),
            //            eq.Resolution, Parameter.DefaultData);

            //        gcr.Updated = DateTime.Now.Ticks;


            //        list.Add(gcr.GenericCalibration2);


            //        cont++;
            //    }


            //}

           

            //return list;


        }

        private static List<GenericCalibrationResult2> NewMethod(PieceOfEquipment eq, int CalibrationSubTypeId)
        {
            var list = new List<GenericCalibrationResult2>();

            var gen = new GenericCalibrationResult2();

            gen.SequenceID = 1;

            gen.ComponentID = eq.PieceOfEquipmentID;

            gen.Component = "PieceOfEquipmentCreate";

            gen.CalibrationSubTypeId = CalibrationSubTypeId;

            gen.Position = 1;
            dynamic gce = new ExpandoObject();

            //gce.Range = "";
            //gce.UoM = 1036;
            //gce.Type = ""; // eq.PieceOfEquipmentID;
            //gce.Density = "";

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(gce);

            gen.Object = json;


            var s = gen as IResult2;

            list.Add(gen);

            return list;
        }
    }
}
