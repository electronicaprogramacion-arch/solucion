using Blazed.Controls;
using Blazed.Controls.Toast;
using Blazor.IndexedDB.Framework;
using Blazored.Modal;
using Blazored.Modal.Services;


using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor;
using BlazorApp1.Blazor.Blazor.Pages.AssetsBasics;

using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Helpers;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using SqliteWasmHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;



using BlazorApp1.Client.Pages.AssetsBasics;
using BlazorApp1.Blazor.Shared;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;

using BlazorApp1.Blazor.Blazor.LTI.Rockwell;
using CalibrationSaaS.Infraestructure.Blazor.Components;


namespace BlazorApp1.Client.Pages.AssetsBasics
{
    public class PieceOfEquipmen_CreateBase : Base_Create<PieceOfEquipment, Func<dynamic, CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>>, CalibrationSaaS.Domain.Aggregates.Shared.Basic.AppState>
    {



        [CascadingParameter(Name = "EquipmentTypeObject")]
        public EquipmentType EquipmentTypeObject { get; set; }

        [Parameter]
        public bool HasChildren { get; set; } = false;

        [Parameter]
        public string ParentID { get; set; }


        [Parameter]
        public List<int> PermitedEquipmentTypeGroup { get; set; }


        public BlazorApp1.Blazor.Pages.AssetsBasics.PieceOfEquipment_SearchChildren Children { get; set; }

        [Inject]
        IConfiguration Configuration { get; set; }

        [Parameter]
        public CalibrationSaaS.Domain.Aggregates.Entities.Customer CustomerChild { get; set; }

        public ModalParameters parameters { get; set; } = new ModalParameters();

        [Inject] public Func<dynamic, CalibrationSaaS.Application.Services.IWorkOrderDetailServices<CallContext>> WODServices { get; set; }

        public BlazorApp1.Client.Pages.Basics.TestPoints.GroupTestPoint2 GroupComponent;

        public BlazorApp1.Client.Pages.Basics.TestPoints.GroupTestPoint2 GroupComponent2;

        public BlazorApp1.Blazor.Blazor.LTI.Rockwell.Scales_Create Scales;


        public ResponsiveTable<PieceOfEquipment> Grid { get; set; } = new ResponsiveTable<PieceOfEquipment>();

        public Blazor.Blazor.Pages.Basics.ResolutionComponent ResolutionComponent;

        public List<PieceOfEquipment> _listPoEDueDate = new List<PieceOfEquipment>();

        [Parameter]
        public CalibrationSaaS.Domain.Aggregates.Entities.Customer Customer { get; set; }

        //public string DefaultFilter { get; set; } = "bitterman";

        public List<TestCode> TestCodeList { get; set; } = new List<TestCode>();

        [CascadingParameter]
        public IModalService Modal { get; set; } = default!;
        //[Inject] CalibrationSaaS.Application.Services.IAddressServices _addressService { get; set; }
        [Inject] public Func<dynamic, CalibrationSaaS.Application.Services.IAssetsServices<CallContext>> _assetsServices { get; set; }

        [Inject] public Func<dynamic, CalibrationSaaS.Application.Services.IBasicsServices<CallContext>> _basicsServices { get; set; }

        [Inject] public Func<dynamic, CalibrationSaaS.Application.Services.ICustomerService<CallContext>> _customerServices { get; set; }

        public Blazed.Controls.Typeahead<TestCode, TestCode> Typehead { get; set; }

        public TestCode CurrentTestCode { get; set; }

        public List<User> _userList = new List<User>();
        public List<CalibrationSaaS.Domain.Aggregates.Entities.Customer> _customerList = new List<CalibrationSaaS.Domain.Aggregates.Entities.Customer>();

        public CalibrationSaaS.Domain.Aggregates.ValueObjects.AddressResultSet _addressFiltered;

        public WeightSet eqWeightSet = new WeightSet();
        public ICollection<WeightSet> _listWeightSets;

        public Certificate eqCertificate = new Certificate();
        public ICollection<CertificatePoE> _listCetificate = new List<CertificatePoE>();

        public bool isDisable { get; set; }
        public int _CustomerId { get; set; }
        public string _CustomerValue { get; set; }

        public int? _EquipmentTemplateID { get; set; }
        public string _Model { get; set; }
        public string _Manufacturer { get; set; }
        public string _Name { get; set; }

        [Parameter]
        public string CustomerId { get; set; }
        [Parameter]
        public string CustomerName { get; set; }
        [Parameter]
        public int AddressId { get; set; }

        [Parameter]
        public string AddressStreet { get; set; }
        public string _message { get; set; }

        public dynamic WeightCreate;

        public BlazorApp1.Blazor.Pages.AssetsBasics.Certificate_CreatePoE CertificateCreate = new BlazorApp1.Blazor.Pages.AssetsBasics.Certificate_CreatePoE();

        public bool IsAccredited { get; set; }
        public List<PieceOfEquipment> _pieceOfEquipmentsFiltered = new List<PieceOfEquipment>();

        public StatusResultSet _statusList;


        public PieceOfEquipment _listPoEIndicator = new PieceOfEquipment();

        public ICollection<WorkOrderDetail> _listWorkOrderDetail = new List<WorkOrderDetail>();

        public BlazorApp1.Blazor.Pages.AssetsBasics.WorkOrderDetailHist_Search WorkOrderDetailSearch { get; set; } = new BlazorApp1.Blazor.Pages.AssetsBasics.WorkOrderDetailHist_Search();

        //public List<UnitOfMeasure> UnitofMeasureList { get; set; }

        //[CascadingParameter] BlazoredModalInstance _BlazoredModal { get; set; }

        public JavaMessage<PieceOfEquipment> JavaMessage2 { get; set; }


        public ModalParameters ModalParameters = new ModalParameters();


        public int? CurrentEquipmentType { get; set; }
        public ModalParameters ModalParametersET = new ModalParameters();
        public bool editCustomer { get; set; } = false;
        public bool HashCapacity { get; set; }

        public string url { get; set; }
        public string ToleranceLabel { get; set; } = "Tol +/- (HV)";


        public string UncertaintyLabel { get; set; } = "Unc. Block (HV)";

        public async Task ChangeControl2(ChangeEventArgs arg)
        {

            if (arg.Value != null && arg.Value.ToString() == "1")
            {

                ToleranceLabel = "Tol +/- (HV)";
                UncertaintyLabel = "Unc. Block (HV)";

            }

            if (arg.Value != null && arg.Value.ToString() == "2")
            {

                ToleranceLabel = "Tol +/- (HK)";
                UncertaintyLabel = "Unc. Block (HK)";

            }

        }

        PieceOfEquipmentGRPC poegrpc;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (Saving)
            {
                return;
            }

            IsAccredited = false;
            if (_listCetificate != null && CertificateCreate != null && IsWeigthSet && firstRender)
            {

                CertificateCreate.Show(_listCetificate.ToList());

            }

            if (EquipmentTypeObject != null && EquipmentTypeObject.CalibrationTypeID != 1)
            {
                IsAccredited = false;
            }

            if (!firstRender)
            {
                await LoadResolution();
            }


            if (firstRender)
            {
                var con = await CallOptions(Component.Name);

                poegrpc = new PieceOfEquipmentGRPC(Client, DbFactory, con.CallOptions);
            }

            await base.OnAfterRenderAsync(firstRender);

        }


        public async Task LoadResolution()

