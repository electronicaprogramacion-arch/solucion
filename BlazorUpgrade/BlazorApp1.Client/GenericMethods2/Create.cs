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
    public class Create : CreateBase<WorkOrderDetail, GenericCalibration2, GenericCalibrationResult2>, ICreateItems<WorkOrderDetail, GenericCalibration2>//ComponentBase, ICreateItems<PieceOfEquipment,GenericCalibration2>
    {
       
        public  async Task<List<GenericCalibration2>>  CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId,string GroupName= null)
        {
            //eq.PieceOfEquipment.TestPointResult[0].

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
            //        gcr.CreateNew(cont, CalibrationSubTypeId,Parameter?.Component != null ? Parameter?.Component : "WorkOrderItem", eq.ComponentID,
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
            //        gcr.CreateNew(cont, CalibrationSubTypeId, Parameter?.Component != null ? Parameter?.Component : "WorkOrderItem", eq.ComponentID,
            //            eq.Resolution, Parameter.DefaultData);

            //        gcr.Updated = DateTime.Now.Ticks;


            //        list.Add(gcr.GenericCalibration2);


            //        cont++;
            //    }


            //}
            //else if (Parameter.Range != null)
            //{



            //}

            //if (Parameter != null && Parameter.Header != null)
            //{

            //    if (Parameter.Header.Data != null && Parameter.Header.Data.Count > 0)
            //    {
            //        int cont = 1;

            //        //for (int i = 0; i < Parameter.Header.Data.Count; i++)
            //        //{

            //        GenericCalibrationResult2 gcr = new GenericCalibrationResult2();



            //        dynamic gce = new ExpandoObject();


            //        //gce.TorqueSettings = item;
            //        //gce.Standard = 0;
            //        //gce.Result = 0;
            //        //gce.AccuracySpecification = 0;
            //        gcr.CreateNew(-1, CalibrationSubTypeId, Parameter?.Component != null ? Parameter?.Component : "WorkOrderItem", eq.ComponentID,
            //            eq.Resolution, Parameter.Header.Data);


            //        gcr.Updated = DateTime.Now.Ticks;

            //        list.Add(gcr.GenericCalibration2);

            //        gcr.Position = -1;

            //        cont++;
            //        //}






            //    }



            //    if (Parameter.Header.Data == null && Parameter.Header.DefaultHeader.HasValue && Parameter.Header.DefaultHeader.Value)
            //    {

            //        GenericCalibrationResult2 gcr = new GenericCalibrationResult2();



            //        dynamic gce = new ExpandoObject();


            //        //gce.TorqueSettings = item;
            //        //gce.Standard = 0;
            //        //gce.Result = 0;
            //        //gce.AccuracySpecification = 0;
            //        gcr.CreateNew(-1, CalibrationSubTypeId, Parameter?.Component != null ? Parameter?.Component : "WorkOrderItem", eq.ComponentID,
            //            eq.Resolution, Parameter.DefaultData);

            //        gcr.Updated = DateTime.Now.Ticks;

            //        gcr.Position = -1;

            //        list.Add(gcr.GenericCalibration2);

            //    }
            //}




            //return list;





        }

    }
}
