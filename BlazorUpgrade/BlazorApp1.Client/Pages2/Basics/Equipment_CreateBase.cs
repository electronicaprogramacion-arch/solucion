
using AKSoftware.Blazor.Utilities;
using Blazed.Controls.Toast;
using Blazor.Extensions.Logging;
using Blazor.IndexedDB.Framework;
using BlazorApp1.Blazor.Shared;


using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using BlazorApp1.Blazor.Blazor.Helper;
using BlazorApp1.Blazor.Blazor.Pages.AssetsBasics;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using Reports.Domain.ReportViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalibrationSaaS.Infraestructure.Blazor.Components;

namespace BlazorApp1.Blazor.Blazor.Pages.Basics
{
    public class Equipment_CreateBase :
        Base_Create23<EquipmentTemplate, CalibrationSaaS.Application.Services.IBasicsServices<CallContext>, CalibrationSaaS.Domain.Aggregates.Shared.Basic.AppState>
    {



        public EquipmentType EquipmentTypeObject { get; set; }

        public ICollection<EquipmentTypeGroup> EquipmentTypeGroup { get; set; }

        [Inject]
        public Func<dynamic, CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>> Client2 { get; set; }

        public object element;

        public TestPointGroup GroupVar = new TestPointGroup();

        //public List<Option<string>>? selectedStrings = new List<Option<string>>();

        //public IEnumerable<Option<string>>? selectedStrings2 = new List<Option<string>>();

        public List<RangeTolerance> Ranges = new List<RangeTolerance>();

        public Client.Pages.Basics.TestPoints.GroupTestPoint2 GroupComponent;

        //public RangeTestPoint RangeComponent;

        //public RangeTestPoint RangeAccuracy;

        public BlazorApp1.Blazor.Blazor.Pages.Basics.ResolutionComponent ResolutionComponent;

        public string selectedAnswer = "";

        public bool IsPercentage = false;

        public BlazorApp1.Blazor.Blazor.Pages.AssetsBasics.POEByEqTemplate_Search PieceOfEquipmentSearch = new BlazorApp1.Blazor.Blazor.Pages.AssetsBasics.POEByEqTemplate_Search();
        //public int MyProperty { get; set; }

        public ICollection<PieceOfEquipment> _listPieceOfEquipment = new List<PieceOfEquipment>();

        public CalibrationTypeComponent<EquipmentTemplate, GenericCalibration2, GenericCalibrationResult2> CalibrationTypeComponent { get; set; }

        [Parameter]
        public bool? AddFromPoe { get; set; }

        [Inject] public CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext> _poeServices { get; set; }
        public ModalParameters ModalParameters = new ModalParameters();

        public string activeId = "accordion-1";

        public bool OpenAditional { get; set; }
        
        //FluentAccordionItem? changed;
        //public void HandleOnAccordionItemChange(FluentAccordionItem item)
        //{
        //    //if (IsNew)
        //    //{
        //    //    return;
        //    //}

        //    changed = item;
        //    if (changed.Class == "acco7" && !OpenAditional)
        //    {
        //        OpenAditional = true;
        //    }
        //    else
        //    {
        //        OpenAditional = false;
        //    }
        //}

        private string persistentValue;


        public bool IsNew { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
                                                                                                       
            try
            {


                if (firstRender)
                {
                    
                  

                    await JSRuntime.InvokeVoidAsync("topaccordion");


                    NameValidationMessage = "valid";

                    if (string.IsNullOrEmpty(EntityID))
                    {

                        EntityID = "0";

                    }



                        if (EntityID == "0" )
                    {
                        
                        eq = new EquipmentTemplate();
                        eq.EquipmentTypeObject = new EquipmentType();
                        eq.EquipmentTemplateID= NumericExtensions.GetUniqueID(eq.EquipmentTemplateID);
                        eq.TestPointResult = new List<GenericCalibrationResult2>();
                        EntityID= eq.EquipmentTemplateID.ToString();
                        //OpenAditional = true;
                        //IsNew = true;

                    }
                    
                    else
                    {
                        eq.EquipmentTemplateID = Convert.ToInt32(EntityID);

                       
                        //Logger.LogDebug("EntityID");

                        //Logger.LogDebug(EntityID);

                        BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);


                        await ShowProgress();

                        //var result = await poegrpc.GetDynamicConfiguration(ct); //.ConfigureAwait(false).GetAwaiter().GetResult();

                        eq = await basics.GetEquipmentByID(eq);//Client.GetEquipmentByID(eq);//AppState.Equipments.Where(x => x.EquipmentTemplateID == Convert.ToInt32(EntityID)).FirstOrDefault();

                        var con = await CallOptions(Component.Name);
                        PieceOfEquipmentGRPC poegrpc = new PieceOfEquipmentGRPC(Client2, DbFactory, con.CallOptions);
                        var result = await poegrpc.GetDynamicConfiguration(new CalibrationType() { CalibrationTypeId=eq.EquipmentTypeObject.CalibrationTypeID});

                        eq.EquipmentTypeObject.CalibrationType = result;

                        EquipmentTypeObject = eq.EquipmentTypeObject;

                        if (eq != null)
                        {
                            var res = await poegrpc.GetPieceOfEquipmentByET(eq);


                            //     Uncertainty u = new Uncertainty()
                            //{
                            //    EquipmentTemplateID = eq.EquipmentTemplateID
                            //};

                            //Uncertainty=await _poeServices.GetUncertainty(u, new CallContext());

                            if (res.PieceOfEquipments != null)
                            {
                                _listPieceOfEquipment = res.PieceOfEquipments; //eq.PieceOfEquipments;
                                PieceOfEquipmentSearch.LIST1 = _listPieceOfEquipment.ToList();
                                PieceOfEquipmentSearch.Show(_listPieceOfEquipment.ToList());
                            }
                        }


                        var resultdec = NumericExtensions.CalculateDecimalNumber(Convert.ToDecimal(eq.Resolution));
                        eq.DecimalNumber = (int)resultdec;


                    }


                    if (eq != null)
                    {
                        if ( eq?.Tolerance?.ToleranceTypeID == 2)
                        {
                            IsPercentage = true;
                        }

                        //if (eq.ToleranceTypeID == 0)
                        //{
                        //    eq.ToleranceTypeID = 1;
                        //    IsPercentage = false;
                        //}


                        if (eq.TestGroups != null && eq.TestGroups.Count > 0)
                        {
                            var selectgroup = eq.TestGroups.ElementAt<TestPointGroup>(0);
                            GroupVar = selectgroup;

                        }
                        else
                        {
                            List<TestPointGroup> gr = new List<TestPointGroup>();

                            TestPointGroup tpg = new TestPointGroup();

                            //tpg.EquipmentTemplateID = eq.EquipmentTemplateID;

                            //if ((!IsOnline || 1 == 1) && tpg.TestPoitGroupID == 0)
                            //{
                                tpg.TestPoitGroupID = NumericExtensions.GetUniqueID(tpg.TestPoitGroupID);
                            //}



                            gr.Add(tpg);

                            eq.TestGroups = gr;

                            GroupVar = eq.TestGroups.ElementAt<TestPointGroup>(0);


                        }


                        if (eq.Ranges == null)
                        {
                            eq.Ranges = new List<RangeTolerance>();

                            Ranges = eq.Ranges.ToList();
                        }
                        else
                        {
                            Ranges = eq.Ranges.ToList();
                        }

                        CurrentEditContext = new EditContext(eq);
                        //Logger.LogDebug("eq valor");
                        //Logger.LogDebug(eq.ManufacturerID);
                    }

                    StateHasChanged();

                }

                if (!Saving && Ranges != null && ResolutionComponent != null && eq != null)
                {

                    var eqq = eq;
#pragma warning disable CS4014 // Como esta llamada no es 'awaited', la ejecuci�n del m�todo actual continuar� antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
                    ResolutionComponent.Show(eqq);
#pragma warning restore CS4014 // Como esta llamada no es 'awaited', la ejecuci�n del m�todo actual continuar� antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
                }



                if (!Saving && GroupVar != null && GroupComponent != null)
                {
                    //GroupComponent.Show(GroupVar);
                }

                ModalParameters.Add("EquipmentTemplateID", eq.EquipmentTemplateID);
            }
            catch(Exception ex) {

                await ExceptionManager(ex);
            }

            finally
            {
                await CloseProgress();
            }
            //await base.OnParametersSetAsync();
           

        }

