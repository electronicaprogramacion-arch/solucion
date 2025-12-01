using IndexedDB.Blazor;

using CalibrationSaaS.Application.Services;

using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Tools;
using System.Collections.Specialized;
using Reports.Domain.ReportViewModels;
using WorkOrder = CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder;
using static SQLite.SQLite3;
using CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite;
using SqliteWasmHelper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CalibrationSaaS.Domain.BusinessExceptions;
using System.Timers;
using Blazed.Controls;
using Helpers.Controls.ValueObjects;
using Helpers.Controls;
using Blazed.Controls.Toast;
using Helpers;

using Microsoft.JSInterop;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Offline
{
    public class WorkOrderDetailOff2_SearchBase : Base_Create<WorkOrderDetail, Application.Services.IWorkOrderDetailServices<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    //,IPage<WorkOrder, Application.Services.IAssetsServices<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
    {
        public bool visible = false;
        public JustifyContent justification = JustifyContent.Center;
        public Align alignment = Align.Center;

        public int Technician { get; set; }

        [Inject]
        public Application.Services.ICustomerService<CallContext> CustomerServices { get; set; }


        [Inject] public Application.Services.IWorkOrderDetailServices<CallContext> _wodServices { get; set; }

        [Inject]
        public Application.Services.IBasicsServices<CallContext> BasicsServices { get; set; }




        [Inject]
        public Application.Services.IAssetsServices<CallContext> AssetsServices { get; set; }

        [Inject]
        public Application.Services.IUOMService<CallContext> UOMServices { get; set; }

        [Inject]
        public IPieceOfEquipmentService<CallContext> POEService { get; set; }


        //[CascadingParameter] BlazoredModalInstance _BlazoredModal { get; set; }
        //[CascadingParameter] public IModalService Modal { get; set; }



        [Inject]
        public Func<dynamic, Application.Services.ISampleService2<CallContext>> OffLineClient { get; set; }

        [Inject]
        Func<dynamic, Application.Services.IBasicsServices<CallContext>> BasicsServices2 { get; set; }


         [Inject]
        Func<dynamic, Application.Services.IWorkOrderDetailServices<CallContext>> Client2 { get; set; }


        public List<WorkOrderDetail> List = new List<WorkOrderDetail>();

        public List<Domain.Aggregates.Entities.Status> _statusList = new List<Domain.Aggregates.Entities.Status>();
        public List<EquipmentType> _equipmentTypeList = new List<EquipmentType>();

        

        public ResponsiveTable<WorkOrderDetail> Grid { get; set; }

        public ResponsiveTable<WorkOrderDetail> GridOff { get; set; }


        public List<WorkOrderDetail> _pieceOfEquipmentsFiltered { get; set; } = new List<WorkOrderDetail>();

        public List<WorkOrderDetail> _pieceOfEquipmentsFilteredOff { get; set; } = new List<WorkOrderDetail>();


        public async Task addToListPoES(bool? arg,int ID,int WOID)

        {

            if (arg.HasValue && arg.Value == true && selectState == true || selectAllButton.HasValue && selectAllButton.Value == true)
            {
                return;
            }
        
           var listPoe = Grid.Items.Where(x => x.WorkOrderDetailID == ID && x.WorkOrderID== WOID).FirstOrDefault();


            if (listPoe != null)
            {
                var listwods = Grid.Items.Where(x => x.WorkOrderID == listPoe.WorkOrderID && x.WorkOrderID==WOID).ToList();

                var xx = _pieceOfEquipmentsFiltered.Where(x => x.WorkOrderDetailID == ID && x.WorkOrderID==WOID).FirstOrDefault();

                if(xx != null)
                {
                    await addToListPoE(ID, WOID, false);
                }
                else
                {
                    foreach (var item in listwods)
                    {
                        await addToListPoE(item.WorkOrderDetailID, item.WorkOrderID, false);
                    }
                }
               

                
            }

            await Task.Yield();

        }

        public async Task addToListPoES2(WorkOrderDetail wod)

        {

            int ID = wod.WorkOrderDetailID;
            int WOID = wod.WorkOrderID;


            var listPoe = Grid.Items.Where(x => x.WorkOrderDetailID == ID && x.WorkOrderID==WOID).FirstOrDefault();


            if (listPoe != null)
            {
                var listwods = Grid.Items.Where(x => x.WorkOrderID == listPoe.WorkOrderID).ToList();

                var xx = _pieceOfEquipmentsFiltered.Where(x => x.WorkOrderDetailID == ID && x.WorkOrderID==WOID).FirstOrDefault();

                if (xx != null)
                {
                    await addToListPoE(ID, WOID, true);
                }
                else
                {
                    foreach (var item in listwods)
                    {
                        var xx2 = _pieceOfEquipmentsFiltered.Where(x => x.WorkOrderDetailID == item.WorkOrderDetailID && x.WorkOrderID==item.WorkOrderID).FirstOrDefault();
                        if(xx2 == null)
                        {
                            await addToListPoE(item.WorkOrderDetailID,item.WorkOrderID, true);
                        }
                       
                    }
                }



            }

            await Task.Yield();

        }


        public async Task addToListPoE(int ID,int WOID, bool changeState = true)

        {

            if (_pieceOfEquipmentsFiltered == null)
            {
                _pieceOfEquipmentsFiltered = new List<WorkOrderDetail>();
            }


            if (Grid.Items != null && _pieceOfEquipmentsFiltered != null)
            {

                var listPoe = Grid.Items.Where(x => x.WorkOrderDetailID == ID && x.WorkOrderID==WOID).FirstOrDefault();
                
               
                
                WorkOrderDetail _poe = new WorkOrderDetail();
                _poe = new WorkOrderDetail()
                {
                    WorkOrderDetailID = ID,
                    WorkOrderID=WOID
                    //SerialNumber = listPoe.SerialNumber
                };
                try
                {

                    //Console.WriteLine(_pieceOfEquipmentsFiltered.Count);
                    var xx = _pieceOfEquipmentsFiltered.Where(x => x.WorkOrderDetailID == ID && x.WorkOrderID==WOID).FirstOrDefault();
                    if (xx != null )
                    {
                        var res = _pieceOfEquipmentsFiltered.Remove(xx);
                    }
                    else
                    {
                        _pieceOfEquipmentsFiltered.Add(_poe);
                    }
                    //Console.WriteLine(_pieceOfEquipmentsFiltered.Count);
                    if (changeState)
                    {
                        StateHasChanged();
                    }
                   
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                }

            }
            //await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));

        }

#pragma warning disable CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task addToListPoEOff(int ID,int WOID)
#pragma warning restore CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            if (_pieceOfEquipmentsFilteredOff == null)
            {
                _pieceOfEquipmentsFilteredOff = new List<WorkOrderDetail>();
            }


            if (GridOff.Items != null && _pieceOfEquipmentsFilteredOff != null)
            {

                var listPoe = GridOff.Items.Where(x => x.WorkOrderDetailID == ID && x.WorkOrderID==WOID).FirstOrDefault();
                WorkOrderDetail _poe = new WorkOrderDetail();
                _poe = new WorkOrderDetail()
                {
                    WorkOrderDetailID = ID,
                    //SerialNumber = listPoe.SerialNumber
                };
                try
                {

                    //Console.WriteLine(_pieceOfEquipmentsFilteredOff.Count);
                    var xx = _pieceOfEquipmentsFilteredOff.Where(x => x.WorkOrderDetailID == ID && x.WorkOrderID == WOID).FirstOrDefault();
                    if (xx != null)
                    {
                        var res = _pieceOfEquipmentsFilteredOff.Remove(xx);
                    }
                    else
                    {
                        _pieceOfEquipmentsFilteredOff.Add(_poe);
                    }
                    //Console.WriteLine(_pieceOfEquipmentsFilteredOff.Count);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("error: " + ex.Message);
                }

            }
            //await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));

        }


        //protected override void OnInitialized()
        //{
        //    //paginaSize = 5;
        //    base.OnInitialized();
        //}

        CalibrationSaaS.Domain.Aggregates.Entities.User UserDomain = new CalibrationSaaS.Domain.Aggregates.Entities.User();

        public async Task GetTechID()
        {
            try
            {
                var u = await CurrentUserName();
                  if (Technician == 0 && !string.IsNullOrEmpty(u) && !AppSecurity.IsNotGrpc) 
            
                {

                BasicsServiceGRPC basics = new BasicsServiceGRPC(BasicsServices);

                //var u = await CurrentUserName();

                //Console.WriteLine(u);

                var users = await basics.GetUsers();


                    var uss = users.Users.Where(x => x.UserName == u).FirstOrDefault();
                    //if (uss == null)
                    //{
                    //    uss = users.Users.Where(x => x.Name == u).FirstOrDefault();
                    //}

                    if (uss != null)
                    {
                    Technician = uss.UserID;
                    UserDomain = uss;
                    }
                    else
                    {
                        await ShowToast("Actual User is no tech, doesn't use offline mode ", ToastLevel.Info);
                        return;
                    }

                }

                else
                {

                    await GetTechOffline();

                }

                AppSecurity.CurrentUserID = Technician;
                //Console.WriteLine(Technician);
            }
            catch(RpcException ex)
            {

            }
            catch (Exception ex)
            {
                if (ex.Message == "Communication error")
                {
                    AppSecurity.IsNotGrpc = true;

                    await GetTechOffline();

                    AppSecurity.CurrentUserID = Technician;

                }

                //await ExceptionManager(ex);

            }


        }

        public async  Task GetTechOffline()
        {
            CanShow = false;
            CurrentUser uss1 = await OffLineClient(DbFactory).GetUser(new CallOptions());
            BasicsServiceGRPC basics = new BasicsServiceGRPC(BasicsServices2, DbFactory);
            var users = await basics.GetUsers();

            if (uss1.Claims.Count > 5)
            {
                var user = uss1.Claims.Where(x=>x.Key=="name").FirstOrDefault();

                if(user != null)
                {
                    var uss = users.Users.Where(x => x.UserName == user.Value).FirstOrDefault();
                    if (uss != null)
                    {
                        Technician = uss.UserID;
                        UserDomain = uss;
                    }
                }

                
            }
            
        }



        public  WorkOrderDetail AlreadyInOff(WorkOrderDetail wod)
        {

            var b = WODOffList.Where(x => x.WorkOrderDetailID == wod.WorkOrderDetailID).FirstOrDefault();
            
            //GridOff
            var AlreadyInOff = false;

            if (b!= null)
        {
            Grid.RowClass = "row grid-row expired";
            
        }
            
            return wod;

        //GridOff
        }

        List<WorkOrderDetail> WODOnlineList = new List<WorkOrderDetail>();
        public override async Task<ResultSet<WorkOrderDetail>> LoadData(Pagination<WorkOrderDetail> pag)
        {

            ResultSet<WorkOrderDetail> Eq = new ResultSet<WorkOrderDetail>();

            if (WODOnlineList != null  && WODOnlineList.Count > 0 && !string.IsNullOrEmpty(pag.Filter))
            {

                Eq.List = WODOnlineList.Where(x=>x.WorkOrderDetailID.ToString().Contains(pag.Filter) 
                || x.WorkOrderID.ToString().Contains(pag.Filter)
                || x.PieceOfEquipment.Customer.Name.Contains(pag.Filter)
                || x.PieceOfEquipment.Capacity.ToString().Contains(pag.Filter)
                
                ).ToList();

                _pieceOfEquipmentsFiltered.Clear();

                return Eq;

            }

            //Grid.Clear();
            //Grid.Clear2();
            //AppSecurity.User = System.Security.Claims.ClaimsPrincipal.Current;
            WODOnlineList = new List<WorkOrderDetail>();

           

            try
            {
                


                //Console.WriteLine("LoadData off");

                await GetTechID();

                _pieceOfEquipmentsFiltered = new List<WorkOrderDetail>();

                eq.TechnicianID = Technician;

                pag.Entity = eq;

                pag.Show = 100000;

                pag.SaveCache = false;



                var eqt3 = await GetOrdersByStatus(pag, 0);

                var list = new List<WorkOrderDetail>();

                if (eqt3?.List?.Count > 0)
                {
                    list.AddRange(eqt3.List);
                }




                var eqt = await GetOrdersByStatus(pag,2);

                //var list= new List<WorkOrderDetail>();

                if(eqt?.List?.Count > 0)
                {
                    list.AddRange(eqt.List);
                }

               

                var eqt2 = await GetOrdersByStatus(pag, 1);

                if (eqt2?.List?.Count > 0 && list?.Count != null)
                {
                    foreach (var itex in eqt2.List)
                    {
                        list.Add(itex);
                    }

                    
                    Grid.TotalRows = list.Count;
                }
                else
                {

                }
                if(list?.Count > 0)
                {
                    Eq.List = list.OrderBy(x=>x.WorkOrderID).ToList();
                }
               
                
               

                //Console.WriteLine("LoadData off2");
                
            }

            catch (RpcException ex)
            {   // AppSecurity.IsNotGrpc = true;
                

                await ExceptionManager(ex);

            }
            catch (Exception ex)
            {
                if(ex.Message=="Communication error")
                {
                    AppSecurity.IsNotGrpc = true;
                }

                await ExceptionManager(ex);

            }

            if(Eq.List == null || Eq?.List?.Count==0)
            {
                Eq.List = new List<WorkOrderDetail>();
            }
            else
            {
                var ResOff = await GetOffileWOD(pag);
                

                if(ResOff != null && ResOff.List != null && ResOff?.List?.Count > 0)
                {

                    var listtotal = new List<WorkOrderDetail>();

                    foreach (var item in Eq.List)
                    {
                        if(ResOff.List.Where(x=>x.WorkOrderDetailID==item.WorkOrderDetailID && x.WorkOrderID==item.WorkOrderID).FirstOrDefault() == null)
                        {
                            listtotal.Add(item);
                        }
                    }

                    Eq.List = listtotal;
                }

            }

            _pieceOfEquipmentsFiltered.Clear();

            WODOnlineList = Eq.List;
            return Eq;

        }


        public async Task<ResultSet<WorkOrderDetail>> GetOrdersByStatus(Pagination<WorkOrderDetail> pag,int currentStatus)
        {
            WorkOrderDetailGrpc assets = new WorkOrderDetailGrpc(Client);

            ResultSet<WorkOrderDetail> Eq = new ResultSet<WorkOrderDetail>();

            pag.Entity.CurrentStatusID = currentStatus; // 2;

            Eq = await assets.GetByTechnicianPag(pag);

            if (Eq?.List?.Count > 0)
            {
                Eq.List = Eq.List.Distinct().ToList();

                Pagination<WorkOrderDetail> pag1 = new Pagination<WorkOrderDetail>();

                pag1.Page = 1;
                pag1.Show = 1000;

                ResultSet<WorkOrderDetail> resultoff = await OffLineClient(DbFactory).GetWOD(pag1, new CallOptions());

                if (resultoff?.List?.Count > 0)
                {
                    var List2 = Eq.List.Where(item => !resultoff.List.Any(item2 => item2.WorkOrderDetailID == item.WorkOrderDetailID && item2.WorkOrderID==item.WorkOrderID));

                    Eq.List = List2.Distinct().ToList();
                }

                if(Eq?.List?.Count> 0)
                {
                    foreach (var item in Eq.List)
                    {
                        if (item != null)
                        {
                            _pieceOfEquipmentsFiltered.Add(item);
                        }

                    }
                }
                


            }

            return Eq;
        }


        public static bool Launch { get; set; }

        List<EquipmentTemplate> lstET = new List<EquipmentTemplate>();
        public async Task LoadParametrics(int Technician)
        {

            //return;
            //try
            //{
            var uss = await GetUser();
            //Console.WriteLine("loadparametrics");
            //Console.WriteLine(uss.Identity.Name);

            var ussss = new CurrentUser();
            List<CustomClaim> lc = new List<CustomClaim>();

            //var user = new CalibrationSaaS.Domain.Aggregates.Entities.User();

            //user.UserName = uss.Identity.Name;
            ussss.Type = uss.Identity.AuthenticationType;

            foreach (var clai in uss.Claims)
            {

                lc.Add(new CustomClaim() { Key = clai.Type, Value = clai.Value, User = ussss });
            }



            //var principal = new ClaimsPrincipal(new ClaimsIdentity(lc,uss.Identity.AuthenticationType ));


            ussss.Claims = lc;
            await OffLineClient(DbFactory).LoadUser(ussss, new CallOptions());

            var uss1 = await OffLineClient(DbFactory).GetUser(new CallOptions());


             await OffLineClient(DbFactory).LoadComponents(AppSecurity.Components);

            //Console.WriteLine("LoadUser");
            //Console.WriteLine("LoadParametrics");

            WorkOrderDetailGrpc assets = new WorkOrderDetailGrpc(Client);
            UOMServiceGRPC uom = new UOMServiceGRPC(UOMServices);
            BasicsServiceGRPC basics = new BasicsServiceGRPC(BasicsServices);

             //var con = await CallOptions("WorkOrderItem");

            var con = await CallOptions("");


            PieceOfEquipmentGRPC poes = new PieceOfEquipmentGRPC(POEService,con);

            var a = await assets.GetStatus();

            //var result0 = await OffLineClient(DbFactory).LoadStatus(a.ToList(), new CallOptions());



            var types = await basics.GetEquipmenTypes();


            List<EquipmentTypeGroup> etgro = new List<EquipmentTypeGroup>();


            foreach (var irrr in types.EquipmentTypes)
            {
                if(irrr.EquipmentTypeGroup != null && etgro.Where(x=>x.EquipmentTypeGroupID==irrr.EquipmentTypeGroupID).FirstOrDefault()==null)
                {
                    etgro.Add((EquipmentTypeGroup)irrr.EquipmentTypeGroup.CloneObject());
                }            

            }

            //var result = await OffLineClient(DbFactory).LoadEquipmentType(types.EquipmentTypes, new CallOptions());

            Pagination<Procedure> procedurepag = new Pagination<Procedure>();

            

            var procedures = (await basics.GetProcedures(procedurepag));


            // Use pagination to get TestCodes efficiently instead of fetching all
            Pagination<TestCode> testCodePag = new Pagination<TestCode>();
            testCodePag.Show = 1000; // Reasonable page size
            testCodePag.Page = 1;
            var testcodes = await assets.GetTestCodes(testCodePag, new CallContext());


            List<TestCode> notes = new List<TestCode>();

            foreach (var testcodei in testcodes.List)
            {
                
                var resy= await assets.GetTestCodeByID(testcodei);

                //if(resy.Notes != null && resy.Notes.Count > 0)
                //{
                    notes.Add(resy);
                //}
               


            }


            var manu = await basics.GetAllManufacturers();

            //var result2 = await OffLineClient(DbFactory).LoadManufacturer(manu.Manufacturers, new CallOptions());
            Pagination<EquipmentTemplate> PagEt = new Pagination<EquipmentTemplate>();

            PagEt.Show = 1000;

            var ets = await basics.GetEquipment(PagEt);

            lstET = ets.List;

            var uomstype = await uom.GetTypes();

            var uoms = await uom.GetAll();

            AssetsServiceGRPC Assets = new AssetsServiceGRPC(AssetsServices);

            var cal = await Assets.GetCalibrationType();

            var users = await basics.GetUsers();

            var cert = await basics.GetCertifications();

            Pagination<WorkOrder> p0 = new Pagination<WorkOrder>();

            WorkOrder w = new WorkOrder();

            w.Users = new List<User>();

            var use = new User() { UserID = Technician };

            w.Users.Add(use);

            p0.Show = 1000;
            p0.Page = 1;
            p0.Entity = w;

            var wo = new ResultSet<WorkOrderDetail>();//await Assets.GetWorkOrdersOff(p0);


            List<WorkOrderDetail> wodtemp = new List<WorkOrderDetail>();


            var listfilter = Grid.ItemList.ToList();

            var idsFiltrados = listfilter.Select(x => x.WorkOrderDetailID).ToHashSet();

            var list = _pieceOfEquipmentsFiltered
                .Where(x => idsFiltrados.Contains(x.WorkOrderDetailID))
                .ToList();




            foreach (var iyy in WODOnlineList)
            {
                if (list.Where(x => x.WorkOrderDetailID == iyy.WorkOrderDetailID && iyy.WorkOrderID==x.WorkOrderID).FirstOrDefault() !=null)
                {
                    wodtemp.Add(iyy);
                }
            }

            wo.List = wodtemp;

            //Console.WriteLine("Data loading completed, from server");

            //var result7 = await OffLineClient(DbFactory).LoadUOMType(uomstype, new CallOptions());

            AppState.UnitofMeasureTypeList = uomstype.ToList();

            AppState.UnitofMeasureList = uoms.UnitOfMeasureList;

            AppStateBasics.UnitofMeasureTypeList = uomstype.ToList();

            AppStateBasics.UnitofMeasureList = uoms.UnitOfMeasureList;
            //var result6 = await OffLineClient(DbFactory).LoadUOM(uoms.UnitOfMeasureList, new CallOptions());
            AppSecurity.StatusList = a.ToList();

           

            //var result5 = await OffLineClient(DbFactory).LoadCalibrationType(cal.CalibrationTypes, new CallOptions());


            await OffLineClient(DbFactory).LoadMany(AppStateBasics.ToleranceList2,etgro, testcodes.List, procedures.List,cal.CalibrationTypes, a.ToList(), types.EquipmentTypes, manu.Manufacturers, uoms.UnitOfMeasureList, uomstype);

            //Console.WriteLine("end LoadMany");

            var result3 = await OffLineClient(DbFactory).LoadUser(users.Users, new CallOptions());


            //Console.WriteLine("end LoadUser");


            if (cert != null)
            {
                var result4 = await OffLineClient(DbFactory).LoadCertification(cert.ToList(), new CallOptions());
            }



            List<CalibrationType> lstCat = new List<CalibrationType>();

            if (wo != null && wo?.List?.Count > 0)
            {
                

                foreach (var eto in wo.List)
                {
                    if (eto?.PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject != null)
                    {
                        var eto2 = eto.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject;

                        if (eto2 != null && eto2.DynamicConfiguration2 == true && lstCat.Where(x => x.CalibrationTypeId == eto2.CalibrationTypeID).FirstOrDefault() == null)
                        {
                            //Console.WriteLine("-----------Dynamic calibration-------------   " + eto2.CalibrationType.Name + "----------------");

                            var confi = await poes.GetDynamicConfiguration(eto2.CalibrationType);

                            //Console.WriteLine("get Calibration Type " + confi.Name);

                            var result15 = await OffLineClient(DbFactory).CreateNormalConfiguration(confi, new CallOptions());

                            //Console.WriteLine("saved Calibration Type" + confi.Name);

                            lstCat.Add(confi);
                            ////// code to 
                            await OffLineClient(DbFactory).ini2();
                        }
                    }
                }

                if (lstCat.Count > 0)
                {

                }


            }


            if (tenant == "Bitterman" && lstCat.Count ==0)
            {

                CalibrationType eto2 = new CalibrationType();

                eto2.CalibrationTypeId = 1000;

                var confi = await poes.GetDynamicConfiguration(eto2);

                //Console.WriteLine("get Calibration Type " + confi.Name);

                var result15 = await OffLineClient(DbFactory).CreateNormalConfiguration(confi, new CallOptions());

                //Console.WriteLine("saved Calibration Type" + confi.Name);

                await OffLineClient(DbFactory).ini2();

            }



            //Console.WriteLine("end LoadCertification");

            Pagination<PieceOfEquipment> p = new Pagination<PieceOfEquipment>();

            p.Show = 10000;
            p.Page = 1;

            p.Entity = new PieceOfEquipment();

            List<User> userList = new List<User>();
            User u = new User();
            u.UserID = Technician;
            userList.Add(u);

            p.Entity.Users = userList;






            

            //var addresslst= await customer.GetAddressesAsync();


            if (wo.List != null)
            {
                //var result8 = await OffLineClient(DbFactory).LoadWorkOrder(wo.List, new CallOptions());

                List<PieceOfEquipment> piecs = new List<PieceOfEquipment>();
                List<CalibrationSaaS.Domain.Aggregates.Entities.Customer> custlst = new List<CalibrationSaaS.Domain.Aggregates.Entities.Customer>();

                CustomerGRPC customer = new CustomerGRPC(CustomerServices);

                List<PieceOfEquipment> poeprinc = new List<PieceOfEquipment>();

                bool downloadDynamic = false;

                if(tenant == "Bitterman")
                {
                    downloadDynamic = true;
                }



                foreach (var item22 in wo.List)
                {
                    //Console.WriteLine("load  WorkOrder " + item22.WorkOrderID);


                    CalibrationSaaS.Domain.Aggregates.Entities.Customer _custId = new CalibrationSaaS.Domain.Aggregates.Entities.Customer();

                    if(item22?.PieceOfEquipment?.CustomerId > 0)
                    {
                        _custId.CustomerID = item22.PieceOfEquipment.CustomerId;
                    }
                    else if (item22?.WorkOder?.CustomerId > 0)
                    {
                        _custId.CustomerID = item22.WorkOder.CustomerId;
                    }
                    

                    var tempcus = custlst.Where(x => x.CustomerID == _custId.CustomerID).FirstOrDefault();

                    if (tempcus == null)
                    {
                        var cust = await customer.GetCustomersByID(_custId);

                        custlst.Add(cust);

                       


                        PieceOfEquipment poe = new PieceOfEquipment();
                        poe.IsWeigthSet = true;
                        poe.IsForAccreditedCal = true;
                        poe.CustomerId = _custId.CustomerID;
                        poe.Customer = cust;
                        Pagination<PieceOfEquipment> p3 = new Pagination<PieceOfEquipment>();

                        p3.Show = 3;
                        p3.Page = 1;
                        p3.Entity = poe;
                        p3.LoadDynamic = downloadDynamic;

                        bool endquery1 = true;
                        int min1 = 0;
                        int max1 = 3;
                        int pag1 = 1;

                        int total1 = 0;

                        List<PieceOfEquipment> lsttemp = new List<PieceOfEquipment>();                      


                        while (endquery1)

                        {
                            //Console.WriteLine("Load Poe By Customer pag: " + p3.Page);
                            
                            var Eq1 = (await poes.GetPieceOfEquipmentByCustomer(p3));
                            //Console.WriteLine("End Load Poe By Customer pag: " + p3.Page);
                            if (Eq1?.List == null || Eq1?.List?.Count ==0)
                            {
                                endquery1 = false;
                            }
                            else
                            {
                                lsttemp.AddRange(Eq1.List);

                                p3.Page = p3.Page + 1;
                            }
                           
                        }

                        /////////////////////////////////////////////////////////
                        if (!string.IsNullOrEmpty(item22?.PieceOfEquipment?.PieceOfEquipmentID))
                        {
                            var poepri = (await poes.GetPieceOfEquipmentXId(item22.PieceOfEquipment.PieceOfEquipmentID));

                            poeprinc.Add(poepri);

                            lsttemp.Add(poepri);
                        }

                         
                       
                      


                        ResultSet<PieceOfEquipment> Eq = new ResultSet<PieceOfEquipment>();

                        Eq.List = lsttemp;
                        //var result12 = await OffLineClient(DbFactory).LoadPOE(Eq.List, new CallOptions());

                        if (Eq.List != null)
                        {
                            piecs.AddRange(Eq.List);

                            foreach (var et in Eq.List)
                            {
                                var ret = lstET.Where(x => x.EquipmentTemplateID == et.EquipmentTemplateId).FirstOrDefault();

                                if (ret != null)
                                {
                                    lstET.Remove(ret);
                                }
                                lstET.Add(et.EquipmentTemplate);
                            }

                        }

                        



                        //Console.WriteLine("end  WorkOrder " + item22.WorkOrderID);
                    }


                }

                await OffLineClient(DbFactory).ini2();


                //BITTERMAN CUSTOMER
                CalibrationSaaS.Domain.Aggregates.Entities.Customer _custId2 = new CalibrationSaaS.Domain.Aggregates.Entities.Customer();

                _custId2.CustomerID = 25;
                //bitterman 
                var cust2 = await customer.GetCustomersByID(_custId2);

                custlst.Add(cust2);

                var result10 = await OffLineClient(DbFactory).LoadCustomer(custlst, true, new CallOptions());


                //Console.WriteLine("end  LoadCustomer ");

                if (lstET.Count > 0)
                {

                    List<EquipmentTemplate> etdeflst = new List<EquipmentTemplate>();
                    foreach (var itemET in lstET)
                    {
                        if (itemET?.EquipmentTypeObject != null && itemET?.EquipmentTypeObject?.DynamicConfiguration2 == true
                            && itemET?.EquipmentTypeObject?.CalibrationType != null 
                            && itemET?.EquipmentTypeObject?.CalibrationType.CalibrationSubTypes.Count > 0 )
                        {
                            etdeflst.Add(itemET);

                           
                        }
                        
                        else if(itemET?.EquipmentTypeObject != null && itemET?.EquipmentTypeObject?.DynamicConfiguration2 == true)
                        {
                            var itemET2 = await basics.GetEquipmentByID(itemET);
                            etdeflst.Add(itemET2);
                        }
                        else
                        {
                            etdeflst.Add(itemET);
                        }

                       
                    }
                        


                    var result4 = await OffLineClient(DbFactory).LoadEquipmentTemplate(etdeflst, new CallOptions());
                }




                //await OffLineClient(DbFactory).LoadAddress(addresslst,false, new CallOptions());




                //Console.WriteLine("end  LoadEquipmentTemplate ");

                await OffLineClient(DbFactory).LoadRol(AppSecurity.RolesList, new CallOptions());

                //Console.WriteLine("end  LoadRol ");

                //await OffLineClient(DbFactory).LoadPOE(poeprinc, new CallOptions());

                piecs.AddRange(poeprinc);

                try
                {

                    //piecs = piecs.DistinctBy(x=>x.PieceOfEquipmentID).ToList();

                    await OffLineClient(DbFactory).LoadPOE(piecs, new CallOptions());

                    
                }
                catch(Exception ex)
                {
                    await ExceptionManager(ex,"Not all POES can download, please review",ToastLevel.Warning);
                }
                

                //Console.WriteLine("end  LoadPOE ");

            }

            var ety = new EquipmentType();
            ///TODO should be configurated
            ety.CalibrationTypeID = 1;
            p.Entity.EquipmentTemplate = new EquipmentTemplate();
            p.Entity.EquipmentTemplate.EquipmentTypeObject = ety;

            p.Show = 3;
            bool endquery = true;
            int min = 0;
            int max = 3;
            int pag = 1;

            int total=0;
            while(endquery)             
            {

                //p.Min = min;
                //p.Max = max;
                p.Page = pag;

                var ppp = await poes.GetAllWeightSetsPag(p);


                if (ppp.List == null || ppp?.List?.Count==0)  //if (max >= ppp?.Count )
                {
                    endquery = false;
                } 
                //Console.WriteLine("end  GetAllWeightSetsPag ");
                min = min + 3;
                max = max + 3;
                pag++;

                


                if (ppp?.List != null)
                {
                    foreach (var itpu in ppp.List)
                    {
                        //Console.WriteLine("weight : " + itpu.PieceOfEquipmentID);
                    }

                    total = total + ppp.List.Count;
                    var result15 = await OffLineClient(DbFactory).LoadPOE(ppp.List, new CallOptions());

                    await OffLineClient(DbFactory).ini2();
                }
            }


            //p.Show = 1000;
            //p.Page = 1;
            //var ppppp= await OffLineClient(DbFactory).GetAllWeightSetsPag(p, new CallOptions());

            //Console.WriteLine("--------------weightset--------------------------");
            //Console.WriteLine(total);
            //Console.WriteLine(ppppp?.List?.Count);





            //Console.WriteLine("end  LoadPOE2 ");

           
         


            //
            await OffLineClient(DbFactory).ini2();

            //await ShowToast("Parametric data loaded successfully. Please wait", ToastLevel.Success);

            //}
            //catch (Exception ex)
            //{
            //    await ShowError(ex.Message);
            //}
            //finally
            //{

            //    //await CloseProgress();
            //    //Launch = false;
            //    Launch = true;

            //}
            /////////////////////////////////////////////load dynamic configuration
           

            //////////////////////////////////////////////////////////////////////////

        }


        public async Task<ResultSet<WorkOrderDetail>> GetOffileWOD(Pagination<WorkOrderDetail> pag)
        {


            //Console.WriteLine("LoadData2 off");

            await GetTechID();


            eq.TechnicianID = Technician;

            eq.CurrentStatusID = 1;

            pag.Entity = eq;

            pag.ShowProgress = false;

            pag.Show = 10000;

            pag.SaveCache = false;

            //WorkOrderDetailGrpc assets = new WorkOrderDetailGrpc(Client);

            //pag.ShowProgress = false;

            var result = await OffLineClient(DbFactory).GetWOD(pag, new CallOptions());


            var listnew = new List<WorkOrderDetail>();

            if (result?.List != null)
            {
                listnew.AddRange(result.List);

            }

            pag.Entity.CurrentStatusID = 2;

            result = await OffLineClient(DbFactory).GetWOD(pag, new CallOptions());


            if (result?.List != null)
            {
                result?.List.AddRange(listnew);

                GridOff.TotalRows = result.List.Count;

                foreach (var item in result.List)
                {
                    if (item.OfflineStatus == 1)
                    {
                        _pieceOfEquipmentsFilteredOff.Add(item);
                    }

                }


                result.List = result.List.Where(x=>x.WorkOrderDetailID !=0).ToList();


            }

            //Console.WriteLine("LoadData2 off");
            return result;


        }



        public async Task<ResultSet<WorkOrderDetail>> LoadData2(Pagination<WorkOrderDetail> pag)
        {

            try
            {


                return await GetOffileWOD(pag);


            }

            catch (RpcException ex)
            {   // AppSecurity.IsNotGrpc = true;


                await ExceptionManager(ex);
                throw ex;

            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);
                throw ex;
            }
           



        }


        public async Task<ResultSet<WorkOrderDetail>> LoadData3(Pagination<WorkOrderDetail> pag)
        {

            await GetTechID();


            eq.TechnicianID = Technician;

            pag.Entity = eq;

            WorkOrderDetailGrpc assets = new WorkOrderDetailGrpc(Client);


            var result = await OffLineClient(DbFactory).GetWODUploaded(pag, new CallOptions());

            //if (result.List != null)
            //{
            //    foreach (var item in result.List)
            //    {
            //        if (item.OfflineStatus == 1)
            //        {
            //            _pieceOfEquipmentsFilteredOff.Add(item);
            //        }

            //    }
            //}


            return result;

        }


