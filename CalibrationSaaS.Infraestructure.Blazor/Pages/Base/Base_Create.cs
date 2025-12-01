using Blazor.IndexedDB.Framework;

using CalibrationSaaS.Domain.Aggregates.Entities;
using Helpers.Controls.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor;
using CalibrationSaaS.Infraestructure.Blazor.Shared;
using Grpc.Core;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Helpers.Controls;
using Policies = Helpers.Controls.Policies;
using Blazed.Controls.Toast;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Basics
{
    [Authorize(Policy = "HasAccess")]
    public class Base_Create<T, S, D> : KavokuComponentBase<T> where T :  new()
    {
        public JavaMessage<T> JavaMessage { get; set; }     

        [Inject] public ILogger<T> Logger { get; set; }

        [Inject]
        public IIndexedDbFactory DbFactory { get; set; }

        public int paginaActual { get; set; } = 1;
        public int paginasTotales { get; set; } = 10;
        public int paginaSize { get; set; } = 10;
        public int TotalRows { get; set; } = 0;

        //public CustomValidator customValidator;
        // public bool IsClosed { get; set; }

        [Parameter]
        public bool Enabled { get; set; }

        //  [Inject] public IToastService Toast { get; set; }

        public bool LabelDown { get; set; } = false;


        //public bool Enabled { get; set; } = true;


        //[CascadingParameter] public BlazoredModalInstance BlazoredModal { get; set; }
        [Inject] public S Client { get; set; }
        // [Inject] public IJSRuntime JSRuntime { get; set; }
        //[Inject] public Domain.Aggregates.Shared.Basic.AppState AppState { get; set; }
        [Inject] public D AppState { get; set; }

        //[Inject] ILogger Logger { get; set; } 
        [Inject] public CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany CommonList { get; set; }

        [Parameter]
        public T eq { get; set; } = new T();

        public string _Disable = "disabled";
        public bool formInvalid = true;
        public string LastSubmitResult;
        public EditContext CurrentEditContext { get; set; }

        public string Roles { get; set; }




        //public StandardModal child;



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

        public string NameValidationMessage { get; set; }

        public string FormName { get; set; }

        public string TypeName { get; set; }

        public T Result { get; set; }

        public string ID { get; set; }

        public string NextProcessUrl { get; set; }

        public string NextProcessLabel { get; set; }

        //public Dictionary<string, string> EmptyValidationDictionary { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, string> EmptyValidationMessage { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, string> ValidationMessages = new Dictionary<string, string>();

        //public async Task ShowModal()
        //{
        //    await child.ShowModal("We are creating: " + eq.Name, "Please wait", true, true, "Add more " + TypeName);

        //}






        //public void  ShowResult() 
        //{ 

        //child.ShowResult("The " + TypeName + "  was created: " + ID, TypeName + " Name: " + Result.Name, NextProcessUrl, NextProcessLabel);

        //}

        //protected async Task InvalidFormSubmitted(EditContext editContext)
        //{
        //    LastSubmitResult = "OnInvalidSubmit was executed";
        //}
        //[Inject]
        //public IAccessTokenProvider TokenProvider { get; set; }

        //public async Task<Metadata> GetHeaderAsync()
        //{
        //    var headers = new Metadata();
        //    var accessTokenResult = await TokenProvider.RequestAccessToken();
        //    var AccessToken = string.Empty;
        //    if (accessTokenResult.TryGetToken(out var token))
        //    {
        //        AccessToken = token.Value;
        //        headers.Add("Authorization", $"Bearer {AccessToken}");
        //    }

        //    return headers;
        //}

        //public CallOptions Header { get; set; }


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
                await RemoveValidateClass();
            }
        }


        public async Task RemoveValidateClass()
        {
            await JSRuntime.InvokeVoidAsync("removeValidClass");
        }


        protected override async Task OnInitializedAsync()
        {

            CurrentEditContext = new EditContext(eq);

            LoadComponent();            

            Component.Group = "admin,tech";

            SetEnabled();
            //IsClosed = false;
            Header = new CallOptions(await GetHeaderAsync());


            CurrentEditContext.OnFieldChanged += EditContext_OnFieldChanged;

            await base.OnInitializedAsync();


        }


        public string GetErrorMessage(string PropertyName)
        {
            return "";
            //if (EmptyValidationMessage.ContainsKey(PropertyName)) { 
            //return EmptyValidationMessage[PropertyName];
            //}
            //else
            //{
            //return     "";
            //}
        }

        public void EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            var entity = e.FieldIdentifier.Model.GetType();
            //Logger.LogDebug(entity.Name);
            //Logger.LogDebug("EditContext_OnFieldChanged");
            StateHasChanged();
            return;


