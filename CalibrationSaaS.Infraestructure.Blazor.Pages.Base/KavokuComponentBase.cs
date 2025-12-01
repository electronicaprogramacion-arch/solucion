
using Blazed.Controls;
using Blazed.Controls.Toast;
using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite;
using Fluxor;
using Grpc.Core;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using SqliteWasmHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using static SQLite.SQLite3;
using Component = Helpers.Controls.Component;


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

        public  IComponentRenderMode Rendermode { get; set; }= new InteractiveAutoRenderMode(prerender: false);


        [Inject]
        IServiceProvider Services { get; set; }


        public ISqliteWasmDbContextFactory<CalibrationSaaSDBContextOff2> DbFactory { get; set; } 

        public string ErrorMessage { get; set; }

        public User User { get; set; }


        [Parameter]
        public bool SelectOnly { get; set; }

        [Parameter]
        public bool IsModal { get; set; }

        [Parameter]
        public int? FilterId { get; set; }

        [Inject]
        public  NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Policy { get; set; } = "HasAccess";
        public CallOptions Header { get; set; }

       
        public IAccessTokenProvider TokenProvider { get; set; }

        //[Inject]
        //public IState<TodosState<T>> _ResultState { get; set; }



        //[Inject]
        //public StateFacade Facade { get; set; }


        //        [Inject]
        //public GrpcBearerTokenProvider TokenProvider { get; set; }
        public async Task<CallContext> CallOptions(Component Component = null)
        {

            if(Component != null)
            {
                return new CallOptions(await GetHeaderAsync(Component.Name));
            }
            else
            {
                return new CallOptions(await GetHeaderAsync());
            }
           

        }



        public async Task<CallContext> CallOptions(string Component = "")
        {

            return new CallOptions(await GetHeaderAsync(Component));

        }




        public async Task<Metadata> GetHeaderAsync(string Component="")

        {
            var headers = new Metadata();
           
            var AccessToken = string.Empty;
            //AccessTokenRequestOptions op= new AccessTokenRequestOptions();  

            //op.

            if(TokenProvider != null)
            {
                var accessToken = await TokenProvider.RequestAccessToken();
                if (accessToken?.TryGetToken(out var token) == true)
                {
                    AccessToken = token.Value;
                    headers.Add("Authorization", $"Bearer {AccessToken}");
                    
                }
            }
            if (!string.IsNullOrEmpty(Component))
            {
                headers.Add("Component", Component);
            }


            return headers;
        }

        [Inject]
        public CalibrationSaaS.Domain.Aggregates.Shared.AppSecurity AppSecurity { get; set; }

        [Inject]
        public IAuthorizationService AuthorizationService { get; set; }

        [Parameter]
        public Helpers.Controls.Component Component { get; set; } = new Helpers.Controls.Component();

        public bool Saving { get; set; }

        public bool Loading { get; set; }


        public bool LoadingWait { get; set; }

        public bool IsOnline { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] public IToastService Toast { get; set; }
        public bool IsClosed { get; set; }
        [CascadingParameter] public BlazoredModalInstance BlazoredModal { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && JSRuntime != null)
            {
               

                await JSRuntime.InvokeVoidAsync("removeValidClass");
            }

            if (await IsOffline())
            {
                Rendermode = new InteractiveWebAssemblyRenderMode(prerender: false);
            }
            else
            {
                Rendermode = new InteractiveAutoRenderMode(prerender: false);
            }

                IsOnline = ConnectionStatusService.GetCurrentStatus();
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

        public async Task ExceptionManager(RpcException rpcException, string Additionalinformation = "")
        {
            Console.WriteLine("ExceptionManager");
            //Console.WriteLine(Additionalinformation);
            //Console.WriteLine(rpcException.Message);
            
            if(rpcException.InnerException != null)
            {
                Console.WriteLine(rpcException.InnerException.Message);
            }


            if (rpcException.StackTrace != null)
            {
                Console.WriteLine(rpcException.StackTrace);
            }
            Console.WriteLine("ExceptionManager");

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
                await ShowError(Additionalinformation + " " + Error);
            }
            else
            {
                await ShowError("Error in operation");
            }
            Saving = false;
            Loading = false;
            await CloseProgress();
        }


        public async Task ExceptionManager(Exception rpcException,string Additionalinformation="",ToastLevel level= ToastLevel.Error)
        {

            Console.WriteLine("ExceptionManager");
            //Console.WriteLine(Additionalinformation);


            Console.WriteLine(rpcException.Message);

            if (rpcException.InnerException != null)
            {
                Console.WriteLine(rpcException.InnerException.Message);
            }


            if (rpcException.StackTrace != null)
            {
                Console.WriteLine(rpcException.StackTrace);
            }
            Console.WriteLine("ExceptionManager");




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
                    await ShowError(it,"","",level);
                }

            }
            else

           if (!string.IsNullOrEmpty(Error))
            {

                await ShowError(Additionalinformation + " " + Error,"","",level);
                //Logger.LogError(Error);
                //Toast.ShowError(Error);
            }
            Saving = false;
            Loading = false;
            await CloseProgress();
        }

        public async Task CloseProgress(bool excecuteScript=true)
        {
            try
            {
                 //Console.WriteLine("close");
            if (JSRuntime != null && excecuteScript)
            {
                await JSRuntime.InvokeVoidAsync("hideProgress", "progressBar");
               
            }
            Saving = false;
            Loading = false;
            }
            catch (Exception ex)
            {

            }

           

        }


         public async Task ScrollPosition()
        {
           
            if (JSRuntime != null)
            {
               
                await JSRuntime.InvokeVoidAsync("Nav_ScrollIntoView", "page-top");
            }
           

        }


        public async Task ScrollPosition(string div = "page-top")
        {

            if (JSRuntime != null) //gridWod
            {

                await JSRuntime.InvokeVoidAsync("Nav_ScrollIntoView", div);
            }


        }

        public async Task ShowError(Exception ex,string method="",string line="")
        {
            var Message= ex.Message + " " + method + " " + line;   

            

            await ShowToast(Message, ToastLevel.Error);

            //Console.WriteLine(Message + "stack: " + ex.StackTrace);

            if(ex.InnerException != null)
            {
                //Console.WriteLine(ex.InnerException.Message);
            }

            await CloseProgress();

        }

        public async Task ShowError(string Message, string method = "", string line = "", ToastLevel level = ToastLevel.Error)
        {
             Message =  Message + " " + method + " " + line;

            await ShowToast(Message, level);

            //Console.WriteLine(Message );

            await CloseProgress();

        }

        string messtemp = "";
