using Blazed.Controls.Toast;
using Blazed.Controls;
using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazored.Modal.Services;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using Microsoft.JSInterop;
using ResolutionComponente = CalibrationSaaS.Infraestructure.Blazor.Pages.Basics.ResolutionComponent;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;


namespace CalibrationSaaS.Infraestructure.Blazor.Shared
{
    public class ModalHandler : ComponentBase, IModalHandler
    {
        public string _message { get; set; }
     
        private readonly IModalService _modalService;

        public Func<IModalService, string, Task<string>> OnShowModal { get; set; }

        public ModalHandler(IModalService modalService)
        {
            _modalService = modalService;
        }



        public async Task<string> ShowModalAsync(string jsonTolerancePerTestPoint)
        {
            if (OnShowModal != null)
            {
                var result =  await OnShowModal(_modalService, jsonTolerancePerTestPoint);
                return result;

            }
            return "No se ha asignado un manejador de modal.";

        }

   

    }

}
