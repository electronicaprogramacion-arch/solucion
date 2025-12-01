using Blazed.Controls;
using Blazed.Controls.Toast;
using Blazor.IndexedDB.Framework;
using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Helpers;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static SQLite.SQLite3;


namespace BlazorApp1.Blazor.Pages.AssetsBasics
{
    public class WorkOrder_CreateBase : Base_Create<WorkOrder, Func<dynamic, CalibrationSaaS.Application.Services.IAssetsServices<CallContext>>, CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany>
    {


         [Inject] 
        public Func<dynamic, CalibrationSaaS.Application.Services.IWorkOrderDetailServices<CallContext>> WODServices { get; set; }
        public List<TestCode> TestCodeList { get; set; } = new List<TestCode>();

        public TestCode CurrentTestCode { get; set; }

        public int TestCodeID { get; set; }
        public int OptionID { get; set; }
        [CascadingParameter] public IModalService Modal { get; set; }
        [Inject] CalibrationSaaS.Application.Services.IAddressServices _addressService { get; set; }
        [Inject] CalibrationSaaS.Application.Services.ICustomerService<CallContext> _customerService { get; set; }
        //[Inject] public CalibrationSaaS.Application.Services.ICompanyServices _companyService { get; set; }
        //[Inject] public CalibrationSaaS.Application.Services.IWorkOrderDetailServices<CallContext> _workOrderDetailService { get; set; } 
#pragma warning disable CS0108 // 'WorkOrder_CreateBase.Logger' oculta el miembro heredado 'KavokuComponentBase<WorkOrder>.Logger'. Use la palabra clave new si su intenci�n era ocultarlo.
        [Inject] ILogger<WorkOrder_CreateBase> Logger { get; set; }
#pragma warning restore CS0108 // 'WorkOrder_CreateBase.Logger' oculta el miembro heredado 'KavokuComponentBase<WorkOrder>.Logger'. Use la palabra clave new si su intenci�n era ocultarlo.

        //[Inject] CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext> _pieceOfEquipmentService { get; set; }
        
        public Blazed.Controls.Typeahead<TestCode,TestCode> Typehead { get; set; }
        
        
        public TestPointGroup GroupVar = new TestPointGroup();
        public CalibrationSaaS.Domain.Aggregates.ValueObjects.AddressResultSet _addressFiltered;
        public CalibrationSaaS.Domain.Aggregates.ValueObjects.ContactResultSet _userFiltered;
        public CalibrationSaaS.Domain.Aggregates.ValueObjects.PieceOfEquipmentResultSet _pieceOfEquipmentFiltered;
        public string _CustomerId { get; set; }

        public string _CustomerValue { get; set; }




        public TenantDTO _tenant;
        public string _PieceOfEquipmentId { get; set; }
        public string _PieceOfEquipmentValue { get; set; }
        public string _message { get; set; }
        public string _addressId { get; set; }

        public List<User> _userList = new List<User>();
        // public List<CalibrationType> _calibrationTypeList = new List<CalibrationType>();
        public List<PieceOfEquipment> _listPoEDueDate = new List<PieceOfEquipment>();

        public List<PieceOfEquipment> _listPoEDueDateChildren = new List<PieceOfEquipment>();

        public ICollection<WorkOrderDetail> _listWorkOrderDetail = new List<WorkOrderDetail>();
        public BlazorApp1.Blazor.Pages.AssetsBasics.WorkOrderDetail_Search WorkOrderDetailSearch 
            = new BlazorApp1.Blazor.Pages.AssetsBasics.WorkOrderDetail_Search();

        /// YPPP   <summary>
        /// YPPP 9515 
        ///
        /// </summary>
        /// 
        [Inject] Microsoft.Extensions.Configuration.IConfiguration Configuration { get; set; }

        public string url { get; set; }

        public string StandardComponent { get; set; }
        public string UsersAccess { get; set; } = "HasAccess";
        public EquipmentType EquipmentTypeObject { get; set; }

        public string ClearWeightSets { get; set; } = "Clear All";
        public CalibrationType calibrationType { get; set; }

        [Inject] public Func<dynamic, CalibrationSaaS.Application.Services.IBasicsServices<CallContext>> _basicsServices { get; set; }

        public string AddWeightSets { get; set; } = "Add Standard";

