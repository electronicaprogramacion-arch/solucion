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




namespace BlazorApp1.Client.Security
{

    public interface IModalHandler
    {

        public Task<string> ShowModalAsync(string jsonTolerancePerTestPoint);

        Func<IModalService, string, Task<string>> OnShowModal { get; set; }



    }
    public interface IDynamicModalHandler
    {

        public Task<string> ShowModalAsync(string componentName, string jsonData);
        Func<IModalService, string, string, Task<string>> OnShowModal { get; set; }



    }


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
