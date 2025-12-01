
using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Fluxor;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using Blazed.Controls.Toast;

namespace CalibrationSaaS.Infraestructure.Blazor
{
   public enum JavaMessageOptions
    {
        Alert,
        Confirm,
        Promp
    }
    public class JavaMessage<T>
    {

        public string Message { get; set; }

        public T Source { get; set; }

        public dynamic Other { get; set; }

        public JavaMessageOptions Type { get; set; }

       public bool IsShowed { get; set; }

        public bool Result { get; set; }

        public dynamic ResultObject { get; set; }

    }

    public abstract class FComponentBase : ComponentBase
    { 

        
        [Parameter]
        public bool SelectOnly { get; set; }

        [Parameter]
        public bool IsModal { get; set; }


        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Policy { get; set; } = "HasAccess";
        public CallOptions Header { get; set; }

        [Inject]
        public IAccessTokenProvider TokenProvider { get; set; }

        //[Inject]
        //public IState<TodosState<T>> _ResultState { get; set; }

       

        //[Inject]
        //public StateFacade Facade { get; set; }


        //        [Inject]
        //public GrpcBearerTokenProvider TokenProvider { get; set; }


        public async Task<Metadata> GetHeaderAsync()

        {
            var headers = new Metadata();
            //var accessTokenResult = await TokenProvider.GetTokenAsync();
            //headers.Add("Authorization", $"Bearer {accessTokenResult}");
            //var accessTokenResult = await TokenProvider.RequestAccessToken();


            // try
            // {
            //     accessTokenResult = await this.TokenProvider.RequestAccessToken(
            //new AccessTokenRequestOptions()
            // {
            //     Scopes = new[] { "GRPC" }
            // });


            // }
            // catch(Exception ex)
            // {
            //     Console.WriteLine("Security error");
            //     throw new Exception("Security error");
            // }

            // if (accessTokenResult == null)
            // {
            //     Console.WriteLine("Security error");
            // }

            var AccessToken = string.Empty;
            var accessToken = await TokenProvider.RequestAccessToken();
            if (accessToken.TryGetToken(out var token))
            {
                AccessToken = token.Value;
                headers.Add("Authorization", $"Bearer {AccessToken}");
            }

            // var accessToken =  await TokenProvider.RequestAccessToken();
            //if (accessToken.TryGetToken(out var token))
            //{
            //    var _token = token.Value;

            //    var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            //    {
            //        if (!string.IsNullOrEmpty(_token))
            //        {
            //            headers.Add("Authorization", $"Bearer {_token}");
            //        }
            //        return Task.CompletedTask;
            //    });
            //}

            //headers.Add("Authorization", $"Bearer {accessToken}");

            return headers;
        }

        [Inject]
        public CalibrationSaaS.Domain.Aggregates.Shared.AppSecurity AppSecurity { get; set; }

        [Inject]
        public IAuthorizationService AuthorizationService { get; set; }

        [Parameter]
        public Component Component { get; set; } = new Component();

        public bool Saving { get; set; }

        public bool Loading { get; set; }

       
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

        public async Task ExceptionManager(RpcException rpcException)
        {
            string Error = "";

            //|| rpcException.StatusCode == StatusCode.Unauthenticated
            //   || rpcException.StatusCode == StatusCode.Cancelled
            //   || rpcException.StatusCode == StatusCode.AlreadyExists


            if(rpcException.StatusCode == StatusCode.DeadlineExceeded)
            {
                 await ShowError("Timeout exception");

                return;
            }


            if (rpcException.StatusCode > 0)
            {
                Error = rpcException.Message;
            }
            else
            {
                Error = rpcException.Status.Detail;
            };


            if (rpcException.StatusCode == StatusCode.FailedPrecondition)
            {

                var arr = Error.Split(Environment.NewLine);

                foreach (var it in arr)
                {
                    await ShowError(it);
                }

                return;
            }


            //if (MessageTmp == Error)
            //{
            //    return;
            //}
            MessageTmp = Error;

            if (!string.IsNullOrEmpty(Error) && Error.Contains("|"))
            {
                var a = Error.Split("|");

                foreach (var it in a)
                {
                    await ShowError(it);
                }

            }
            else

                if (!string.IsNullOrEmpty(Error))
            {
                //Logger.LogError(Error);
                await ShowError(Error);
            }
            else
            {
                await ShowError("Error in operation");
            }
            Saving = false;
            Loading = false;
            await CloseProgress();
        }


        public async Task ExceptionManager(Exception rpcException)
        {
            string Error = "";

            Error = rpcException.Message;

            //if (MessageTmp == Error)
            //{
            //    return;
            //}
            //Console.WriteLine(Error);
            MessageTmp = Error;

            if (!string.IsNullOrEmpty(Error) && Error.Contains("|"))
            {
                var a = Error.Split("|");

                foreach (var it in a)
                {
                    await ShowError(it);
                }

            }
            else

           if (!string.IsNullOrEmpty(Error))
            {

                await ShowError(Error);
                //Logger.LogError(Error);
                //Toast.ShowError(Error);
            }
            Saving = false;
            Loading = false;
            await CloseProgress();
        }

        public async Task CloseProgress()
        {
            //Console.WriteLine("close");
            if (JSRuntime != null)
            {
                await JSRuntime.InvokeVoidAsync("hideProgress", "progressBar");
               
            }
            Saving = false;
            Loading = false;

        }


