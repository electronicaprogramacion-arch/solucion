using Blazed.Controls.Toast;
using Blazor.IndexedDB.Framework;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Infraestructure.Blazor;
using CalibrationSaaS.Infraestructure.Blazor.Controls;
using CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite;
using Castle.Components.DictionaryAdapter.Xml;
using Grpc.Core;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using Radzen.Blazor;
using SqliteWasmHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Threading.Tasks;
using Component = Helpers.Controls.Component;
using Policies = Helpers.Controls.Policies;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Basics
{

    //public class MyClass : INew<MyClass>
    //{
    //    public required string Component { get; init; }

    //    public static MyClass New()
    //    {
    //        return new MyClass() { Component = "<DefaultValue>" };
    //    }
    //}

    [Authorize(Policy = "HasAccess")]
    public class Base_Create23<T, S, D> : KavokuComponentBase<T> where T : class, INew<T>, new()
    {
        public virtual void LoadComponent()
        {
            var BaseUrl = NavigationManager.BaseUri;

            var CurrUrl = NavigationManager.Uri;

            CurrUrl = CurrUrl.Replace(BaseUrl, "");

            //Roles = "testrol";

            Policy = "HasAccess";

            Component.Route = CurrUrl + "_" + typeof(T).Name;


            var ass = CurrUrl.Split('/');

            string componenttmp = "";
            if (ass.Length > 0)
            {
                componenttmp = ass[0];
            }
            else
            {
                componenttmp = CurrUrl;
            }

            if (AppSecurity != null && AppSecurity?.Components?.Count > 0)
            {

                var assto = AppSecurity.Components.Where(x => x.Route != null && x.Route.ToLower() == componenttmp.ToLower()).FirstOrDefault();

                if (assto != null)
                {
                    if (!string.IsNullOrEmpty(assto.Group) && string.IsNullOrEmpty(Component.Group))
                    {
                        Component.Group = assto.Group;
                    }

                    componenttmp = assto.Route;
                }
            }


            Component.Name = componenttmp;


            Component.EntityID = EntityID;

            Component.IsModal = IsModal;

            if (!string.IsNullOrEmpty(configCode) && configCode == "JKG")
            {
                Component.ConfigMode = true;
            }
            //if (Component == null)
            //{
            //    Component = new Component();
            //}
            //return Component.Name;
        }
        public bool EnableSubTypeFilter { get; set; } = false;


        public JavaMessage<T> JavaMessage { get; set; }

        [Inject] public ILogger<T> Logger { get; set; }

       
        public ISqliteWasmDbContextFactory<CalibrationSaaSDBContextOff2> DbFactory { get; set; }


        [Inject]
        IServiceProvider Services { get; set; }


        public int paginaActual { get; set; } = 1;
        public int paginasTotales { get; set; } = 10;
        public int paginaSize { get; set; } = 10;
        public int TotalRows { get; set; } = 0;


        [Parameter]
        public bool Enabled { get; set; }



        public bool LabelDown { get; set; } = false;



        [Inject] public S Client { get; set; }
        [Inject] public D AppState { get; set; }



        [Parameter]
        public T eq { get; set; } = new();

       
        public bool formInvalid = true;
        public string LastSubmitResult;
        public EditContext CurrentEditContext { get; set; }

        public string Roles { get; set; }       

        public string NameValidationMessage { get; set; }

        public string FormName { get; set; }

        public string TypeName { get; set; }

        public T Result { get; set; }






        public Dictionary<string, string> ValidationMessages = new Dictionary<string, string>();



        public string Concat(string string1, string string2 = "", string string3 = "", string string4 = "", string string5 = "")
        {

            return string1 + string2 + string3 + string4 + string5;
        }


        public object Format(object value)
        {
            try
            {
                if (value == null)
                {
                    return "";
                }

                return value ??= "";
            }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {

            }

            return "";
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Initializar();
                await RemoveValidateClass();
            }
        }


        public async Task RemoveValidateClass()
        {
            await JSRuntime.InvokeVoidAsync("removeValidClass");
        }


        public bool initializar { get; set; }
        public void Initializar()
        {

            if (initializar)
            {
                return;
            }

            User userDTO = null;
            //TODO : no erase 


            

            LoadComponent();

            if (string.IsNullOrEmpty(Component.Group))
            {
                Component.Group = "admin,tech.HasView";
            }
            else
            {
                Component.Group = Component.Group + "";
            }


            SetEnabled();
            //IsClosed = false;
            //Header = new CallOptions(await GetHeaderAsync());
            eq = T.New(Component.Name);

            CurrentEditContext = new EditContext(eq);

            CurrentEditContext.OnFieldChanged += EditContext_OnFieldChanged;

            initializar = true;
        }


        protected override async Task OnInitializedAsync()
        {

                

               if (Services.GetService<ISqliteWasmDbContextFactory<CalibrationSaaSDBContextOff2>>() is { } env)
                {
                    DbFactory = env;
                }

            Initializar();

            await base.OnInitializedAsync();


        }


        public string GetErrorMessage(string PropertyName)
        {
            return "";

        }

        public void EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            var entity = e.FieldIdentifier.Model.GetType();

            StateHasChanged();
            return;


        }


        public virtual async Task<bool> ContextValidation(bool validate)
        {
            Saving = true;

            //TODO: JP:This line of code is deleting information in the website (Tolerance and testpoint) Is a JS Code, disabled for now
            //await ShowProgress();

            var a = CurrentEditContext.Validate();

            if (a == false)
            {
                ValidationMessages = CurrentEditContext.Model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                   .ToDictionary(prop => prop.Name, prop => "");


                List<string> properties = new List<string>();

                properties = ValidationMessages.Keys.ToList();
                int cont = 0;
                foreach (var prop in properties)
                {
                    var fieldIdentifier = new FieldIdentifier(CurrentEditContext.Model, prop);

                    var mes = CurrentEditContext.GetValidationMessages(fieldIdentifier);

                    string mesagge = "";
                    foreach (var msg in mes)
                    {
                        mesagge = msg;
                    }
                    if (!string.IsNullOrEmpty(mesagge))
                    {
                        //Logger.LogDebug(prop + " " + mesagge);
                        ValidationMessages[prop] = mesagge;
                        cont += 1;
                    }

                }

                if (cont > 4)
                {

                    await ShowToast("Many empty fields, Please Fill all Fields", ToastLevel.Warning);


                    await CloseProgress();
                    return false;

                }


                foreach (var item in ValidationMessages)
                {
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        await ShowToast("Error in Property " + item.Key + ": " + item.Value, ToastLevel.Info);

                        await Task.Delay(200);
                    }
                }

                await CloseProgress();

            }


            //StateHasChanged();
            return a;


        }


        public virtual bool CustomValidation(T Model)
        {
            return true;
        }





        public void Dispose()
        {

        }

        protected override async Task OnParametersSetAsync()
        {

            if (IsClosed)
            {
                return;
            }

            await base.OnParametersSetAsync();

        }