        {
            if (eq.DueDate.ToString("MM/dd/yyyy") == "01/01/0001")
                eq.DueDate = DateTime.Now;

            if (eq.IsToleranceImport == false)
            {
                var equipmentTemplate = eq?.EquipmentTemplate;


                if (equipmentTemplate == null)
                {
                    return;
                }

                if (eq != null && (!eq?.Tolerance?.ToleranceTypeID.HasValue == true || eq?.Tolerance?.ToleranceTypeID == 0)
                    && equipmentTemplate?.Tolerance?.ToleranceTypeID > 0)
                {
                    eq.Tolerance.ToleranceTypeID = equipmentTemplate.Tolerance.ToleranceTypeID;

                }


                if (eq != null && eq?.Tolerance?.AccuracyPercentage == 0 && equipmentTemplate?.Tolerance?.AccuracyPercentage > 0)
                {
                    eq.Tolerance.AccuracyPercentage = equipmentTemplate.Tolerance.AccuracyPercentage;

                }

                if (eq != null && eq?.Resolution == 0 && equipmentTemplate?.Resolution > 0)
                {
                    eq.Resolution = equipmentTemplate.Resolution;

                }


                if (equipmentTemplate?.Ranges != null && equipmentTemplate?.Ranges?.Count > 0)
                {
                    eq.Ranges = equipmentTemplate.Ranges;

                }

                if (eq != null && eq?.ClassHB44 == 0 && equipmentTemplate?.ClassHB44 > 0)
                {
                    eq.ClassHB44 = equipmentTemplate.ClassHB44;
                }

                if (eq != null && eq?.Tolerance?.ToleranceFixedValue == 0 && equipmentTemplate?.Tolerance?.ToleranceFixedValue > 0)
                {
                    eq.Tolerance.ToleranceFixedValue = equipmentTemplate.Tolerance.ToleranceFixedValue;

                }

                if (eq != null && eq?.Tolerance?.ToleranceValue == 0 && equipmentTemplate?.Tolerance?.ToleranceValue > 0)
                {
                    eq.Tolerance.ToleranceValue = equipmentTemplate.Tolerance.ToleranceValue;

                }

                if (eq == null || eq?.CalibrationDate.Year == 1)
                {
                    eq.CalibrationDate = DateTime.Now;
                }

            }

            //if (ResolutionComponent != null)
            //{
            //    EquipmentTemplate et = new EquipmentTemplate();
            //    et.ToleranceTypeID = eq.ToleranceTypeID;
            //    et.AccuracyPercentage = eq.AccuracyPercentage;
            //    et.Resolution = eq.Resolution;
            //    et.DecimalNumber = eq.DecimalNumber;

            //    et.ClassHB44 = eq.ClassHB44;
            //    var tol = ResolutionComponent.IsToleranceImport;
            //    eq.IsToleranceImport = tol;



            //    et.Ranges = eq.Ranges;

            //    await ResolutionComponent.Show(et);
            //}

            if (!eq?.Tolerance?.ToleranceTypeID.HasValue == true)
            {

                eq.Tolerance.ToleranceTypeID = 0;
            }


            if (ResolutionComponent != null && eq?.Tolerance != null)
            {
                EquipmentTemplate et = new EquipmentTemplate();

                et.Tolerance = eq.Tolerance;
                et.Tolerance.ToleranceTypeID = eq.Tolerance.ToleranceTypeID;
                et.Tolerance.AccuracyPercentage = eq.Tolerance.AccuracyPercentage;
                et.Resolution = eq.Resolution;
                et.DecimalNumber = eq.DecimalNumber;
                et.Tolerance.ToleranceFixedValue = eq.Tolerance.ToleranceFixedValue;
                et.Tolerance.ToleranceValue = eq.Tolerance.ToleranceValue;
                ////////Ranges
                ///

                et.ClassHB44 = eq.ClassHB44;
                var tol = eq.IsToleranceImport;
                //eq.IsToleranceImport = tol;
                ResolutionComponent.IsToleranceImport = tol;

                //if (eq.IsToleranceImport == true && ResolutionComponent != null)
                //{

                //    List<RangeTolerance> rl = new List<RangeTolerance>();
                //    var rangecom = ResolutionComponent?.RangeComponent;
                //    if (rangecom?.RT?.Items?.Count > 0)
                //    {
                //        var re2 = rangecom.RT.Items;
                //        rl.AddRange(re2);

                //    }

                //    var resolcom = ResolutionComponent?.RangeAccuracy;
                //    if (resolcom?.RT?.Items?.Count > 0)
                //    {
                //        var re3 = resolcom.RT.Items;
                //        rl.AddRange(re3);
                //    }
                //    //if (rl.Count > 0)
                //    //{
                //    eq.Ranges = rl;
                //    //}
                //}


                et.Ranges = eq.Ranges;
                //if (!eq.IsToleranceImport)
                //{
                //    et.ToleranceTypeID = 0;
                //    et.AccuracyPercentage = 0;
                //    et.Resolution =0;
                //    et.DecimalNumber = 0;
                //    et.Ranges = null;
                //}

                await ResolutionComponent.Show(et);
            }


        }



        public EquipmentTemplate ET;

        public async Task LoadET(EquipmentTemplate ET)
        {

            if (ET == null)
            {
                return;
            }
            EquipmentTypeObject = ET.EquipmentTypeObject;


            if (EquipmentTypeObject?.CalibrationType == null
                || (EquipmentTypeObject?.CalibrationType?.CalibrationSubTypes == null || EquipmentTypeObject?.CalibrationType?.CalibrationSubTypes.Count == 0))
            {
                var con = await CallOptions(Component.Name);
                PieceOfEquipmentGRPC poegrpc = new PieceOfEquipmentGRPC(Client, DbFactory, con.CallOptions);
                var result = await poegrpc.GetDynamicConfiguration(new CalibrationType() { CalibrationTypeId = EquipmentTypeObject.CalibrationTypeID });
                EquipmentTypeObject.CalibrationType = result;
            }



            eq.EquipmentTemplate = ET;

            _EquipmentTemplateID = ET.EquipmentTemplateID;
            _Model = ET.Model;

            _Manufacturer = ET.Manufacturer1.Name;

            _Name = _Manufacturer + " - " + _Model;


            CurrentEditContext.NotifyValidationStateChanged();
            _message = "";

            if (eq.Capacity == 0 && ET.Capacity >= 0)
            {
                eq.Capacity = ET.Capacity;
                eq.UnitOfMeasureID = ET.UnitofmeasurementID;
            }
            //TODO REVIEW
            if (EquipmentTypeObject.HasWorkOrderDetail)
            {
                //IsWeigthSet = false;
                //IsStandard = false;
                IsScale = true;
            }


            if (eq.EquipmentTemplate.EquipmentTypeGroupID == 57)////EquipmentTypeObject.HasWorkOrderDetail && eq.EquipmentTemplate.EquipmentTypeObject.IsBalance)
            {
                IsScale = true;
            }
            else
            {
                IsScale = false;
            }

            if (EquipmentTypeObject.HasCapacity)
            {
                HashCapacity = true;
            }
            else
            {
                HashCapacity = false;
            }


            if (EquipmentTypeObject != null && EquipmentTypeObject.HasStandard || EquipmentTypeObject != null && EquipmentTypeObject.HasStandardConfiguration)
            {
                IsWeigthSet = true;
                IsStandard = true;
                //IsScale = false;

                if (!string.IsNullOrEmpty(ET.EquipmentTypeObject.DefaultCustomer))
                {
                    Pagination<CalibrationSaaS.Domain.Aggregates.Entities.Customer> pag = new Pagination<CalibrationSaaS.Domain.Aggregates.Entities.Customer>();
                    pag.Filter = ET.EquipmentTypeObject.DefaultCustomer;
                    CustomerGRPC _customerGrpc = new CustomerGRPC(_customerServices, DbFactory);
                    var Eq1 = (await _customerGrpc.GetCustomers(pag, new CallOptions()));

                    if (Eq1?.List?.Count == 1)
                    {
                        eq.CustomerId = Eq1.List[0].CustomerID;
                        _CustomerId = eq.CustomerId;


                        _CustomerValue = Eq1.List[0].Name;

                        eq.Customer = Eq1.List[0];

                    }
                }


            }
            else
            {
                IsWeigthSet = false;
                IsStandard = false;

            }




            StateHasChanged();
        }

        public async Task AfterValues()

        {
            GroupComponent.MapObject();


            var eTC = GroupComponent.Eq;
            eq.TestGroups = eTC.TestGroups;

        }

        public async Task UpdateValues()

        {

            GroupComponent.Eq.Resolution = ResolutionComponent.eq.Resolution;
            GroupComponent.Eq.Tolerance.AccuracyPercentage = ResolutionComponent.eq.Tolerance.AccuracyPercentage;
            GroupComponent.Eq.Tolerance.ToleranceTypeID = ResolutionComponent.eq.Tolerance.ToleranceTypeID;
            GroupComponent.Eq.ClassHB44 = ResolutionComponent.eq.ClassHB44;
            GroupComponent.Eq.Ranges = ResolutionComponent.eq.Ranges;
            GroupComponent.Eq.TestGroups = eq.TestGroups;
            GroupComponent.Eq.Tolerance.ToleranceFixedValue = eq.Tolerance.ToleranceFixedValue;
            GroupComponent.Eq.Tolerance.ToleranceValue = eq.Tolerance.ToleranceValue;
        }



        [Inject]
        public CalibrationSaaS.Application.Services.IFileUpload fileUpload { get; set; }

        public BlazorApp1.Blazor.Shared.UncertaintyComponent UncertaintyComponent { get; set; }