        public BlazorApp1.Blazor.Shared.UncertaintyComponent UncertaintyComponent { get; set; }

        private List<Uncertainty> Uncertainty;



        


        public async Task ChangeControlGroup(ChangeEventArgs arg, dynamic lin, bool AddWeight = false)
        {
            if (arg.Value == null)
            {

                return;
            }

            var id = (int)arg.Value;

            eq.EquipmentTypeID = id;
            //eq.EquipmentType = (selectedStrings != null ? string.Join(',', selectedStrings.Select(o => o.Value)) : "");

            //if (selectedStrings != null && selectedStrings.Count > 0)
            //{
            //    eq.EquipmentTypeID = Convert.ToInt32(selectedStrings[0].Value);
            //}


            var c = eq.GetEquipmentTypeId();

            await GetEquipmentType(c);

            EquipmentTypeObject = eq.EquipmentTypeObject;


        }

        public async Task GetEquipmentType(int? c,bool group=false)
        {


            group = true;
            try
            {
                await ShowProgress();

                //var c = eq.GetEquipmentTypeId();
                //Convert.ToInt32(arg.Value);
                List<EquipmentType> a = null;

                if (!group)
                {
                    a = AppState.EquipmentTypes.Where(x => x.EquipmentTypeID == c).ToList();
                }
                else
                {
                    a = AppState.EquipmentTypes.Where(x => x.EquipmentTypeGroupID == c).ToList();
                }
                if (eq.AditionalEquipmentTypes == null)
                {

                    eq.AditionalEquipmentTypes  = new List<EquipmentType>();
                }


                var con = await CallOptions(Component.Name);

                PieceOfEquipmentGRPC poegrpc = new PieceOfEquipmentGRPC(Client2, DbFactory, con.CallOptions);
                //var result = await poegrpc.GetDynamicConfiguration(ct); //.ConfigureAwait(false).GetAwaiter().GetResult();
                if(a != null && a.Count > 0)
                {
                    foreach (var b in a.DistinctBy(x => x.CalibrationTypeID))
                    {
                        var per = AppState.EquipmentTypes.Where(x => x.CalibrationTypeID == b.CalibrationTypeID).FirstOrDefault();

                        if (per != null && per?.CalibrationType?.CalibrationSubTypes?.Count > 0 && per?.CalibrationType?.CalibrationSubTypes.Where(x=>x.CalibrationSubTypeView==null).FirstOrDefault()==null)
                        {
                            b.CalibrationType = per.CalibrationType;

                        }


                        if (b?.CalibrationType?.CalibrationSubTypes == null
                            || b.CalibrationType?.CalibrationSubTypes?.Count == 0
                            || b.CalibrationType?.CalibrationSubTypes?.Where(x => x.DynamicPropertiesSchema == null).FirstOrDefault()!=null
                            )
                        {
                            CalibrationType ct = new CalibrationType();

                            ct.CalibrationTypeId = b.CalibrationTypeID;

                            var result = await poegrpc.GetDynamicConfiguration(ct); //.ConfigureAwait(false).GetAwaiter().GetResult();

                            b.CalibrationType = result;
                        }                       

                        eq.AditionalEquipmentTypes.Add(b);
                    }


                    foreach (var c23 in a.DistinctBy(x=>x.EquipmentTypeID))
                    {
                        eq.AditionalEquipmentTypes.Add(c23);
                    }


                }

                EquipmentTypeObject = eq.EquipmentTypeObject;
                //eq.EquipmentTypeObject = b;

                StateHasChanged();
            }
            catch(Exception ex) 
            {
                await ExceptionManager(ex);
                Console.WriteLine(ex.Message);
            }
            finally
            {

                await CloseProgress();  
            }
           

        }
        protected async Task FormSubmitted(EditContext editContext)
        {

            bool re = await ContextValidation(true);
            try
            {
               

                if (re)
                {
                    

                    var con = await CallOptions(Component.Name);

                    BasicsServiceGRPC basics = new BasicsServiceGRPC(Client,con.CallOptions);

                    var msg = ValidationMessages;

                    if (eq?.TestPointResult?.Count > 0)
                    {
                        foreach (var itemres in eq?.TestPointResult)
                        {
                            if (itemres?.GenericCalibration2?.TestPointResult != null)
                            {
                                itemres.GenericCalibration2.TestPointResult = null;
                            }

                        }
                    }

                    var equipmentExist = await basics.GetEquipmentXName(eq, new CallContext());

                    //if (equipmentExist != null && equipmentExist.Model != null)
                    //{
                    //    await ShowError("There is already a Equipment with the same Manufacturer and Model");
                    //    return;
                    //}

                    eq.AddFromPoe = AddFromPoe;

                    if (eq.EquipmentTypeObject != null && eq.EquipmentTypeObject.HasTolerance)
                    {
                        if (!eq.Tolerance.ToleranceTypeID.HasValue || eq.Tolerance.ToleranceTypeID == 0 || !eq.UnitofmeasurementID.HasValue
                            || eq.UnitofmeasurementID == 0 || eq.Capacity == 0)
                        {
                            await ShowToast("Review Capacity or Tolerance Values", ToastLevel.Warning);

                        }
                    }
                    else
                    {
                        //eq.Capacity = 0;
                        // eq.Tolerance.ToleranceTypeID = null;
                        //eq.UnitofmeasurementID = null;

                    }

                    if (eq.EquipmentTypeObject != null && !eq.EquipmentTypeObject.HasCapacity)
                    {
                        eq.Capacity = 0;
                        eq.UnitOfMeasure = null;
                        eq.UnitofmeasurementID = null;


                    }


                    if ((eq.EquipmentTypeObject != null && eq.EquipmentTypeObject.HasTestpoint && GroupComponent == null) || ((GroupComponent?.RT == null || GroupComponent?.RT?.Items == null
                       || GroupComponent?.RT?.Items?.Count == 0)
                       && eq.EquipmentTypeObject.HasTestpoint))
                    {

                        await ShowToast("No Test Points Configurated", ToastLevel.Warning);
                        eq.TestGroups = null;

                        //return ;
                    }
                    else if (eq.EquipmentTypeObject != null && eq.EquipmentTypeObject.HasTestpoint)
                    {
                        //llena la propiedad base creo que no va
                        if (GroupComponent?.RT?.Items?.Count >= 0)
                        {
                            List<TestPoint> listtestpoint;
                            if (GroupComponent?.RT.IsDragAndDrop == false)
                            {
                                listtestpoint = GroupComponent.RT.Items;
                            }
                            else
                            {
                                listtestpoint = GroupComponent.RT.ItemList;
                            }

                            var group = GroupComponent.Group;




                            if (eq.TestGroups != null)
                            {


                                foreach (var item in eq.TestGroups)
                                {
                                    //foreach (var item2 in listtestpoint)

                                    listtestpoint.ForEach(item2 =>
                                    {
                                        //ChangeEventArgs arg = new ChangeEventArgs();
                                        //arg.Value = item.OutUnitOfMeasurementID.ToString();
                                        //Calculate.Conversion<UnitOfMeasure>(item2.UnitOfMeasurementOut, arg, AppState.UnitofMeasureList, nameof(item2.UnitOfMeasurement.UnitOfMeasureID));
                                        //item2.UnitOfMeasurementID = item2.UnitOfMeasurement.UnitOfMeasureID;

                                        //item2.UnitOfMeasurementOutID = item.OutUnitOfMeasurementID;

                                        //item2.UnitOfMeasurementID = item2.UnitOfMeasurement.UnitOfMeasureID;
                                        item2.UnitOfMeasurement = null;
                                        item2.UnitOfMeasurementOut = null;

                                        Console.WriteLine(item2.NominalTestPoit);

                                    });
                                }



                            }
                            group.TestPoints = listtestpoint;

                            eq.TestGroups = new List<TestPointGroup>();

                            eq.TestGroups.Add(group);
                        }
                    }

                    //try
                    //{

                  



                    List<RangeTolerance> rl = new List<RangeTolerance>();

                    if (!eq.Tolerance.ToleranceTypeID.HasValue)
                    {
                        eq.Tolerance.ToleranceTypeID = 0;
                    }
                    ////////Ranges
                    ///
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

                    if (eq.EquipmentTypeObject.Name == null)
                        eq.EquipmentTypeObject.Name = "";

                    await ShowProgress();

                    Result = (await basics.CreateEquipment(eq));

                    PieceOfEquipmentGRPC poegrpc = new PieceOfEquipmentGRPC(Client2, DbFactory, con.CallOptions);
                    var result = await poegrpc.GetDynamicConfiguration(new CalibrationType() { CalibrationTypeId = eq.EquipmentTypeObject.CalibrationTypeID });


                    eq = Result;


                    eq.EquipmentTypeObject.CalibrationType = result;


                    if (Result == null)
                    {
                        throw new Exception("Server validation Error");
                    }

                    if (Result.EquipmentTemplateID == 0)
                    {
                        await ShowToast("Thread combination not configured", ToastLevel.Error);
                        throw new Exception();
                    }

                    

                    if (Result?.TestGroups?.ElementAtOrDefault(0) != null)
                    {
                        GroupVar = Result.TestGroups.ElementAtOrDefault(0);
                    }


                    if (UncertaintyComponent != null
                        && UncertaintyComponent.Grid.TableChanges.HasChanges())
                    {
                        //Uncertainty = UncertaintyComponent.Grid.;

                        //if (Uncertainty.Count == 0)
                        //{
                        //    Uncertainty.Add(new Uncertainty() { IsEmpty = true });
                        //}


                        foreach (var item in UncertaintyComponent.Grid.TableChanges.DeleteList)
                        {
                            item.EquipmentTemplateID = Result.EquipmentTemplateID;
                        }

                        foreach (var item in UncertaintyComponent.Grid.TableChanges.AddList)
                        {
                            item.EquipmentTemplateID = Result.EquipmentTemplateID;
                        }


                        var unc = await _poeServices.CreateUncertainty(UncertaintyComponent.Grid.TableChanges, new CallOptions());

                        UncertaintyComponent.Grid.TableChanges.Clear();
                    }

                    // Save pricing information for Thermotemp customers
                    try
                    {
                        await SaveEquipmentTemplatePrices();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "Error saving equipment template prices, but equipment was saved successfully");
                        // Don't fail the entire operation if pricing save fails
                    }

                    await ShowToast("The information has been saved successfully.", ToastLevel.Success);

                    await CloseModal(Result);


                }
                else
                {

                    await CloseProgress();

                }
            }
            catch (RpcException ex)
            {

                await ExceptionManager(ex);

            }
            catch (Exception ex)
            {
                await ShowError("Error Save " + ex.Message);
            }
          