#pragma warning disable CS0162 // Se detectó código inaccesible
            var properties = e.FieldIdentifier.Model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
#pragma warning restore CS0162 // Se detectó código inaccesible
              .ToDictionary(prop => prop.Name, prop => prop.GetValue(e.FieldIdentifier.Model, null));
            //SetOkDisabledStatus();
            //Logger.LogDebug("EditContext_OnFieldChanged");
            if (entity.Name == "PhoneNumber")
            {
                if (e.FieldIdentifier.FieldName == "CountryID")
                {
                    //var value=  CommonList.Countries[properties[e.FieldIdentifier.FieldName].ToString()];
                    //Type type = entity;
                    //PropertyInfo prop = type.GetProperty("Country");
                    //prop.SetValue(e.FieldIdentifier.Model, value, null);

                }
                else if (e.FieldIdentifier.FieldName == "TypeID")
                {

                    var value = CommonList.PhoneNumberTypes[properties[e.FieldIdentifier.FieldName].ToString()];
                    Type type = entity;
                    PropertyInfo prop = type.GetProperty("Country");
                    prop.SetValue(e.FieldIdentifier.Model, value, null);


                }



            }






        }


        //public virtual bool ContextValidation()
        //{

        //    var a = CurrentEditContext.Validate();
        //    //StateHasChanged();
        //    return a;


        //}


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



        //protected void FunctionName(EventArgs args)
        //{
        //    if(eq.Name.Length < 1)
        //    {
        //        NameValidationMessage = "invalid";
        //    }
        //    else
        //    {
        //        NameValidationMessage = "valid";
        //    }
        //    Logger.LogDebug(this.eq.Name);
        //    StateHasChanged();

        //}

        public void IsEmptyValidator(string FieldName, object Field)
        {
            //var value = Field;

            //if (value != null)
            //{
            //    string valueString = value.ToString();

            //    if (valueString.Length < 1)
            //    {
            //        EmptyValidationDictionary[FieldName] = "invalid";
            //    }
            //    else
            //    {
            //        EmptyValidationDictionary[FieldName] = "valid";
            //        // NameValidationMessage = "valid";
            //    }
            //    Logger.LogDebug(valueString);
            //    StateHasChanged();
            //}

        }


#pragma warning disable CS0693 // El parámetro de tipo 'T' tiene el mismo nombre que el parámetro de tipo del tipo externo 'Base_Create<T, S, D>'
        public static T GetAttributeFrom<T>(object instance, string propertyName) where T : Attribute
#pragma warning restore CS0693 // El parámetro de tipo 'T' tiene el mismo nombre que el parámetro de tipo del tipo externo 'Base_Create<T, S, D>'
        {
            var attributeType = typeof(T);
            var property = instance.GetType().GetProperty(propertyName);
            if (property == null) return default(T);
            return (T)property.GetCustomAttributes(attributeType, false).FirstOrDefault();
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


#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public virtual async Task<ResultSet<T>> LoadData(Pagination<T> pag)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            throw new NotImplementedException();

        }

        //[Parameter]
        //public Component Component { get; set; } = new Component();
        public void LoadComponent()
        {
            var BaseUrl = NavigationManager.BaseUri;

            var CurrUrl = NavigationManager.Uri;

            CurrUrl = CurrUrl.Replace(BaseUrl, "");

            //Roles = "testrol";

            Policy = "HasAccess";

            Component.Route = CurrUrl + "_" + typeof(T).Name;

            Component.Name = CurrUrl;

            Component.IsModal = IsModal;   

        }



        public bool GetAccess(Policies policy, Component component)
        {

            var Hasfull = IsInPolicy(policy, component).ConfigureAwait(false).GetAwaiter().GetResult();

            return Hasfull;
        }



        public async Task<string> CurrentUserName()
        {
            var user = (await stateTask).User;

            //var u = context.User.Identity.Name;
            //var ed = await AuthorizationService.AuthorizeAsync(user, null, Policy);
            return user.Identity.Name;
        }






        //public void IsInCondition(Policies pol)
        //{
        //    var Hasfull = IsInPolicy().ConfigureAwait(false).GetAwaiter().GetResult();

        //    if (Hasfull)
        //    {
        //        Enabled = true;
        //        return;
        //    }
        //}


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
                Enabled = false;
            }
        }

        public Dictionary<string, object> EnabledFunction()
        {
            var dict = new Dictionary<string, object>();
            if (!Enabled)
            {
                dict = new Dictionary<string, object>();
                dict.Add("disabled", "disabled");
                return dict;
            }
            else
            {
                return dict;
            }

        }
    }
}