        public CalibrationTypeComponent<PieceOfEquipment, GenericCalibration2, GenericCalibrationResult2> CalibrationTypeComponent { get; set; }

        protected async Task FormSubmitted(EditContext editContext)
        {
            Saving = true;

            try
            {
                EquipmentTemplate ETC = null;

                if (GroupComponent != null)
                {
                    GroupComponent.MapObject();


                    ETC = GroupComponent.Eq;
                    eq.TestGroups = ETC.TestGroups;
                }



                if (string.IsNullOrEmpty(eq.PieceOfEquipmentID))
                {
                    eq.PieceOfEquipmentID = Guid.NewGuid().ToString();
                }

                if (eq.PieceOfEquipmentID == eq.IndicatorPieceOfEquipmentID)
                {
                    throw new Exception("Indicator cannot be equal to the part");
                }


                var tg1 = new List<TestPointGroup>();



                Tenant tenant = new Tenant();

                if (eq?.EquipmentTemplate == null && eq.EquipmentTemplateId != null)
                {

                    BasicsServiceGRPC bas = new BasicsServiceGRPC(_basicsServices, DbFactory);
                    EquipmentTemplate equipmentTemplate = new EquipmentTemplate();
                    equipmentTemplate.EquipmentTemplateID = eq.EquipmentTemplateId;
                    ET = await bas.GetEquipmentByID(equipmentTemplate);

                    eq.EquipmentTemplate = ET;
                }


                if (EquipmentTypeObject != null && EquipmentTypeObject.HasWorkOrderDetail)
                {

                    //if (EquipmentTypeObject.HasWorkOrderDetail)
                    //{
                    //    IsWeigthSet = false;
                    //    IsStandard = false;
                    //}


                    if (eq.UnitOfMeasureID.HasValue == false)
                    {
                        await ShowError("There is an error with the unit of measurement.");
                        return;
                    }
                    if (EquipmentTypeObject.HasWorkOrderDetail && eq.Capacity == 0)
                    {
                        await ShowError("There is an error with the specified capacity.");
                        return;
                    }


                    Console.WriteLine("Equipmenttypeok");

                    if (IsScale)
                    {


                        var tespointpresent = EquipmentTypeObject.HasTestpoint;
                        if (EquipmentTypeObject?.HasTestpoint == true)
                        {

                            if (eq?.TestGroups?.Count == 0 || eq?.TestGroups?.ElementAtOrDefault(0)?.TestPoints == null || eq?.TestGroups?.ElementAtOrDefault(0)?.TestPoints?.Count == 0)
                            {
                                tespointpresent = false;
                                await ShowToast("No tespoints configured...", ToastLevel.Warning);

                            }
                        }


                        if (EquipmentTypeObject?.HasTolerance == true && (eq?.Tolerance?.ToleranceTypeID == 0 || !eq?.UnitOfMeasureID.HasValue == true || eq?.UnitOfMeasureID == 0 || eq?.Capacity == 0))
                        {
                            await ShowToast("Review Capacity or Tolerance Values", ToastLevel.Warning);

                        }
                        else if (tespointpresent)
                        {
                            Console.WriteLine("poe 1");

                            Console.WriteLine("poe 2");
                            if (eq.IsTestPointImport == false)
                            {
                                Console.WriteLine("poe 3");
                                eq.TestGroups = null;

                            }
                            else
                            {
                                Console.WriteLine("poe 5");

                                Console.WriteLine("poe 7");
                                if (ETC.TestGroups.Count > 0)
                                {
                                    Console.WriteLine("poe 9");


                                }
                                Console.WriteLine("poe 10");
                                if (eq?.TestGroups?.Count > 0)
                                {
                                    Console.WriteLine("poe 10.5");
                                    foreach (var item in eq.TestGroups)
                                    {
                                        item.Type = "POE";
                                        item.TypeID = "POE";
                                        item.PieceOfEquipmentID = eq.PieceOfEquipmentID;
                                        item.EquipmentTemplateID = null;


                                    }
                                    tg1 = eq.TestGroups.ToList();
                                    Console.WriteLine("poe 11");


                                }
                                else
                                {
                                    await ShowToast("No tespoint group configured", ToastLevel.Warning);
                                }

                            }


                        }
                    }
                    else
                    {
                        Console.WriteLine("poe 32");
                        eq.TestGroups = null;
                    }




                }
                else if (EquipmentTypeObject == null)
                {
                    Console.WriteLine("Equipmenttypeerrororooror");
                    await ShowError("Equipment Template Not Configured, Please select one");
                    return;

                }
                else
                {
                    Console.WriteLine("poe 32");
                    eq.TestGroups = null;
                }





                if (IsWeigthSet == true && (WeightCreate != null && (WeightCreate?.RT?.Items == null || WeightCreate?.RT.Items.Count == 0)))
                {

                    await ShowToast("No Standards Configured", ToastLevel.Warning);

                    eq.WeightSets = null;

                    eq.WeightSets = WeightCreate.RT.ItemList;

                }

                if (WeightCreate != null && WeightCreate?.RT.Items.Count > 0)
                {
                    eq.WeightSets = WeightCreate.RT.Items;


                }


                if (IsWeigthSet == true && (eq.Users == null || eq.Users.Count == 0))
                {

                    await ShowToast("No Technician Configured", ToastLevel.Warning);


                }

                if (!IsWeigthSet == true && ResolutionComponent != null && EquipmentTypeObject?.HasTolerance == true)
                {
                    if (!ResolutionComponent?.eq?.Tolerance?.ToleranceTypeID.HasValue == true)
                    {
                        await ShowError("Review Tolerance Type");
                        return;
                    }

                    eq.IsToleranceImport = ResolutionComponent.IsToleranceImport;

                    if (!eq.IsToleranceImport)
                    {
                        eq.Tolerance = new Tolerance();
                        eq.Tolerance.ToleranceTypeID = 0;
                        eq.Resolution = 0;
                        eq.Tolerance.AccuracyPercentage = 0;
                        eq.Ranges = null;
                        eq.Tolerance.ToleranceFixedValue = 0;
                        eq.Tolerance.ToleranceValue = 0;
                    }
                    else
                    {
                        eq.Tolerance.ToleranceTypeID = ResolutionComponent.eq.Tolerance.ToleranceTypeID;
                        eq.Resolution = ResolutionComponent.eq.Resolution;
                        eq.Tolerance.AccuracyPercentage = ResolutionComponent.eq.Tolerance.AccuracyPercentage;
                        eq.ClassHB44 = ResolutionComponent.eq.ClassHB44;
                        eq.Ranges = ResolutionComponent.eq.Ranges;
                        eq.Tolerance.ToleranceFixedValue = ResolutionComponent.eq.Tolerance.ToleranceFixedValue;
                        eq.Tolerance.ToleranceValue = ResolutionComponent.eq.Tolerance.ToleranceValue;
                        List<RangeTolerance> rl = new List<RangeTolerance>();
                        var rangecom = ResolutionComponent?.RangeComponent;
                        if (rangecom?.RT?.Items?.Count > 0)
                        {
                            var re2 = rangecom.RT.Items;
                            rl.AddRange(re2);

                        }

                        var resolcom = ResolutionComponent?.RangeAccuracy;
                        if (resolcom?.RT?.Items?.Count > 0)
                        {
                            var re3 = resolcom.RT.Items;
                            rl.AddRange(re3);
                        }

                        eq.Ranges = rl;



                    }

                    if (eq.Ranges != null && string.IsNullOrEmpty(eq.PieceOfEquipmentID))
                    {

                        foreach (var r in eq.Ranges)
                        {
                            r.RangeToleranceID = 0;
                            r.PieceOfEquipmentID = eq.PieceOfEquipmentID;
                            r.EquipmentTemplateID = null;


                        }

                    }
                }




                if (eq.POE_POE == null)
                {
                    eq.POE_POE = new List<POE_POE>();
                }
                eq.POE_POE.Clear();
                foreach (var item in _listPoEDueDate)
                {
                    POE_POE poepoe = new POE_POE();

                    poepoe.PieceOfEquipmentID = eq.PieceOfEquipmentID;
                    poepoe.PieceOfEquipmentID2 = item.PieceOfEquipmentID;

                    eq.POE_POE.Add(poepoe);
                }






                eq.CustomerId = _CustomerId;

                eq.EquipmentTemplateId = _EquipmentTemplateID.Value;

                eq.Customer = null;

                eq.EquipmentTemplate = null;


                var HasCert = false;


                if (CertificateCreate?.RT?.Items?.Count() > 0)
                {

                    eq.CertificatePoEs = CertificateCreate.RT.Items;
                    foreach (var certi in eq.CertificatePoEs)
                    {

                        certi.Name = certi.CertificateNumber + "_" + eq.PieceOfEquipmentID + ".pdf";
                        certi.PieceOfEquipmentID = eq.PieceOfEquipmentID;



                    }
                    HasCert = true;
                    //CertificatePoE Lastcpo = eq.CertificatePoEs.ElementAtOrDefault(eq.CertificatePoEs.Count() - 1);

                    //if (Lastcpo != null && Lastcpo.AffectDueDate && Lastcpo.DueDate > eq.DueDate)
                    //{
                    //    eq.DueDate = Lastcpo.DueDate;
                    //}

                }

                if (IsScale && string.IsNullOrEmpty(eq.InstallLocation))
                {
                    await ShowToast("Please fill Install Location", ToastLevel.Info);
                }

                bool formIsValid = await ContextValidation(true);
                LastSubmitResult =
                formIsValid
                ? ""
                : "";


                if (Scales != null)
                {
                    eq.POE_Scale = Scales.RT.ItemList;
                }


                if (CalibrationTypeComponent != null && CalibrationTypeComponent._refs != null && CalibrationTypeComponent._refs.Count > 0)
                {


                    await CalibrationTypeComponent.CloseGenericWindow();



                }




                if (formIsValid)
                {
                    if (!string.IsNullOrEmpty(ParentID) && IsModal)
                    {
                        eq.ParentID = ParentID;
                    }

                    //var con = await CallOptions(Component.Name);


                    //PieceOfEquipmentGRPC poegrpc = new PieceOfEquipmentGRPC(Client, DbFactory,con.CallOptions);
                    if (CurrentTestCode != null)
                        eq.TestCodeID = CurrentTestCode.TestCodeID;

                    await ShowProgress();

                    Result = await poegrpc.PieceOfEquipmentCreate(eq);

                    eq = Result;

                    eq.IsNew = false;

                    if (UncertaintyComponent != null
                     && UncertaintyComponent.Grid.TableChanges.HasChanges())
                    {



                        foreach (var item in UncertaintyComponent.Grid.TableChanges.DeleteList)
                        {
                            item.PieceOfEquipmentID = Result.PieceOfEquipmentID;
                        }

                        foreach (var item in UncertaintyComponent.Grid.TableChanges.AddList)
                        {
                            item.PieceOfEquipmentID = Result.PieceOfEquipmentID;
                        }


                        var unc = await poegrpc.CreateUncertainty(UncertaintyComponent.Grid.TableChanges);

                        UncertaintyComponent.Grid.TableChanges.Clear();
                    }



                    if (CertificateCreate?.RT?.Items?.Count() > 0 && HasCert && CertificateCreate.fileinfo.Count > 0)
                    {


                        foreach (var certi2 in CertificateCreate?.fileinfo)
                        {
                            var ccer = CertificateCreate.RT.Items.FirstOrDefault(x => x.Description == certi2.Name);
                            if (ccer != null)
                            {
                                certi2.Name = ccer.CertificateNumber + "_" + eq.PieceOfEquipmentID + ".pdf";
                                await fileUpload.UploadAsync(CertificateCreate.fileinfo.ToArray());
                            }
                        }




                    }

                    AssetsServiceGRPC ass = new AssetsServiceGRPC(_assetsServices, DbFactory);

                    _listCetificate = await ass.GetCertificateXPoE(eq);

                    // Save pricing information for Thermotemp customers
                    try
                    {
                        await SavePieceOfEquipmentPrices();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "Error saving piece of equipment prices, but equipment was saved successfully");
                        // Don't fail the entire operation if pricing save fails
                    }

                    // Save procedure associations for Thermotemp customers
                    try
                    {
                        await SaveProcedureAssociations();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "Error saving procedure associations, but equipment was saved successfully");
                        // Don't fail the entire operation if procedure association save fails
                    }

                    await ShowToast("The information has been saved successfully.", ToastLevel.Success);
                    editCustomer = false;
                    if (IsModal)
                    {
                        await CloseModal(Result);
                    }

                    LastSubmitResult = "Insert was executed";
                }
                else
                {
                    throw new Exception("Not Valid Form");
                }

            }
            catch (RpcException ex)
            {

                await ExceptionManager(ex);

            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);

            }
            finally
            {
                Saving = false;


                await CloseProgress();

            }

        }


        public async Task<List<GenericCalibrationResult2>> DynamicResult(List<GenericCalibrationResult2> result, string CalibrationSubType)
        {


            if (string.IsNullOrEmpty(CalibrationSubType))
            {
                eq.TestPointResult = result;
            }
            else
            {
                if (eq.TestPointResult != null)
                {
                    eq.TestPointResult.RemoveAll(x => x.CalibrationSubTypeId == Convert.ToInt32(CalibrationSubType));

                    eq.TestPointResult.AddRange(result);
                }
                else
                {
                    eq.TestPointResult = result;
                }


            }



            //eq.TestPointResult = result;

            return eq.TestPointResult;

        }

        public string Tenant { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var tenant = Configuration.GetSection("Reports:Customer")?.Value;


            if (tenant == "LTI" && AppSecurity?.CustomSequences?.Where(x => x.Component == "PieceOfEquipmentCreate")?.FirstOrDefault() != null)
            {

                Tenant = tenant;


            }


            if (eq.DueDate.ToString("MM/dd/yyyy") == "01/01/0001")
                eq.DueDate = DateTime.Now;
            await base.OnInitializedAsync();
            TypeName = "PieceOfEquipment";


            //List<int> list = new List<int>();

            //list.Add(1);
            //list.Add(1);
            //list.Add(3);

            //var modelJson = System.Text.Json.JsonSerializer.Serialize(list, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });


            if (IsModal == false)
            {

                AddressId = 0;
                AddressStreet = "Choose";

            }

            CalibrationSaaS.Infraestructure.Blazor.Services.BasicsServiceGRPC basics = new CalibrationSaaS.Infraestructure.Blazor.Services.BasicsServiceGRPC(_basicsServices, DbFactory);


            var _user = await basics.GetUsers();
            _userList = _user.Users;


            var umt = AppState.UnitofMeasureList;
            if (umt != null && umt.Count > 0)
            {
                //UnitofMeasureList = umt;
            }

            try
            {
                //await ShowProgress();

                Loading = true;

                if (1 == 1)
                {
                    //AssetsServiceGRPC ass = new AssetsServiceGRPC(_assetsServices,DbFactory);
                    //_listWeightSets = await ass.GetWeightSetXPoE(eq);
                    //Logger.LogDebug("_listWeightSets  onParameters " + _listWeightSets.Count());

                    //Tenant tenant = new Tenant();

                    eq.DueDate = DateTime.Now;
                    eq = new CalibrationSaaS.Domain.Aggregates.Entities.PieceOfEquipment();
                    eq.DueDate = DateTime.Now;
                    if (IsModal == true)
                    {
                        _CustomerId = Convert.ToInt32(CustomerId);
                        _CustomerValue = CustomerName;
                        isDisable = true;
                        Console.Write("Customer Id = " + _CustomerId);

                        AddressId = 1;
                        await CustomerChange(_CustomerId.ToString());
                        _addressFiltered.Addresses.FirstOrDefault().AddressId = AddressId;
                        _addressFiltered.Addresses.FirstOrDefault().StreetAddress1 = AddressStreet;
                        eq.AddressId = AddressId;
                        SelectedUser = false;

                        eq.Customer = Customer;
                        eq.CustomerId = _CustomerId;
                        await LoadCustomer();

                    }
                    if (EntityID == "0" || string.IsNullOrEmpty(EntityID))
                    {
                        EntityID = "0";
                        if (!IsModal || eq == null)
                        {
                            eq = new PieceOfEquipment();
                        }

                        eq.IsNew = true;
                        eq.EquipmentTemplate = new EquipmentTemplate();
                        eq.EquipmentTemplate.EquipmentTypeObject = new EquipmentType();
                        eq.CalibrationDate = DateTime.Now;
                        eq.DueDate = DateTime.Now;
                        editCustomer = true;
                    }

                    else
                    {
                        eq.PieceOfEquipmentID = (EntityID);

                        if (poegrpc == null)
                        {
                            poegrpc = new PieceOfEquipmentGRPC(Client, DbFactory, Header);
                            //new CallContext()
                        }

                        eq = await poegrpc.GetPieceOfEquipmentXId(eq);

                        await LoadET(eq.EquipmentTemplate);

                        AssetsServiceGRPC ass1 = new AssetsServiceGRPC(_assetsServices, DbFactory);

                        _listCetificate = await ass1.GetCertificateXPoE(eq);

                        _CustomerId = eq.CustomerId;
                        _CustomerValue = eq.Customer.Name;
                        Console.Write("q.EquipmentTemplateId ---" + eq.EquipmentTemplateId);
                        Console.Write("q.EquipmentTemplate NAME ---  " + eq.EquipmentTemplate.Name);
                        _EquipmentTemplateID = eq.EquipmentTemplateId;

                        _Model = eq.EquipmentTemplate.Model;
                        _Manufacturer = eq.EquipmentTemplate.Manufacturer;

                        _Name = _Manufacturer + " - " + _Model;

                        AddressId = eq.AddressId;
                        AddressStreet = null;

                        await CustomerChange(_CustomerId.ToString());

                        if (_addressFiltered?.Addresses != null)
                        {
                            var a = _addressFiltered.Addresses.Where(x => x.AddressId == AddressId).FirstOrDefault();
                            if (a != null)
                            {
                                AddressStreet = a.StreetAddress1 + "| " + a.CityID + " " + a.StateID;

                                SelectedUser = false;

                                eq.AddressId = AddressId;


                            }
                        }






                        var res = await poegrpc.GetPieceOfEquipmentHistory(eq);


                        if (res?.WorkOrderDetails != null && res?.WorkOrderDetails?.Count > 0)
                        {

                            _listWorkOrderDetail = res.WorkOrderDetails;
                            WorkOrderDetailSearch.LIST1 = _listWorkOrderDetail.ToList();
                            WorkOrderDetailSearch.Show(_listWorkOrderDetail.ToList());


                        }

                        _listPoEIndicator = eq.Indicator;

                        if (eq.UnitOfMeasureID == null || eq?.UnitOfMeasureID == 0)
                        {
                            eq.UnitOfMeasureID = eq.EquipmentTemplate.UnitofmeasurementID;
                        }

                        CurrentEquipmentType = eq.EquipmentTemplate.EquipmentTypeID;



                        //await LoadET(eq.EquipmentTemplate);


                        if (_listPoEIndicator != null && _listPoEIndicator?.PieceOfEquipmentID != null)
                        {
                            SelectedUser = false;
                        }


                        if (eq?.Peripherals != null)
                        {
                            foreach (var item in eq.Peripherals)
                            {
                                _listPoEDueDate.Add(item);
                            }
                        }


                        var eqtg = AppState.EquipmentTypeGroups.Where(x => x.EquipmentTypeGroupID == EquipmentTypeObject.EquipmentTypeGroupID).FirstOrDefault();

                        if (eqtg != null && !string.IsNullOrEmpty(eqtg.Children))
                        {
                            HasChildren = true;

                            PermitedEquipmentTypeGroup = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(eqtg.Children);

                        }


                        //StateHasChanged();

                    }
                    BasicsServiceGRPC basic = new BasicsServiceGRPC(_basicsServices, DbFactory);

                    _statusList = await basic.GetStatus();

                    if (eq != null)
                    {

                        CurrentEditContext = new EditContext(eq);
                    }
                    StateHasChanged();
                    await JSRuntime.InvokeVoidAsync("removeValidClass");
                    Logger.LogDebug("calling removeClass");


                }

                _listWeightSets = eq.WeightSets;

                if (_listWeightSets != null && WeightCreate != null)
                {
                    IsAccredited = false;

                    Logger.LogDebug("IsForAccreditedCal --" + eq.IsForAccreditedCal);

                    if (eq.IsForAccreditedCal == true && eq.EquipmentTemplate.EquipmentTypeObject.CalibrationTypeID == 1)
                    {
                        IsAccredited = true;
                    }
                    else
                    {
                        IsAccredited = false;
                    }
                    if (_listWeightSets != null && IsWeigthSet)
                    {
                        WeightCreate.Show(_listWeightSets.ToList(), IsAccredited);
                    }


                }



                if (eq.WeightSets == null)
                {
                    eq.WeightSets = new List<WeightSet>();
                }



                if (JavaMessage != null && !JavaMessage.IsShowed)
                {
                    var red2 = await ConfirmAsync(JavaMessage.Message);
                    JavaMessage.Result = red2;

                    if (JavaMessage.Result)
                    {
                        eq.Capacity = JavaMessage.Other.Capacity;
                    }
                    JavaMessage.IsShowed = true;
                }

                if (JavaMessage2 != null && !JavaMessage2.IsShowed)
                {
                    var red = await ConfirmAsync(JavaMessage2.Message);
                    JavaMessage2.Result = red;
                    if (JavaMessage2.Result)
                    {
                        eq.UnitOfMeasureID = JavaMessage.Other.UnitofmeasurementID;
                    }
                    JavaMessage2.IsShowed = true;
                }
                Loading = false;


                ModalParameters.Add("PieceOfEquipmentID", eq.PieceOfEquipmentID);
                ModalParametersET.Add("EntityID", "0");
                ModalParametersET.Add("AddFromPoe", true);



                //Component Test Code
                WorkOrderDetailGrpc wod = new WorkOrderDetailGrpc(WODServices, DbFactory);

                // Use pagination to get TestCodes efficiently
                Pagination<TestCode> testCodePag = new Pagination<TestCode>();
                testCodePag.Show = 1000; // Reasonable page size
                testCodePag.Page = 1;
                var testcodes = await wod.GetTestCodes(testCodePag, new CallContext());

                TestCodeList = testcodes.List;
                CurrentTestCode = await SearchTestCodeSelected(eq.TestCodeID);
                await ScrollPosition();

            }
            catch (RpcException ex)
            {
                await ExceptionManager(ex);
            }

            catch (Exception ex)
            {
                await ExceptionManager(ex);
            }
            finally
            {
                LoadingWait = true;
                await CloseProgress();
            }



        }

        public new void Dispose()
        {

        }

        public bool IsStandard { get; set; }
        public bool IsWeigthSet { get; set; }
        public bool IsScale { get; set; }
        public bool SelectedUser { get; set; } = true;
        public async Task ShowModalCustomer()
        {
            Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();

            var parameters = new ModalParameters();
            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);

            ModalOptions op = new ModalOptions();
            ////op.ContentScrollable = true;
            op.Class = "blazored-modal " + Blazed.Controls.ModalSize.MediumWindow;

            var messageForm = Modal.Show<BlazorApp1.Blazor.Pages.Customer.Customer_Search>("Select Customer", parameters, op);
            var result = await messageForm.Result;

            if (!result.Cancelled)
            {


                eq.Customer = (CalibrationSaaS.Domain.Aggregates.Entities.Customer)result.Data;

                EmptyValidationDictionary = result.Data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                      .ToDictionary(prop => prop.Name, prop => prop.GetValue(result.Data, null));

                await LoadCustomer();


                //_CustomerId = eq.Customer.CustomerID;
                //_CustomerValue = eq.Customer.Name;
                //Customer = eq.Customer;
                //CalibrationSaaS.Domain.Aggregates.Entities.Customer customer = new CalibrationSaaS.Domain.Aggregates.Entities.Customer()
                //{
                //    CustomerID = _CustomerId,
                //    Name = _CustomerValue
                //};
                //eq.CustomerId = eq.Customer.CustomerID;
                //_customerList.Add(customer);

                //await CustomerChange(_CustomerId.ToString());
                //if (IsScale)
                //{
                //    SelectedUser = false;
                //}
            }
        }


        public async Task LoadCustomer()
        {
            _CustomerId = eq.Customer.CustomerID;
            _CustomerValue = eq.Customer.Name;
            Customer = eq.Customer;
            CalibrationSaaS.Domain.Aggregates.Entities.Customer customer = new CalibrationSaaS.Domain.Aggregates.Entities.Customer()
            {
                CustomerID = _CustomerId,
                Name = _CustomerValue
            };
            eq.CustomerId = eq.Customer.CustomerID;
            _customerList.Add(customer);

            await CustomerChange(_CustomerId.ToString());
            if (IsScale)
            {
                SelectedUser = false;
            }
        }

        public async Task ShowModalReport()
        {
            Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();

            var parameters = new ModalParameters();

            //PieceOfEquipmentID
            var par1 = new Helpers.Controls.ValueObjects.ReportView.ParameterClass();

            par1.Name = "POE_No";
            par1.Value = eq.PieceOfEquipmentID;
            par1.Operator = "=";
            par1.ColumnName = "w.PieceOfEquipmentId";

            List<Helpers.Controls.ValueObjects.ReportView.ParameterClass> lstPar = new List<ReportView.ParameterClass>();
            lstPar.Add(par1);
            parameters.Add("EntityID", "1");
            parameters.Add("Parameters", lstPar);


            var par2 = new Helpers.Controls.ValueObjects.ReportView.ParameterClass();

            par2.Name = "POE_No";
            par2.Value = eq.PieceOfEquipmentID;
            par2.Operator = "=";
            par2.ColumnName = "PieceOfEquipmentID";

            List<Helpers.Controls.ValueObjects.ReportView.ParameterClass> lstPar2 = new List<ReportView.ParameterClass>();
            lstPar2.Add(par2);

            parameters.Add("headerParameters", lstPar2);


            //headerParameters

            ModalOptions op = new ModalOptions();
            op.ContentScrollable = true;
            op.Class = "blazored-modal " + Blazed.Controls.ModalSize.FullScreen + " bg-white";

            var messageForm = Modal.Show<BlazorApp1.Blazor.Blazor.DynamicReport>("Report", parameters, op);
            var result = await messageForm.Result;

            if (!result.Cancelled)
            {


            }
        }

        public async Task CustomerChange2(CalibrationSaaS.Domain.Aggregates.Entities.Customer _custId)
        {
            if (Client != null)
            {
                AssetsServiceGRPC assetsServiceGRPC = new AssetsServiceGRPC(_assetsServices, DbFactory);

                _addressFiltered = await assetsServiceGRPC.GetAddressByCustomerId(_custId);


            }
        }


        public async Task CustomerChange(string customerId)
        {
            if (Client != null)
            {
                AssetsServiceGRPC ass = new AssetsServiceGRPC(_assetsServices, DbFactory);

                CalibrationSaaS.Domain.Aggregates.Entities.Customer _custId = new CalibrationSaaS.Domain.Aggregates.Entities.Customer();
                _custId.CustomerID = Convert.ToInt32(customerId);
                _addressFiltered = await ass.GetAddressByCustomerId(_custId);
            }
        }

        public async Task addToListPoE(string ID)
        {
            PieceOfEquipment _poe = new PieceOfEquipment();
            _poe.PieceOfEquipmentID = ID;

            Tenant tenant = new Tenant();

            //var con = await CallOptions(Component.Name);

            //PieceOfEquipmentGRPC poegrpc = new PieceOfEquipmentGRPC(Client, DbFactory, Header);

            var listPoe = await poegrpc.GetPieceOfEquipmentXId(_poe);

            _poe = new PieceOfEquipment()
            {
                PieceOfEquipmentID = ID,
                SerialNumber = listPoe.SerialNumber,
                DueDate = listPoe.DueDate

            };


            _pieceOfEquipmentsFiltered.Add(_poe);

        }

        public async Task ShowModalPoEIndicator()
        {

            if (_listPoEIndicator != null && !string.IsNullOrEmpty(_listPoEIndicator.PieceOfEquipmentID))
            {
                bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "The current indicator will be replaced");
                if (!confirmed)
                {
                    return;
                }
            }
            Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();

            var parameters = new ModalParameters();
            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);
            parameters.Add("Checkbox", false);
            parameters.Add("Indicator", true);
            parameters.Add("CustomerId", _CustomerId);

            ModalOptions op = new ModalOptions();
            ////op.ContentScrollable = true;
            op.Class = "blazored-modal " + Blazed.Controls.ModalSize.MediumWindow;

            var messageForm = Modal.Show<BlazorApp1.Blazor.Pages.AssetsBasics.POEIndicator_Search>("Select Piece Of Equipment", parameters, op);
            var result = await messageForm.Result;
            Console.Write("result " + result);
            if (result != null && !result.Cancelled)
            {


                var res = (PieceOfEquipment)result.Data;

                PieceOfEquipment _poe = new PieceOfEquipment();
                _poe.PieceOfEquipmentID = res.PieceOfEquipmentID;


                Tenant tenant = new Tenant();

                //PieceOfEquipmentGRPC poegrpc = new PieceOfEquipmentGRPC(Client, DbFactory);

                var listPoe = await poegrpc.GetPieceOfEquipmentXId(_poe);

                _listPoEIndicator = listPoe;

                eq.Indicator = _listPoEIndicator;

                StateHasChanged();
            }



            if (!result.Cancelled)
                _message = result.Data?.ToString() ?? string.Empty;

        }

        public async Task DeleteIndicator()
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "The current indicator will be deleted");
            if (!confirmed)
            {
                return;
            }

            _listPoEIndicator = null;
            eq.Indicator = _listPoEIndicator;
            eq.IndicatorPieceOfEquipmentID = null;
        }


