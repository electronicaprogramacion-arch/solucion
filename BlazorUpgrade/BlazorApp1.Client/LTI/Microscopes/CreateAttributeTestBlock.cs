using Bogus;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Helpers.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.Microscopes
{
    public class CreateAttribute : ComponentBase, ICreateItems<WorkOrderDetail,GenericCalibration2>
    {
       
        public AppState AppState { get ; set ; }

        //IPieceOfEquipmentService<ProtoBuf.Grpc.CallContext> ICreateItems<WorkOrderDetail, GenericCalibration2>.PoeServices { get ; set ; }
        //public Application.Services.IPieceOfEquipmentService<ProtoBuf.Grpc.CallContext> PoeServices { get; set; }
        public Func<dynamic, Application.Services.IPieceOfEquipmentService<ProtoBuf.Grpc.CallContext>> PoeServices { get; set; }
        public CreateModel Parameter { get; set; }

        public async Task<List<GenericCalibration2>> CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId, string GroupName = null)
        {
            List<GenericCalibration2> list = new List<GenericCalibration2>();

            GenericCalibration2 cal = new GenericCalibration2();

            
            cal.ComponentID = eq.WorkOrderDetailID.ToString();
            
            cal.CalibrationSubTypeId = CalibrationSubTypeId;
            

            cal.TestPointResult = NewMethod(eq, CalibrationSubTypeId);

            list.Add(cal);

            return list;



        }

        private static List<GenericCalibrationResult2> NewMethod(WorkOrderDetail eq,int CalibrationSubTypeId)
        {
            var list = new List<GenericCalibrationResult2>();

            var gen = new GenericCalibrationResult2();

            gen.SequenceID = 1;

            gen.ComponentID = eq.WorkOrderDetailID.ToString();

            gen.Component = "WorkOrderItem";

            gen.CalibrationSubTypeId= CalibrationSubTypeId;

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

            return list ;
        }
    }
}
