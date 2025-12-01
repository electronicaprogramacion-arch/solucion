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

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.Thread
{
    public class CreateItemsClass : ComponentBase, ICreateItems<PieceOfEquipment, GenericCalibration2>
    {
        //public Application.Services.IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
        public Func<dynamic, Application.Services.IPieceOfEquipmentService<CallContext>> PoeServices { get; set; }
        public AppState AppState { get; set; }
        public CreateModel Parameter { get; set; }
        public async Task<List<GenericCalibration2>> CreateItems(PieceOfEquipment eq, int CalibrationSubTypeId, string GroupName = null)
        {
            List<GenericCalibration2> list = new List<GenericCalibration2>();

            GenericCalibration2 cal = new GenericCalibration2();

            cal.ComponentID = eq.PieceOfEquipmentID;

            cal.CalibrationSubTypeId = CalibrationSubTypeId;


            cal.TestPointResult = NewMethod(eq, CalibrationSubTypeId);

            list.Add(cal);

            return list;



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

            gce.BallDiam = 0;
            gce.Load = 0;
            gce.LTI = eq.PieceOfEquipmentID;


            string json = Newtonsoft.Json.JsonConvert.SerializeObject(gce);

            gen.Object = json;


            var s = gen as IResult2;

            list.Add(gen);

            return list;
        }
    }
}

