using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace CalibrifyApp.Server.Services
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
                var result = await OnShowModal(_modalService, jsonTolerancePerTestPoint);
                return result;
            }
            return "No se ha asignado un manejador de modal.";
        }
    }
}
