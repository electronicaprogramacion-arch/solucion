
using Blazored.Modal;
using Blazored.Modal.Services;
using Blazor.Controls.Toast;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blazor.Controls
{
//    public abstract class KavokuComponentBase2 : ComponentBase
//    {

//        public string Policy { get; set; }
//        public CallOptions Header { get; set; }



//        //[Inject]
//        //public StateFacade Facade { get; set; }


//        //        [Inject]
//        //public GrpcBearerTokenProvider TokenProvider { get; set; }

//#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
//        public async Task<Metadata> GetHeaderAsync()
//#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
//        {
//            var headers = new Metadata();
//            //var accessTokenResult = await TokenProvider.GetTokenAsync();
//            //headers.Add("Authorization", $"Bearer {accessTokenResult}");
//            //var accessTokenResult = await TokenProvider.RequestAccessToken();
//#pragma warning disable CS0219 // La variable 'accessTokenResult' está asignada pero su valor nunca se usa
//            AccessTokenResult accessTokenResult = null;
//#pragma warning restore CS0219 // La variable 'accessTokenResult' está asignada pero su valor nunca se usa

//            // try
//            // {
//            //     accessTokenResult = await this.TokenProvider.RequestAccessToken(
//            //new AccessTokenRequestOptions()
//            // {
//            //     Scopes = new[] { "GRPC" }
//            // });


//            // }
//            // catch(Exception ex)
//            // {
//            //     Console.WriteLine("Security error");
//            //     throw new Exception("Security error");
//            // }

//            // if (accessTokenResult == null)
//            // {
//            //     Console.WriteLine("Security error");
//            // }

//            //     var AccessToken = string.Empty;

//            //     if (accessTokenResult.TryGetToken(out var token))
//            //     {
//            //         AccessToken = token.Value;
//            //         headers.Add("Authorization", $"Bearer {AccessToken}");
//            //     }

//            return null;
//        }

//        [Inject]
//        public CalibrationSaaS.Domain.Aggregates.Shared.AppSecurity AppSecurity { get; set; }

//        [Inject]
//        public IAuthorizationService AuthorizationService { get; set; }

//        [Parameter]
//        public Component Component { get; set; } = new Component();

//        public bool Saving { get; set; }

//        public bool Loading { get; set; }

//        //[Inject] public ILogger<KavokuComponentBase2> Logger { get; set; }
//        [Inject] public IJSRuntime JSRuntime { get; set; }
//        [Inject] public Blazor.Controls.Toast.IToastService Toast { get; set; }
//        public bool IsClosed { get; set; }
//        [CascadingParameter] public BlazoredModalInstance BlazoredModal { get; set; }
//        protected override async Task OnAfterRenderAsync(bool firstRender)
//        {
//            if (firstRender)
//            {
//                await JSRuntime.InvokeVoidAsync("removeValidClass");
//            }
//        }


//        public virtual async Task CloseModal(object result = null)
//        {
//            IsClosed = true;

//            await CloseProgress();

//            if (BlazoredModal == null)
//            {

//                return;
//            }

//            if (result == null)
//            {
//                await BlazoredModal.CloseAsync(ModalResult.Cancel());
//            }
//            else
//            {
//                await BlazoredModal.CloseAsync(ModalResult.Ok(result));
//            }
//        }

//        string MessageTmp = "";

//        public async Task ExceptionManager(RpcException rpcException)
//        {
//            string Error = "";

//            //|| rpcException.StatusCode == StatusCode.Unauthenticated
//            //   || rpcException.StatusCode == StatusCode.Cancelled
//            //   || rpcException.StatusCode == StatusCode.AlreadyExists



//            if (rpcException.StatusCode > 0)
//            {
//                Error = rpcException.Message;
//            }
//            else
//            {
//                Error = rpcException.Status.Detail;
//            };


//            if (rpcException.StatusCode == StatusCode.FailedPrecondition)
//            {

//                var arr = Error.Split(Environment.NewLine);

//                foreach (var it in arr)
//                {
//                    await ShowError(it);
//                }

//                return;
//            }


//            //if (MessageTmp == Error)
//            //{
//            //    return;
//            //}
//            MessageTmp = Error;

//            if (!string.IsNullOrEmpty(Error) && Error.Contains("|"))
//            {
//                var a = Error.Split("|");

//                foreach (var it in a)
//                {
//                    await ShowError(it);
//                }

//            }
//            else

//                if (!string.IsNullOrEmpty(Error))
//            {
//                //Logger.LogError(Error);
//                await ShowError(Error);
//            }
//            else
//            {
//                await ShowError("Error in operation");
//            }
//            Saving = false;
//            Loading = false;
//            await CloseProgress();
//        }


//        public async Task ExceptionManager(Exception rpcException)
//        {
//            string Error = "";

//            Error = rpcException.Message;

//            //if (MessageTmp == Error)
//            //{
//            //    return;
//            //}
//            Console.WriteLine(Error);
//            MessageTmp = Error;

//            if (!string.IsNullOrEmpty(Error) && Error.Contains("|"))
//            {
//                var a = Error.Split("|");

//                foreach (var it in a)
//                {
//                    await ShowError(it);
//                }

//            }
//            else

//           if (!string.IsNullOrEmpty(Error))
//            {

//                await ShowError(Error);
//                //Logger.LogError(Error);
//                //Toast.ShowError(Error);
//            }
//            Saving = false;
//            Loading = false;
//            await CloseProgress();
//        }

//        public async Task CloseProgress()
//        {
//            if (JSRuntime != null)
//            {
//                Console.WriteLine("CloseProgresskavuko2");
//                await JSRuntime.InvokeVoidAsync("hideProgress", "progressBar");
//            }
//            Saving = false;
//            Loading = false;

//        }

//        public async Task ShowError(string Message)
//        {
//            await ShowToast(Message, ToastLevel.Error);

            

//            await CloseProgress();

//        }

//        string messtemp = "";
//#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
//        public async Task ShowToast(string message, ToastLevel level)
//#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
//        {

//            if (messtemp == message)
//            {
//                return;
//            }
//            else
//            {
//                messtemp = message;
//            }



//            if (level == ToastLevel.Success)
//            {

//                Toast.ShowSuccess(message);
//            }
//            if (level == ToastLevel.Error)
//            {
//                Toast.ShowError(message);
//            }
//            if (level == ToastLevel.Info)
//            {
//                Toast.ShowInfo(message);
//            }
//            if (level == ToastLevel.Warning)
//            {
//                Toast.ShowWarning(message);
//            }

//        }




//        public async Task ShowProgress()
//        {
//            if (JSRuntime != null)
//            {
//                await JSRuntime.InvokeVoidAsync("showProgressBar", "progressBar");
//            }
//        }

//        [CascadingParameter]
//        public Task<AuthenticationState> stateTask { get; set; }
//        public async Task<ClaimsPrincipal> GetUser()
//        {
//            var user = (await stateTask).User;

//            return user;
//        }

//        public async Task<bool> IsInPolicy(Policies policy)
//        {
//            var user = (await stateTask).User;

//            //if (user.HasClaim(c => c.Type == "tech"))
//            //{

//            //}



//            Component.Permission = policy;


//            return await IsInPolicy(policy, Component);
//            //var ed = await AuthorizationService.AuthorizeAsync(user, Component, Policy);

//            //if (ed.Failure != null && ed.Failure.FailCalled)
//            //{
//            //    throw new Exception("You not have access to see this resource");
//            //}

//            //if (ed.Succeeded)
//            //{
//            //    return true;
//            //}
//            //return false;

//        }



//        public async Task<bool> IsInPolicy(Policies policy, Component component)
//        {
//            var user = (await stateTask).User;

//            //if (user.HasClaim(c => c.Type == "tech"))
//            //{

//            //}


//            if (component != null)
//            {
//                component.Permission = policy;
//            }

//            var ed = await AuthorizationService.AuthorizeAsync(user, component, Policy);

//            if (ed.Failure != null && ed.Failure.FailCalled)
//            {
//                throw new Exception("You not have access to see this resource");
//            }

//            if (ed.Succeeded)
//            {
//                return true;
//            }
//            return false;

//        }

//    }
}
