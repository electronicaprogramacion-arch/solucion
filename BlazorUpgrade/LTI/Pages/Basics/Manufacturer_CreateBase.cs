
using Blazed.Controls.Toast;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Basics
{
    public class Manufacturer_CreateBase : Base_Create<Manufacturer, Application.Services.IBasicsServices<CallContext>,
        Domain.Aggregates.Shared.Basic.AppState>
    {

#pragma warning disable CS0108 // 'Manufacturer_CreateBase.Logger' oculta el miembro heredado 'KavokuComponentBase<Manufacturer>.Logger'. Use la palabra clave new si su intención era ocultarlo.
        [Inject] ILogger<Manufacturer_CreateBase> Logger { get; set; }
#pragma warning restore CS0108 // 'Manufacturer_CreateBase.Logger' oculta el miembro heredado 'KavokuComponentBase<Manufacturer>.Logger'. Use la palabra clave new si su intención era ocultarlo.

#pragma warning disable CS0108 // 'Manufacturer_CreateBase.EnabledFunction()' oculta el miembro heredado 'Base_Create<Manufacturer, IBasicsServices<CallContext>, AppState>.EnabledFunction()'. Use la palabra clave new si su intención era ocultarlo.
        public Dictionary<string, object> EnabledFunction()
#pragma warning restore CS0108 // 'Manufacturer_CreateBase.EnabledFunction()' oculta el miembro heredado 'Base_Create<Manufacturer, IBasicsServices<CallContext>, AppState>.EnabledFunction()'. Use la palabra clave new si su intención era ocultarlo.
        {
            if (!Enabled)
            {
                var dict = new Dictionary<string, object>();
                dict.Add("disabled", "disabled");
                return dict;
            }
            else
            {
                return null;
            }

        }


        protected override async Task OnParametersSetAsync()
        {

            Logger.LogDebug(EntityID);

            NameValidationMessage = "valid";

            if (Convert.ToInt32(EntityID) == 0)
            {
                eq = new Manufacturer();
                eq.IsEnabled = true;
            }
            else
            {



                if (AppState?.Manufacturers != null && AppState?.Manufacturers?.Count > 0)
                {
                    eq = AppState.Manufacturers.Where(x => x.ManufacturerID == Convert.ToInt32(EntityID)).FirstOrDefault();
                }
                else
                {
                    BasicsServiceGRPC b = new BasicsServiceGRPC(Client);

                    var all = await b.GetAllManufacturers();

                    AppState.Manufacturers = all.Manufacturers;

                    eq = AppState.Manufacturers.Where(x => x.ManufacturerID == Convert.ToInt32(EntityID)).FirstOrDefault();

                }





            }

            CurrentEditContext = new EditContext(eq);
        }

        [Parameter]
        public Func<Manufacturer, Task> EndExecute { get; set; }

        protected async Task FormSubmitted(EditContext editContext)
        {

            var validate = await ContextValidation(true);

            if (validate || CustomValidation(eq))
            {
                Tenant tenant = new Tenant();
                //await ShowModal();

                LastSubmitResult = "OnValidSubmit was executed";


                try
                {

                    BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);

                    var manufacturerExist = await basics.GetManufacturerXName(eq, new CallContext());

                    if (manufacturerExist != null && manufacturerExist.Name != null) 
                    {
                        await ShowError("Manufacturer Name already exists");
                        return;
                    }
                    Result = (await basics.CreateManufacturer(eq, new CallContext()));

                    await ShowToast("The information has been saved successfully.", ToastLevel.Success);

                    await CloseModal(Result);

                    if (EndExecute != null)
                    {
                        await EndExecute(Result);
                    }

                    basics.Dispose();

                }

                catch (Exception ex)

                {
                    

                    await ShowError("Error Manufacturer Creation " + ex.Message);

                }


                //ShowResult();
            }

        }
#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected async Task InvalidFormSubmitted(EditContext editContext)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            LastSubmitResult = "OnInvalidSubmit was executed";
        }


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            TypeName = "Manufacturer";

            //CurrentEditContext.OnFieldChanged += HandleFieldChanged;


        }

        protected void FunctionName(EventArgs args)
        {
            if (eq.Name.Length < 1)
            {
                NameValidationMessage = "invalid";
            }
            else
            {
                NameValidationMessage = "valid";
            }
            Logger.LogDebug(this.eq.Name);
            StateHasChanged();

        }

        public new void Dispose()
        {

        }

    }
}