            finally
            {
                await CloseProgress();
            }





        }

#pragma warning disable CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected async Task InvalidFormSubmitted(EditContext editContext)
#pragma warning restore CS1998 // El m�todo asincr�nico carece de operadores "await" y se ejecutar� de forma sincr�nica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            LastSubmitResult = "OnInvalidSubmit was executed";
        }


        protected override async Task OnInitializedAsync()
        {

            //await base.OnParametersSetAsync();
            await base.OnInitializedAsync();

            TypeName = "Equipment";

            FormName = "EquipmentTemplate";


            //Load Parametric Tables
            if (AppState?.EquipmentTypes?.Count == 0)
            {
                var etypes = await Client.GetEquipmenTypes(new CallOptions());

                foreach (var item in etypes.EquipmentTypes)
                {
                    AppState.AddEquipmentType(item);
                }
                //EquipmentTypes = etypes.EquipmentTypes;
                
            }
            else
            {


            }

            //if (AppState.Manufacturers.Count == 0)
            //{
            var eManufactures = await Client.GetAllManufacturers(new CallOptions());

            foreach (var item in eManufactures.Manufacturers)
            {
                AppState.AddManufacturer(item);
            }

            //Manufacturers = eManufactures.Manufacturers;
            //}

            //StatusList = AppState.StatusList;

            //if (StatusList == null)
            //{

            //    Logger.LogDebug("es null");
            //}
            //Logger.LogDebug(StatusList.Count);

            //CurrentEditContext.OnFieldChanged += HandleFieldChanged;
            //StateHasChanged();
            NameValidationMessage = "valid";


            //  MessagingCenter.Subscribe<CalibrationSaaS.Infraestructure.Blazor.LTI.UncertaintyComponent, Uncertainty>(this, "item_added", (sender, args) =>
            //{
            //    // Recaulcate the items in the cart 
            //    //_cartItems = GetCartListFromCart();

            //    // Notify the UI about the change
            //    StateHasChanged(); 
            //});



        }


        protected void FunctionName(EventArgs args)
        {
            //PropertyInfo myPropInfo = eq.GetType().GetProperty("MyProperty");

            if (eq.Name.Length < 1)
            {
                NameValidationMessage = "invalid";
            }
            else
            {
                NameValidationMessage = "valid";
            }
            //Logger.LogDebug(this.eq.Name);
            StateHasChanged();

        }



        public new void Dispose()
        {

        }


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

            if (!a.Cancelled)
            {

            }
        }

        public async Task<List<GenericCalibrationResult2>> DynamicResult(List<GenericCalibrationResult2> result, string CalibrationSubType)
        {
            if (eq?.TestPointResult== null) 
            {
                eq.TestPointResult= new List<GenericCalibrationResult2>();


            }
           // CalibrationSubType = result.ToList().FirstOrDefault().CalibrationSubTypeId.ToString();
            if (string.IsNullOrEmpty(CalibrationSubType))
            {
                if (eq?.TestPointResult != null)
                {
                    eq.TestPointResult.Clear();

                    eq.TestPointResult.AddRange(result);
                }


                    
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
                    if (eq.TestPointResult != null)
                    {
                        eq.TestPointResult.Clear();

                        eq.TestPointResult.AddRange(result);
                    }
                }


            }



            //eq.TestPointResult = result;

            return eq.TestPointResult;

        }






        public void ChangeUOM(ChangeEventArgs arg)
        {
            StateHasChanged();
        }


        public void KeyHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                return;
            }
        }


        public void DecimalCalculate(ChangeEventArgs args)
        {
            try
            {
                var val = args.Value;
                if (val != null)
                {
                    var result = NumericExtensions.CalculateDecimalNumber(Convert.ToDecimal(val));
                    eq.DecimalNumber = (int)result;
                }
            }
            catch (Exception ex)
            {
            }
        }






        public void IDisposable(ChangeEventArgs args)
        {


            selectedAnswer = args.Value.ToString();

            Logger.LogDebug(selectedAnswer);

            if (selectedAnswer.Contains("2") || selectedAnswer.Contains("3"))
            {
                IsPercentage = true;

                eq.Tolerance.ToleranceTypeID = 2;
            }
            if (selectedAnswer.Contains("3"))
            {
                IsPercentage = true;

                eq.Tolerance.ToleranceTypeID = 3;
            }


            else if (selectedAnswer.Contains("1"))
            {
                IsPercentage = false;
                eq.Tolerance.ToleranceTypeID = 1;

            }
            else if (selectedAnswer.Contains("4"))
            {
                IsPercentage = false;
                eq.Tolerance.ToleranceTypeID = 4;

            }
            else if (selectedAnswer.Contains("5"))
            {
                IsPercentage = false;
                eq.Tolerance.ToleranceTypeID = 5;

            }

            StateHasChanged();
        }

        // Pricing Section Support
        protected PricingSection PricingSectionComponent { get; set; }
        protected List<PriceTypePrice> EquipmentTemplatePrices { get; set; } = new List<PriceTypePrice>();

        protected async Task OnPricesChanged(List<PriceTypePrice> prices)
        {
            try
            {
                EquipmentTemplatePrices = prices ?? new List<PriceTypePrice>();
                Logger.LogInformation($"Equipment template prices updated: {EquipmentTemplatePrices.Count} prices");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error handling price changes");
            }
        }

        protected async Task SaveEquipmentTemplatePrices()
        {
            try
            {
                if (PricingSectionComponent != null)
                {
                    await PricingSectionComponent.SavePrices();
                    Logger.LogInformation("Equipment template prices saved successfully");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error saving equipment template prices");
                await ShowToast("Error saving pricing information", ToastLevel.Error);
            }
        }


    }
}
