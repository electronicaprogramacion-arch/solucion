using CalibrationSaaS.Application.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helpers.Controls.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;

namespace BlazorApp1.Blazor.Blazor.LTI.Rockwell
{
    public class Scales_CreateBase
        : Base_Create<POE_Scale, IPieceOfEquipmentService<CallContext>, CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany>
    // , IPage<POE_Scale, IAssetsServices<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    {
    
        [Inject] 
        public AppState AppState2 { get; set; }



    [CascadingParameter(Name="CascadeParam1")]
    public PieceOfEquipment POE { get; set; }

    [Inject] CalibrationSaaS.Application.Services.IAssetsServices<CallContext> _assetsServices { get; set; }
    public POE_Scale eqWeightSet = new POE_Scale();


    public bool IsForAccredited { get; set; } = false;
        public List<POE_Scale> LIST1 { get; set; } = new List<POE_Scale>();

        public ResponsiveTable<POE_Scale> RT { get; set; } = new ResponsiveTable<POE_Scale>();

        public ResponsiveTable<POE_Scale> Grid { get; set; }


        [Parameter]
        public ICollection<POE_Scale> List { get; set; } = new List<POE_Scale>();


      



        public Task SelectModal(POE_Scale DTO)
        {
            throw new NotImplementedException();
        }

        

        public async Task<bool> Delete(CalibrationSaaS.Domain.Aggregates.Entities.POE_Scale DTO)
        {
           


            return true;

            //searchComponent.ShowResult();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {


            await base.OnAfterRenderAsync(firstRender);




        }

        public override async Task<ResultSet<POE_Scale>> LoadData(Pagination<POE_Scale> pag)
        {
            pag.Entity=new POE_Scale();

            pag.Entity.PieceOfEquipmentID = POE.PieceOfEquipmentID;

            var _poeGrpc = new PieceOfEquipmentGRPC(Client);

            var g = new CallOptions(await GetHeaderAsync());

            var result = (await _poeGrpc.GetPOEScale(pag, Header));


            return result;
        
        
        
        
        
        }


        public POE_Scale DefaultItem()
    {
            POE_Scale w = new POE_Scale();

            w.PieceOfEquipmentID = POE.PieceOfEquipmentID;
            return w;
        

    }

   


   
    public void Show(List<POE_Scale> group, bool IsAccredited)
    {

        LIST1 = group;
        //RT.ItemsDataSource = LIST1;
        IsForAccredited = IsAccredited;
        //Logger.LogDebug("listWeightSets on Show Wc" + LIST1.Count());
        //Logger.LogDebug("IsAccredited on Show Wc" + IsAccredited);
        StateHasChanged();
        //Console.WriteLine(RT.ItemsDataSource.Count());
    }


    EditContext EditContext { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
       
        EditContext = new EditContext(eqWeightSet);


    }



    }
}