#pragma warning disable CS1998
        public async Task AfterAction(RangeTolerance range, string tipo)
#pragma warning restore CS1998
        {

            if (eq.IsToleranceImport == true && ResolutionComponent != null && !Loading)
            {

                List<RangeTolerance> rl = new List<RangeTolerance>();
                var rangecom = ResolutionComponent?.RangeComponent;
                if (rangecom?.RT?.Items?.Count > 0)
                {
                    var re2 = rangecom.RT.Items;
                    rl.AddRange(re2);
                }

                var resolcom = ResolutionComponent?.RangeAccuracy;
                if (resolcom?.RT?.Items?.Count > 0)
                {
                    var re3 = resolcom.RT.Items;
                    rl.AddRange(re3);
                }

                eq.Ranges = rl;

            }

        }

#pragma warning disable CS1998 
        public async Task<bool> Delete(RangeTolerance range)
#pragma warning restore CS1998 
        {

            if (eq.IsToleranceImport == true && ResolutionComponent != null)
            {

                List<RangeTolerance> rl = new List<RangeTolerance>();
                var rangecom = ResolutionComponent?.RangeComponent;
                if (rangecom?.RT?.Items?.Count > 0)
                {
                    var re2 = rangecom.RT.Items;
                    rl.AddRange(re2);

                }

                var resolcom = ResolutionComponent?.RangeAccuracy;
                if (resolcom?.RT?.Items?.Count > 0)
                {
                    var re3 = resolcom.RT.Items;
                    rl.AddRange(re3);
                }

                eq.Ranges = rl;

            }

            return true;

        }

        public async Task ChangeTestCode(ChangeEventArgs args)
        {
            await Typehead.Clear();


        }

        public Task<IEnumerable<TestCode>> SearchTestCode(string searchText)
        {
            var result = TestCodeList.Where(x => x.Code.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0
            || (x.Description != null && x.Description.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
            || x.CalibrationType != null && x.CalibrationType.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0
            || x.Procedure != null && x.Procedure.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0
            );


            return Task.FromResult<IEnumerable<TestCode>>(result);

        }

        public async Task<TestCode> SearchTestCodeSelected(int? code)
        {
            var result = TestCodeList.Where(x => x.TestCodeID == code);


            return result.FirstOrDefault();

        }
        public List<Uncertainty> Uncertainty;
        public async Task CloseUncertainty(ChangeEventArgs arg)
        {
            var a = (ModalResult)arg.Value;

            if (a.Data != null)
            {
                Uncertainty = (List<Uncertainty>)a.Data;

                if (Uncertainty.Count == 0)
                {
                    Uncertainty.Add(new Uncertainty() { IsEmpty = true });
                }

                StateHasChanged();
            }

            if (a.Cancelled)
            {
                Uncertainty = null;
            }
        }




        public string StandardTitle { get; set; } = "Configure Standard Range";


        public EquipmentTemplate et { get; set; } = new EquipmentTemplate();


        public bool GetEnabled()
        {
            // Return the base Enabled property for pricing section
            return Enabled;
        }
        //[Inject] IJSRuntime JSRuntime { get; set; }

        public bool _ImportTestPoint { get; set; }


        async Task ImportTestPoint()
        {
            eq.IsTestPointImport = true;
            _ImportTestPoint = true;
            if (GroupComponent != null && GroupComponent?.Eq.TestGroups != null && GroupComponent.Eq.TestGroups.Count > 0)
            {
                foreach (var item in GroupComponent.Eq.TestGroups)
                {
                    item.TypeID = "POE";
                    item.Type = "POE";
                    item.TestPoitGroupID = 0;
                    item.EquipmentTemplateID = null;
                    Console.WriteLine("ImportTestPoint");
                    Console.WriteLine(GroupComponent.Eq.TestGroups.ElementAtOrDefault(0).TestPoints.Count);
                }
            }

            //eq.TestGroups = GroupComponent.Eq.TestGroups;
            //eq.Capacity = eq.Capacity;
            //eq.UnitofmeasurementID = eq.UnitOfMeasureID.Value;
            //eq.Resolution = eq.Resolution;
            //eq.AccuracyPercentage = eq.AccuracyPercentage;
            //eq.ToleranceTypeID = eq.ToleranceTypeID;

            //StateHasChanged();
        }



        //string _message;
        int _equipmentTemplate;

        public async Task DeleteTolerance()
        {
            if (eq.EquipmentTemplate.Tolerance != null)
            {
                //eq.Resolution = ResolutionComponent.eq.Resolution;
                //eq.DecimalNumber = ResolutionComponent.eq.DecimalNumber;

                eq.Tolerance = new Tolerance();
                //eq.Tolerance.AccuracyPercentage = ResolutionComponent.eq.Tolerance.AccuracyPercentage;

                //eq.Tolerance.ToleranceTypeID = ResolutionComponent.eq.Tolerance.ToleranceTypeID.Value;
                eq.IsToleranceImport = false; // ResolutionComponent.IsToleranceImport;

                eq.Tolerance = eq.EquipmentTemplate.Tolerance;


            }
            eq.TestGroups = null;
            eq.IsTestPointImport = false;
            StateHasChanged();
        }

        public async Task Refresh()
        {
            //ResolutionComponent.eq = (EquipmentTemplate)eq.EquipmentTemplate.CloneObject();

            //ResolutionComponent.Tolerance = (Tolerance)eq.EquipmentTemplate.Tolerance.CloneObject();

            if (ResolutionComponent.Tolerance.ToleranceTypeID.HasValue)
            {
                eq.Resolution = ResolutionComponent.eq.Resolution;

                eq.DecimalNumber = ResolutionComponent.eq.DecimalNumber;
                //eq.Tolerance.ToleranceTypeID = ResolutionComponent.eq.Tolerance.ToleranceTypeID.Value;

                eq.Tolerance = ResolutionComponent.Tolerance;

                eq.Ranges = ResolutionComponent.eq.Ranges;


            }
            eq.IsToleranceImport = ResolutionComponent.IsToleranceImport;
            await ImportTestPoint();

            StateHasChanged();



        }

        public List<string> listBool = new List<string>() { "Choose..", "Y", "N", };
        string selectedString = "Choose..";

        //void selectWeight(ChangeEventArgs e)
        //{
        //    selectedString = e.Value.ToString();
        //    if (selectedString == "Y")
        //    {
        //        eq.IsWeigthSet = true;
        //    }
        //    else
        //    {
        //        eq.IsWeigthSet = false;
        //    }
        //    Logger.LogDebug("It is definitely: " + selectedString);
        //}

        public void selectAccredited(ChangeEventArgs e)
        {
            selectedString = e.Value.ToString();
            if (selectedString == "Y")
            {
                eq.IsForAccreditedCal = true;
            }
            else
            {
                eq.IsForAccreditedCal = false;
            }
            Logger.LogDebug("It is definitely: " + selectedString);
        }



        //[Inject]
        //publicCalibrationSaaS.Application.Services.IBasicsServices<CallContext> Basics { get; set; }

        public async Task ShowModal()
        {
            Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();
            List<EquipmentTemplate> listEquipment;

            var parameters = new ModalParameters();
            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);
            parameters.Add("PermitedEquipmentTypeGroup", PermitedEquipmentTypeGroup);

            ModalOptions op = new ModalOptions();

            op.ContentScrollable = true;
            op.Position = ModalPosition.Center;
            op.Class = "blazored-modal " + ModalSize.MediumWindow;

            var messageForm = Modal.Show<BlazorApp1.Blazor.Pages.Basics.Equipment_Search>("Select Equipment Template", parameters, op);
            var result = await messageForm.Result;

            if (!result.Cancelled)
            {
                await ShowProgress();
                _message = result.Data?.ToString() ?? string.Empty;

                var ET1 = (EquipmentTemplate)result.Data;

                var eqtg = AppState.EquipmentTypeGroups.Where(x => x.EquipmentTypeGroupID == ET1.EquipmentTypeGroupID).FirstOrDefault();

                if (!string.IsNullOrEmpty(eqtg.Children))
                {
                    HasChildren = true;

                    PermitedEquipmentTypeGroup = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(eqtg.Children);

                }

                if (ET1 != null && ET1.EquipmentTemplateID > 0)
                {

                    if (CurrentEquipmentType > 0 && ET1.GetEquipmentTypeId() != CurrentEquipmentType)
                    {
                        await ShowToast("Can't change Equipment Template for a different Equipment Type Template", ToastLevel.Warning);
                        await CloseProgress();
                        return;

                    }

                    ModalParametersET.Add("EntityID", _EquipmentTemplateID.ToString());
                    ModalParametersET.Add("AddFromPoe", true);

                    var con = await CallOptions("");

                    BasicsServiceGRPC bas = new BasicsServiceGRPC(_basicsServices, DbFactory, con.CallOptions);

                    ET = await bas.GetEquipmentByID(ET1);

                    await LoadET(ET);

                }




                await CloseProgress();
            }
            else
            {

                await CloseProgress();

            }


        }



        private void HandleValidSubmit()
        {
            var modelJson = System.Text.Json.JsonSerializer.Serialize(eq, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            JSRuntime.InvokeVoidAsync("alert", $"SUCCESS!! :-)\n\n{modelJson}");
        }

        private void HandleReset()
        {
            eq = new PieceOfEquipment();
            CurrentEditContext = new EditContext(eq);
        }

        public Task<IEnumerable<User>> SearchTechnician(string searchText)
        {
            var result = _userList.Where(x => x.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);


            return Task.FromResult<IEnumerable<User>>(result);
        }



        public async Task CloseET(ChangeEventArgs arg)
        {

            if (arg.Value != null)
            {
                var a = (ModalResult)arg.Value;
                if (a.Data != null)
                {
                    ET = (EquipmentTemplate)a.Data;
                    await LoadET(ET);
                }

            }

            StateHasChanged();
        }

        public async Task CloseAddChild(ChangeEventArgs arg)
        {
            await Children.Refresh();


            StateHasChanged();
        }
        //public async Task ShowModalPoEDueDate()
        //{

        //    if (eq.Customer == null)
        //    {
        //        await ShowError("Select Customer");
        //        return;
        //    }

        //    Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();
        //    //_listPoEDueDate = new List<PieceOfEquipment>();
        //    var parameters = new ModalParameters();
        //    parameters.Add("SelectOnly", true);
        //    parameters.Add("IsModal", true);
        //    parameters.Add("Checkbox", true);
        //    parameters.Add("Customer", eq.Customer);
        //    parameters.Add("pieceOfEquipmentsCh", _listPoEDueDate);
        //    parameters.Add("Parameter1", _listWorkOrderDetail);
        //    parameters.Add("Component", Component);
        //    parameters.Add("TestCodeID", CurrentTestCode.TestCodeID);
        //    ModalOptions op = new ModalOptions();
        //    ////op.ContentScrollable = true;
        //    op.Class = "blazored-modal " + Blazed.Controls.ModalSize.MediumWindow;
        //    //op.UseCustomLayout = true;
        //    //parameters.Add("Indicator", false);
        //    //PieceOfEquipmentDueDate_Search
        //    //var messageForm = Modal.Show<POEWO_Search>("Select Piece Of Equipment", parameters);

        //    var messageForm = Modal.Show<POEWO_MultiSelectPer>("Piece Of Equipment", parameters, op);

        //    var result = await messageForm.Result;
        //    Console.Write("result " + result);
        //    if (result != null && !result.Cancelled)
        //    {
        //        _listPoEDueDate.Clear();
        //        var _listPoEDueDates = (List<PieceOfEquipment>)result.Data;

        //        if (_listPoEDueDates == null)
        //        {
        //            return;
        //        }
        //        foreach (PieceOfEquipment poe in _listPoEDueDates)
        //        {
        //            var asi = _listWorkOrderDetail.Where(x => x.PieceOfEquipmentId == poe.PieceOfEquipmentID).FirstOrDefault();
        //            if (asi == null)
        //            {

        //                _listPoEDueDate.Add(poe);
        //            }
        //            else
        //            {
        //                await ShowToast("Piece of Equipment : " + poe.PieceOfEquipmentID + " already in Order", ToastLevel.Info);
        //            }


        //        }

        //        //StateHasChanged();
        //        // Logger.LogDebug("listapoe " + _listPoEDueDate);
        //        // Logger.LogDebug("Count " + _listPoEDueDate.Count());
        //    }


        //    if (!result.Cancelled)
        //        _message = result.Data?.ToString() ?? string.Empty;


        //    //EmptyValidationDictionary = result.Data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
        //    //      .ToDictionary(prop => prop.Name, prop => prop.GetValue(result.Data, null));

        //}

        public async Task ShowModalPoEDueDate()
        {

            if (eq.Customer == null && _CustomerId == 0)
            {
                await ShowError("Select Customer");
                return;
            }

            Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();

            var parameters = new ModalParameters();
            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);
            parameters.Add("Checkbox", true);
            parameters.Add("Customer", eq.Customer);
            parameters.Add("CustomerId", _CustomerId);
            parameters.Add("pieceOfEquipmentsCh", _listPoEDueDate);
            parameters.Add("Parameter1", _listWorkOrderDetail);
            parameters.Add("Component", Component);

            ModalOptions op = new ModalOptions();
            op.ContentScrollable = true;
            op.Class = "blazored-modal " + ModalSize.MediumWindow;

            var messageForm = Modal.Show<BlazorApp1.Blazor.Pages.AssetsBasics.POEWO_MultiSelectPer>("Piece Of Equipment", parameters, op);

            var result = await messageForm.Result;
            Console.Write("result " + result);
            if (result != null && !result.Cancelled)
            {
                _listPoEDueDate.Clear();
                var _listPoEDueDates = (List<PieceOfEquipment>)result.Data;

                if (_listPoEDueDates == null)
                {
                    return;
                }
                foreach (PieceOfEquipment poe in _listPoEDueDates)
                {
                    var asi = _listWorkOrderDetail.Where(x => x.PieceOfEquipmentId == poe.PieceOfEquipmentID).FirstOrDefault();
                    if (asi == null)
                    {

                        _listPoEDueDate.Add(poe);
                    }
                    else
                    {
                        await ShowToast("Piece of Equipment : " + poe.PieceOfEquipmentID + " already in Order", ToastLevel.Info);
                    }


                }
                if (_listPoEDueDate != null && _listPoEDueDate.Count() > 0)
                {
                    eq.Peripherals = _listPoEDueDate;
                }

            }


            if (!result.Cancelled)
                _message = result.Data?.ToString() ?? string.Empty;


        }


        public async Task ShowModalPoEChildren()
        {

            if (eq.Customer == null)
            {
                await ShowError("Select Customer");
            }

            Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();
            //_listPoEDueDate = new List<PieceOfEquipment>();
            var parameters = new ModalParameters();
            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);
            parameters.Add("Checkbox", true);
            parameters.Add("Customer", eq.Customer);
            //parameters.Add("pieceOfEquipmentsCh", _listPoEDueDate);
            //parameters.Add("Parameter1", _listWorkOrderDetail);
            parameters.Add("Component", Component);

            if (eq?.EquipmentTemplate?.EquipmentTypeGroupID.HasValue == false)
            {
                throw new Exception("PoE not contains Children to configured");
            }

            parameters.Add("EquipmentTypeGroupID", eq.EquipmentTemplate.EquipmentTypeGroupID);



            ModalOptions op = new ModalOptions();
            op.ContentScrollable = true;
            op.Class = "blazored-modal " + ModalSize.MediumWindow;


            var messageForm = Modal.Show<Blazor.Pages.AssetsBasics.POEChildren_Search>("Piece Of Equipment", parameters, op);

            var result = await messageForm.Result;

            Console.Write("result " + result);
            if (result != null && !result.Cancelled)
            {
                _listPoEDueDate.Clear();
                var _listPoEDueDates = (List<PieceOfEquipment>)result.Data;



                foreach (var item in _listPoEDueDates)
                {
                    item.ParentID = eq.PieceOfEquipmentID;

                    Result = await poegrpc.UpdateChildPieceOfEquipment(item);


                }

                await Children.Refresh();

                StateHasChanged();
                //Children code

                //var _poeGrpc = new PieceOfEquipmentGRPC(Client, DbFactory);

                //var g = new CallOptions(await GetHeaderAsync());
                //foreach (PieceOfEquipment poe in _listPoEDueDates)
                //{
                //    var asi = _listWorkOrderDetail.Where(x => x.PieceOfEquipmentId == poe.PieceOfEquipmentID).FirstOrDefault();
                //    if (asi == null)
                //    {
                //        poe.TestCodeID = CurrentTestCode.TestCodeID;
                //        //Add poes childs

                //        _listPoEDueDate.Add(poe);

                //        Pagination<PieceOfEquipment> pag = new Pagination<PieceOfEquipment>();



                //        //pag.SaveCache = false;
                //        pag.Entity = new PieceOfEquipment();
                //        pag.Entity.ParentID = poe.PieceOfEquipmentID;

                //        pag.Show = 1000;

                //        var result2 = (await _poeGrpc.GetPiecesOfEquipmentChildren(pag));

                //        if (result2 != null && result2?.List?.Count > 0)
                //        {
                //            _listPoEDueDateChildren.AddRange(result2.List);
                //        }





                //    }
                //    else
                //    {
                //        await ShowToast("Piece of Equipment : " + poe.PieceOfEquipmentID + " already in Order", ToastLevel.Info);
                //    }


                //}


            }


            if (!result.Cancelled)
                _message = result.Data?.ToString() ?? string.Empty;



        }

        // Pricing Section Support
        protected PricingSection PricingSectionComponent { get; set; }
        protected List<PriceTypePrice> PieceOfEquipmentPrices { get; set; } = new List<PriceTypePrice>();

        // Procedure Association Support
        protected ProcedureAssociationComponent ProcedureAssociationComponent { get; set; }
        protected List<ProcedureEquipment> ProcedureAssociations { get; set; } = new List<ProcedureEquipment>();

        protected async Task OnPricesChanged(List<PriceTypePrice> prices)
        {
            try
            {
                PieceOfEquipmentPrices = prices ?? new List<PriceTypePrice>();
                Logger.LogInformation("Piece of equipment prices updated: {Count} prices", PieceOfEquipmentPrices.Count);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error handling price changes");
            }
        }

        protected async Task SavePieceOfEquipmentPrices()
        {
            try
            {
                if (PricingSectionComponent != null)
                {
                    await PricingSectionComponent.SavePrices();
                    Logger.LogInformation("Piece of equipment prices saved successfully");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error saving piece of equipment prices");
                await ShowToast("Error saving pricing information", ToastLevel.Error);
            }
        }

        protected async Task OnProcedureAssociationsChanged(List<ProcedureEquipment> associations)
        {
            try
            {
                ProcedureAssociations = associations ?? new List<ProcedureEquipment>();
                Logger.LogInformation("Procedure associations updated: {Count} associations", ProcedureAssociations.Count);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error handling procedure association changes");
            }
        }

        protected async Task SaveProcedureAssociations()
        {
            try
            {
                if (ProcedureAssociationComponent != null)
                {
                    await ProcedureAssociationComponent.SaveAssociations();
                    Logger.LogInformation("Procedure associations saved successfully");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error saving procedure associations");
                await ShowToast("Error saving procedure associations", ToastLevel.Error);
            }
        }
        /// <summary>
        /// Helper method to safely convert PieceOfEquipmentID string to int for PricingSection component
        /// </summary>
        protected int GetPieceOfEquipmentIdAsInt()
        {
            try
            {
                if (string.IsNullOrEmpty(eq?.PieceOfEquipmentID))
                    return 0;

                // Try to extract numeric part from alphanumeric ID like "SUP004_77588"
                // For pricing purposes, we can use a hash or just return 0 for new items
                if (eq.PieceOfEquipmentID.Contains("_"))
                {
                    var parts = eq.PieceOfEquipmentID.Split('_');
                    if (parts.Length > 1 && int.TryParse(parts[1], out int numericPart))
                    {
                        return numericPart;
                    }
                }

                // If it's purely numeric, parse it
                if (int.TryParse(eq.PieceOfEquipmentID, out int result))
                {
                    return result;
                }

                // For alphanumeric IDs, return a consistent hash-based integer
                return Math.Abs(eq.PieceOfEquipmentID.GetHashCode());
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Error converting PieceOfEquipmentID to int: {PieceOfEquipmentID}", eq?.PieceOfEquipmentID);
                return 0;
            }
        }





    }
}
