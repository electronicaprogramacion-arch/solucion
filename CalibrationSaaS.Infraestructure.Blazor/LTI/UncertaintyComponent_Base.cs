using Blazor.IndexedDB.Framework;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics.TestPoints;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazed.Controls.Toast;
using Helpers.Controls.ValueObjects;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI
{
    public class UncertaintyComponent_Base:Base_Create<Uncertainty, Func<dynamic, 
        Application.Services.IPieceOfEquipmentService<CallContext>>, 
        Domain.Aggregates.Shared.Basic.AppState>
    {




        [Parameter]
         public int? EquipmentTemplateID { get; set; } 

        [Parameter]
        public string PieceOfEquipmentID { get; set; }


        [CascadingParameter(Name = "CascadeParam1")]
        public PieceOfEquipment POE { get; set; }


        [CascadingParameter(Name = "CascadeParamET")]
        public EquipmentTemplate EquipmentTemplate { get; set; }


        [Parameter]
        public bool IsPieceOfEquipment { get; set; }
        public bool stateChange { get; set; } = false;

        public List<Uncertainty> LIST { get; set; } = new List<Uncertainty>();



        public ResponsiveTable<Uncertainty> Grid = new ResponsiveTable<Uncertainty>();
       
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (eq.UncertaintyID == 0)
            {

                eq.ConfidenceLevel = 2;
            }


           
            if(POE != null)
            {
                EquipmentTemplate= POE.EquipmentTemplate;
            }


        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //Console.WriteLine("Divisor render " + eq.Divisor.ToString());
           
        }

            public void Changetype(ChangeEventArgs test, Uncertainty entity)
        {
             
            
            var typeDistribution = test.Value.ToString();
            //Console.WriteLine("Divisor "  + eq.Divisor.ToString());


            if (typeDistribution == "1")
            {
                entity.Divisor = 1.73;
            }
            else if
                (typeDistribution == "2")
            {
                entity.Divisor = 1;
            }
            else if
                (typeDistribution == "3")
            {
                entity.Divisor = 2;
            }
            else if
                (typeDistribution == "4")
            {
                entity.Divisor = 3.46;
            }

            stateChange = true;
            StateHasChanged();
            //Console.WriteLine("Divisor New " + eq.Divisor.ToString());

        }
        public Uncertainty AddNew()
        {
            Uncertainty un = new Uncertainty();
           
                un.ConfidenceLevel = 2;
           

            
            return un;
        }
        public override async Task<ResultSet<Uncertainty>> LoadData(Pagination<Uncertainty> pag)
        {


            pag.Entity = new Uncertainty();
            var _poeGrpc = new PieceOfEquipmentGRPC(Client, DbFactory);

            if (!string.IsNullOrEmpty(PieceOfEquipmentID))
            {

                pag.Entity.PieceOfEquipmentID = PieceOfEquipmentID;
            }
            if (EquipmentTemplateID.HasValue)
            {
                //pag.Entity = new Uncertainty();
                pag.Entity.EquipmentTemplateID = EquipmentTemplateID;
            }



            var result = await _poeGrpc.GetUncertainty(pag);
            
            return result;
        }

        

    }
}