         public async Task ScrollPosition()
        {
           
            if (JSRuntime != null)
            {
               
                await JSRuntime.InvokeVoidAsync("Nav_ScrollIntoView", "page-top");
            }
           

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




        public async Task ShowProgress(bool focus=true)
        {
            messtemp = "";
            if (JSRuntime != null)
            {
                await JSRuntime.InvokeVoidAsync("showProgressBar", "progressBar",focus);
            }
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



        public async Task<bool> IsInPolicy(Policies policy, Component component)
        {
            var user = (await stateTask).User;

            //if (user.HasClaim(c => c.Type == "tech"))
            //{

            //}


            if (component != null)
            {
                component.Permission = policy;
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


       

        [Inject] IConfiguration Configuration { get; set; }


        public string TypeString { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var AccessToken = string.Empty;
            var accessToken = await TokenProvider.RequestAccessToken();
            if (accessToken.TryGetToken(out var token))
            {
                AppSecurity.Token = token.Value;
                //headers.Add("Authorization", $"Bearer {AccessToken}");
            }
            else
            {
                AppSecurity.Token = "";
            }

            //     if (string.IsNullOrEmpty(TypeString))
            //{
            //    TypeString = Configuration["VersionApp:Assembly"] + "";
            //    Console.WriteLine(TypeString);
            //}

            TypeString = AppSecurity.AssemblyName;

        }

        public async Task<string> Prompt(string message = "Enter a value")
    {
        string valor = "";

        if (JSRuntime != null)
        {
            valor = await JSRuntime.InvokeAsync<string>("prompt", message);
        }

        return valor;


    }

    public async Task<bool> ConfirmAsync(string message = "Are you sure?")
    {
        bool valor =false;

        if (JSRuntime != null)
        {
            valor = await JSRuntime.InvokeAsync<bool>("confirm", message);
        }

        return valor;
    }

    public bool Confirm(string message = "Are you sure?")
    {
        bool valor =false;

        if (JSRuntime != null)
        {
            valor =  JSRuntime.InvokeAsync<bool>("confirm", message).ConfigureAwait(true).GetAwaiter().GetResult();
        }

        return valor;
    }



    }



    //public abstract class KavokuComponentBaseNoCache<T> : FComponentBase
    //{ 
    
    
    //}


    public abstract class KavokuComponentBase<T> : FComponentBase
    {

         public object GetPageState(string route, List<TodosStateDic> aa)
        {

            if (aa != null && aa.Count > 0)
            {
                //foreach (KeyValuePair<string, TodosState> item in aa)
                //{
                //    Logger.LogInformation("keyxxxx " + item.Key);
                //    Logger.LogInformation("key route " + route);


                //        dynamic ent = item.Value.CurrentPagination;
                //        string dd = ent.Component.Route;
                //        Logger.LogInformation("value from component " + dd + " routr " + route);
                //    if (route == dd)
                //    {
                //        return item.Value;


                //    }
                //}

                var a = aa.Where(x => x.Key == route).FirstOrDefault();
                if (a != null)
                {
                    return a.Value;
                }
                else
                {
                    return null;
                }


            }

            return null;
        }



        [Inject]
        public IState<TodosState> _ResultState { get; set; }


        [Inject]
        public StateFacade Facade { get; set; }

         [Inject]
        public IDispatcher _dispatcher { get; set; }

        public async Task<ResultSet<T>> ExecuteServiceWithBlock(Func<Pagination<T>, Task<ResultSet<T>>> Method, Pagination<T> Parameter, Component component)
        {

            try
            {
                 await ShowProgress();
            //Console.WriteLine("ExecuteServiceWithBlock show");

               
            var result = await ExecuteService(Method, Parameter, component);
           
            
            return result;
            }
            catch(Exception ex)
            {
                //Console.WriteLine("ExecuteServiceWithBlock close");
                await ShowError(ex.Message);
                return new ResultSet<T>();
            }
            finally
            {
                 await CloseProgress();
            }


        }

        public async Task<ResultSet<T>> ExecuteService(Func<Pagination<T>, Task<ResultSet<T>>> Method, Pagination<T> Parameter, Component component)
        {
            //Console.WriteLine(component.Route);   
            //Console.WriteLine(component.IsModal);   

            if (component.IsModal || (Parameter != null && !Parameter.SaveCache) || string.IsNullOrEmpty(component.Route))
            {
                 if(Method ==null || Parameter==null || component == null)
                {
                    return new ResultSet<T>();
                }


                var resultitems2 = await Method(Parameter);

                return resultitems2;

            }
            else
            if (_ResultState != null && _ResultState.Value != null && _ResultState.Value.Url != component.Route
                && _ResultState.Value._dicState != null && _ResultState.Value._dicState.Count > 0)
            {
                

                var aa = _ResultState.Value._dicState;

                //if (aa.ContainsKey(component.Route))
                //{
                //Logger.LogInformation("ContainsKey " + component.Route);

                //TodosState r;

                var r = GetPageState(component.Route, aa);

                if (r != null)
                {

                    dynamic entity = r;
                    Component comp = entity.Component;
                    if (entity != null && comp.Route == component.Route)
                    {
                      


                        int page = entity.Page;
                        


                        string filter = entity.Filter;

                        //if (!string.IsNullOrEmpty(filter))
                           



                        dynamic fo = entity.Object;
                       


                        Parameter = entity;

                        

                    }




                }
                else
                {
                    
                }


                //parr = (Pagination<T>)r.CurrentPagination;

                //}


            }
            else
            {
                //Console.WriteLine("no if ");
                IPaginationBase par = Parameter;
            }

            //Console.WriteLine("methodo execute ");

            Parameter.Component = component;
            
             //Console.WriteLine("methodo execute 2");
             if(Method ==null || Parameter==null || component == null)
                {
                    return new ResultSet<T>();
                }
            var resultitems = await Method(Parameter);

            //Console.WriteLine("methodo execute 3");
            //IPaginationBase par = Parameter;
            Facade.LoadTodos2<T>(resultitems, Parameter, component.Route);


            return resultitems;


        }



    }
}