        public List<WeightSet> WeightSetList2 { get; set; }
        public List<WeightSet> Group2 { get; set; }
        public List<dynamic> weigset { get; set; } = new();
        public dynamic WeightSetComponent
        {

            get
            {

                return weigset;
            }

            set
            {


                bool inc = false;
                dynamic itemtmp = null;
                foreach (var item in weigset)
                {
                    if (item.ID == value.ID)
                    {
                        inc = true;
                        itemtmp = item;
                    }
                }
                if (itemtmp != null)
                {
                    weigset.Remove(itemtmp);
                }
                weigset.Add(value);

            }

        }

        ///

        public async Task ChangeDescripcion(ChangeEventArgs arg)
        {

            var poq = ((PieceOfEquipment)arg.Value);
            await SetWeights(poq);

        }
        private async Task SetWeights(PieceOfEquipment poq)
        {

            List<WO_Weight> lsww = new List<WO_Weight>();
            if (poq?.WeightSets == null || poq?.WeightSets?.Count == 0)
            {
                return;
            }
            foreach (var item in poq.WeightSets)
            {
                WO_Weight wow = new WO_Weight();

                wow.WeightSetID = item.WeightSetID;
                wow.WeightSet = item;
               lsww.Add(wow);
            }

            WeightSetList2 = poq.WeightSets.ToList();

            eq.WO_Weights = lsww;

            //Description += ((PieceOfEquipment)poq).PieceOfEquipmentID + " | ";

            //ChangeEventArgs arg = new ChangeEventArgs();

            //arg.Value = poq;

            //await OnChangeDescription.InvokeAsync(arg);


            //if (Compresion != null && Compresion?.WeightSetComponent != null && Compresion.WeightSetComponent.ChangeDescription.HasDelegate)
            //{
            //    await Compresion.WeightSetComponent.ChangeDescription.InvokeAsync(arg);
            //}



        }

        public async Task ClearWeightSet(ChangeEventArgs arg)
        {


            var poq = new PieceOfEquipment();

            WeightSetList2 = new List<WeightSet>();

            poq.WeightSets = new List<WeightSet>();

            //eq.WOD_Weights = new List<WOD_Weight>();

            await SetWeights(poq);


           
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);


            if (weigset != null && WeightSetList2 != null)
            {

                foreach (var wei in WeightSetComponent)
                {
                    await wei.Show(WeightSetList2);
                }

            }
            if (firstRender)
            {

                var uer = await CurrentUserName();

                Component.User = uer;

                Component.Group = Component.Group + ",job.HasView,job.HasEdit,job.HasSave";
                
                TypeName = "WorkOrder";

                await LoadWO2();
            }


