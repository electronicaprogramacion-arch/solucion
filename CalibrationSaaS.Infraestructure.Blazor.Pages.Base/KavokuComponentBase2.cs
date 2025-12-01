
using Blazed.Controls;
using Blazed.Controls.Toast;
using Blazored.Modal;
using Blazored.Modal.Services;

using CalibrationSaaS.Domain.Aggregates.Entities;
using Grpc.Core;
using Helpers.Controls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor
{
    public abstract class KavokuComponentBase2 : ComponentBase
    {

        public string Policy { get; set; }
        public CallOptions Header { get; set; }



        [Inject]
        public IStateFacade Facade { get; set; }      

        [Inject]
        public CalibrationSaaS.Domain.Aggregates.Shared.AppSecurity AppSecurity { get; set; }

        [Inject]
        public IAuthorizationService AuthorizationService { get; set; }

        [Parameter]
        public Helpers.Controls.Component Component { get; set; } = new Helpers.Controls.Component();

        public bool Saving { get; set; }

        public bool Loading { get; set; }

        [Inject] public ILogger<KavokuComponentBase2> Logger { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] public IToastService Toast { get; set; }
        public bool IsClosed { get; set; }
        [CascadingParameter] public BlazoredModalInstance BlazoredModal { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("removeValidClass");
            }
        }


        public virtual async Task CloseModal(object result = null)
        {
            IsClosed = true;

            await CloseProgress();

            if (BlazoredModal == null)
            {

                return;
            }

            if (result == null)
            {
                await BlazoredModal.CloseAsync(ModalResult.Cancel());
            }
            else
            {
                await BlazoredModal.CloseAsync(ModalResult.Ok(result));
            }
        }

        string MessageTmp = "";

        


     

        public async Task CloseProgress(bool excecuteScript = true)
        {
            if (JSRuntime != null && excecuteScript )
            {
                //Console.WriteLine("CloseProgresskavuko2");
                await JSRuntime.InvokeVoidAsync("hideProgress", "progressBar");
            }
            Saving = false;
            Loading = false;

        }

        public async Task ShowError(string Message)
        {
            await ShowToast(Message, ToastLevel.Error);

            

            await CloseProgress();

        }

        string messtemp = "";
#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task ShowToast(string message, ToastLevel level)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            if (messtemp == message)
            {
                return;
            }
            else
            {
                messtemp = message;
            }



            if (level == ToastLevel.Success)
            {

                Toast.ShowSuccess(message);
            }
            if (level == ToastLevel.Error)
            {
                Toast.ShowError(message);
            }
            if (level == ToastLevel.Info)
            {
                Toast.ShowInfo(message);
            }
            if (level == ToastLevel.Warning)
            {
                Toast.ShowWarning(message);
            }

        }




        public async Task ShowProgress(bool excecuteScript = true)
        {
            if (JSRuntime != null && excecuteScript )
            {
                await JSRuntime.InvokeVoidAsync("showProgressBar", "progressBar");
            }
            Saving = true;
            Loading = true;
        }

        [CascadingParameter]
        public Task<AuthenticationState> stateTask { get; set; }
        public async Task<ClaimsPrincipal> GetUser()
        {
            var user = (await stateTask).User;

            return user;
        }

        public async Task<bool> IsInPolicy(Policies policy)
        {
            var user = (await stateTask).User;

            //if (user.HasClaim(c => c.Type == "tech"))
            //{

            //}



            Component.Permission = policy;


            return await IsInPolicy(policy, Component);
            //var ed = await AuthorizationService.AuthorizeAsync(user, Component, Policy);

            //if (ed.Failure != null && ed.Failure.FailCalled)
            //{
            //    throw new Exception("You not have access to see this resource");
            //}

            //if (ed.Succeeded)
            //{
            //    return true;
            //}
            //return false;

        }



        public async Task<bool> IsInPolicy(Policies policy, Helpers.Controls.Component component)
        {
            var user = (await stateTask).User;

            //if (user.HasClaim(c => c.Type == "tech"))
            //{

            //}


            if (component != null)
            {
                component.Permission = policy;
            }

            if (string.IsNullOrEmpty(Policy))
            {
                Policy = "HasAccess";
            }

            var ed = await AuthorizationService.AuthorizeAsync(user, component, Policy);

            if (ed.Failure != null && ed.Failure.FailCalled)
            {
                throw new Exception("You not have access to see this resource");
            }

            if (ed.Succeeded)
            {
                return true;
            }
            return false;

        }

    }
}
