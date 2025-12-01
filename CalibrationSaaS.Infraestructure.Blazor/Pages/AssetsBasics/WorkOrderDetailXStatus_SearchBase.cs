using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Aggregates.Views;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.AssetsBasics
{
    public class WorkOrderDetailXStatus_SearchBase : Base_Create<WorkOrderDetailByStatus, Application.Services.IAssetsServices<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    //,IPage<WorkOrder, Application.Services.IAssetsServices<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
    {

       

        [Inject] public Application.Services.IBasicsServices<CallContext> _basicsServices { get; set; }
        [Inject] public Application.Services.IWorkOrderDetailServices<CallContext> _wodServices { get; set; }
        //public Search<WorkOrder, IAssetsServices<CallContext>, AppState> searchComponent
        //{ get; set; }
        //Search<WorkOrder, IAssetsServices<CallContext>, AppState> IPage<WorkOrder, IAssetsServices<CallContext>, AppState>.searchComponent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public List<Domain.Aggregates.Views.WorkOrderDetailByStatus> List = new List<Domain.Aggregates.Views.WorkOrderDetailByStatus>();

        public List<Status> _statusList = new List<Status>();
        public List<EquipmentType> _equipmentTypeList = new List<EquipmentType>();

        public void ClearList(EventArgs arg)
        {

        }

        public ResponsiveTable<Domain.Aggregates.Views.WorkOrderDetailByStatus> Grid { get; set; } = new ResponsiveTable<WorkOrderDetailByStatus>();


        public override async Task<ResultSet<WorkOrderDetailByStatus>> LoadData(Pagination<WorkOrderDetailByStatus> pag)
        {
            pag.Entity = eq;

            AssetsServiceGRPC assets = new AssetsServiceGRPC(Client);
            var Eq = await assets.GetWorkOrderDetailByStatus(pag);
            return Eq;




        }

        public WorkOrderDetail wod1 = new WorkOrderDetail();

        public EditContext ec1;

        protected override async Task OnInitializedAsync()
        {
            ec1 = new EditContext(wod1);
            eq.StatusDate = null;
            eq.WorkOrderReceiveDate = null;

            NameValidationMessage = "valid";

            BasicsServiceGRPC bsgrpc = new BasicsServiceGRPC(_basicsServices);
            WorkOrderDetailGrpc wod = new WorkOrderDetailGrpc(_wodServices);
            var _status = await wod.GetStatus();  //Tecnhicians_companyService.GetAll();
            _statusList = _status.ToList();


            var _eqType = await bsgrpc.GetEquipmenTypes();
            _equipmentTypeList = _eqType.EquipmentTypes;

            await base.OnInitializedAsync();

        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnAfterRenderAsync(bool firstRender)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            if (!firstRender)
            {
                return;
            }
            else
            {

            }

            //searchComponent = new Search<EquipmentTemplate, IBasicsServices, AppState>();

            //BaseCreateUrl = "WorkOrderCreate";

            //BaseDetailUrl = "WorkOrderDetail";

            FormName = "Search WorkOrder";

            TypeName = "WorkOrder";

            IsModal = IsModal;

            SelectOnly = SelectOnly;

            //var Eq = (await searchComponent.Client.GetEquipment());

            //searchComponent.List = Eq.EquipmentTemplates;

            //if (searchComponent.Client != null)
            //{
            //    var Eq = (await searchComponent.Client.GetWorkOrders(Pnew CallContext()));

            //    if (Eq != null)
            //    {
            //        searchComponent.List = Eq.WorkOrders;

            //        searchComponent.FilteredToDos = Eq.WorkOrders;

            //        StateHasChanged();
            //    }
            //}
            //else
            //{
            //    Logger.LogDebug("null value");
            //}
        }


#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnParametersSetAsync()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            //eq.StatusDate = null;
            //eq.WorkOrderReceiveDate = null;

            //NameValidationMessage = "valid";

            //BasicsServiceGRPC bsgrpc = new BasicsServiceGRPC(_basicsServices);
            //WorkOrderDetailGrpc wod = new WorkOrderDetailGrpc(_wodServices);
            //var _status = await wod.GetStatus();  //Tecnhicians_companyService.GetAll();
            //_statusList = _status.ToList();


            //var _eqType = await bsgrpc.GetEquipmenTypes();
            //_equipmentTypeList = _eqType.EquipmentTypes;

        }



#pragma warning disable CS0108 // 'WorkOrderDetailXStatus_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<WorkOrderDetailByStatus, IAssetsServices<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        public void Dispose()
#pragma warning restore CS0108 // 'WorkOrderDetailXStatus_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<WorkOrderDetailByStatus, IAssetsServices<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        {

        }

        //public IEnumerable<WorkOrder> FilterList(string filter)
        //{
        //    try
        //    {

        //        List<WorkOrder> lequ = searchComponent.List.ToList();
        //        //    .Where(x => (x.Name != null && x.Name.ToLower().StartsWith(filter.ToLower()))
        //        //|| (x.Model != null && x.Model.ToLower().Contains(filter.ToLower())) || (x.Status != null && x.Status.ToLower().Contains(filter.ToLower()))
        //        //|| (x.Manufacturer != null && x.Manufacturer.ToLower().Contains(filter.ToLower()))
        //        //|| (x.Capacity != null && x.Capacity.ToString().ToLower().ToString().Contains(filter.ToLower()))).ToList();


        //        if (lequ == null)
        //        {
        //            return new List<WorkOrder>();
        //        }

        //        return lequ;

        //    }
        //    catch (Exception ex)
        //    {
        //        return new List<WorkOrder>();

        //    }
        //}

        //public async Task SelectModal(WorkOrder DTO)
        //{
        //    await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));
        //}

        //public async Task Delete(WorkOrder DTO)
        //{
        //    await searchComponent.ShowModalAction();

        //    searchComponent.Detail = await searchComponent.Client.DeleteWorkOrder(DTO, new CallContext());

        //    searchComponent.ShowResult();
        //}



    }
}
