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

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.AssetsBasics
{
    public class Weight_CreateBase
        : Base_Create<WeightSet, IAssetsServices<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
    // , IPage<WeightSet, IAssetsServices<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    {
    
        [Inject] 
        public AppState AppState2 { get; set; }

        [Parameter]
        public EquipmentType EquipmentTypeObject { get; set; }

        [CascadingParameter(Name="CascadeParam1")]
    public PieceOfEquipment POE { get; set; }

    [Inject] Application.Services.IAssetsServices<CallContext> _assetsServices { get; set; }
    public WeightSet eqWeightSet = new WeightSet();


    public bool IsForAccredited { get; set; } = false;
        public List<WeightSet> LIST1 { get; set; } = new List<WeightSet>();

        public ResponsiveTable<WeightSet> RT { get; set; } = new ResponsiveTable<WeightSet>();

        public ResponsiveTable<WeightSet> Grid { get; set; }


        [Parameter]
        public ICollection<WeightSet> List { get; set; } = new List<WeightSet>();


        public bool stateChange { get; set; } = false;



        public Task SelectModal(WeightSet DTO)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WeightSet> FilterList(string filter = "")
        {

            if (Grid != null && Grid.ItemsDataSource != null)
            {
                var templist = Grid.ItemsDataSource;

                //return null;

                return templist.Where(i => i.Name.ToLower().Contains(filter.ToLower()

                    )).ToArray();
            }
            else
            {
                return null;
            }


        }

        public async Task<bool> Delete(Domain.Aggregates.Entities.WeightSet DTO)
        {
            //await searchComponent.ShowModalAction();

            await Client.DeleteWeightSet(DTO, new CallContext());


            return true;

            //searchComponent.ShowResult();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {


            await base.OnAfterRenderAsync(firstRender);




        }




    public WeightSet DefaultItem()
    {
        WeightSet w = new WeightSet();
        if (POE.UnitOfMeasureID.HasValue)
        {
            w.UnitOfMeasureID = POE.UnitOfMeasureID.Value;
            w.UncertaintyUnitOfMeasureId = POE.UnitOfMeasureID.Value;

            return w;
        }
       
            return w;
        

    }

    public WeightSet DefaultItemAcr()
    {
        WeightSet w = new WeightSet();
            if (POE.UnitOfMeasureID.HasValue)
            {
                 w.UnitOfMeasureID = POE.UnitOfMeasureID.Value;
            }
       

        return w;

    }



    
    public void NewItem()
    {

    }

    public void Show(List<WeightSet> group, bool IsAccredited)
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
        
            LIST1 = POE.WeightSets.ToList();
            EditContext = new EditContext(eqWeightSet);


    }



        public void Changetype(ChangeEventArgs test, WeightSet entity)
        {


            var typeDistribution = test.Value.ToString();
            Console.WriteLine("Divisor " + eq.Divisor.ToString());


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
            Console.WriteLine("Divisor New " + eq.Divisor.ToString());

        }


    }
}
