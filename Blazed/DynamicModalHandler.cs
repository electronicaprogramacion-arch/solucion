using Blazed.Controls.Toast;
using Blazed.Controls;
using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CalibrationSaaS.Domain.Aggregates.Interfaces;
using Microsoft.JSInterop;


using BlazorApp1.Client.Security;


namespace BlazorApp1.Blazor.Blazor.Shared
{
    public class DynamicModalHandler : ComponentBase, IDynamicModalHandler
    {
        public string _message { get; set; }
     
        private readonly IModalService _modalService;

        public Func<IModalService, string, string, Task<string>> OnShowModal { get; set; }

        public DynamicModalHandler(IModalService modalService)
        {
            _modalService = modalService;
        }



        public async Task<string> ShowModalAsync(string component, string json)
        {
            if (OnShowModal != null)
            {
                var result =  await OnShowModal(_modalService, component, json);
                return result;

            }
            return "No se ha asignado un manejador de modal.";

        }

   

    }

}