#pragma warning disable CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnAfterRenderAsync(bool firstRender)
#pragma warning restore CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            if (!firstRender)
            {
                //await ScrollPosition();
                return;
            }
            else
            {
                //if (!ConnectionStatusService.GetCurrentStatus() && ConnectionStatusService.GetChangeStatus())
                //{
                //    await LoadParametrics(Technician);
                //}

                //await LoadParametrics(Technician);

            }

            FormName = "Search WorkOrder";

            TypeName = "WorkOrder";

            IsModal = IsModal;

            SelectOnly = SelectOnly;

            if(bl != null)
            {
                await bl.RefreshAsync();

                
            }
            

        }




#pragma warning disable CS0108 // 'WorkOrderDetailOff_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<WorkOrderDetail, IWorkOrderDetailServices<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intenci�n era ocultarlo.
        public void Dispose()
#pragma warning restore CS0108 // 'WorkOrderDetailOff_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<WorkOrderDetail, IWorkOrderDetailServices<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intenci�n era ocultarlo.
        {

        }

        [Inject] 
        public NavigationManager Navigation { get; set; }

        
        
        public string btnSelectAllText= "Deselect All";

        public bool? selectState { get; set; } = false;

        public bool? selectAllButton { get; set; }
        public async Task SelectAll()
        {
            selectAllButton = true;
            if (selectState.HasValue && selectState.Value==true)
            {
                btnSelectAllText = "Select All";
                _pieceOfEquipmentsFiltered.Clear();
                selectState = false;
            }
            else
            {
                btnSelectAllText = "Deselect All";

                //foreach (var item in Grid.Items)
                //{
                //   await  addToListPoE(item.WorkOrderDetailID,false);
                //}
                _pieceOfEquipmentsFiltered = Grid.Items.ToList();

                selectState = true;
            }


            
            //WODOnlineList.Clear();

            StateHasChanged();

            selectAllButton = false;


        }

        public async Task DeleteDataBase()
        {
            var loadpa = await ConfirmAsync("Delete Data in Offline Database");

            await ShowProgress();

            if (loadpa)
            {
                //await DatabaseService.deleteDatabaseAsync();

                //await DatabaseService.InitDatabaseAsync();

                //await OffLineClient(DbFactory).InitializeAsync();

                await OffLineClient(DbFactory).DeleteDatabase();


                await LoadParametrics(Technician);
            }

            await CloseProgress();

        }

            public async Task NewItemOff()
        {
            if (!ConnectionStatusService.GetCurrentConnected() || !ConnectionStatusService.GetCurrentStatus())
            {

                await ShowToast("To upload to the server, verify that it is in Online mode", ToastLevel.Info);
                return;

            }
            try
            {
                await ShowProgress();

                await ScrollPosition("gridWod");

                var cu = await CurrentUserName();

                if (string.IsNullOrEmpty(cu))
                {
                    Navigation.NavigateTo($"authentication/login?returnUrl={Uri.EscapeDataString(Navigation.Uri)}");
                }


                _pieceOfEquipmentsFilteredOff = _pieceOfEquipmentsFilteredOff.DistinctBy2(x => x.WorkOrderDetailID).ToList();


               if (_pieceOfEquipmentsFilteredOff== null || _pieceOfEquipmentsFilteredOff?.Count == 0)
                {
                    return;
                }

                //BasicsServiceGRPC basics = new BasicsServiceGRPC(,DbFactory);


                WorkOrderDetailGrpc wod = new WorkOrderDetailGrpc(Client);
                //using (var db = await DbFactory.CreateDbContextAsync())
                //{
                    foreach (var item in _pieceOfEquipmentsFilteredOff)
                    {
                        var a = await OffLineClient(DbFactory).GetWODById(item);//db.WorkOrderDetail.Where(x => x.WorkOrderDetailID == item.WorkOrderDetailID).FirstOrDefault();

                        a.OfflineID = Guid.NewGuid().ToString();

                        var all = await wod.SaveWodOff(a);

                        a.OfflineStatus = 1;
                        

                        //var b = await OffLineClient(DbFactory).UpdateOfflineID(a);

                        var b = await OffLineClient(DbFactory).Delete(a);

                    if (!b)
                    {
                        throw new Exception("Error delete offline record" );
                    }

                        await ShowToast("Save success", ToastLevel.Success);

                        await OffLineClient(DbFactory).ini2();

                    //_pieceOfEquipmentsFilteredOff.Add(a);
                }

                //}
                await GridOff.SearchFunction();
                StateHasChanged();

            }
            catch (RpcException ex)
            {   // AppSecurity.IsNotGrpc = true;
                await ShowError("Review server connection");

                await ExceptionManager(ex);

            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);

            }
            finally
            {
                await CloseProgress();
            }

        }

        public static System.Timers.Timer aTimer;
        public async Task NewItem()
        {
            try
            {            


                var loadpa = await ConfirmAsync("Load Parametrics");

                await ShowProgress();

                if (loadpa)
                {
                    //await DatabaseService.deleteDatabaseAsync();

                    //await DatabaseService.InitDatabaseAsync();

                    //await OffLineClient(DbFactory).InitializeAsync();

                    await OffLineClient(DbFactory).DeleteDatabase();


                    await LoadParametrics(Technician);
                }

                 await GetTechID();


                var listfilter = Grid.ItemList.ToList();

                //var list = _pieceOfEquipmentsFiltered;

                // Suponiendo que WorkOrderDetailID es la clave para comparar
                var idsFiltrados = listfilter.Select(x => x.WorkOrderDetailID).ToHashSet();

                var list = _pieceOfEquipmentsFiltered
                    .Where(x => idsFiltrados.Contains(x.WorkOrderDetailID))
                    .ToList();



                if (list != null && list.Count > 0)
                {
                    WorkOrderDetailGrpc assets = new WorkOrderDetailGrpc(Client);

                    AssetsServiceGRPC assetsS = new AssetsServiceGRPC(AssetsServices);

                    PieceOfEquipmentGRPC poes = new PieceOfEquipmentGRPC(POEService);

                    List<WorkOrderDetail> lst = new List<WorkOrderDetail>();

                    List<PieceOfEquipment> lstpoe = new List<PieceOfEquipment>();

                    List<WorkOrder> lstWO = new List<WorkOrder>();

                    List<User_WorkOrder> lstUW = new List<User_WorkOrder>();

                    foreach (var item in list)
                    {
                        //Console.WriteLine("----------------------wod id:" + item.WorkOrderDetailID);
                        if (item != null)
                        {
                            WorkOrder wo = new WorkOrder();
                            wo.WorkOrderId = item.WorkOrderID;
                            if (item.WorkOrderDetailID > 0)
                            {
                                var a = await assets.GetByID(item);                              
                                wo.WorkOrderDetails = new List<WorkOrderDetail>();
                                var wodw = new WorkOrderDetail();
                                wodw.CurrentStatusID = 2;
                                wo.WorkOrderDetails.Add(wodw);
                                var resul = await assets.GetConfiguredWeights(a);
                                //wo.WorkOrderId = a.WorkOrderID;
                                a.BalanceAndScaleCalibration = resul.BalanceAndScaleCalibration;


                                if (a != null && a.PieceOfEquipment != null && a.PieceOfEquipment.EquipmentTemplate != null
                                    && a.PieceOfEquipment.EquipmentTemplate.EquipmentTemplateID > 0)
                                {
                                    var et1 = lstET.Where(x => x.EquipmentTemplateID
                                    == a.PieceOfEquipment.EquipmentTemplate.EquipmentTemplateID).FirstOrDefault();
                                    if (et1 != null)
                                    {
                                        lstET.Remove(et1);
                                    }
                                    lstET.Add(a.PieceOfEquipment.EquipmentTemplate);


                                }

                                if (!string.IsNullOrEmpty(a?.PieceOfEquipment?.IndicatorPieceOfEquipmentID)
                                    && !string.IsNullOrEmpty(a?.PieceOfEquipment?.IndicatorPieceOfEquipmentID))
                                {
                                    var p = new PieceOfEquipment();
                                    p.PieceOfEquipmentID = a.PieceOfEquipment.IndicatorPieceOfEquipmentID;

                                    var po1 = lstpoe.Where(x => x.IndicatorPieceOfEquipmentID == a.PieceOfEquipment.IndicatorPieceOfEquipmentID).FirstOrDefault();
                                    if (po1 == null)
                                    {
                                        var poe = await poes.GetPieceOfEquipmentXId(p);
                                        lstpoe.Add(poe);
                                    }

                                }

                                var po2 = lstpoe.Where(x => x.PieceOfEquipmentID == a.PieceOfEquipment.PieceOfEquipmentID).FirstOrDefault();
                                if (po2 == null && !string.IsNullOrEmpty(a.PieceOfEquipment.PieceOfEquipmentID))
                                {
                                    var poe2 = await poes.GetPieceOfEquipmentXId(a.PieceOfEquipment);
                                    lstpoe.Add(poe2);
                                }


                                //TODO cargar indicadores y cargar perifericos
                                a.OfflineStatus = 0;
                                lst.Add(a);

                            }
                           

                            var w = await assetsS.GetWorkOrderByID(wo);
                            //w.WorkOrderDetails = new List<WorkOrderDetail>();
                            //w.WorkOrderDetails.Add(a);
                         

                            foreach (var itemuw in w.UserWorkOrders)
                            {
                                var uwr = lstUW.Where(x => x.WorkOrderID == w.WorkOrderId && x.UserID == itemuw.UserID).FirstOrDefault();
                                if(uwr== null)
                                {
                                    User_WorkOrder uw = new User_WorkOrder();
                                    uw.UserID = itemuw.UserID;
                                    uw.WorkOrderID = itemuw.WorkOrderID;
                                    lstUW.Add(uw);
                                }
                            }

                            lstWO.Add(w);
                            //lst.Add(item);
                        }

                    }

                    var customerlst = new List<CalibrationSaaS.Domain.Aggregates.Entities.Customer>();

                    foreach (var ity in lstpoe)
                    {
                        customerlst.Add(ity.Customer);
                    }

                    var custom = customerlst.DistinctBy(x => x.CustomerID);


                    var resultcustomer = await OffLineClient(DbFactory).LoadCustomer(custom.ToList(), false, new CallOptions());

                    //Console.WriteLine("--------------------------finish load customer------------------------------------ ");

                    var etlst23 = new List<CalibrationSaaS.Domain.Aggregates.Entities.EquipmentTemplate>();

                    foreach (var ity in lstpoe.ToList())
                    {
                        var etss =(EquipmentTemplate) ity.EquipmentTemplate.CloneObject();

                        if (ity?.EquipmentTemplate?.EquipmentTypeObject != null)
                        {
                            etss.EquipmentTypeObject = null;
                        }

                      //if(ity?.EquipmentTemplate?.EquipmentTypeObject != null)
                      //  {
                      //      etss.EquipmentTypeObjectTemp = null;
                      //  }

                      if(ity?.EquipmentTemplate?.EquipmentTypeGroup != null)
                        {
                            etss.EquipmentTypeGroup = null;
                        }
                        
                        
                        etlst23.Add(etss);
                        ity.EquipmentTemplate = null;
                        ity.Customer = null;
                        ity.Indicator = null;
                    }

                    try
                    {
                        var result5g = await OffLineClient(DbFactory).LoadEquipmentTemplate(etlst23.ToList(), new CallOptions()); //ERROR: Tries to recreate Calibration types

                        





                        ///////////////////////////////////////////////



                        var result3 = await OffLineClient(DbFactory).LoadPOE(lstpoe.ToList(), new CallOptions());

                        
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex?.InnerException?.Message);
                        //Console.WriteLine(ex?.StackTrace);

                        //Console.WriteLine(ex.Message);
                    }
                    //Console.WriteLine("--------------------------finish load poe------------------------------------ ");
                    foreach (var yu in lstWO)
                    {
                        yu.Customer = null;
                        //yu.Users = null;
                        //yu.WorkOrderDetails = null;



                    }

                    lstWO = lstWO.DistinctBy(x => x.WorkOrderId).ToList();


                    foreach (var wod in lstWO)
                    {


                        if(wod?.WorkOrderDetails?.Count > 0)
                        {
                            foreach (var wodd in wod.WorkOrderDetails)
                            {
                                var wodddt = _pieceOfEquipmentsFiltered.Where(x => x.WorkOrderDetailID == wodd.WorkOrderDetailID && x.WorkOrderID == wodd.WorkOrderID && x.IsSync == false).FirstOrDefault();

                                if (wodddt == null)
                                {
                                    //wod.WorkOrderDetails.Remove(wodd);
                                    wodd.IsSync = true;
                                }


                            }


                            wod.WorkOrderDetails = wod.WorkOrderDetails.Where(x => x.IsSync == false).ToList();
                        }
                       
                    }


                

                    

                    var result5 = await OffLineClient(DbFactory).LoadWorkOrder(lstWO, new CallOptions()); // ERROR: Tries to recreate UserId
                    //Console.WriteLine("--------------------------finish load work order------------------------------------ ");
                    
                    foreach (var yu in lst)
                    {
                        yu.PieceOfEquipment = null;

                        //yu.Users = null;
                        yu.Technician = null;

                        yu.WorkOder = null;

                        yu.Certificate = null;
                    }

                    try
                    {

                        var resultuw = await OffLineClient(DbFactory).LoadUserWorkOrder(lstUW, new CallOptions()); // ERROR: Tries to recreate UserId

                       
                        
                    }
                    catch(Exception ex)
                    {
                        //Console.WriteLine("Is posible that Technician already load------------------------------------ " + ex.Message);
                        
                        await ShowToast("Is posible that Technician already load " + ex.Message,ToastLevel.Warning);
                    }


                    try
                    {

                        

                        var result = await OffLineClient(DbFactory).InsertWOD(lst, new CallOptions());

                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("error  in  WOD------------------------------------ " + ex.Message);

                        await ShowError("Error Load Data " + ex.Message);
                    }



                    //Console.WriteLine("--------------------------finish load data------------------------------------ ");

                    await ShowToast("Data loading completed.", ToastLevel.Success);


                    _pieceOfEquipmentsFiltered.Clear();

                    _pieceOfEquipmentsFilteredOff.Clear();

                    await OffLineClient(DbFactory).ini2();

                    //await GridOff.SearchFunction();


                    aTimer.Stop();

                    aTimer.Start();


                    //await Grid.SearchFunction();
                    await CloseProgress();

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("error newitem-------------------------------------------------------------- "+ ex.Message);
                await ExceptionManager(ex);
            }
            finally
            {
                await CloseProgress();
            }

        }
        public async Task<bool> Delete(WorkOrderDetail work)
        {


            try
            {
                await ShowProgress();
                WorkOrderDetailGrpc assets = new WorkOrderDetailGrpc(Client);

                var result = await OffLineClient(DbFactory).Delete(work);

                await GridOff.SearchFunction();
                await Grid.SearchFunction();

                return true;

            }
            catch (RpcException ex)
            {   // AppSecurity.IsNotGrpc = true;


                await ExceptionManager(ex);

                return false;

            }
            catch (Exception ex)
            {
                

                await ExceptionManager(ex);

                return false;

            }

            finally
            {
                await CloseProgress();
                
            }
            

           
        }

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

        protected bool CanShow { get; set; } = true;


        List<WorkOrderDetail> WODOffList = new List<WorkOrderDetail>();

        [Inject]
        IConfiguration Configuration { get; set; }

        public string tenant { get; set; }

        protected  override async Task OnInitializedAsync()
        {

            tenant = Configuration.GetSection("Reports:Customer")?.Value;

            Component.Group = "admin,tech.HasEdit,tech.HasView,tech.HasNew,job.HasView,job.HasEdit,job.HasSave,job.HasNew";
            
            await base.OnInitializedAsync();

            //Component.Group = Component.Group.Replace("tech.HasView", "");
           


            ConnectionStatusService.OnChangeConnection += DecideMenuElementsToshow;
            ConnectionStatusService.OnChange += LoadOffline;
            


             Pagination<WorkOrderDetail> pag1 = new Pagination<WorkOrderDetail>();

                    pag1.Page = 1;
                    pag1.Show = 1000;

            if(DbFactory != null)
            {
                //var resultoff = await OffLineClient(DbFactory)
                //    .GetWOD(pag1, new CallOptions());

                WODOffList = new List<WorkOrderDetail>();

                aTimer = new System.Timers.Timer(500);

                aTimer.Elapsed += OnUserFinish;

                aTimer.AutoReset = false;
            }


            //if (JSRuntime != null)
            //{
            //    await JSRuntime.InvokeVoidAsync("backupbase");
            //}


        }

        private void OnUserFinish(Object source, ElapsedEventArgs e)
        {

            aTimer.Stop();  

            InvokeAsync(async () =>
            {
                try
                {
                    await Grid.SearchFunction();
                }
                catch(Exception ex) 
                { 
                }
                finally 
                {
                    await CloseProgress();
                }   
                

                //await GridOff.SearchFunction();
            });

        }

            private async Task LoadOffline(bool connectionStus)
        {
            await InvokeAsync(async () =>
            {
                //CanShow = connectionStus;//show only when app is online
                //StateHasChanged();
                if (!ConnectionStatusService.GetCurrentStatus() && ConnectionStatusService.GetChangeStatus())
                {
                    if (!Launch)
                    {
                        bool exce = false;
                        try
                        {
                            //Console.WriteLine("LoadOffline");
                            await ShowProgress();
                            //await GetTechID();
                            //await LoadParametrics(Technician);
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine("Error load the server information " + ex.Message);
                            await ShowError("Error load the server information");
                            Launch = false;
                            exce = true;
                        }
                        finally
                        {
                            if (!exce)
                            {
                                Launch = true;
                            }

                            await CloseProgress();
                        }

                    }

                }
            });
        }

        private void DecideMenuElementsToshow(bool connectionStus)
        {
            InvokeAsync(() =>
            {
                CanShow = connectionStus;//show only when app is online
                StateHasChanged();
            });
        }

        public BackupLink bl = null!;


    }
}
