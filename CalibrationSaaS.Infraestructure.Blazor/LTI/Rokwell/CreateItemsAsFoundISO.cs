using Bogus;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.Rockwell
{
    public class CreateItemsAsFoundISO : ComponentBase, ICreateItems
    {

        public AppState AppState { get; set; }
        public IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
        public CreateModel Parameter { get; set; }
        public async Task<List<GenericCalibration>> CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId)
        {

            List<GenericCalibration> list = new List<GenericCalibration>();


            var pag = new Pagination<POE_Scale>();

            pag.Show = 1000;

            pag.Entity = new POE_Scale();

            pag.Entity.PieceOfEquipmentID = eq.PieceOfEquipmentId;

            var _poeGrpc = new PieceOfEquipmentGRPC(PoeServices);

            //var g = new CallOptions(await GetHeaderAsync());

            var result = (await _poeGrpc.GetPOEScale(pag));


            List<GenericCalibration> listlienarity = new List<GenericCalibration>();//LTILogic.CreateListGeneric<CalibrationItem>(WorkOrderItemCreate.eq, IsCompresion);

            //var cs = LTILogic.GetCalibrationSubType(eq, IsCompresion);
            if (result.List == null)
            {
                return listlienarity;
            }

            int cont2 = 1;
            int cont = 1;
            foreach (var itemr in result.List)
            {

                var rock = new GenericCalibration();
                rock.BasicCalibrationResult = new GenericCalibrationResult();

                rock.CalibrationSubTypeId = CalibrationSubTypeId;
                rock.BasicCalibrationResult.CalibrationSubTypeId = rock.CalibrationSubTypeId;

                rock.BasicCalibrationResult.Position = cont;

                rock.BasicCalibrationResult.SequenceID = cont;
                rock.SequenceID = cont;
                rock.BasicCalibrationResult.Resolution = eq.Resolution;
                dynamic gce = new ExpandoObject();

                gce.ScaleRange = itemr.Scale + "-LO";

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(gce);

                rock.BasicCalibrationResult.Object = json;


                var rock2 = new GenericCalibration();
                rock2.BasicCalibrationResult = new GenericCalibrationResult();


                rock2.CalibrationSubTypeId = CalibrationSubTypeId;
                rock2.BasicCalibrationResult.CalibrationSubTypeId = rock2.CalibrationSubTypeId;
                rock2.BasicCalibrationResult.Position = cont++;
                rock2.BasicCalibrationResult.SequenceID = cont;
                rock2.SequenceID = cont;
                rock2.BasicCalibrationResult.Resolution = eq.Resolution;
                dynamic gce2 = new ExpandoObject();

                gce2.ScaleRange = itemr.Scale + "-HI";

                string json2 = Newtonsoft.Json.JsonConvert.SerializeObject(gce2);

                rock2.BasicCalibrationResult.Object = json2;


                listlienarity.Add(rock);
                listlienarity.Add(rock2);

                cont++;
            }

            return listlienarity;



        }
    }
}
