using Bogus;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Helpers.Models;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.Mass
{
    public class NewItemsPOE : ComponentBase, INewItem<PieceOfEquipment,GenericCalibration2>
    {
        public Application.Services.IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
        public CreateModel Parameter { get; set ; }

        public async Task<List<GenericCalibration2>> DefaultNewItem(PieceOfEquipment eq, int CalibrationSubTypeId)
        {
            List<GenericCalibration2> list = new List<GenericCalibration2>();

            GenericCalibration2 cal = new GenericCalibration2();

            //cal.SequenceID = 1;

            if (string.IsNullOrEmpty(eq.PieceOfEquipmentID))
            {
                throw new Exception("ID Poe Empty");
            }

            if (string.IsNullOrEmpty(eq.SerialNumber))
            {
                throw new Exception("Serial POE is Empty");
            }
            cal.ComponentID = eq.PieceOfEquipmentID;

            cal.CalibrationSubTypeId = CalibrationSubTypeId;


            cal.TestPointResult = NewMethod(eq, CalibrationSubTypeId);

            list.Add(cal);

            return list;



        }

        private static List<GenericCalibrationResult2> NewMethod(PieceOfEquipment eq,int CalibrationSubTypeId)
        {
            var list = new List<GenericCalibrationResult2>();

            var gen = new GenericCalibrationResult2();

            //gen.SequenceID = 1;

            gen.ComponentID = eq.PieceOfEquipmentID;

            gen.CalibrationSubTypeId= CalibrationSubTypeId;

            gen.Position = 1;
            
            dynamic gce = new ExpandoObject();

            gce.Mass = "";
            gce.UoM = 1036;
            gce.Note = "";
           

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(gce);

            gen.Object = json;


            var s = gen as IResult2;

            list.Add(gen);

            return list ;
        }
    }
}
