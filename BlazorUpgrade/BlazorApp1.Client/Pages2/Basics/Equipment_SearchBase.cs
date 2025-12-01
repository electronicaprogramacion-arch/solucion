using Blazor.IndexedDB.Framework;
using Blazored.Modal;
using Blazored.Modal.Services;

using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazed.Controls.Toast;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;

namespace BlazorApp1.Blazor.Pages.Basics
{
    public class EquipmentTemplate_SearchBase : Base_Create<EquipmentTemplate, Func<dynamic, CalibrationSaaS.Application.Services.IBasicsServices<CallContext>>, CalibrationSaaS.Domain.Aggregates.Shared.Basic.AppState>
   
    {
        [CascadingParameter] BlazoredModalInstance BlazoredModal1 { get; set; }
        [CascadingParameter] public IModalService Modal { get; set; }


        [Parameter]
        public List<int> PermitedEquipmentTypeGroup { get; set; }



        public ResponsiveTable<EquipmentTemplate> Grid { get; set; } = new ResponsiveTable<EquipmentTemplate>();
        public List<EquipmentTemplate> ListETemp = new List<EquipmentTemplate>();


        public override async Task<ResultSet<EquipmentTemplate>> LoadData(Pagination<EquipmentTemplate> pag)
        {

            BasicsServiceGRPC basics = new BasicsServiceGRPC(Client, DbFactory);

            if(PermitedEquipmentTypeGroup != null && PermitedEquipmentTypeGroup?.Count > 0)
            {
                pag.EntityType = Newtonsoft.Json.JsonConvert.SerializeObject(PermitedEquipmentTypeGroup); ;
            }
           

            var Eq = await basics.GetEquipment(pag);

            return Eq;
        }


#pragma warning disable CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnAfterRenderAsync(bool firstRender)
#pragma warning restore CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            //await ShowProgress();

            if (!firstRender)
            {
                return;
            }

           

            FormName = "Search Equipment";

            TypeName = "Equipment";

            //IsModal = IsModal;

            //SelectOnly = SelectOnly;

          
        }



#pragma warning disable CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task GetDetail(int ID)
#pragma warning restore CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            //searchComponent.Detail = searchComponent.List.Where(x => x.EquipmentTemplateID == ID).FirstOrDefault();


            ////var messageForm = Modal.Show<Equipment_Create>("Passing Data");
            ////var result = await messageForm.Result;
            //await searchComponent.ShowModal();

            //StateHasChanged();

            //await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));

        }

#pragma warning disable CS0108 // 'EquipmentTemplate_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<EquipmentTemplate, Func<IIndexedDbFactory, IBasicsServices<CallContext>>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intenci�n era ocultarlo.
        public void Dispose()
#pragma warning restore CS0108 // 'EquipmentTemplate_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<EquipmentTemplate, Func<IIndexedDbFactory, IBasicsServices<CallContext>>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intenci�n era ocultarlo.
        {

        }

        public IEnumerable<EquipmentTemplate> FilterList(string filter)
        {

            return null;

        }

#pragma warning disable CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task SelectModal(EquipmentTemplate DTO)
#pragma warning restore CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            //await BlazoredModal1.Close(ModalResult.Ok(searchComponent.Detail));
        }

        public async Task<bool> Delete(EquipmentTemplate DTO)
        {
            BasicsServiceGRPC basic = new BasicsServiceGRPC(Client, DbFactory);

            var result = await basic.DeleteEquipment(DTO);

            return true;
        }

#pragma warning disable CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected async Task FormSubmitted(EditContext editContext)
#pragma warning restore CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            //CurrentEditContext = new EditContext(Grid.currentEdit);

            //bool re = await ContextValidation(true);

            //var msg = ValidationMessages;
            ////if (  re  || CustomValidation(eq))
            //if (1 == 1)
            //{

            //    try
            //    {

            //        BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);


            //        Result = (await basics.CreateEquipment(eq));


            //        await ShowToast("Succes Save", ToastLevel.Success);

            //        await CloseModal(Result);

            //    }
            //    catch (Exception ex)
            //    {
            //        await ShowError("Error Save " + ex.Message);

            //    }

            //}
        }

        //protected async Task Submitted(ChangeEventArgs arg)
        //{

        //    var aa = (EquipmentTemplate)arg.Value;

        //    if (1 == 1)
        //    {


        //        try
        //        {

        //            BasicsServiceGRPC basics = new BasicsServiceGRPC(Client, DbFactory);


        //            Result = (await basics.CreateEquipment(aa));


        //            await ShowToast("The information has been saved successfully.", ToastLevel.Success);


        //            await CloseModal(Result);


        //        }

        //        catch (Exception ex)
        //        {


        //            await ShowError("Error Save " + ex.Message);

        //            throw ex;

        //        }

        //    }
        //}


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            

            eq = new EquipmentTemplate();
            eq.Manufacturer1 = new Manufacturer();
            eq.EquipmentTypeObject = new EquipmentType();
            if (AppState.EquipmentTypes == null || AppState?.EquipmentTypes?.Count == 0)
            {
                BasicsServiceGRPC basic = new BasicsServiceGRPC(Client, DbFactory);

                var a = await basic.GetEquipmenTypes();

                AppState.EquipmentTypes = a.EquipmentTypes;
            }
            //eq. = new EquipmentTemplate();
        }

#pragma warning disable CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
//        protected async Task SubmittedUP(ChangeEventArgs arg)
//#pragma warning restore CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
//        {

//            var aa = (EquipmentTemplate)arg.Value;


      
//        }
    }
}
