using Blazored.Modal;
using Blazored.Modal.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp1.Blazor.Blazor.Shared;



namespace BlazorApp1.Blazor.Blazor.GenericMethods
{
    public class StandardComponent_Base : WeightSetComponentBase//CalibrationSaaS.Infraestructure.Blazor.KavokuComponentBase<WeightSet>
    {


        public void CreateFields()
        {

        }

       



        public async Task CancelItem(WeightSet arg)
        {

            POE = arg.PieceOfEquipmentID;

            await Grid.FilterList(arg);


        }

       
        

       
      



       

        public WeightSet DefultTestPoint = new WeightSet();

     

       

       

       

       

       


        public List<WeightSet> Format()
        {

            return _Group;
        }

     
       

       

    }
}
