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
    public class WorkOrderDetailXEquipment_SearchBase : Base_Create<WorkOrderDetailByStatus, Application.Services.IAssetsServices<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    //,IPage<WorkOrder, Application.Services.IAssetsServices<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
    {


        [Inject] public Application.Services.IBasicsServices<CallContext> _basicsServices { get; set; }
        [Inject] public Application.Services.IWorkOrderDetailServices<CallContext> _wodServices { get; set; }

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
            var Eq = await assets.GetWorkOrderDetailByEquipment(pag);
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

            FormName = "Search WorkOrder";

            TypeName = "WorkOrder";

            IsModal = IsModal;

            SelectOnly = SelectOnly;


            if (firstRender)
            {

            }


        }


#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnParametersSetAsync()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            //eq.StatusDate = DateTime.Now;
            //eq.WorkOrderReceiveDate = DateTime.Now;
            //NameValidationMessage = "valid";
            //BasicsServiceGRPC bsgrpc = new BasicsServiceGRPC(_basicsServices);
            //WorkOrderDetailGrpc wod = new WorkOrderDetailGrpc(_wodServices);
            //var _status = await wod.GetStatus();  //Tecnhicians_companyService.GetAll();
            //_statusList = _status.ToList();


            //var _eqType = await bsgrpc.GetEquipmenTypes();
            //_equipmentTypeList = _eqType.EquipmentTypes;

        }


#pragma warning disable CS0108 // 'WorkOrderDetailXEquipment_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<WorkOrderDetailByStatus, IAssetsServices<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        public void Dispose()
#pragma warning restore CS0108 // 'WorkOrderDetailXEquipment_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<WorkOrderDetailByStatus, IAssetsServices<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        {

        }

    }
}