#pragma warning disable CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public virtual async Task<Helpers.Controls.ValueObjects.ResultSet<T>> LoadData(Pagination<T> pag)
#pragma warning restore CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            throw new NotImplementedException();

        }

        public virtual async Task<Helpers.Controls.ValueObjects.ResultSet<T>> LoadData<F>(Pagination<T, F> pag) where F : class, new()
#pragma warning restore CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            throw new NotImplementedException();

        }


       

       
        public  string LoadComponent2()
        {
            var BaseUrl = NavigationManager.BaseUri;

            var CurrUrl = NavigationManager.Uri;

            CurrUrl = CurrUrl.Replace(BaseUrl, "");

            //Roles = "testrol";

            Policy = "HasAccess";

            Component.Route = CurrUrl + "_" + typeof(T).Name;


            var ass = CurrUrl.Split('/');

            string componenttmp = "";
            if (ass.Length > 0)
            {
                componenttmp = ass[0];
            }
            else
            {
                componenttmp = CurrUrl;
            }

            if (AppSecurity != null && AppSecurity?.Components?.Count > 0)
            {

                var assto = AppSecurity.Components.Where(x => x.Route != null && x.Route.ToLower() == componenttmp.ToLower()).FirstOrDefault();

                if (assto != null)
                {
                    if (!string.IsNullOrEmpty(assto.Group) && string.IsNullOrEmpty(Component.Group))
                    {
                        Component.Group = assto.Group;
                    }

                    componenttmp = assto.Route;
                }
            }


            Component.Name = componenttmp;


            Component.EntityID = EntityID;

            Component.IsModal = IsModal;

            if (!string.IsNullOrEmpty(configCode) && configCode == "JKG")
            {
                Component.ConfigMode = true;
            }
            if (Component == null)
            {
                Component = new Component();
            }
            return Component.Name;
        }

        public bool GetAccess(Policies policy, Component component)
        {

            var Hasfull = IsInPolicy(policy, component, false).ConfigureAwait(false).GetAwaiter().GetResult();

            return Hasfull;
        }



        public async Task<string> CurrentUserName()
        {
            //For test only TODO
            //return "Yuliana";

            var user = (await stateTask).User;

            var user2 = user.Identity.Name;

            //Console.WriteLine("----------------CURRENTUSER-----------: " + user2);
            //var u = context.User.Identity.Name;
            //var ed = await AuthorizationService.AuthorizeAsync(user, null, Policy);
            return user2;

        }






        public void SetEnabled()
        {
            //var user= GetUser().ConfigureAwait(false).GetAwaiter().GetResult();


            //if(user.IsInRole())



            var Hasfull = IsInPolicy(Policies.HasFullAccess).ConfigureAwait(false).GetAwaiter().GetResult();

            if (Hasfull)
            {
                Enabled = true;
                return;
            }


            var HasView = IsInPolicy(Policies.HasView).ConfigureAwait(false).GetAwaiter().GetResult();

            var HasEdit = IsInPolicy(Policies.HasEdit).ConfigureAwait(false).GetAwaiter().GetResult();

            if (HasView && !HasEdit)
            {
                Enabled = false;
            }
            else if (HasView && HasEdit)
            {
                Enabled = true;
            }
        }

        //public Dictionary<string, object> EnabledFunction()
        //{
        //    var dict = new Dictionary<string, object>();
        //    if (!Enabled)
        //    {
        //        dict = new Dictionary<string, object>();
        //        dict.Add("disabled", "disabled");
        //        return dict;
        //    }
        //    else
        //    {
        //        return dict;
        //    }

        //}

        public virtual bool GetShowControl()
        {
            return true;
        }


        public virtual bool GetShowLabel()
        {
            return true;
        }


    }



    [Authorize(Policy = "HasAccess")]
    public class Base_Create<T, S, D> : KavokuComponentBase<T> where T : class,new()
    {

        

        //public static IComponentRenderMode Rendermode { get; set; } = new InteractiveServerRenderMode(prerender: false);
        public bool EnableSubTypeFilter { get; set; } = false;

        
        public JavaMessage<T> JavaMessage { get; set; }     

        [Inject] public ILogger<T> Logger { get; set; }

        //@using Microsoft.AspNetCore.Components.WebAssembly.Hosting
        public int paginaActual { get; set; } = 1;
        public int paginasTotales { get; set; } = 10;
        public int paginaSize { get; set; } = 10;
        public int TotalRows { get; set; } = 0;


        [Parameter]
        public bool Enabled { get; set; }

        

        public bool LabelDown { get; set; } = false;


        
        [Inject] public S Client { get; set; }

        [Inject] public D AppState { get; set; }

        [Parameter]
        public T eq { get; set; } = new T();

      
        public bool formInvalid = true;
        public string LastSubmitResult;
        public EditContext CurrentEditContext { get; set; }

        public string Roles { get; set; }
       

        public string NameValidationMessage { get; set; }

        public string FormName { get; set; }

        public string TypeName { get; set; }

        public T Result { get; set; }
       

        public Dictionary<string, string> ValidationMessages = new Dictionary<string, string>();

      

        public string Concat(string string1, string string2 = "", string string3 = "", string string4 = "", string string5 = "")
        {

            return string1 + string2 + string3 + string4 + string5;
        }


        public object Format(object value)
        {
            try
            {
                if (value == null)
                {
                    return "";
                }

                return value ??= "";
            }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {

            }

            return "";
        }


        public bool initializar { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Initializar();
                initializar = true;
                await RemoveValidateClass();
            }
        }


        public async Task RemoveValidateClass()
        {
            await JSRuntime.InvokeVoidAsync("removeValidClass");
        }



        public void Initializar()
        {

            if (initializar)
            {
                return;
            }

            User userDTO = null;
            //TODO : no erase 


            CurrentEditContext = new EditContext(eq);

            LoadComponent();

            if (string.IsNullOrEmpty(Component.Group))
            {
                Component.Group = "admin,tech.HasView";
            }
            else
            {
                Component.Group = Component.Group + "";
            }


            SetEnabled();
            //IsClosed = false;
            //Header = new CallOptions(await GetHeaderAsync());


            CurrentEditContext.OnFieldChanged += EditContext_OnFieldChanged;

            initializar = true;
        }

        protected override async Task OnInitializedAsync()
        {
           

            Initializar();
            

            await base.OnInitializedAsync();


        }


        public string GetErrorMessage(string PropertyName)
        {
            return "";
           
        }

        public void EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            var entity = e.FieldIdentifier.Model.GetType();
         
            StateHasChanged();
            return;


        }


        public virtual async Task<bool> ContextValidation(bool validate)
        {
            Saving = true;

            //TODO: JP:This line of code is deleting information in the website (Tolerance and testpoint) Is a JS Code, disabled for now
            //await ShowProgress();

            var a = CurrentEditContext.Validate();

            if (a == false)
            {
                ValidationMessages = CurrentEditContext.Model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                   .ToDictionary(prop => prop.Name, prop => "");


                List<string> properties = new List<string>();

                properties = ValidationMessages.Keys.ToList();
                int cont = 0;
                foreach (var prop in properties)
                {
                    var fieldIdentifier = new FieldIdentifier(CurrentEditContext.Model, prop);

                    var mes = CurrentEditContext.GetValidationMessages(fieldIdentifier);

                    string mesagge = "";
                    foreach (var msg in mes)
                    {
                        mesagge = msg;
                    }
                    if (!string.IsNullOrEmpty(mesagge))
                    {
                        //Logger.LogDebug(prop + " " + mesagge);
                        ValidationMessages[prop] = mesagge;
                        cont += 1;
                    }

                }

                if (cont > 4)
                {

                    await ShowToast("Many empty fields, Please Fill all Fields", ToastLevel.Warning);


                    await CloseProgress();
                    return false;

                }


                foreach (var item in ValidationMessages)
                {
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        await ShowToast("Error in Property " + item.Key + ": " + item.Value, ToastLevel.Info);

                        await Task.Delay(200);
                    }
                }

                await CloseProgress();

            }


            //StateHasChanged();
            return a;


        }


        public virtual bool CustomValidation(T Model)
        {
            return true;
        }





        public void Dispose()
        {

        }

        protected override async Task OnParametersSetAsync()
        {

            if (IsClosed)
            {
                return;
            }

            await base.OnParametersSetAsync();

        }


