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
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.Rockwell
{
    public class CreateItemsAsLeftISO : ComponentBase, ICreateItems
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
                                                                                    //var cs = LTILogic.GetCalibrationSubType(WorkOrderItemCreate.eq, IsCompresion);

            if (result.List == null)
            {
                return listlienarity;
            }


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

                cont = cont + 1;
                var rock3 = new GenericCalibration();
                rock3.BasicCalibrationResult = new GenericCalibrationResult();

                rock3.BasicCalibrationResult.Resolution = eq.Resolution;

                rock3.BasicCalibrationResult.Position = cont;
                rock3.BasicCalibrationResult.SequenceID = cont;
                rock3.SequenceID = cont;

                rock3.CalibrationSubTypeId = CalibrationSubTypeId;
                rock3.BasicCalibrationResult.CalibrationSubTypeId = rock3.CalibrationSubTypeId;

                dynamic gce2 = new ExpandoObject();

                gce2.ScaleRange = itemr.Scale + "-MID";

                json = Newtonsoft.Json.JsonConvert.SerializeObject(gce2);

                rock3.BasicCalibrationResult.Object = json;




                cont = cont + 1;

                var rock2 = new GenericCalibration();
                rock2.BasicCalibrationResult = new GenericCalibrationResult();



                rock2.BasicCalibrationResult.Position = cont;
                rock2.BasicCalibrationResult.SequenceID = cont;
                rock2.SequenceID = cont;
                rock2.CalibrationSubTypeId = CalibrationSubTypeId;
                rock2.BasicCalibrationResult.CalibrationSubTypeId = rock2.CalibrationSubTypeId;
                rock2.BasicCalibrationResult.Resolution = eq.Resolution;


                dynamic gce3 = new ExpandoObject();

                gce3.ScaleRange = itemr.Scale + "-HI";

                json = Newtonsoft.Json.JsonConvert.SerializeObject(gce3);

                rock2.BasicCalibrationResult.Object = json;

                listlienarity.Add(rock);
                listlienarity.Add(rock3);
                listlienarity.Add(rock2);
                cont++;

            }

            return listlienarity;


        }
    }
}
