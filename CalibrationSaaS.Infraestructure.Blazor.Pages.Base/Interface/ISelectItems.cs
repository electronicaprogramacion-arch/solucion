using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using Helpers.Models;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces
{
    public interface ISelectItems<Entity>
    {

       
         Func<dynamic, Application.Services.IPieceOfEquipmentService<CallContext>> PoeServices { get; set; }


        //Application.Services.IPieceOfEquipmentService<CallContext> PoeServices { get; set; }

        public CreateModel Parameter { get; set; }

        Task<SelectList> SelectItems(Entity eq,int CalibrationSubTypeId, IGenericCalibrationSubTypeCollection<GenericCalibrationResult2> item, int Position,string Map ="");


        

        /// <summary>
        /// /
        /// </summary>
        /// <param name="eq">actual entity</param>
        /// <param name="r">selectitemsresult</param>
        /// <param name="resultado">get the poe service result</param>
        /// <param name="itemParent">item select</param>
        /// <param name="Items">grid item's</param>
        /// <param name="position"></param>
        /// <returns></returns>
        Task StandardTask(Entity eq, WeightSetResult r, List<PieceOfEquipment> resultado, IGenericCalibrationSubTypeCollection<GenericCalibrationResult2> itemParent, List<IGenericCalibrationSubTypeCollection<GenericCalibrationResult2>> Items, int position, string Map = "");





    }
}