using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using CalibrationSaaS.Infraestructure.Blazor.Shared;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using CalibrationSaaS.Domain.Aggregates.Entities;
using Microsoft.AspNetCore.Components.Forms;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Web;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using System.Runtime.CompilerServices;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using Blazored.Toast.Services;
using ProtoBuf.Grpc;
using Microsoft.Extensions.Logging;
using CalibrationSaaS.Blazor.Controls;
using CalibrationSaaS.Domain.Aggregates.Views;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using ProtoBuf.Grpc;
using System;
using Blazor.IndexedDB.Framework;
using Grpc.Core;
using ProtoBuf.Grpc;
using System;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Order
{
    public class OffLineData_SearchBase : Base_Create<WorkOrderDetail, Application.Services.IWorkOrderDetailServices<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    //,IPage<WorkOrder, Application.Services.IAssetsServices<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
    {
        [Inject] ILogger<WorkOrderDetailOff_SearchBase> Logger { get; set; }

        //[CascadingParameter] BlazoredModalInstance _BlazoredModal { get; set; }
        //[CascadingParameter] public IModalService Modal { get; set; }
        [Inject]
        public IIndexedDbFactory DbFactory { get; set; }

        [Inject]
        public Func<IIndexedDbFactory, Application.Services.ISampleService2<CallContext>> OffLineClient { get; set; }

        [Parameter]
        public bool SelectOnly { get; set; }

        [Parameter]
        public bool IsModal { get; set; }

        [Inject] public Application.Services.IBasicsServices<CallContext> _basicsServices { get; set; }
        [Inject] public Application.Services.IWorkOrderDetailServices<CallContext> _wodServices { get; set; }
 
        public List<WorkOrderDetail> List = new List<WorkOrderDetail>();

        public List<Domain.Aggregates.Entities.Status> _statusList = new List<Domain.Aggregates.Entities.Status>();
        public List<EquipmentType> _equipmentTypeList = new List<EquipmentType>();
         
        public void ClearList(EventArgs arg)
        {

        }

        public ResponsiveTable<WorkOrderDetail> Grid { get; set; }

        public ResponsiveTable<WorkOrderDetail> GridOff { get; set; }


        public List<WorkOrderDetail> _pieceOfEquipmentsFiltered { get; set; } = new List<WorkOrderDetail>();

        public async Task addToListPoE(int ID)
        {

            if (_pieceOfEquipmentsFiltered == null)
            {
                _pieceOfEquipmentsFiltered = new List<WorkOrderDetail>();
            }


            if (Grid.Items != null && _pieceOfEquipmentsFiltered != null)
            {

                var listPoe = Grid.Items.Where(x => x.WorkOrderDetailID == ID).FirstOrDefault();
                WorkOrderDetail _poe = new WorkOrderDetail();
                _poe = new WorkOrderDetail()
                {
                    WorkOrderDetailID = ID,
                    //SerialNumber = listPoe.SerialNumber
                };
                try
                {

                    //Console.WriteLine(_pieceOfEquipmentsFiltered.Count);
                    var xx = _pieceOfEquipmentsFiltered.Where(x => x.WorkOrderDetailID == ID).ToArray();
                    if (xx != null && xx?.Count() > 0)
                    {
                        var res = _pieceOfEquipmentsFiltered.Remove(xx[0]);
                    }
                    else
                    {
                        _pieceOfEquipmentsFiltered.Add(_poe);
                    }
                    //Console.WriteLine(_pieceOfEquipmentsFiltered.Count);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                }

            }
            //await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));

        }

        [Inject]
        public Application.Services.IBasicsServices<CallContext> BasicsServices { get; set; }


        [Inject]
        public Application.Services.IAssetsServices<CallContext> AssetsServices { get; set; }

        [Inject]
        public Application.Services.IUOMService<CallContext> UOMServices { get; set; }

        public override async Task<ResultSet<WorkOrderDetail>> LoadData(Domain.Aggregates.ValueObjects.Pagination<WorkOrderDetail> pag)
        {
            eq.TechnicianID = 11;

            pag.Entity = eq;

            WorkOrderDetailGrpc assets = new WorkOrderDetailGrpc(Client);


            var Eq = await assets.GetByTechnicianPag(pag);


            //GetConfiguredWeights

            var a = await assets.GetStatus();

            var result0 = await OffLineClient(DbFactory).LoadStatus(a.ToList(), new CallOptions());



            await LoadMeasurament();


            BasicsServiceGRPC basics = new BasicsServiceGRPC(BasicsServices);

            var types = await basics.GetEquipmenTypes();

            var result = await OffLineClient(DbFactory).LoadEquipmentType(types.EquipmentTypes, new CallOptions());


            var manu =await basics.GetAllManufacturers();

            var result2 = await OffLineClient(DbFactory).LoadManufacturer(manu.Manufacturers, new CallOptions());


            var users = await basics.GetUsers();

            var result3 = await OffLineClient(DbFactory).LoadUser(users.Users, new CallOptions());

            var cert = await basics.GetCertifications();

            var result4 = await OffLineClient(DbFactory).LoadCertification(cert.ToList(), new CallOptions());

            AssetsServiceGRPC Assets = new AssetsServiceGRPC(AssetsServices);

            var cal = await Assets.GetCalibrationType();

            var result5 = await OffLineClient(DbFactory).LoadCalibrationType(cal.CalibrationTypes, new CallOptions());

            UOMServiceGRPC uom = new UOMServiceGRPC(UOMServices);

            var uoms = await uom.GetAll();
;
            var result6 = await OffLineClient(DbFactory).LoadUOM(uoms.UnitOfMeasureList, new CallOptions());

            var uomstype = await uom.GetTypes();

            var result7 = await OffLineClient(DbFactory).LoadUOMType(uomstype, new CallOptions());


            PieceOfEquipmentGRPC poes = new PieceOfEquipmentGRPC(POEService);

            Pagination<PieceOfEquipment> p = new Pagination<PieceOfEquipment>();

            var ppp = await poes.GetAllWeightSetsPag(p);



            return Eq;

        }


        public  async Task<ResultSet<WorkOrderDetail>> LoadData2(Domain.Aggregates.ValueObjects.Pagination<WorkOrderDetail> pag)
        {
            eq.TechnicianID = 11;

            pag.Entity = eq;

            WorkOrderDetailGrpc assets = new WorkOrderDetailGrpc(Client);


            var result = await OffLineClient(DbFactory).GetWOD(pag, new CallOptions());

           

            return result;

        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }

            FormName = "Search WorkOrder";

            TypeName = "WorkOrder";

            IsModal = IsModal;

            SelectOnly = SelectOnly;
        
        }

        
        protected override async Task OnParametersSetAsync()
        {
           

        }


        public void Dispose()
        {

        }

        [Inject]
        public IPieceOfEquipmentService<CallContext> POEService { get; set; }

        public async Task NewItem()
        {

            
            var list = _pieceOfEquipmentsFiltered;

            if(list != null && list.Count > 0)
            {
                WorkOrderDetailGrpc assets = new WorkOrderDetailGrpc(Client);

                PieceOfEquipmentGRPC poes = new PieceOfEquipmentGRPC(POEService);

                List<WorkOrderDetail> lst = new List<WorkOrderDetail>();

                List<PieceOfEquipment> lstpoe = new List<PieceOfEquipment>();

                foreach (var item in list)
                {
                    var a = await assets.GetByID(item);

                    var resul = await assets.GetConfiguredWeights(item);

                    if(item.PieceOfEquipment.IndicatorPieceOfEquipmentID.HasValue && item.PieceOfEquipment.IndicatorPieceOfEquipmentID.Value > 0)
                    {
                        var poe = await poes.GetPieceOfEquipmentXId(item.PieceOfEquipment.Indicator);
                        lstpoe.Add(poe);
                    }
                    var poe2 = await poes.GetPieceOfEquipmentXId(item.PieceOfEquipment);
                    lstpoe.Add(poe2);

                    //TODO cargar indicadores y cargar perifericos

                    lst.Add(resul);
                }

                var result = await OffLineClient(DbFactory).InsertWOD(lst, new CallOptions());

                await GridOff.SearchFunction();
            }
        }

        //[Inject] public CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany AppState { get; set; }
        [Inject] public CalibrationSaaS.Domain.Aggregates.Shared.Basic.AppState AppStateBasics { get; set; }
        [Inject] public CalibrationSaaS.Application.Services.IUOMService<CallContext> UOM { get; set; }
        public async Task LoadMeasurament()
        {

            if (AppState.UnitofMeasureList == null || AppState.UnitofMeasureList.Count == 0)
            {
                AppState.UnitofMeasureTypeList = new List<UnitOfMeasureType>();

                AppState.UnitofMeasureList = new List<UnitOfMeasure>();

                var a = await UOM.GetTypes(new Grpc.Core.CallOptions());

                AppState.UnitofMeasureTypeList = a.ToList();

                var b = await UOM.GetAllEnabled(new Grpc.Core.CallOptions());

                if (b != null && b.UnitOfMeasureList != null && b.UnitOfMeasureList.Count > 0)
                {

                    AppState.UnitofMeasureList = b.UnitOfMeasureList.ToList();
                }
            }

            if (AppStateBasics.UnitofMeasureList == null || AppStateBasics.UnitofMeasureList.Count == 0)
            {
                AppStateBasics.UnitofMeasureTypeList = new List<UnitOfMeasureType>();

                AppStateBasics.UnitofMeasureList = new List<UnitOfMeasure>();

                var a = await UOM.GetTypes(new Grpc.Core.CallOptions());

                AppStateBasics.UnitofMeasureTypeList = a.ToList();

                var b = await UOM.GetAll(new Grpc.Core.CallOptions());
                if (b != null && b.UnitOfMeasureList != null && b.UnitOfMeasureList.Count > 0)
                {
                    AppStateBasics.UnitofMeasureList = b.UnitOfMeasureList.ToList();
                }

            }

        }


        public async Task<bool> Delete(WorkOrderDetail work)
        {
            WorkOrderDetailGrpc assets = new WorkOrderDetailGrpc(Client);

            var result = await OffLineClient(DbFactory).Delete(work);
                       

            return true;
        }

    }
}
