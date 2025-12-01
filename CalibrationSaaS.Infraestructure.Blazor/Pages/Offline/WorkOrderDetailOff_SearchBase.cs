using IndexedDB.Blazor;
using Blazored.Toast.Services;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Blazor.Controls;
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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CalibrationSaaS.Domain.BusinessExceptions;
using System.Timers;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Offline
{
    public class WorkOrderDetailOff_SearchBase : Base_Create<WorkOrderDetail, Application.Services.IWorkOrderDetailServices<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    //,IPage<WorkOrder, Application.Services.IAssetsServices<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
    {


        public int Technician { get; set; }


        [Inject]
         public DatabaseService<CalibrationSaaSDBContextOff> DatabaseService { get; set; }
        


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


        public async Task addToListPoES(int ID)

        { 
        
                   var listPoe = Grid.Items.Where(x => x.WorkOrderDetailID == ID).FirstOrDefault();
            

                   var listwods = Grid.Items.Where(x => x.WorkOrderID == listPoe.WorkOrderID).ToList();


            foreach (var item in listwods)
            {
                addToListPoE(item.WorkOrderDetailID);
            }
        
        }


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

//                    Console.WriteLine(_pieceOfEquipmentsFiltered.Count);
                    var xx = _pieceOfEquipmentsFiltered.Where(x => x.WorkOrderDetailID == ID).ToArray();
                    if (xx != null && xx?.Count() > 0)
                    {
                        var res = _pieceOfEquipmentsFiltered.Remove(xx[0]);
                    }
                    else
                    {
                        _pieceOfEquipmentsFiltered.Add(_poe);
                    }
//                    Console.WriteLine(_pieceOfEquipmentsFiltered.Count);

                }
                catch (Exception ex)
                {
//                    Console.WriteLine(ex.Message);
                }

            }
            //await BlazoredModal.Close(ModalResult.Ok(searchComponent.Detail));

        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task addToListPoEOff(int ID)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            if (_pieceOfEquipmentsFilteredOff == null)
            {
                _pieceOfEquipmentsFilteredOff = new List<WorkOrderDetail>();
            }


            if (GridOff.Items != null && _pieceOfEquipmentsFilteredOff != null)
            {

                var listPoe = GridOff.Items.Where(x => x.WorkOrderDetailID == ID).FirstOrDefault();
                WorkOrderDetail _poe = new WorkOrderDetail();
                _poe = new WorkOrderDetail()
                {
                    WorkOrderDetailID = ID,
                    //SerialNumber = listPoe.SerialNumber
                };
                try
                {

//                    Console.WriteLine(_pieceOfEquipmentsFilteredOff.Count);
                    var xx = _pieceOfEquipmentsFilteredOff.Where(x => x.WorkOrderDetailID == ID).FirstOrDefault();
                    if (xx != null)
                    {
                        var res = _pieceOfEquipmentsFilteredOff.Remove(xx);
                    }
                    else
                    {
                        _pieceOfEquipmentsFilteredOff.Add(_poe);
                    }
//                    Console.WriteLine(_pieceOfEquipmentsFilteredOff.Count);
                }
                catch (Exception ex)
                {
//                    Console.WriteLine("error: " + ex.Message);
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
                  if (Technician == 0 && !string.IsNullOrEmpty(u)) 
            
                {

                BasicsServiceGRPC basics = new BasicsServiceGRPC(BasicsServices);

                //var u = await CurrentUserName();

//                Console.WriteLine(u);

                var users = await basics.GetUsers();

                var uss = users.Users.Where(x => x.UserName == u).FirstOrDefault();
                if (uss != null)
                {
                    Technician = uss.UserID;
                    UserDomain = uss;
                }

            }

                else
                {
                    var uss1 = await OffLineClient(DbFactory).GetUser(new CallOptions());
                    BasicsServiceGRPC basics = new BasicsServiceGRPC(BasicsServices2,DbFactory);
                     var users = await basics.GetUsers();

                    var uss = users.Users.Where(x => x.UserID == uss1.CurrentUserID).FirstOrDefault();
                    if (uss != null)
                    {
                    Technician = uss.UserID;
                    UserDomain = uss;
                    }


                }


//            Console.WriteLine(Technician);
            }
            catch(RpcException ex)
            {

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
        public override async Task<ResultSet<WorkOrderDetail>> LoadData(Domain.Aggregates.ValueObjects.Pagination<WorkOrderDetail> pag)
        {



            Grid.Clear();
            Grid.Clear2();
            //AppSecurity.User = System.Security.Claims.ClaimsPrincipal.Current;
            WODOnlineList = new List<WorkOrderDetail>();

            ResultSet<WorkOrderDetail> Eq = new ResultSet<WorkOrderDetail>();

            try
            {
                


//                Console.WriteLine("LoadData off");

                await GetTechID();

                _pieceOfEquipmentsFiltered = new List<WorkOrderDetail>();

                eq.TechnicianID = Technician;

                pag.Entity = eq;

                pag.Show = 100000;

                pag.SaveCache = false;

                var eqt = await GetOrdersByStatus(pag,2);

                var list= new List<WorkOrderDetail>();

                if(eqt?.List?.Count > 0)
                {
                    list.AddRange(eqt.List);
                }

               

                eqt = await GetOrdersByStatus(pag, 1);

                if (eqt?.List?.Count > 0 && list?.Count > 0)
                {
                    eqt.List.AddRange(list);
                    Grid.TotalRows = eqt.List.Count;
                }
                else
                {

                }
                Eq = eqt;
               

//                Console.WriteLine("LoadData off2");
                
            }

            catch (RpcException ex)
            {  
                

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

            if(Eq.List == null)
            {
                Eq.List = new List<WorkOrderDetail>();
            }
            WODOnlineList = Eq.List;
            return Eq;

        }


        public async Task<ResultSet<WorkOrderDetail>> GetOrdersByStatus(Pagination<WorkOrderDetail> pag,int currentStatus)
        {
            WorkOrderDetailGrpc assets = new WorkOrderDetailGrpc(Client);

            ResultSet<WorkOrderDetail> Eq = new ResultSet<WorkOrderDetail>();

            pag.Entity.CurrentStatusID = currentStatus; // 2;

            Eq = await assets.GetByTechnicianPag(pag);

            if (Eq.List != null)
            {
                Eq.List = Eq.List.Distinct().ToList();

                Pagination<WorkOrderDetail> pag1 = new Pagination<WorkOrderDetail>();

                pag1.Page = 1;
                pag1.Show = 1000;

                var resultoff = await OffLineClient(DbFactory).GetWOD(pag1, new CallOptions());

                if (resultoff.List != null)
                {
                    var List2 = Eq.List.Where(item => !resultoff.List.Any(item2 => item2.WorkOrderDetailID == item.WorkOrderDetailID));

                    Eq.List = List2.Distinct().ToList();
                }

                foreach (var item in Eq.List)
                {
                    if (item != null)
                    {
                        _pieceOfEquipmentsFiltered.Add(item);
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
//            Console.WriteLine("loadparametrics");
//            Console.WriteLine(uss.Identity.Name);

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
//            Console.WriteLine("LoadUser");
//            Console.WriteLine("LoadParametrics");

            WorkOrderDetailGrpc assets = new WorkOrderDetailGrpc(Client);
            UOMServiceGRPC uom = new UOMServiceGRPC(UOMServices);
            BasicsServiceGRPC basics = new BasicsServiceGRPC(BasicsServices);

            var a = await assets.GetStatus();

            //var result0 = await OffLineClient(DbFactory).LoadStatus(a.ToList(), new CallOptions());



            var types = await basics.GetEquipmenTypes();

            //var result = await OffLineClient(DbFactory).LoadEquipmentType(types.EquipmentTypes, new CallOptions());



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


            foreach(var iyy in WODOnlineList)
            {
                if (_pieceOfEquipmentsFiltered.Where(x => x.WorkOrderDetailID == iyy.WorkOrderDetailID).FirstOrDefault() !=null)
                {
                    wodtemp.Add(iyy);
                }
            }

            wo.List = wodtemp;

//            Console.WriteLine("End Load Data from server");

            //var result7 = await OffLineClient(DbFactory).LoadUOMType(uomstype, new CallOptions());

            AppState.UnitofMeasureTypeList = uomstype.ToList();

            AppState.UnitofMeasureList = uoms.UnitOfMeasureList;

            AppStateBasics.UnitofMeasureTypeList = uomstype.ToList();

            AppStateBasics.UnitofMeasureList = uoms.UnitOfMeasureList;
            //var result6 = await OffLineClient(DbFactory).LoadUOM(uoms.UnitOfMeasureList, new CallOptions());
            AppSecurity.StatusList = a.ToList();



            //var result5 = await OffLineClient(DbFactory).LoadCalibrationType(cal.CalibrationTypes, new CallOptions());


            await OffLineClient(DbFactory).LoadMany(cal.CalibrationTypes, a.ToList(), types.EquipmentTypes, manu.Manufacturers, uoms.UnitOfMeasureList, uomstype);

//            Console.WriteLine("end LoadMany");

            var result3 = await OffLineClient(DbFactory).LoadUser(users.Users, new CallOptions());


//            Console.WriteLine("end LoadUser");


            if (cert != null)
            {
                var result4 = await OffLineClient(DbFactory).LoadCertification(cert.ToList(), new CallOptions());
            }

//            Console.WriteLine("end LoadCertification");

            Pagination<PieceOfEquipment> p = new Pagination<PieceOfEquipment>();

            p.Show = 10000;
            p.Page = 1;

            p.Entity = new PieceOfEquipment();

            List<User> userList = new List<User>();
            User u = new User();
            u.UserID = Technician;
            userList.Add(u);

            p.Entity.Users = userList;



            //w.UserId = Technician;

            //Pagination<EquipmentTemplate> pag = new Pagination<EquipmentTemplate>();

            // pag.Show = 10000;
            //pag.Page = 1;
            ////pag.Filter = "Balance";
            //pag.Entity = new EquipmentTemplate();
            //pag.Entity.IsGeneric = false;
            //var result13 = await basics.GetEquipment(pag);

            //if (result13.List != null)
            //{
            //    var result14 = await OffLineClient(DbFactory).LoadEquipmentTemplate(result13.List, new CallOptions());
            //}





            PieceOfEquipmentGRPC poes = new PieceOfEquipmentGRPC(POEService);

            //var addresslst= await customer.GetAddressesAsync();


            if (wo.List != null)
            {
                //var result8 = await OffLineClient(DbFactory).LoadWorkOrder(wo.List, new CallOptions());

                List<PieceOfEquipment> piecs = new List<PieceOfEquipment>();
                List<CalibrationSaaS.Domain.Aggregates.Entities.Customer> custlst = new List<CalibrationSaaS.Domain.Aggregates.Entities.Customer>();

                CustomerGRPC customer = new CustomerGRPC(CustomerServices);



                foreach (var item22 in wo.List)
                {
//                    Console.WriteLine("load  WorkOrder " + item22.WorkOrderID);


                    CalibrationSaaS.Domain.Aggregates.Entities.Customer _custId = new CalibrationSaaS.Domain.Aggregates.Entities.Customer();

                    _custId.CustomerID = item22.PieceOfEquipment.CustomerId;


                    var tempcus = custlst.Where(x => x.CustomerID == item22.PieceOfEquipment.CustomerId).FirstOrDefault();

                    if (tempcus == null)
                    {
                        var cust = await customer.GetCustomersByID(_custId);

                        custlst.Add(cust);

                        if (cust.Aggregates != null && cust.Aggregates.Count > 0)
                        {


                        }

                        //var _addressFiltered = await Assets.GetAddressByCustomerId(_custId);

                        //var result11 = await OffLineClient(DbFactory).LoadAddress(_addressFiltered.Addresses, new CallOptions());


                        PieceOfEquipment poe = new PieceOfEquipment();
                        poe.IsWeigthSet = true;
                        poe.IsForAccreditedCal = true;
                        poe.CustomerId = _custId.CustomerID;
                        poe.Customer = cust;
                        Pagination<PieceOfEquipment> p3 = new Pagination<PieceOfEquipment>();

                        p3.Show = 1000;
                        p3.Page = 1;
                        p3.Entity = poe;


                        var Eq = (await poes.GetPieceOfEquipmentByCustomer(p3));

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

                        



//                        Console.WriteLine("end  WorkOrder " + item22.WorkOrderID);
                    }



                    //poe.IsForAccreditedCal = false;
                    //p3.Entity = poe;

                    //Eq = (await poes.GetPieceOfEquipmentByCustomer(p3));


                    //if (Eq.List != null)
                    //{
                    //    piecs.AddRange(Eq.List);
                    //}                        
                }




                //BITTERMAN CUSTOMER
                CalibrationSaaS.Domain.Aggregates.Entities.Customer _custId2 = new CalibrationSaaS.Domain.Aggregates.Entities.Customer();

                _custId2.CustomerID = 25;
                //bitterman 
                var cust2 = await customer.GetCustomersByID(_custId2);

                custlst.Add(cust2);

                var result10 = await OffLineClient(DbFactory).LoadCustomer(custlst, true, new CallOptions());


//                Console.WriteLine("end  LoadCustomer ");

                if (lstET.Count > 0)
                {
                    var result4 = await OffLineClient(DbFactory).LoadEquipmentTemplate(lstET, new CallOptions());
                }




                //await OffLineClient(DbFactory).LoadAddress(addresslst,false, new CallOptions());




//                Console.WriteLine("end  LoadEquipmentTemplate ");

                await OffLineClient(DbFactory).LoadRol(AppSecurity.RolesList, new CallOptions());

//                Console.WriteLine("end  LoadRol ");


                await OffLineClient(DbFactory).LoadPOE(piecs, new CallOptions());

//                Console.WriteLine("end  LoadPOE ");

            }



            var ppp = await poes.GetAllWeightSetsPag(p);

//            Console.WriteLine("end  GetAllWeightSetsPag ");

            if (ppp.List != null)
            {
                var result15 = await OffLineClient(DbFactory).LoadPOE(ppp.List, new CallOptions());
            }

//            Console.WriteLine("end  LoadPOE2 ");





            await ShowToast("Success Load Parametrics, Please wait", ToastLevel.Success);

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


        }

        public async Task<ResultSet<WorkOrderDetail>> LoadData2(Domain.Aggregates.ValueObjects.Pagination<WorkOrderDetail> pag)
        {

            try
            {

                //GridOff.Clear();

                //GridOff.Clear2();

                ////await ShowProgress();

//                Console.WriteLine("LoadData2 off");
                
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

                pag.Entity.CurrentStatusID=2;

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
                }

//                Console.WriteLine("LoadData2 off");
                return result;
            }


            catch (Exception ex)
            {
                await ShowError(ex.Message);
                throw ex;
            }
            finally
            {
                //await CloseProgress();
            }



        }


        //public async Task<ResultSet<WorkOrderDetail>> LoadData3(Domain.Aggregates.ValueObjects.Pagination<WorkOrderDetail> pag)
        //{

        //    await GetTechID();


        //    eq.TechnicianID = Technician;

        //    pag.Entity = eq;

        //    WorkOrderDetailGrpc assets = new WorkOrderDetailGrpc(Client);


        //    var result = await OffLineClient(DbFactory).GetWODUploaded(pag, new CallOptions());

        //    //if (result.List != null)
        //    //{
        //    //    foreach (var item in result.List)
        //    //    {
        //    //        if (item.OfflineStatus == 1)
        //    //        {
        //    //            _pieceOfEquipmentsFilteredOff.Add(item);
        //    //        }

        //    //    }
        //    //}


        //    return result;

        //}


#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnAfterRenderAsync(bool firstRender)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            if (!firstRender)
            {
               await ScrollPosition();
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

        }




#pragma warning disable CS0108 // 'WorkOrderDetailOff_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<WorkOrderDetail, IWorkOrderDetailServices<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        public void Dispose()
#pragma warning restore CS0108 // 'WorkOrderDetailOff_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<WorkOrderDetail, IWorkOrderDetailServices<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        {

        }

        [Inject] 
        public NavigationManager Navigation { get; set; }

        public bool selectAll;
        
        public string btnSelectAllText= "Select All";
        public async Task SelectAll()
        {
            //if (selectAll)
            //{
            //    btnSelectAllText = "Deselect All";
            //}
            //else
            //{
            //    btnSelectAllText = "Select All";
            //}

            _pieceOfEquipmentsFiltered.Clear();


            StateHasChanged();
        
        }



            public async Task NewItemOff()
        {

            try
            {
                await ShowProgress();

                await ScrollPosition("gridWod");

                var cu = await CurrentUserName();

                if (string.IsNullOrEmpty(cu))
                {
                    Navigation.NavigateTo($"authentication/login?returnUrl={Uri.EscapeDataString(Navigation.Uri)}");
                }               


               if(_pieceOfEquipmentsFilteredOff== null || _pieceOfEquipmentsFilteredOff?.Count == 0)
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


                var list = _pieceOfEquipmentsFiltered;

                if (list != null && list.Count > 0)
                {
                    WorkOrderDetailGrpc assets = new WorkOrderDetailGrpc(Client);

                    AssetsServiceGRPC assetsS = new AssetsServiceGRPC(AssetsServices);

                    PieceOfEquipmentGRPC poes = new PieceOfEquipmentGRPC(POEService);

                    List<WorkOrderDetail> lst = new List<WorkOrderDetail>();

                    List<PieceOfEquipment> lstpoe = new List<PieceOfEquipment>();

                    List<WorkOrder> lstWO = new List<WorkOrder>();

                    foreach (var item in list)
                    {
//                        Console.WriteLine("----------------------wod id:" + item.WorkOrderDetailID);
                        if (item != null)
                        {
                            var a = await assets.GetByID(item);
                            WorkOrder wo = new WorkOrder();
                            wo.WorkOrderId = a.WorkOrderID;
                            wo.WorkOrderDetails = new List<WorkOrderDetail>();
                            var wodw = new WorkOrderDetail();
                            wodw.CurrentStatusID = 2;
                            wo.WorkOrderDetails.Add(wodw);
                            var w = await assetsS.GetWorkOrderByID(wo);
                            //w.WorkOrderDetails = new List<WorkOrderDetail>();
                            //w.WorkOrderDetails.Add(a);
                            var resul = await assets.GetConfiguredWeights(a);

                            a.BalanceAndScaleCalibration = resul.BalanceAndScaleCalibration;

                            lstWO.Add(w);
                            if (a != null && a.PieceOfEquipment != null && a.PieceOfEquipment.EquipmentTemplate != null)
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
                            if (po2 == null)
                            {
                                var poe2 = await poes.GetPieceOfEquipmentXId(a.PieceOfEquipment);
                                lstpoe.Add(poe2);
                            }


                            //TODO cargar indicadores y cargar perifericos
                            a.OfflineStatus = 0;
                            lst.Add(a);



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

//                    Console.WriteLine("--------------------------finish load customer------------------------------------ ");

                    var etlst = new List<CalibrationSaaS.Domain.Aggregates.Entities.EquipmentTemplate>();

                    foreach (var ity in lstpoe)
                    {
                        etlst.Add(ity.EquipmentTemplate);
                        ity.EquipmentTemplate = null;
                        ity.Customer = null;
                        ity.Indicator = null;
                    }

                    try
                    {
                        var result5g = await OffLineClient(DbFactory).LoadEquipmentTemplate(etlst, new CallOptions());

                        var result3 = await OffLineClient(DbFactory).LoadPOE(lstpoe, new CallOptions());
                    }
                    catch (Exception ex)
                    {
//                        Console.WriteLine(ex.Message);
                    }
//                    Console.WriteLine("--------------------------finish load poe------------------------------------ ");
                    foreach (var yu in lstWO)
                    {
                        yu.Customer = null;
                        //yu.Users = null;
                        //yu.WorkOrderDetails = null;


                    }

                    lstWO = lstWO.DistinctBy(x => x.WorkOrderId).ToList();


                    foreach (var wod in lstWO)
                    {

                        foreach (var wodd in wod.WorkOrderDetails)
                        {
                            var wodddt = _pieceOfEquipmentsFiltered.Where(x=>x.WorkOrderDetailID==wodd.WorkOrderDetailID).FirstOrDefault();

                            if (wodddt == null)
                            {
                                wod.WorkOrderDetails.Remove(wodd);
                            }


                        }


                    }

                    var result5 = await OffLineClient(DbFactory).LoadWorkOrder(lstWO, new CallOptions());
//                    Console.WriteLine("--------------------------finish load work order------------------------------------ ");

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

                         var result = await OffLineClient(DbFactory).InsertWOD(lst, new CallOptions());
                    }
                    catch(Exception ex)
                    {
//                        Console.WriteLine("error  in  WOD------------------------------------ " + ex.Message);
                        
                        await ShowError("Error Load Data " + ex.Message);
                    }
                                       

//                    Console.WriteLine("--------------------------finish load data------------------------------------ ");

                    await ShowToast("End Load Data", ToastLevel.Success);


                    _pieceOfEquipmentsFiltered.Clear();

                    _pieceOfEquipmentsFilteredOff.Clear();



                    //await GridOff.SearchFunction();


                    aTimer.Stop();

                    aTimer.Start();


                    //await Grid.SearchFunction();


                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
//                Console.WriteLine("error newitem-------------------------------------------------------------- "+ ex.Message);
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

        protected async override Task OnInitializedAsync()
        {

            ConnectionStatusService.OnChangeConnection += DecideMenuElementsToshow;
            ConnectionStatusService.OnChange += LoadOffline;
            await base.OnInitializedAsync();


             Pagination<WorkOrderDetail> pag1 = new Pagination<WorkOrderDetail>();

                    pag1.Page = 1;
                    pag1.Show = 1000;

            var resultoff = await OffLineClient(DbFactory).GetWOD(pag1, new CallOptions());

            WODOffList = resultoff.List;

            aTimer = new System.Timers.Timer(500);

            aTimer.Elapsed += OnUserFinish;

            aTimer.AutoReset = false;

        }

        private void OnUserFinish(Object source, ElapsedEventArgs e)
        {

            InvokeAsync(async () =>
            {


                await Grid.SearchFunction();
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
//                            Console.WriteLine("LoadOffline");
                            await ShowProgress();
                            //await GetTechID();
                            //await LoadParametrics(Technician);
                        }
                        catch (Exception ex)
                        {
//                            Console.WriteLine("Error load the server information " + ex.Message);
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
    }
}
