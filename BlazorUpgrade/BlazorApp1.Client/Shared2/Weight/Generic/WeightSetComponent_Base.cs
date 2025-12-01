using Blazored.Modal;
using Blazored.Modal.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helpers.Controls.ValueObjects;

using System.Text.RegularExpressions;
using BlazorApp1.Blazor.Blazor.Shared;


namespace BlazorApp1.Blazor.Blazor.GenericMethods
{
   


    public class WeightSetComponent_Base : WeightSetComponentBase
    {


        public void CreateFields()
        {

        }
      

        [Parameter]
        public bool Certified { get; set; }

       


        [Parameter]
        public Func<WeightSet,Task> DeleteFunction { get; set; }


       

       

        public WeightSet DefultTestPoint = new WeightSet();

      

      

       

      

       

        public List<WeightSet> Format()
        {

            return _Group;
        }

        [Parameter]
        public int GroupTestPointID { get; set; }

        //public EditContext CurrentEditContext { get; set; }


        
     

        public void ChangeCheckBox(ChangeEventArgs e)
        {
            if (Accredited)
            {
                Accredited = false;
            }
            else
            {
                Accredited = true;
            }

        }

        

    }
}
