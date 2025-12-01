using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using Helpers.Models;
using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces
{
    public interface ICreateItems<Entity, CalibrationItem> where CalibrationItem : class, new()
    //where CalibrationItemResult : class, IResult2, IResultComp, IResultGen2, IDynamic, IUpdated, new()
    //where Entity : class
       // , //IGenericCalibrationCollection<CalibrationItemResult>,
        //IGenericCalibrationCollection<GenericCalibrationResult2>, 
        //IResolution, ITolerance, new()
    {

        public Domain.Aggregates.Shared.Basic.AppState AppState { get; set; }
        Func<dynamic, Application.Services.IPieceOfEquipmentService<CallContext>> PoeServices { get; set; }
        public CreateModel Parameter { get; set; }
        public Task<List<CalibrationItem>> CreateItems(Entity eq, int CalibrationSubTypeId, string GroupName = null);



    }

   
    public interface INewItem<Entity, CalibrationItem> where CalibrationItem : class, new()
    {
        Application.Services.IPieceOfEquipmentService<CallContext> PoeServices { get; set; }

        public CreateModel Parameter { get; set; }

        public Task<List<CalibrationItem>> DefaultNewItem(Entity eq, int CalibrationSubTypeId);
       
       
    }

    

}