using Blazored.Modal.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Basics
{
    public class EquipmentTp_SearchBase : Base_Create<EquipmentType, Application.Services.IBasicsServices<CallContext>,
        Domain.Aggregates.Shared.Basic.AppState>
    //,  IPage<EquipmentType, Application.Services.IBasicsServices<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
    {


        //[Inject]
        //NavigationManager NavigationManager { get; set; }

        [CascadingParameter] public IModalService Modal { get; set; }

        public ResponsiveTable<EquipmentType> Grid { get; set; } = new ResponsiveTable<EquipmentType>();

       


        //public Search<EquipmentType, IBasicsServices<CallContext>, AppState> searchComponent
        //{ get; set; } = new Search<EquipmentType, IBasicsServices<CallContext>, AppState>();

        public List<EquipmentType> ListET = new List<EquipmentType>();


        public override async Task<ResultSet<EquipmentType>> LoadData(Pagination<EquipmentType> pag)
        {

            BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);

            //var Eq = await basics.GetEquipment();//(await searchComponent.Client.GetEquipment());
            // pag.ColumnName = "EquipmentTypeID";
            var Eq = (await basics.GetEquipmenTypesPag(pag));

            Eq.List = Eq.List.Where(x => x.IsEnabled == true).ToList();

            if (Eq.List != null)
            {
                foreach (var item in Eq.List)
                {
                    var item2 = AppState.EquipmentTypes.Where(x => x.EquipmentTypeID == item.EquipmentTypeID).FirstOrDefault();

                    if (item2 == null && item.IsEnabled)
                    {
                        AppState.AddEquipmentType(item);
                    }

                }
            }
            

            return Eq;

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }
            await base.OnParametersSetAsync();

            //   await searchComponent.ShowProgress();
            //searchComponent = new Search<EquipmentTemplate, IBasicsServices, AppState>();

            //BaseCreateUrl = "EquipmentTypeCreate";

            //BaseDetailUrl = "EquipmentTypeDetail";

            FormName = "Search Equipment Type";

            TypeName = "EquipmentType";

            IsModal = IsModal;

            SelectOnly = SelectOnly;

            //var Eq = (await searchComponent.Client.GetEquipment());

            //searchComponent.List = Eq.EquipmentTemplates;

            //if (searchComponent.Client != null)
            //{

            //BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);

            //var Eq = await basics.GetEquipment();//(await searchComponent.Client.GetEquipment());

            //var Eq = (await basics.GetEquipmenTypes());

            //if (Eq != null && Eq.EquipmentTypes != null
            //    && Eq.EquipmentTypes.Count() > 0)
            //{
            //    Logger.LogDebug(Eq.EquipmentTypes.Count.ToString());

            //    //searchComponent.List = Eq.EquipmentTypes;

            //    //searchComponent.FilteredToDos = Eq.EquipmentTypes;
            //    ListET = Eq.EquipmentTypes;

            //    AppState.EquipmentTypes = ListET;

            //    StateHasChanged();

            //    //await searchComponent.CloseProgress();
            //}
            //}
            //else
            //{
            //    Logger.LogDebug("cliente nulloooo");
            //}
        }



#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task GetDetail(int ID)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            //searchComponent.Detail = searchComponent.List.Where(x => x.EquipmentTypeID == ID).FirstOrDefault();


            ////var messageForm = Modal.Show<Equipment_Create>("Passing Data");
            ////var result = await messageForm.Result;
            //await searchComponent.ShowModal();

            //StateHasChanged();

            ////await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));

        }

#pragma warning disable CS0108 // 'EquipmentTp_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<EquipmentType, IBasicsServices<CallContext>, AppState>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        public void Dispose()
#pragma warning restore CS0108 // 'EquipmentTp_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<EquipmentType, IBasicsServices<CallContext>, AppState>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        {

        }

        //public IEnumerable<EquipmentType> FilterList(string filter)
        //{
        //    try
        //    {

        //        List<EquipmentType> lequ = ListET.Where(i => i.Name.ToLower().Contains(filter.ToLower())).ToList();
        //        ListET = lequ;

        //        if (lequ == null)
        //        {
        //            return new List<EquipmentType>();
        //        }

        //        return lequ;

        //    }
        //    catch (Exception ex)
        //    {
        //        return new List<EquipmentType>();

        //    }
        //}

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task SelectModal(EquipmentType DTO)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            //await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));
        }

        public async Task<bool> Delete(EquipmentType DTO)
        {
            try
            {
                //await searchComponent.ShowModalAction();
                BasicsServiceGRPC basic = new BasicsServiceGRPC(Client);

                var result = await basic.DeleteEquipmentType(DTO);

                return true;
                // searchComponent.ShowResult();
            }
            catch (RpcException ex)
            {

                await ExceptionManager(ex);
                return false;

            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);
                return false;

            }
        }

        //public Component Component { get; set; } = new Component();
        protected override async Task OnInitializedAsync()
        {
            
            await base.OnInitializedAsync();
          
           
            //var BaseUrl = NavigationManager.BaseUri;

            //var CurrUrl = NavigationManager.Uri;

            //CurrUrl = CurrUrl.Replace(BaseUrl, "");

            //Roles = "testrol";

            //Policy = "HasAccess";

            //Component.Loacation = CurrUrl;

            //Component.Name = CurrUrl;

            



        }



    }
}