            if (eq.WO_Weights != null)
            {

                foreach (var itenx in eq.WO_Weights)
                {
                    if (itenx?.WeightSet?.PieceOfEquipment != null)
                    {
                        itenx.WeightSet.PieceOfEquipment.EquipmentTemplate = null;
                        itenx.WeightSet.PieceOfEquipment.WeightSets = null;
                        itenx.WeightSet.WO_Weights = null;
                    }

                }
            }
        }
        WorkOrderDetailGrpc wod;
        public async Task LoadWO2()
        {

            int line = 0;
            WeightSetList2 = new List<WeightSet>();

            if (EntityID == "0")
            {
                line = 2;
                eq = new CalibrationSaaS.Domain.Aggregates.Entities.WorkOrder();

            }
            else
            {
                eq.WorkOrderId = Convert.ToInt32(EntityID);
            }

            //parameters.Add("WorkOrderDetail", eq);

          
        }
        protected override async Task OnParametersSetAsync()
        {

            await base.OnParametersSetAsync();

        }
        protected async Task FormSubmitted(EditContext editContext)
        {

            if (_CustomerId != null)
            {
                eq.CustomerId = Convert.ToInt32(_CustomerId);
            }

            var validate = await ContextValidation(true);

            WorkOrderDetailGrpc wodG = new WorkOrderDetailGrpc(WODServices,DbFactory);
            List<User> usertmp =new  List<User>();

            if (validate)
            {
                try
                {
                    await ShowProgress();

                    // await ShowModal();
                    LastSubmitResult = "Insert was executed";



                    eq.Customer = null;


                    //eq.Users = _userList;

                    //  Result = (await Client.CreateWorkOrder(eq, new CallContext()));

                    //PieceOfEquipment _poe = new PieceOfEquipment();
                    WorkOrder _wo = new WorkOrder();

                    Logger.LogDebug("_listPoEDueDate " + _listPoEDueDate);

                    List<WorkOrderDetail> _listWorkOrderDetail2 = new List<WorkOrderDetail>();

                    int cont = _listWorkOrderDetail.Count;
                    cont = cont + 1;

                    foreach (PieceOfEquipment poe in _listPoEDueDate)
                    {
                        //TestCode testCode = new TestCode();
                        //testCode.TestCodeID = (int)poe.TestCodeID;
                        //testCode = await wodG.GetTestCodeByID(testCode);
                        WorkOrderDetail wod = new WorkOrderDetail();
                        wod.PieceOfEquipment = new PieceOfEquipment();
                        wod.PieceOfEquipmentId = poe.PieceOfEquipmentID;
                      
                        wod.PieceOfEquipment = poe;
                        wod.IsAccredited = eq.IsAccredited;
                        wod.WorkOder = _wo;
                        
                        wod.TestCodeID = poe.TestCodeID;
                        wod.CalibrationDate= poe.CalibrationDate;   
                        wod.WorkOrderDetailUserID= eq.WorkOrderId.ToString() + "-" + cont.ToString();
                        wod.WorkOrderID = eq.WorkOrderId;
                        wod.AddressID = eq.AddressId;
                        if (!IsOnline || 1 == 1)
                        {
                            wod.WorkOrderDetailID = NumericExtensions.GetUniqueID(wod.WorkOrderDetailID);// Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2) + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString());

                            wod.IsOffline = true;

                        }
                        wod.CurrentStatusID = 1;
                        _listWorkOrderDetail2.Add(wod);
                        cont++;
                        ////if childs
                        int cont2 = 1;

                        int wodid = wod.WorkOrderDetailID;
                        foreach (PieceOfEquipment poe2 in _listPoEDueDateChildren.Where(x => x.ParentID ==poe.PieceOfEquipmentID))
                        {
                            // testCode = new TestCode();
                            //testCode.TestCodeID = (int)poe.TestCodeID;
                            //testCode = await wodG.GetTestCodeByID(testCode);
                             wod = new WorkOrderDetail();
                            wod.PieceOfEquipment = new PieceOfEquipment();
                            wod.PieceOfEquipmentId = poe2.PieceOfEquipmentID;

                            wod.PieceOfEquipment = poe2;
                            wod.IsAccredited = eq.IsAccredited;
                            wod.WorkOder = _wo;

                            wod.TestCodeID = poe2.TestCodeID;
                            wod.CalibrationDate = poe2.CalibrationDate;
                            wod.WorkOrderDetailUserID = eq.WorkOrderId.ToString() + "-" + cont.ToString() + "-" + cont2.ToString();
                            wod.WorkOrderID = eq.WorkOrderId;
                            wod.ParentID = wodid;
                            wod.AddressID = eq.AddressId;
                            if (!IsOnline || 1 == 1)
                            {
                                wod.WorkOrderDetailID = NumericExtensions.GetUniqueID(0);// Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2) + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString());

                                wod.IsOffline = true;

                            }
                            wod.CurrentStatusID = 1;
                            _listWorkOrderDetail2.Add(wod);
                            cont2++;

                        };


                    };

                 




                    //var anyDuplicate = _listWorkOrderDetail.GroupBy(x => x.PieceOfEquipmentId).Any(g => g.Count() > 1);
                    //var anyDuplicate = _listWorkOrderDetail.Where(x=>x.PieceOfEquipmentId==)

                    HashSet<string> diffids = new HashSet<string>(_listWorkOrderDetail2.Select(s => s.PieceOfEquipmentId));
                    //You will have the difference here
                    var results = _listWorkOrderDetail.Where(m => diffids.Contains(m.PieceOfEquipmentId)).ToList();

                    if (results != null && results?.Count > 0)
                    {
                        throw new Exception("Duplicate Piece of Equipment");

                    }

                    eq.WorkOrderDetails = _listWorkOrderDetail2;

                    AssetsServiceGRPC assets = new AssetsServiceGRPC(Client, DbFactory);
                    if(eq?.Users != null)
                    {
                        usertmp = eq.Users.ToList();
                    }

                    ////
                    ///


                    ////YPPP

                    List<WO_Standard> standards = new List<WO_Standard>();
                    List<WeightSet> weightSets = new List<WeightSet>();
                    List<WO_Weight> lsww = new List<WO_Weight>();
                    var groupedWeightSets = WeightSetList2
                        .GroupBy(ws => ws.PieceOfEquipmentID)
                        .Select(group => new
                        {
                            PieceOfEquipmentID = group.Key,
                            WeightSets = group.ToList()
                        })
                        .ToList();

                    foreach (var group in groupedWeightSets)
                    {
                        WO_Standard ws = new WO_Standard();
                        ws.WorkOrderID = eq.WorkOrderId;
                        ws.PieceOfEquipmentID = group.PieceOfEquipmentID;
                        standards.Add(ws);
                    }
                    
                    foreach (var item in WeightSetList2)
                    {
                        WO_Weight wow = new WO_Weight();

                        wow.WorkOrderID = eq.WorkOrderId;
                        wow.WeightSetID = item.WeightSetID;
                      
                      
                        lsww.Add(wow);
                    }

                  

                    eq.WO_Weights = lsww;
                    eq.WO_Standard = standards;
                    //Description += ((PieceOfEquipment)poq).PieceOfEquipmentID + " | ";

                    //ChangeEventArgs arg = new ChangeEventArgs();

                    //arg.Value = poq;




                    ////
                    ///

                    Result = (await assets.CreateWorkOrder(eq, new CallContext()));

                    eq = Result;

                     _listPoEDueDate = new List<PieceOfEquipment>();

                    if (eq.WorkOrderDetails != null && eq.WorkOrderDetails.Count > 0)
                    {

                        foreach (var item in eq.WorkOrderDetails)
                        {
                            item.WorkOder = eq;
                        }
                        var tmp = eq.WorkOrderDetails;

                        _listWorkOrderDetail = tmp;
                        Console.WriteLine("sss");
                        Console.WriteLine(_listWorkOrderDetail.Count);
                        if (WorkOrderDetailSearch?.Grid != null)
                        {
                            WorkOrderDetailSearch.Grid.Clear();
                        }


                        //WorkOrderDetailSearch._listWorkOrderDetail = _listWorkOrderDetail.ToList();
                        WorkOrderDetailSearch.Show(_listWorkOrderDetail.ToList());


                        //StateHasChanged();
                    }
                    await ShowToast("The information has been saved successfully.", ToastLevel.Success);

                    await CloseModal();


                }
                catch (Exception ex)
                {
                    Console.WriteLine("workorder error");
                    Console.WriteLine(usertmp?.Count);
                    
                    await ExceptionManager(ex);
                    
                }
                finally
                {
                     if (eq != null && eq.UserWorkOrders != null && eq.UserWorkOrders.Count > 0)
                    {

                        eq.Users = new List<User>();

                        foreach (var item in eq.UserWorkOrders)
                        {
                            eq.Users.Add(item.User);
                        }
                    }
                     await CloseProgress();
                }
               

            }

        }
#pragma warning disable CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected async Task InvalidFormSubmitted(EditContext editContext)
#pragma warning restore CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            LastSubmitResult = "OnInvalidSubmit was executed";
        }


    
        public ICollection<Certification> Certifications { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Loading = true;

            await base.OnInitializedAsync();

            url = Configuration.GetSection("Reports")["URL"];

            if (AppSecurity?.UserList?.Count > 0)
            {
                _userList = AppSecurity.UserList.Where(x=>x.IsDelete==false && ((x.IsEnabled.HasValue && x.IsEnabled==true) || x.IsEnabled.HasValue==false)).ToList(); //_user.Users;
            }

            if (eq.WorkOrderDate == default)
            {
                eq.WorkOrderDate = DateTime.Now;
            }

            Certifications = AppSecurity.CertificationList;


            if (1 == 1)
            {

                await ShowProgress();

                NameValidationMessage = "valid";


                if (EntityID == "0")
                {
                    eq = new WorkOrder();
                    eq.WorkOrderDate = DateTime.Now;
                    

                }
                else
                {
                    eq.WorkOrderId = Convert.ToInt32(EntityID);

                    try
                    {

                        AssetsServiceGRPC assets = new AssetsServiceGRPC(Client, DbFactory);

                        eq = await assets.GetWorkOrderByID(eq);

                        if (eq.WorkOrderDetails != null && eq.WorkOrderDetails.Count > 0)
                        {

                            foreach (var item in eq.WorkOrderDetails)
                            {
                                item.WorkOder = eq;
                            }

                            _listWorkOrderDetail = eq.WorkOrderDetails;

                        }

                        if (eq != null && eq.CustomerId > 0)
                        {
                            _CustomerId = eq.CustomerId.ToString();


                            _CustomerValue = eq.Customer.Name;

                            Customer = eq.Customer;

                            await CustomerChange(eq.CustomerId.ToString());

                        }

                        if (eq != null && eq?.UserWorkOrders != null && eq?.UserWorkOrders.Count > 0)
                        {

                            eq.Users = new List<User>();

                            foreach (var item in eq.UserWorkOrders)
                            {
                                eq.Users.Add(item.User);
                            }
                        }

                        if (eq != null && eq?.WorkOrderDetails != null && eq?.WorkOrderDetails?.Count > 0 && eq?.WorkOrderDetails?.FirstOrDefault()?.CalibrationTypeID.HasValue==true)
                        {
                            calibrationType = new CalibrationType()
                            {
                                CalibrationTypeId = (int)eq?.WorkOrderDetails?.FirstOrDefault()?.CalibrationTypeID
                            };

                            BasicsServiceGRPC basics = new BasicsServiceGRPC(_basicsServices, DbFactory);
                            var result = await basics.GetCalibrationTypeById(calibrationType);
                            calibrationType = result;
                        }

                        ////YPPP
                        ///9515
                        WeightSetList2 = new List<WeightSet>();
                  
                        if (eq?.WO_Weights != null)
                        {
                           
                            PieceOfEquipment POETemp = new PieceOfEquipment();
                            foreach (var wi in eq.WO_Weights)
                            {
                                if (wi.WeightSet != null && wi.WeightSet.WeightSetID > 0)
                                {
                                   
                                    var Uom = wi.WeightSet.UnitOfMeasureID.GetUoM(AppState.UnitofMeasureList);
                                    wi.WeightSet.UnitOfMeasure = Uom;
                                    WeightSetList2.Add(wi.WeightSet);


                                }

                            }

                            POETemp.WeightSets = WeightSetList2;

                        }
                     //////

                    }

                    catch (Exception ex)
                    {
                        await ExceptionManager(ex);
                    }


                }


                WorkOrderDetailGrpc wod = new WorkOrderDetailGrpc(WODServices,DbFactory);

                Pagination<TestCode> testCodePag = new Pagination<TestCode>();
                testCodePag.Show = 5000; // Reasonable page size
                testCodePag.Page = 1;
                var testcodes = await wod.GetTestCodes(testCodePag, new CallContext());


                TestCodeList = testcodes.List;

                CurrentEditContext = new EditContext(eq);

                if (eq.WorkOrderDate == default)
                {
                    eq.WorkOrderDate = DateTime.Now;
                }

                await CloseProgress();  

                Loading = false;

                
                //StateHasChanged();

            }
        }
        protected void FunctionName(EventArgs args)
        {
        
        }
        public new void Dispose()
        {

        }

        public async Task ShowModalPoECreate()
        {
            Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();

            var parameters = new ModalParameters();
            //parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);
            parameters.Add("CustomerId", _CustomerId);
            //parameters.Add("AddressId", _addressId);
            parameters.Add("AddressId", 1);
            parameters.Add("EntityID", "0");
           

            ModalOptions op = new ModalOptions();

           

            Address address = new Address()
            {

                AddressId = eq.AddressId
            };

            CalibrationSaaS.Domain.Aggregates.Entities.Customer customer = new CalibrationSaaS.Domain.Aggregates.Entities.Customer()
            {
                CustomerID = Convert.ToInt32(_CustomerId)
            }
           ;
            var addressStreet = await _addressService.GetByID(address);
            var customerName = await _customerService.GetCustomersByID(customer, new CallOptions());
            parameters.Add("AddressStreet", addressStreet.StreetAddress1);
            parameters.Add("CustomerName", customerName.Name);
            parameters.Add("Customer", customerName);
            var messageForm = Modal.Show<BlazorApp1.Client.Pages.AssetsBasics.PieceOfEquipment_Create> ("Piece of Equipment", parameters);
            var result = await messageForm.Result;

            if (result != null && !result.Cancelled)
            {

                //Logger.LogDebug("result " + result.Data);
                var _listPoEDueDates = (PieceOfEquipment)result.Data;
                _listPoEDueDate.Add(_listPoEDueDates);

                StateHasChanged();

            }
            else
            {
                return;
            }


        }

        public CalibrationSaaS.Domain.Aggregates.Entities.Customer Customer { get; set; }

        public PieceOfEquipmentGRPC _poeGrpc { get; set; }

        [Inject] public Func<dynamic, CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>> _poeServices { get; set; }


        public async Task ShowModalPoEDueDate()
        {

            if (Customer == null)
            {
                await ShowError("Select Customer");
            }

            Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();

            //var parameters = new ModalParameters();

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            
            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);
            parameters.Add("Checkbox", true);
            parameters.Add("Customer", Customer);
            parameters.Add("pieceOfEquipmentsCh", _listPoEDueDate);
            parameters.Add("Parameter1", _listWorkOrderDetail);
            parameters.Add("Component", Component);

            parameters.Add("CalibrationTypeID", OptionID);
            if(CurrentTestCode != null)
            {
                parameters.Add("TestCodeID", CurrentTestCode.TestCodeID);
            }
            else
            {
                await ShowError("Please select a Test Code first");
                return;
            }
            

            ModalOptions op = new ModalOptions();
            op.ContentScrollable = true;
            op.Class = "blazored-modal " + ModalSize.MediumWindow;
          

            //var messageForm =  Modal.Show<BlazorApp1.Blazor.Pages.AssetsBasics.POEWO_Search> ("Piece Of Equipment", parameters, op);

            //var result = await messageForm.Result;
            
            //Console.Write("result " + result);
            //if (result != null && !result.Cancelled)
            if(1==1)
            {
                //var _listPoEDueDates = (List<PieceOfEquipment>)result.Data;

                var _listPoEDueDates = await OpenModal(parameters);

                if (_listPoEDueDates== null)
                {
                    return;
                }

                _listPoEDueDate.Clear();
                

                _poeGrpc = new PieceOfEquipmentGRPC(_poeServices, DbFactory);

                var g = new CallOptions(await GetHeaderAsync());
                foreach (PieceOfEquipment poe in _listPoEDueDates)
                {
                    var asi = _listWorkOrderDetail.Where(x => x.PieceOfEquipmentId == poe.PieceOfEquipmentID).FirstOrDefault();
                    if (asi == null)
                    {
                        if (CurrentTestCode != null)
                        {
                            poe.TestCodeID = CurrentTestCode.TestCodeID;
                            //Add poes childs
                        }


                        _listPoEDueDate.Add(poe);

                        Pagination<PieceOfEquipment> pag = new Pagination<PieceOfEquipment>();

                       

                        //pag.SaveCache = false;
                        pag.Entity = new PieceOfEquipment();
                        pag.Entity.ParentID = poe.PieceOfEquipmentID;

                        pag.Show = 1000;

                        var result2 = (await _poeGrpc.GetPiecesOfEquipmentChildren(pag));

                        if(result2 != null && result2?.List?.Count > 0)
                        {
                            _listPoEDueDateChildren.AddRange(result2.List);
                        }
                      




                    }
                    else
                    {
                        await ShowToast("Piece of Equipment : " + poe.PieceOfEquipmentID + " already in Order", ToastLevel.Info);
                    }


                }

               
            }


            //if (!result.Cancelled)
            //    _message = result.Data?.ToString() ?? string.Empty;


   
        }

        public Task changeAddress(ChangeEventArgs e)
        {
            _addressId = e.Value.ToString();
            Logger.LogDebug("It is definitely: " + _addressId);
            return Task.CompletedTask;
        }

        public bool IsDisabledEditCustomer { get; set; } = true;
      
        public async Task CustomerChange(string customerId)
        {
             //IsDisabledEditCustomer = true;

            if (Client != null)
            {

                try
                {
                    bool configureCustomer = false;

                    CalibrationSaaS.Domain.Aggregates.Entities.Customer _custId = new CalibrationSaaS.Domain.Aggregates.Entities.Customer();
                    _custId.CustomerID = Convert.ToInt32(customerId);

                    AssetsServiceGRPC assets = new AssetsServiceGRPC(Client, DbFactory);

                    _addressFiltered = await assets.GetAddressByCustomerId(_custId);
                   
                    if (_addressFiltered == null || _addressFiltered.Addresses == null || _addressFiltered.Addresses.Count == 0)
                    {
                        configureCustomer = true;
                    }
                    //User _userId = new User();
                    //_userFiltered = await Client.GetUsersByCustomerId(_custId);

                    Contact _contactId = new Contact();
                    _userFiltered = await assets.GetContactsByCustomerId(_custId);

                    if (_userFiltered == null || _userFiltered.Contacts == null || _userFiltered.Contacts.Count == 0)
                    {
                        configureCustomer = true;
                    }

                    if (_addressFiltered.Addresses != null && _addressFiltered.Addresses.Count() > 0) 
                    {
                        configureCustomer = false;
                    }

                    if (configureCustomer)
                    {
                        await ShowError("Please Configure Customer Address and Contacts");
                        IsDisabledEditCustomer = false;
                       


                    }
                }

                catch (Exception ex)

                {
                    await ShowError("Please Configure Customer Address and Contacts");
                    IsDisabledEditCustomer = false;
                   

                }
                finally
                {
                    StateHasChanged();
                }



            }
        }


        public async Task ChangeTestCode(ChangeEventArgs args)
        {
            await Typehead.Clear();

 
        }

        public List<KeyValueOption> GetStandardComponentArray()
        {

            StandardComponent = _listWorkOrderDetail?.FirstOrDefault().PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject?.StandardComponent;
            if (!string.IsNullOrEmpty(StandardComponent))
            {
             
                var nvc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyValueOption>>(StandardComponent);
                
                return nvc;

            }

            return new List<KeyValueOption>(); ;
        }



        [Inject]
        public Radzen.DialogService DialogService { get; set; }

        public async Task<List<PieceOfEquipment>> OpenModal(Dictionary<string, object> parameters)
        {

            Dictionary<string, object> param = new Dictionary<string, object>();

            await LoadStateAsync();

            var result = await DialogService.OpenAsync<BlazorApp1.Blazor.Pages.AssetsBasics.POEWO_Search>("Select",
                   parameters,
                   new DialogOptions()
                   {
                       Resizable = true,
                       Draggable = true,
                       Resize = OnResize,
                       Drag = OnDrag,
                       Width = Settings != null ? Settings.Width : "700px",
                       Height = Settings != null ? Settings.Height : "512px",
                       Left = Settings != null ? Settings.Left : null,
                       Top = Settings != null ? Settings.Top : null
                   });

            await SaveStateAsync();

            if (result == null)
            {

                return null;
            }
            else
            {
                return (List<PieceOfEquipment>)result;
            }


        }

        void OnDrag(System.Drawing.Point point)
        {
            JSRuntime.InvokeVoidAsync("eval", $"console.log('Dialog drag. Left:{point.X}, Top:{point.Y}')");

            if (Settings == null)
            {
                Settings = new DialogSettings();
            }

            Settings.Left = $"{point.X}px";
            Settings.Top = $"{point.Y}px";

            InvokeAsync(SaveStateAsync);
        }

        void OnResize(System.Drawing.Size size)
        {
            JSRuntime.InvokeVoidAsync("eval", $"console.log('Dialog resize. Width:{size.Width}, Height:{size.Height}')");

            if (Settings == null)
            {
                Settings = new DialogSettings();
            }

            Settings.Width = $"{size.Width}px";
            Settings.Height = $"{size.Height}px";

            InvokeAsync(SaveStateAsync);
        }

        DialogSettings _settings;
        public DialogSettings Settings
        {
            get
            {
                return _settings;
            }
            set
            {
                if (_settings != value)
                {
                    _settings = value;
                    InvokeAsync(SaveStateAsync);
                }
            }
        }

        private async Task LoadStateAsync()
        {
            await Task.CompletedTask;

            var result = await JSRuntime.InvokeAsync<string>("window.localStorage.getItem", "DialogSettings_WO");
            if (!string.IsNullOrEmpty(result))
            {
                _settings = JsonSerializer.Deserialize<DialogSettings>(result);
            }
        }

        private async Task SaveStateAsync()
        {
            await Task.CompletedTask;

            await JSRuntime.InvokeVoidAsync("window.localStorage.setItem", "DialogSettings_WO", JsonSerializer.Serialize<DialogSettings>(Settings));
        }

        public class DialogSettings
        {
            public string Left { get; set; }
            public string Top { get; set; }
            public string Width { get; set; }
            public string Height { get; set; }
        }












    }
}