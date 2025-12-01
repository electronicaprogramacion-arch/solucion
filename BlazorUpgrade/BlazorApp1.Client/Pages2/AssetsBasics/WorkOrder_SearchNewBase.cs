using Blazor.IndexedDB.Framework;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BlazorApp1.Blazor.Pages.AssetsBasics
{
    public class WorkOrder_SearchNewBase : Base_Create<WorkOrder, Func<dynamic, CalibrationSaaS.Application.Services.IAssetsServices<CallContext>>, CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany>
    //,IPage<WorkOrder, CalibrationSaaS.Application.Services.IAssetsServices<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
    {
#pragma warning disable CS0108 // 'WorkOrder_SearchNewBase.Logger' oculta el miembro heredado 'KavokuComponentBase<WorkOrder>.Logger'. Use la palabra clave new si su intención era ocultarlo.
        [Inject] ILogger<WorkOrder_SearchNewBase> Logger { get; set; }
#pragma warning restore CS0108 // 'WorkOrder_SearchNewBase.Logger' oculta el miembro heredado 'KavokuComponentBase<WorkOrder>.Logger'. Use la palabra clave new si su intención era ocultarlo.

        [Inject]
        IConfiguration Configuration { get; set; }

        public bool showEbms { get; set; } = false;

        public List<CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder> List = new List<CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder>();


        public void ClearList(EventArgs arg)
        {

        }

        public ResponsiveTable<CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder> Grid { get; set; } = new ResponsiveTable<CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder>();


        public override async Task<ResultSet<WorkOrder>> LoadData(Pagination<WorkOrder> pag)
        {

            AssetsServiceGRPC assets = new AssetsServiceGRPC(Client, DbFactory);

            var Eq = (await assets.GetWorkOrders(pag, new CallContext()));

            //if (Eq != null)
            //{
            //    searchComponent.List = Eq.WorkOrders;

            //    searchComponent.FilteredToDos = Eq.WorkOrders;

            //    StateHasChanged();
            //}


            return Eq;
        }


#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnAfterRenderAsync(bool firstRender)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

           


            if (!firstRender)
            {
                return;
            }

            //searchComponent = new Search<EquipmentTemplate, IBasicsServices, AppState>();

            //BaseCreateUrl = "WorkOrderCreate";

            //BaseDetailUrl = "WorkOrderDetail";

            FormName = "Search WorkOrder";

            TypeName = "WorkOrder";

            IsModal = IsModal;

            SelectOnly = SelectOnly;

            await base.OnAfterRenderAsync(firstRender);

        }



        //public async Task GetDetail(int ID)
        //{
        //    searchComponent.Detail = searchComponent.List.Where(x => x.WorkOrderId == ID).FirstOrDefault();


        //    //var messageForm = Modal.Show<Equipment_Create>("Passing Data");
        //    //var result = await messageForm.Result;
        //    await searchComponent.ShowModal();

        //    StateHasChanged();

        //    //await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));

        //}

#pragma warning disable CS0108 // 'WorkOrder_SearchNewBase.Dispose()' oculta el miembro heredado 'Base_Create<WorkOrder, Func<IIndexedDbFactory, IAssetsServices<CallContext>>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        public void Dispose()
#pragma warning restore CS0108 // 'WorkOrder_SearchNewBase.Dispose()' oculta el miembro heredado 'Base_Create<WorkOrder, Func<IIndexedDbFactory, IAssetsServices<CallContext>>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
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


        public async Task<bool> Delete(WorkOrder DTO)
        {
            try
            {
                //await searchComponent.ShowModalAction();
                AssetsServiceGRPC assets = new AssetsServiceGRPC(Client, DbFactory);

                var result = await assets.DeleteWorkOrder(DTO, new CallContext());

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

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            string customer = Configuration.GetSection("Reports")["Customer"];

            if(customer == "Bitterman")
            {
                showEbms = true;
            }
            else
            {
                showEbms = false;
            }

        }

    }
}