#pragma warning disable CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public virtual async Task<Helpers.Controls.ValueObjects.ResultSet<T>> LoadData(Pagination<T> pag)
#pragma warning restore CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            throw new NotImplementedException();

        }

         public virtual async Task<Helpers.Controls.ValueObjects.ResultSet<T>> LoadData<F>(Pagination<T,F> pag) where F:class,new()
#pragma warning restore CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            throw new NotImplementedException();

        }

        public bool GetAccess(Policies policy, Component component)
        {

            var Hasfull = IsInPolicy(policy, component,false).ConfigureAwait(false).GetAwaiter().GetResult();

            return Hasfull;
        }


        public async Task<string> CurrentUserName()
        {
            //For test only TODO
            //return "Yuliana";

            var user = (await stateTask).User;

            var user2 = user.Identity.Name;

            //Console.WriteLine("----------------CURRENTUSER-----------: " + user2);
            //var u = context.User.Identity.Name;
            //var ed = await AuthorizationService.AuthorizeAsync(user, null, Policy);
            return user2;




        }




        public virtual void LoadComponent()
        {
            var BaseUrl = NavigationManager.BaseUri;

            var CurrUrl =  NavigationManager.Uri;

            CurrUrl = CurrUrl.Replace(BaseUrl, "");

            //Roles = "testrol";

            Policy = "HasAccess";

            Component.Route = CurrUrl + "_" + typeof(T).Name;


            var ass = CurrUrl.Split('/');

            string componenttmp = "";
            if (ass.Length > 0)
            {
                componenttmp = ass[0];
            }
            else
            {
                componenttmp = CurrUrl;
            }

            if (AppSecurity != null && AppSecurity?.Components?.Count > 0)
            {

                var assto = AppSecurity.Components.Where(x => x.Route != null && x.Route.ToLower() == componenttmp.ToLower()).FirstOrDefault();

                if (assto != null)
                {
                    if (!string.IsNullOrEmpty(assto.Group) && string.IsNullOrEmpty(Component.Group))
                    {
                        Component.Group = assto.Group;
                    }

                    componenttmp = assto.Route;
                }
            }


            Component.Name = componenttmp;


            Component.EntityID = EntityID;

            Component.IsModal = IsModal;

            if (!string.IsNullOrEmpty(configCode) && configCode == "JKG")
            {
                Component.ConfigMode = true;
            }
            //if (Component == null)
            //{
            //    Component = new Component();
            //}
            //return Component.Name;
        }



        public void SetEnabled()
        {
            //var user= GetUser().ConfigureAwait(false).GetAwaiter().GetResult();


            //if(user.IsInRole())
           


            var Hasfull = IsInPolicy(Policies.HasFullAccess).ConfigureAwait(false).GetAwaiter().GetResult();

            if (Hasfull)
            {
                Enabled = true;
                return;
            }


            var HasView = IsInPolicy(Policies.HasView).ConfigureAwait(false).GetAwaiter().GetResult();

            var HasEdit = IsInPolicy(Policies.HasEdit).ConfigureAwait(false).GetAwaiter().GetResult();

            if (HasView && !HasEdit)
            {
                Enabled = false;
            }
            else if (HasView && HasEdit)
            {
                Enabled = true;
            }
        }

        //public Dictionary<string, object> EnabledFunction()
        //{
        //    var dict = new Dictionary<string, object>();
        //    if (!Enabled)
        //    {
        //        dict = new Dictionary<string, object>();
        //        dict.Add("disabled", "disabled");
        //        return dict;
        //    }
        //    else
        //    {
        //        return dict;
        //    }

        //}

        public virtual bool  GetShowControl()
        {
            return true;
        }


        public virtual bool GetShowLabel()
        {
            return true;
        }

        public void NavigateToEdit(string Route, string EntityID)
        {
            NavigationManager.NavigateTo($"{Route}/{EntityID}");
        }


        public IEnumerable<int> pageSizeOptions = new int[] { 5, 10, 20, 30, 50, 100 };

        // Method to handle page size change
        public async Task OnPageSizeChange(int value)
        {
            pageSize = value;
            if (dataGrid != null)
            {
                // Force a reload of the grid with the new page size
                await dataGrid.Reload();
            }
        }

        // Keep this for backward compatibility
        public RadzenResponsiveTable<T> Grid { get; set; }
        public List<T> ListMan = new List<T>();

        // For the Radzen DataGrid
        public RadzenDataGrid<T> dataGrid;
        public int totalCount;
        public bool isLoading = false;
        public string searchTerm = string.Empty;

        // Paging properties
        public int pageSize = 10;
        public int currentPage = 1;

    }
}