#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task ShowToast(string message, ToastLevel level,bool always=true)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            if (messtemp == message && always==false)
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




        public async Task ShowProgress(bool focus=true,bool ExecuteScript=true)
        {
            messtemp = "";
            if (JSRuntime != null && ExecuteScript)
            {
                await JSRuntime.InvokeVoidAsync("showProgressBar", "progressBar",focus);
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


        //public async Task<ClaimsPrincipal> CreatePrincipal(ClaimsPrincipal principal)
        //{
        //    var clone = principal.Clone();
        //    var newIdentity = (ClaimsIdentity)clone.Identity;

        //    // Support AD and local accounts
        //    var nameId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier ||
        //                                                      c.Type == ClaimTypes.Name);
        //    if (nameId == null)
        //    {
        //        return principal;
        //    }



        //    // Get user from database
        //    var user = await _userService.GetByUserName(nameId.Value);
        //    if (user == null)
        //    {
        //        return principal;
        //    }

        //    // Add role claims to cloned identity
        //    foreach (var role in user.Roles)
        //    {
        //        var claim = new Claim(newIdentity.RoleClaimType, role.Name);
        //        newIdentity.AddClaim(claim);
        //    }

        //    return clone;
        //}
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // Clone current identity
            var clone = principal.Clone();
            var newIdentity = (ClaimsIdentity)clone.Identity;

            // Support AD and local accounts
            var nameId = principal.Claims.Where(c => c.Type == "name" || c.Type == ClaimTypes.NameIdentifier ||
                                                              c.Type == ClaimTypes.Name ).FirstOrDefault();
            if (nameId == null || User == null)
            {
                return principal;
            }

            
           
            // Get user from database
            //var user = await _userService.GetByUserName(nameId.Value);
            var url = "";//config["Kestrel:Endpoints:Http3:Url"];

            var user = User;//await Program.GetUserById(nameId.Value, url);
            if (user == null)
            {
                return principal;
            }

            //Add role claims to cloned identity
            foreach (var role in user.RolesList2)
            {
                //var claim = new Claim(role., role.Name);
                var ad = newIdentity.Claims.Where(x =>  x.Value == role.Name).FirstOrDefault();
                if (ad == null)
                {
                    Claim c = new Claim("role", role.Name);

                    newIdentity.AddClaim(c);
                }

            }

            foreach (var role in newIdentity.Claims)
            {
                //var claim = new Claim(role., role.Name);
                var ad = user.RolesList2.Where(x =>  x.Name == role.Value).FirstOrDefault();
                if (ad == null && role.Type=="role")
                {
                    newIdentity.RemoveClaim(role);
                }

            }

            return clone;
        }
        public async Task<string> CurrentUserName()
        {
            //For test only TODO
            //return "Yuliana";

            var user = (await stateTask).User;

            var user2 = user.Identity.Name;

            Console.WriteLine("----------------CURRENTUSER-----------: " + user2);
            //var u = context.User.Identity.Name;
            //var ed = await AuthorizationService.AuthorizeAsync(user, null, Policy);
            return user2;

        }


        public async Task<bool> IsOffline()
        {

            if (JSRuntime == null)
            {
                return false;
            }

            try
            {
                var result1 = await JSRuntime.InvokeAsync<string>("isOffline.get");

                if (result1 == "true")
                {
                    return true;
                }
                //CurrentUser result2 = Newtonsoft.Json.JsonConvert.DeserializeObject<CurrentUser>(result);
                return false;


            }
            catch (Exception ex)
            {
                Console.WriteLine("GetPrincipal error: " + ex.Message);
                return false;
            }


        }



        public async Task SetIsOffline(bool IsOffline)
        {

            if (JSRuntime == null)
            {
               await  Task.CompletedTask;
            }

            try
            {
                AppSecurity.IsOffline = IsOffline;

                await JSRuntime.InvokeVoidAsync("isOffline.set", IsOffline);

                //CurrentUser result2 = Newtonsoft.Json.JsonConvert.DeserializeObject<CurrentUser>(result);

                await Task.CompletedTask;

            }
            catch (Exception ex)
            {
                Console.WriteLine("GetPrincipal error: " + ex.Message);
                
            }
        }


        public async Task<CurrentUser> GetPrincipal()
        {

            if (JSRuntime == null)
            {
                Console.WriteLine("JSRuntime error: ");
                return null;
            }

            try
            {
                var result = await JSRuntime.InvokeAsync<string>("currentUser.get");

                CurrentUser result2 = Newtonsoft.Json.JsonConvert.DeserializeObject<CurrentUser>(result);

                return result2;

            }
            catch (Exception ex)
            {
                Console.WriteLine("GetPrincipal error: " + ex.Message);
                return null;
            }
           
            
        }

        public async Task SignInOffline()
        {
            //var currentUser = AuthService.CurrentUser;
            var user = await GetPrincipal();

            if (user != null)
            {
                List<Claim> claims = new List<Claim>();
                var nameClaim = user.Claims.FirstOrDefault(c => c.Key == ClaimTypes.NameIdentifier || c.Key == ClaimTypes.Name);
                foreach (var item in user.Claims)
                {
                    
                    claims.Add(new Claim(item.Key, item.Value));
                }

              

                claims.Add(new Claim(ClaimTypes.Name, nameClaim.Value));

                if (!string.IsNullOrEmpty(user?.Roles))
                {
                    var roles = user.Roles.Split(",");
                    foreach (var rol in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, rol));
                    }

                }

                var identity = new ClaimsIdentity(
                    claims,
                    "WebAssenbly");

                var newUser = new ClaimsPrincipal(identity);

                // AuthService.CurrentUser = newUser;
            }



        }

        public async Task SetPrincipal()
        {
            var uss = (await stateTask).User;

            if(uss.Identity == null)
            {
                throw new Exception("Identity not Found");
            }
                        
            Console.WriteLine("loadparametrics");
            Console.WriteLine(uss.Identity.Name);

            var ussss = new CurrentUser();
            List<CustomClaim> lc = new List<CustomClaim>();

            //var user = new CalibrationSaaS.Domain.Aggregates.Entities.User();

            //user.UserName = uss.Identity.Name;
            ussss.Type = uss.Identity.AuthenticationType;

            foreach (var clai in uss.Claims)
            {

                lc.Add(new CustomClaim() { Key = clai.Type, Value = clai.Value });
            }


            //var principal = new ClaimsPrincipal(new ClaimsIdentity(lc,uss.Identity.AuthenticationType ));

            ussss.Claims = lc;

            if (JSRuntime != null)
            {
                var result = Newtonsoft.Json.JsonConvert.SerializeObject(ussss);


                await JSRuntime.InvokeVoidAsync("currentUser.set", result);
            }



            await Task.CompletedTask;
        }

        public async Task DeletePrincipal()
        {
            if (JSRuntime != null)
            {

                await JSRuntime.InvokeVoidAsync("currentUser.set", "");
            }
            await Task.CompletedTask;
        }



        public async Task<bool> IsInPolicy(Policies policy, Component component,bool showMessage=true)
        {
            var user = (await stateTask).User;

          

            var aa = await TransformAsync(user);

            if (component != null)
            {
                component.Permission = policy;
            }

            if (string.IsNullOrEmpty(Policy))
            {
                Policy = "HasAccess";
            }
            
            var ed = await AuthorizationService.AuthorizeAsync(aa, component, Policy);

            if (ed.Failure != null && ed.Failure.FailCalled && showMessage)
            {
                if(ed.Failure.FailureReasons != null)
                {
                    foreach (var item in ed.Failure.FailureReasons)
                    {
                        await ShowError(item.Message);
                    }
                    return false;
                }

                await ShowError("You not have access to see this resource");
                return false;
                //throw new Exception("You not have access to see this resource");
            }
            else if(ed.Failure != null && ed.Failure.FailCalled && !showMessage)
            {
                return false;
            }

            if (ed.Succeeded)
            {
                return true;
            }
            return false;

        }


       

        [Inject] public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; set; }

       


        public string TypeString { get; set; }



        protected override async Task OnInitializedAsync()
        {
           

            if (Services.GetService<ISqliteWasmDbContextFactory<CalibrationSaaSDBContextOff2>>() is { } db)
            {
                DbFactory = db;
            }

            try
            {
                if (DbFactory != null && Services.GetService<IAccessTokenProvider>() is { } tp)
                {
                    TokenProvider = tp;
                }
                IsOnline = ConnectionStatusService.GetCurrentStatus();

                if (TokenProvider != null)
                {
                    var AccessToken = string.Empty;
                    var accessToken = await TokenProvider.RequestAccessToken();
                    if (accessToken?.TryGetToken(out var token) == true)
                    {
                        AppSecurity.Token = token.Value;
                        //headers.Add("Authorization", $"Bearer {AccessToken}");
                    }
                    else
                    {
                        AppSecurity.Token = "";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in KavokuComponentBase OnInitializedAsync: " + ex.Message);
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

    public abstract class GridResultsComponentBase2<T, R> : FComponentBase where T  : class, IResult2, IResultComp, IResultGen2, IDynamic, IUpdated, ISelect, IResultTesPointGroup, IGenericCalibrationSubType<R>, new() where R : IResult2
    {
        public Func<WorkOrderDetail, int, bool> ValidateGrid { get; set; }

        public Func<WorkOrderDetail, int, List<T>> CreateListGrid { get; set; }

        public Func<WorkOrderDetail, int, List<T>> GetListGrid { get; set; }

        public T RowAfterRender(T lin)
        {
            return default(T);
        }

        public T RowChange(T lin)
        {
            return default(T);
        }

        public T DefaultNew()
        {
            return default(T);
        }

        public async Task ExecuteFormula()
        {

        }
        public static System.Timers.Timer aTimer;


        public void OnUserFinish(Object source, ElapsedEventArgs e)
        {



            aTimer.Stop();




            InvokeAsync(async () =>
            {

                await ExecuteFormula();

            });

        }


        [Parameter]
        public bool Enabled { get; set; } = false;


        [Parameter]
        public Blazed.Controls.ResponsiveTable<T> RT { get; set; } = new Blazed.Controls.ResponsiveTable<T>();


        public bool FirstRender { get; set; }


        public bool EnableWeightSet { get; set; } = true;

        [Parameter]
        public List<WeightSet> WeightSetList2 { get; set; } = new List<WeightSet>();


        public dynamic WeightSetComponent { get; set; } //= new WeightSetComponent();


        public List<T> LIST { get; set; } = new List<T>();


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {



            await base.OnAfterRenderAsync(firstRender);


            var a = Enabled;

            try
            {
                if (firstRender && RT != null)
                {
                    RT.ShowControl = true;
                    RT.ShowLabel = false;
                }


                if (WeightSetComponent == null || WeightSetComponent?.lstWeightSet == null || WeightSetComponent?.lstWeightSet?.Count == 0)
                {

                    EnableWeightSet = false;

                }
                else
                {
                    EnableWeightSet = true;
                }



                FirstRender = firstRender;

                if (WeightSetList2 == null)
                {
                    WeightSetList2 = new List<WeightSet>();
                }






            }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {

            }
            finally
            {

            }


        }
    }

    public abstract class GridResults2ComponentBase2<T,R> : KavokuComponentBase<T> where R : class, new() //where T : IGenericCalibrationCollection<IResult2>
    {

        [CascadingParameter(Name = "CascadeParam3")]
        public T WorkOrderItemCreate { get; set; }


        //[Parameter]
        //public CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces.ICreateItems<T,R> _strCreate { get; set; }

        public dynamic _strCreate { get; set; }

        //[Parameter]
        public CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces.IGetItems<T,R> _strGet { get; set; }


        //[Parameter]
        public CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces.ISelectItems<T> SelectStandards { get; set; }


        //[Parameter]
        public CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces.ISelectItems<T> Select2Standards { get; set; }

        //[Parameter]
        public CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces.INewItem<T, R> _strNew { get; set; }


    }

    public abstract class GridResultsComponentBase<T> : KavokuComponentBase<T>
    {

       

        public Func<WorkOrderDetail, int, bool> ValidateGrid { get; set; }

        public Func<WorkOrderDetail, int, List<T>> CreateListGrid { get; set; }

        public Func<WorkOrderDetail, int, List<T>> GetListGrid { get; set; }

        //public  T RowAfterRender(T lin)
        //{
        //    return default(T);
        //}

        //public T RowChange(T lin)
        //{
        //    return default(T);
        //}

        //public T DefaultNew()
        //{
        //    return default(T);
        //}

    }

    public abstract class KavokuComponentBase<T> : FComponentBase
    {

       
       


        public ModalParameters ModalParameters { get; set; } = new ModalParameters();

        [Parameter]
        [SupplyParameterFromQuery]
        public string? configCode { get; set; }

        public string _Disable = "disabled";

        [Parameter]
        public string EntityID
        {

            get
            {
                return _Disable;
            }
            set
            {

                _Disable = value;


            }
        }

       


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
        public IStateFacade Facade { get; set; }

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
                await ShowError(ex, "ExecuteServiceWithBlock");
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

        // [Parameter]
        //public T eq { get; set; } = new T();





    }
}
