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
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.AssetsBasics
{
    public class WorkOrderDetailXCustomer_SearchBase : Base_Create<WorkOrderDetailByCustomer, Application.Services.IAssetsServices<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    //,IPage<WorkOrder, Application.Services.IAssetsServices<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
    {


        

        [Inject] public Application.Services.IBasicsServices<CallContext> _basicsServices { get; set; }
        [Inject] public Application.Services.IWorkOrderDetailServices<CallContext> _wodServices { get; set; }
        //public Search<WorkOrder, IAssetsServices<CallContext>, AppState> searchComponent
        //{ get; set; }
        //Search<WorkOrder, IAssetsServices<CallContext>, AppState> IPage<WorkOrder, IAssetsServices<CallContext>, AppState>.searchComponent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public List<Domain.Aggregates.Views.WorkOrderDetailByCustomer> List = new List<Domain.Aggregates.Views.WorkOrderDetailByCustomer>();

        public List<Status> _statusList = new List<Status>();
        public List<EquipmentType> _equipmentTypeList = new List<EquipmentType>();

        public void ClearList(EventArgs arg)
        {

        }

        public ResponsiveTable<Domain.Aggregates.Views.WorkOrderDetailByCustomer> Grid { get; set; } = new ResponsiveTable<WorkOrderDetailByCustomer>();


        public override async Task<ResultSet<WorkOrderDetailByCustomer>> LoadData(Pagination<WorkOrderDetailByCustomer> pag)
        {
            pag.Entity = eq;

            AssetsServiceGRPC assets = new AssetsServiceGRPC(Client);
            var Eq = await assets.GetWorkOrderDetailByCustomer(pag);
            return Eq;
        }


#pragma warning disable CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnAfterRenderAsync(bool firstRender)
#pragma warning restore CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            if (!firstRender)
            {
                return;
            }
            else
            {
                eq.StatusDate = null;
                eq.DueDate = null;
                eq.DateEnd = null;
                eq.WorkOrderReceiveDate = DateTime.Now;
                NameValidationMessage = "valid";
            }

            //searchComponent = new Search<EquipmentTemplate, IBasicsServices, AppState>();

            //BaseCreateUrl = "WorkOrderCreate";

            //BaseDetailUrl = "WorkOrderDetail";

            FormName = "Search WorkOrder";

            TypeName = "WorkOrder";

            IsModal = IsModal;

            SelectOnly = SelectOnly;

        }


#pragma warning disable CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnParametersSetAsync()
#pragma warning restore CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            //eq.StatusDate = DateTime.Now;
            //eq.DueDate = DateTime.Now;
            //eq.DateEnd = DateTime.Now;
            //eq.WorkOrderReceiveDate = DateTime.Now;
            //NameValidationMessage = "valid";

        }



#pragma warning disable CS0108 // 'WorkOrderDetailXCustomer_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<WorkOrderDetailByCustomer, IAssetsServices<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intenci�n era ocultarlo.
        public void Dispose()
#pragma warning restore CS0108 // 'WorkOrderDetailXCustomer_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<WorkOrderDetailByCustomer, IAssetsServices<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intenci�n era ocultarlo.
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
        public WorkOrderDetail wod1 = new WorkOrderDetail();

        public EditContext ec1;
        protected override async Task OnInitializedAsync()
        {
            ec1 = new EditContext(wod1);

            await base.OnInitializedAsync();
        }

    }
}
