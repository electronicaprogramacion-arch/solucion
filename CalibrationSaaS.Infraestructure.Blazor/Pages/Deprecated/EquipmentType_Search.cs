using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Basics
{
    public class EquipementType_SearchBase : ComponentBase
    {
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] System.Net.Http.HttpClient Http { get; set; }
        [Inject] Application.Services.IBasicsServices<CallContext> Client { get; set; }
        [Inject] AppState AppState { get; set; }
        protected string SearchTerm { get; set; } = "";
        List<EquipmentType> EquipmentTypeList = new List<EquipmentType>();
        protected List<EquipmentType> FilteredToDos => EquipmentTypeList.Where(i => i.Name.ToLower().Contains(SearchTerm.ToLower())).ToList();

        protected override async Task OnInitializedAsync()
        {
            BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);

            var Eq = (await basics.GetEquipmenTypes(new CallOptions()));

            EquipmentTypeList = Eq.EquipmentTypes;

            Console.Write("OnInitializedAsync");


            if (AppState.EquipmentTypes == null || AppState.EquipmentTypes.Count > 0)
            {
                foreach (var item in EquipmentTypeList)
                {
                    AppState.AddEquipmentType(item);
                }
            }


        }





        public void Dispose()
        {

        }

    }
}
