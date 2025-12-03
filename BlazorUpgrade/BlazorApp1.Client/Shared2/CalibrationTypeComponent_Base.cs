using Blazed.Controls;
using Blazed.Controls.MultiComponent;
using Blazed.Controls.Toast;
using Blazor.IndexedDB.Framework;
using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics.TestPoints;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Order;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Helpers;
using Helpers.Controls;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using Modal = Blazed.Controls.Modal;
using Type = System.Type;


namespace BlazorApp1.Blazor.Shared
{
    //WorkOrderItem
    //PieceOfEquipmentCreate
    public partial class CalibrationTypeComponent<Entity,CalibrationItem, CalibrationItemResult> : FComponentBase 
        where Entity: class, IResolution, CalibrationSaaS.Domain.Aggregates.Interfaces.IConfiguration,ITolerance ,
        IGenericCalibrationCollection<GenericCalibrationResult2>,
        IUncetainty,
        new()
        where CalibrationItem : class, IResultComp, IGenericCalibrationSubTypeCollection<GenericCalibrationResult2>, IResultTesPointGroup, new()
        where CalibrationItemResult : class, IResultComp, IResultGen2, IResult2, IDynamic, IUpdated, ISelect, IResultTesPointGroup, new()

    {



        public RadzenTabs tabs;
        public int selectedIndex = 0;

        [Parameter]
        public bool? RuleValue { get; set; }

        [Parameter]
        public bool Enabled { get; set; }

        [Parameter]
        public string ParentAccordionID { get; set; } = "accordionDynamic";

        public int CurrentSubTypes { get; set; }

        public int NumSubTypes { get; set; }


        public int ExistsCalibration { get; set; }

        public int NumGroup { get; set; }

        public int MaxNumberCopies { get; set; } = 160;

        [Inject] public CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext> _poeServices { get; set; }


        [CascadingParameter(Name = "CascadeParam3")]
        public Entity DomainEntityProp { get; set; }



        [CascadingParameter(Name = "CascadeParam2")]
        public ICalibrationType eq { get; set; }

        public EventCallback<ChangeEventArgs> OnChangeDescription { get; set; }

        [Parameter]
        public string GetGroupsProperty { get; set; }


        [Parameter]
        public bool ChangeResolution { get; set; }

        [Parameter]
        public bool HasBeenCompleted { get; set; }

        [Parameter]
        public CalibrationType cmcValues { get; set; }

        [CascadingParameter(Name = "CurrentStatusId")]
        public int CurrentStatusId { get; set; }

        public List<CalibrationSubType> TestPointGroupsList { get; set; }

        //public List<string> TestPointGroupsListS { get; set; }


        public Dictionary<string, List<string>> DicTestPointGroupsListS { get; set; } = new Dictionary<string, List<string>>();

        [Parameter]
        public string TestPointGroupsWod { get; set; }

        /// <summary>
        /// value set by user before create a new TestPointGroup, if this text is empty the name was "TestPointResult" + Sequence
        /// </summary>
        public string TestPointGroupName { get; set; }

        public static IJSInProcessRuntime JSInProcRuntime { get; set; }


        public bool MustBeReload { get; set; }






        protected override async Task OnInitializedAsync()
        {

            JSInProcRuntime = Program.JSInProcRuntime;
            await base.OnInitializedAsync();

            await CreateGrids();




        }


        [Parameter]
        public bool IsInit { get; set; }

        public bool Init { get; set; }
        public void InitComponent(CalibrationSubType _CalibrationSubType)
        {


            if (_CalibrationSubType.CreateGroups.HasValue == false || _CalibrationSubType.CreateGroups.Value == false || DicTestPointGroupsListS.Count > 0)
            {
                return;
            }


            List<GenericCalibrationResult2> TestPointResult = null;

            List<string> TestPointGroupsListS = null;


            //DicTestPointGroupsListS = new Dictionary<string, List<string>>();
            

            if (!string.IsNullOrEmpty(TestPointGroupsWod))
            {
                var selectedValues = System.Text.Json.JsonSerializer.Deserialize<List<string>>(TestPointGroupsWod);

                if (selectedValues != null && selectedValues.Count > 0)
                {
                    selectedValues= selectedValues.Where(x => x != "TestPointResult").ToList();
                }


                    if (selectedValues != null && selectedValues.Count > 0)
                {
                    TestPointGroupsListS = selectedValues;


                    if (DicTestPointGroupsListS.ContainsKey(_CalibrationSubType.CalibrationSubTypeId.ToString()))
                    {
                        DicTestPointGroupsListS[_CalibrationSubType.CalibrationSubTypeId.ToString()] = TestPointGroupsListS;
                    }
                    else
                    {
                        DicTestPointGroupsListS.Add(_CalibrationSubType.CalibrationSubTypeId.ToString(), TestPointGroupsListS);
                    }


                    return;
                }
            }

            if (DomainEntityProp.TestPointResult != null && DomainEntityProp.TestPointResult.Count > 0)
            {
                TestPointResult = DomainEntityProp.TestPointResult;
            }
            else
           if (GetGroupsProperty == "EquipmentTemplate")
            {
                dynamic tesp = DomainEntityProp;

                TestPointResult = tesp?.EquipmentTemplate.TestPointResult;
            }
            else if (GetGroupsProperty == "PieceOfEquipment")
            {
                dynamic tesp = DomainEntityProp;

                TestPointResult = tesp?.PieceOfEquipment?.TestPointResult;

                if (TestPointResult == null)
                {
                    TestPointResult = tesp?.PieceOfEquipment.EquipmentTemplate.TestPointResult;
                }
            }

            if (TestPointResult != null)
            {
                var testpoints = TestPointResult.DistinctBy(x => x.GroupName).Where(x => x.GroupName != "TestPointResult").ToList();

                //List<GenericCalibrationResult2> testpoints = new List<GenericCalibrationResult2>(); //TestPointResult.DistinctBy(x => x.GroupName ).Where(x=> x.CalibrationSubTypeId == Convert.ToInt32(CalibrationSubType)).ToList();


                //var distinctItems = TestPointResult.GroupBy(x => new { x.GroupName, x.CalibrationSubTypeId })
                //                  .Select(g => g.First())
                //                  .ToList();

                //foreach (var item in distinctItems)
                //{
                //    testpoints.Add(item);
                //}
                

                TestPointGroupsListS = new List<string>();

                foreach (var item in testpoints)
                {

                    TestPointGroupsListS.Add(item.GroupName);

                }
                MustBeReload = false;

                if(DicTestPointGroupsListS.ContainsKey(_CalibrationSubType.CalibrationSubTypeId.ToString()))
                {
                    DicTestPointGroupsListS[_CalibrationSubType.CalibrationSubTypeId.ToString()]=TestPointGroupsListS;
                }
                else
                {
                    DicTestPointGroupsListS.Add(_CalibrationSubType.CalibrationSubTypeId.ToString(), TestPointGroupsListS);
                }
                    

            }
            else
            {
                //TestPointGroupsListS= new List<string>();

                //TestPointGroupsListS.Add("TestPointResult");


                //if (DicTestPointGroupsListS.ContainsKey("TestPointResult"))
                //{
                //    DicTestPointGroupsListS[_CalibrationSubType.CalibrationSubTypeId.ToString()] = TestPointGroupsListS;
                //}
                //else
                //{
                //    DicTestPointGroupsListS.Add(_CalibrationSubType.CalibrationSubTypeId.ToString(), TestPointGroupsListS);
                //}

                
            }
        }



        [Parameter]
        public Func<ICalibrationType,int, Task> NewCalibrationFunc { get; set; }

        [Parameter]
       public Func<List<GenericCalibrationResult2>, string, Task<List<GenericCalibrationResult2>>> ResultCalibration { get; set; }


        [Parameter]
        public Func<Task> SaveAction { get; set; }


        public bool Closing { get; set; }

        ///// <summary>
        ///// YP 
        ///// </summary>
        //public string? GroupName { get; set; }

        /// <summary>
        /// Get calibration results, Grid results
        /// </summary>
        /// <returns></returns>
        public async Task CloseGenericWindow()
        {
            try
            {

                // await ShowProgress();

                await CloseGenericWindow2();
                Closing = true;


            }
            finally
            {

                //Closing = false;
                //await CloseProgress();

            }
        }


        /// <summary>
        /// Get calibration results, Grid results
        /// </summary>
        /// <returns></returns>
        public async Task UpdateData(string CalibrationSubType)
        {
            try
            {
                // await ShowProgress();

                //await CloseGenericWindow2(CalibrationSubType);


            }
            finally
            {


                //await CloseProgress();

            }
        }





        public  Task CloseGenericWindow2(string CalibrationType= "")
        {
           

            List<GenericCalibrationResult2> list = new List<GenericCalibrationResult2>();
            List<GenericCalibrationResult2> list2 = new List<GenericCalibrationResult2>();

            var Selectlist = new List<GenericCalibrationResult2>();

            List<dynamic> gridss = new();

            if (!string.IsNullOrEmpty(CalibrationType))
            {
                foreach (var itemxx in _refs)
                {
                    if(itemxx.IdGroup== CalibrationType)
                    {
                        gridss.Add(itemxx);
                    }
                }
            }
            else
            {
                gridss = _refs;
            }
            //var refsd= item.CalibrationSubType


            foreach (var item in gridss)
            {


                if (item != null 
                    && (string.IsNullOrEmpty(item.SelectValue) || item.SelectValue=="-1")  
                    && item?.tableListResult != null 
                    && item?.tableListResult?.Count > 0 
                    && (item?.RT == null || item?.RT?.Items?.Count == 0))
                {
                    
                    var lista = item.tableListResult;
                    list.AddRange(lista);



                }
               if (item != null && item.SelectValue != "-1" && item?.tableListResult != null
                   && item?.tableListResult?.Count > 0
                   && (item?.RT == null || item?.RT?.Items?.Count == 0))
                {

                    var lista22= new List<GenericCalibrationResult2>();

                    lista22.Add(item.SelectRow);

                    Selectlist.AddRange(lista22);

                    var lista = item.tableListResult;
                    list.AddRange(lista);



                }
                else if(item.SelectRow != null )
                {
                    if (item != null && item?.RT != null
                        && item?.RT?.Items?.Count > 0)
                    {
                        var lista = item.RT.Items;
                        var lista2 = new List<GenericCalibrationResult2>();
                        foreach (var itemsele in lista)
                        {
                            if (itemsele.Position == item.SelectPosition)
                            {


                                lista2.Add(itemsele);

                                
                            }
                           

                        }

                        Selectlist.AddRange(lista2); 

                        list.AddRange(lista);

                    }

                }

                else
                {
                    if (item != null && item?.RT != null 
                        && item?.RT?.Items?.Count > 0)
                    {
                        var lista = item.RT.Items;

                        list.AddRange(lista);
                    }
                   
                }




                if (item != null && item?.NewItem != null)
                {
                    var item23 = item.NewItem;

                    item23.Position = -1;

                    list.Add(item23);
                }


            }




            if (list != null)
            {

                foreach (var item in list)
                {
                    if (item?.GenericCalibration2 != null)
                    {
                        item.GenericCalibration2.CalibrationSubTypeId = item.CalibrationSubTypeId;
                        item.GenericCalibration2.SequenceID = item.SequenceID;
                        item.GenericCalibration2.Component = item.Component;
                        item.GenericCalibration2.ComponentID = item.ComponentID;
                        item.GenericCalibration2.TestPointResult = null;
                    }

                    Console.WriteLine($"DEBUG: Item Object content: {item.Object}");

                    GenericCalibrationResult2 genericCalibration2 = null; 
 
                    if (Selectlist?.Count > 0)
                    {
                        // Filter by Position, CalibrationSubTypeId, and GroupName to avoid cross-clone data sharing
                        genericCalibration2 = Selectlist.Where(x => x.Position == item.Position
                                                                && x.CalibrationSubTypeId == item.CalibrationSubTypeId).FirstOrDefault();

                      
                        if(genericCalibration2 == null && Selectlist?.Count > 0) 
                        {
                            //item.Updated = 0;
                        }
                        else if(genericCalibration2 != null && Selectlist?.Count > 0)
                        {
                            item.KeyObject = genericCalibration2.KeyObject;
                        }
                        

                    }

                    //if (item?.GenericCalibration2.Position != null)
                    //{

                    //}


                }

            }

             if (ResultCalibration != null && list != null ) 
            {

                 ResultCalibration(list.OrderBy(x=>x.CalibrationSubTypeId).ToList(), CalibrationType).ConfigureAwait(false).GetAwaiter().GetResult();
            }

            

            return Task.CompletedTask;
            
            


        }

        public async Task SaveGeneric()
        {

            try
            {

                await ShowProgress();

                await CloseGenericWindow();


                //await WorkOrderItemCreate.SetCurrentStatus(eq.CurrentStatus, true);
                              

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

        [Parameter]
        public bool ShowProgress { get; set; }


        public async Task SaveAndCloseGeneric()
        {

            try
            {
                


                //await ShowProgress();

                //await CloseGenericWindow();

                if(JSRuntime != null)
                {
                    //await JSRuntime.InvokeVoidAsync("executeMethod", "CloseButtonModal");
                    await JSRuntime.InvokeVoidAsync("showP");
                }

                if (JSRuntime != null)
                {
                    await JSRuntime.InvokeVoidAsync("executeMethod", "executebutton");
                }




                //if (SaveAction != null)
                //{

                //    await SaveAction();

                //}



                //foreach (var item in _refModals)
                //{
                //    await item.CancelItem("");
                //}

            }
            catch (Exception ex) {

                await ExceptionManager(ex);
            
            
            }
            finally 
            {
                await CloseProgress();
                await Task.Delay(10);
                StateHasChanged();

            } 
            
        }



        public class Grid
        {

            public string Name { get; set; }
            public dynamic GridObject { get; set; }

        }


        public List<Grid> Grids { get; set; }


        public List<dynamic> _refs { get; set; } = new();
        public dynamic Ref
        
        
        {
            //get
            //{

            //    foreach (var item in _refs)
            //    {

            //    }
            //}

            //CalibrationSubType
            set
            {
                //if (!_refs.Contains(value))
                //{
                //    _refs.Add(value);
                //}

                bool inc = false;
                dynamic itemtmp = null; 
                foreach (var item in _refs)
                {
                    if (item.IdGroup == value.IdGroup)
                    {
                        inc = true;
                        itemtmp= item;
                    }
                }
                if(itemtmp != null)
                {
                    _refs.Remove(itemtmp);
                }
                _refs.Add(value);

               
               
            }                
                
        
        
        }



        public List<Modal> _refModals = new();
        public Modal RefModal { set => _refModals.Add(value); }



        public string Argument { get; set; } = "LTI";
        private bool _initialized = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var a = new WorkOrderDetail();

            //a.PieceOfEquipment.TestPointResult

            //a.PieceOfEquipment.EquipmentTemplate

            
            if (firstRender || IsInit)
            {
                IsInit = false;
                

            }
          


            await base.OnAfterRenderAsync(firstRender);

        }




        private async Task CreateGrids()
        {
            if (eq != null && eq.CalibrationSubTypes != null && eq.CalibrationTypeId >= 1)
            {
                // Clear TestPointGroupList for all CalibrationSubTypes to ensure fresh state
                foreach (var calibrationSubType in eq.CalibrationSubTypes)
                {
                    calibrationSubType.TestPointGroupList = null;
                }

                Grids = new List<Grid>();

                foreach (var item in eq.CalibrationSubTypes
                    .DistinctBy(x => x.CalibrationTypeId)
                    .Where(x => x.CalibrationSubTypeView != null))
                {
                    Grid grid = new Grid
                    {
                        Name = item.Name,
                        GridObject = new GenericTestComponent<
                            CalibrationItem, CalibrationItemResult, Entity>()
                    };

                    Grids.Add(grid);
                }
            }
        }

        public async Task NewVisibleCalibration(string CalibrationTypeId)
        {

            try
            {
                var CurrentSubTypes2 = CurrentSubTypes + NumGroup;

                await ShowProgress();

                if (CurrentSubTypes2 >= 120)
                {
                    await ShowToast("No More Calibrations Allowed", ToastLevel.Info);
                }
                else
                {
                    CurrentSubTypes = CurrentSubTypes + NumGroup;

                    var cals = eq.CalibrationSubTypes.Take(CurrentSubTypes).ToList();

                    var eq1 = (ICalibrationType)eq.CloneObject();

                    eq1.CalibrationSubTypes = cals;

                    DomainEntityProp.Configuration = CurrentSubTypes.ToString();


                    await ShowToast("Add Calibration Succes", ToastLevel.Info);
                    //if (NewCalibrationFunc != null)
                    //{
                    //    await NewCalibrationFunc(eq1, CurrentSubTypes);
                    //}
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
                await CloseProgress();
            }


        }
        public async Task NewVisibleTestPointGroup(CalibrationSubType subType)
        {

            try
            {

                var confirm = await Prompt("Please New Group Name");


                var kvtmp=DicTestPointGroupsListS.Where(x => x.Key == subType.CalibrationSubTypeId.ToString()).FirstOrDefault();


                List<string> TestPointGroupsListS = null;

                if (kvtmp.Value != null)
                {
                    TestPointGroupsListS= kvtmp.Value;
                }
                else
                {
                    //throw new Exception("SubType  is required");
                }

                if (string.IsNullOrEmpty(confirm))
                {
                    throw new Exception("Group Name is required");

                }



                //if (DicTestPointGroupsListS != null & DicTestPointGroupsListS.Where(x=>x.Key.Contains(confirm)).ToList().Count > 0)
                //{
                //    throw new Exception("Group Name must be unique");
                   
                //}


               

                var CurrentSubTypes2 = CurrentSubTypes + NumGroup;

                await ShowProgress();

                if (CurrentSubTypes2 >= 120)
                {
                    await ShowToast("No More Calibrations Allowed", ToastLevel.Info);
                }
                else
                {
               

                    if (subType != null)
                    {
             
                        if (TestPointGroupsListS == null)
                            TestPointGroupsListS = new List<string>();

                        // Create a proper clone of the CalibrationSubType
                        var TestPointGroupId = NumericExtensions.GetUniqueID();
                      
                            TestPointGroupsListS.Add(confirm.ToString());

                            
                            DicTestPointGroupsListS[subType.CalibrationSubTypeId.ToString()]= TestPointGroupsListS;
                        //// Set the GroupName on the cloned subtype if it implements IResultComp
                        //if (clonedSubType is IResultComp componentInterface)
                        //{
                        //    componentInterface.GroupName = 
                        //}
                    }


                    TestPointGroupName = null;


                        StateHasChanged();


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
                await CloseProgress();
            }

        }
        public async Task DeleteTestPointGroup(CalibrationSubType subType, string groupName)
        {
            try
            {
                var confirm = await ConfirmAsync($"Are you sure you want to delete the group '{groupName}'?");
                if (!confirm)
                {
                    return;
                }
                await ShowProgress();
                var kvtmp = DicTestPointGroupsListS.Where(x => x.Key == subType.CalibrationSubTypeId.ToString()).FirstOrDefault();
                List<string> TestPointGroupsListS = null;
                if (kvtmp.Value != null)
                {
                    TestPointGroupsListS = kvtmp.Value;
                }
                if (TestPointGroupsListS != null && TestPointGroupsListS.Contains(groupName))
                {
                    TestPointGroupsListS.Remove(groupName);
                    DicTestPointGroupsListS[subType.CalibrationSubTypeId.ToString()] = TestPointGroupsListS;
                    // Remove items from _refs that belong to this group
                    if (_refs != null && _refs.Count > 0)
                    {
                        var itemsToRemove = new List<dynamic>();
                        foreach (var item in _refs)
                        {
                            if (item != null && item.GroupName == groupName.ToString())
                            {
                               
                                if (item?.tableListResult != null && item.tableListResult.Count > 0)
                                {
                                    var itemsToRemoveFromTable = new List<GenericCalibrationResult2>();
                                    foreach (var tableItem in item.tableListResult)
                                    {
                                        if (tableItem != null && tableItem.GroupName == groupName)
                                        {
                                            itemsToRemoveFromTable.Add(tableItem);
                                        }
                                    }
                                    foreach (var tableItem in itemsToRemoveFromTable)
                                    {
                                        item.tableListResult.Remove(tableItem);
                                    }
                                }
                                // Check if this item has RT.Items with items from the deleted group
                                if (item?.RT != null && item.RT.Items != null && item.RT.Items.Count > 0)
                                {
                                    var itemsToRemoveFromRT = new List<GenericCalibrationResult2>();
                                    foreach (var rtItem in item.RT.Items)
                                    {
                                        if (rtItem != null && rtItem.GroupName == groupName)
                                        {
                                            itemsToRemoveFromRT.Add(rtItem);
                                        }
                                    }
                                    foreach (var rtItem in itemsToRemoveFromRT)
                                    {
                                        item.RT.Items.Remove(rtItem);
                                    }
                                }
                            }
                        }
                    }
                    await ShowToast($"Group '{groupName}' deleted successfully", ToastLevel.Success);
                    StateHasChanged();
                }
                else
                {
                    await ShowError("Group not found");
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
                await CloseProgress();
            }
        }
        public async Task EditTestPointGroup(CalibrationSubType subType, string oldGroupName)
        {
            try
            {
                var newGroupName = await Prompt($"Enter new name for group '{oldGroupName}'");
                if (string.IsNullOrEmpty(newGroupName))
                {
                    await ShowError("Group name cannot be empty");
                    return;
                }
                if (newGroupName == oldGroupName)
                {
                    return;
                }
                await ShowProgress();
                // Update the dictionary
                var kvtmp = DicTestPointGroupsListS.Where(x => x.Key == subType.CalibrationSubTypeId.ToString()).FirstOrDefault();
                List<string> TestPointGroupsListS = null;
                if (kvtmp.Value != null)
                {
                    TestPointGroupsListS = kvtmp.Value;
                }
                if (TestPointGroupsListS != null && TestPointGroupsListS.Contains(oldGroupName))
                {
                    // Update the group name in the list
                    var index = TestPointGroupsListS.IndexOf(oldGroupName);
                    TestPointGroupsListS[index] = newGroupName;
                    DicTestPointGroupsListS[subType.CalibrationSubTypeId.ToString()] = TestPointGroupsListS;
                    // Update all GenericCalibrationResult2 records with the old group name
                    // Get TestPointResult from DomainEntityProp
                    List<GenericCalibrationResult2> TestPointResult = null;
                    if (DomainEntityProp.TestPointResult != null && DomainEntityProp.TestPointResult.Count > 0)
                    {
                        TestPointResult = DomainEntityProp.TestPointResult;
                    }
                    else if (GetGroupsProperty == "EquipmentTemplate")
                    {
                        dynamic tesp = DomainEntityProp;
                        TestPointResult = tesp?.EquipmentTemplate.TestPointResult;
                    }
                    else if (GetGroupsProperty == "PieceOfEquipment")
                    {
                        dynamic tesp = DomainEntityProp;
                        TestPointResult = tesp?.PieceOfEquipment?.TestPointResult;
                        if (TestPointResult == null)
                        {
                            TestPointResult = tesp?.PieceOfEquipment.EquipmentTemplate.TestPointResult;
                        }
                    }
                    if (TestPointResult != null)
                    {
                        var itemsToUpdate = TestPointResult
                            .Where(x => x.GroupName == oldGroupName && x.CalibrationSubTypeId == subType.CalibrationSubTypeId)
                            .ToList();
                        foreach (var item in itemsToUpdate)
                        {
                            item.GroupName = newGroupName;
                        }
                    }
                    await ShowToast($"Group '{oldGroupName}' renamed to '{newGroupName}' successfully", ToastLevel.Success);
                    StateHasChanged();
                }
                else
                {
                    await ShowError("Group not found");
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
                await CloseProgress();
            }
        }

        public ModalParameters ModalParameters = new ModalParameters();

        [CascadingParameter]
        public IModalService Modal { get; set; }
        public async Task CalibrationConfig(ICalibrationType CalibrationType)
        {
            var parameters = new ModalParameters();
            parameters.Add("NewPerson", CalibrationType);
            //parameters.Add("IsModal", true);

            ModalOptions op = new ModalOptions();
            op.ContentScrollable = true;
            op.Class = "blazored-modal " + ModalSize.MediumWindow;

            var messageForm = Modal.Show<CalibrationTypeForm>("Config", parameters, op);
            var result = await messageForm.Result;

            if (!result.Cancelled)
            {

            }


            }


            public async Task DelVisibleCalibration(string CalibrationTypeId)
        {

            try
            {

                var conf= await ConfirmAsync();

                if (conf == false)
                {
                    return;
                }


                await ShowProgress();

                var CurrentSubTypes2 = CurrentSubTypes - NumGroup;

                if (CurrentSubTypes2  < NumGroup)
                {
                    await ShowToast("No More Delete Calibrations Allowed", ToastLevel.Info);
                }
                else
                {

                    CurrentSubTypes = CurrentSubTypes - NumGroup;

                    var cals = eq.CalibrationSubTypes.Take(CurrentSubTypes).ToList();

                    var eq1 = (ICalibrationType)eq.CloneObject();

                    eq1.CalibrationSubTypes = cals;

                    DomainEntityProp.Configuration = CurrentSubTypes.ToString();
                    if (NewCalibrationFunc != null)
                    {
                        await NewCalibrationFunc(null, CurrentSubTypes);
                    }

                    await ShowToast("Delete Success", ToastLevel.Info);
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
                await CloseProgress();
            }


        }
        public async Task NewCalibration(string CalibrationTypeId)
        {
            try
            {   //https://localhost:5001/WorkOrderItem/827
                await ShowProgress();

                PieceOfEquipmentGRPC grpc = new PieceOfEquipmentGRPC(_poeServices);


                CalibrationType calibrationType = new CalibrationType();

                calibrationType.CalibrationTypeId = Convert.ToInt32(CalibrationTypeId);

                //////////////////////////////////////////////////////
                //var calsub = new CalibrationSubType();

                //calsub.CalibrationSubTypeId = 30;
                
                //calsub.Name = "ForceGage2";

                //calsub.CalibrationTypeId = 19;

                //calibrationType.CalibrationSubTypes= new List<CalibrationSubType>();

                //calibrationType.CalibrationSubTypes.Add(calsub);

                ////////////////////////////////////////////
                ///

                //for(int i = 0; i < 19; i++)
                //{
                    var cal = await grpc.CreateConfiguration(calibrationType);

                    eq = cal;

                    if (NewCalibrationFunc != null)
                    {
                        await NewCalibrationFunc(cal, CurrentSubTypes);
                    }


                    await ShowToast("New Calibration Success", ToastLevel.Success);
                //}
               


            }
            catch(RpcException ex)
            {
                await ExceptionManager(ex);
            }
            catch(Exception ex) 
            {

                await ExceptionManager(ex);
            
            }
            finally
            {
                await CloseProgress();
            }



        }

        public async Task DisableCalibration(string CalibrationSubTypeId)
        {
            try
            {
                await ShowProgress();

                PieceOfEquipmentGRPC grpc = new PieceOfEquipmentGRPC(_poeServices);


                CalibrationSubType calibrationType = new CalibrationSubType();

                calibrationType.CalibrationSubTypeId = Convert.ToInt32(CalibrationSubTypeId);

                var cal = await grpc.DeleteConfiguration(calibrationType);

                await ShowToast("Delete Success", ToastLevel.Success);                

                if (NewCalibrationFunc != null)
                {
                    await NewCalibrationFunc(eq,0);
                }

                eq.CalibrationSubTypes.Remove(cal);
                //StateHasChanged();



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
                await CloseProgress();
            }


        }

            public async Task ShowGenericCalibration(CalibrationSubType itemcs)
        {
            // Call the overloaded method with null GroupName for backward compatibility
            await ShowGenericCalibration(itemcs, null);
        }

        public async Task ShowGenericCalibration(CalibrationSubType itemcs, int? GroupName)
        {

            Closing = false;


            string CalibrationSubTypeId = itemcs.CalibrationSubTypeId.ToString();

            // Construct the correct modal ID including GroupName
            string modalId;
            if (GroupName.HasValue)
            {
                modalId = CalibrationSubTypeId + "-" + GroupName.Value.ToString() + "-modal";
                Console.WriteLine($"DEBUG: ShowGenericCalibration looking for modal with ID: {modalId} (with GroupName: {GroupName.Value})");
            }
            else
            {
                modalId = CalibrationSubTypeId + "-modal";
                Console.WriteLine($"DEBUG: ShowGenericCalibration looking for modal with ID: {modalId} (no GroupName)");
            }

            //if (Param.Contains("Compression"))
            //{
            //    await ShowEccentricity();
            //}
            //if (Param.Contains("Tension") || Param.Contains("Universal"))
            //{
            //    await ShowLinearity();
            //}

            // Log all available modals for debugging
            Console.WriteLine($"DEBUG: ShowGenericCalibration - Available modals in _refModals:");
            foreach (var modal in _refModals)
            {
                Console.WriteLine($"  - Modal ID: {modal.ID}, Style: {modal.Style}");
            }

            foreach (var item in _refModals)
            {
                if (item.Style == "Modal")
                {
                    item.Hide();
                }
            }

            bool modalFound = false;
            foreach (var item in _refModals)
            {
                if(item.ID == modalId && item.Style == "Modal")
                {
                    {
                        item.Toggle();
                        modalFound = true;
                        Console.WriteLine($"DEBUG: ShowGenericCalibration found and toggled modal with ID: {modalId}");
                    }

                }

            }

            if (!modalFound)
            {
                Console.WriteLine($"DEBUG: ShowGenericCalibration - Modal with ID '{modalId}' not found in _refModals");
            }

            StateHasChanged();



        }

        [Parameter]
        public bool Expanded { get; set; }

        [Parameter]
        public bool IsVisible { get; set; }

        [Parameter]
        public bool UseIsVisible { get; set; }




        public async Task ShowTests(string Param="")
        {

           
            Closing = false;

            if (!UseIsVisible)
            {
                return;
            }


            if (!IsVisible)
            {
                IsVisible = true;
            }
            else
            {
                IsVisible = false;
            }
            
            


        }

        /// <summary>
        /// Generates a sequential name for cloned CalibrationSubType to ensure unique naming
        /// </summary>
        /// <param name="originalName">The original name of the CalibrationSubType</param>
        /// <param name="sequenceNumber">The sequence number for this clone</param>
        /// <returns>A unique sequential name</returns>
        private string GenerateSequentialName(string originalName, int sequenceNumber)
        {
            if (string.IsNullOrEmpty(originalName))
            {
                return $"Calibration {sequenceNumber}";
            }

            // For the first clone (sequenceNumber = 2), append " 2"
            // For subsequent clones, append " 3", " 4", etc.
            if (sequenceNumber <= 1)
            {
                return originalName; // Original name for first instance
            }
            else
            {
                return $"{originalName} {sequenceNumber}";
            }
        }

        /// <summary>
        /// Creates a proper clone of CalibrationSubType with independent data and assigns the correct GroupName
        /// </summary>
        /// <param name="original">The original CalibrationSubType to clone</param>
        /// <param name="testPointGroupId">The GroupName for this clone</param>
        /// <returns>A new independent CalibrationSubType instance</returns>
        private CalibrationSubType CloneCalibrationSubType(CalibrationSubType original, int testPointGroupId)
        {
            var cloned = new CalibrationSubType
            {
                CalibrationSubTypeId = original.CalibrationSubTypeId,
                CalibrationTypeId = original.CalibrationTypeId,
                Name = original.Name,
                NameToShow = original.NameToShow,
                CalibrationSubTypeView = original.CalibrationSubTypeView,
                CreateClass = original.CreateClass,
                GetClass = original.GetClass,
                CalibrationSubTypeViewID = original.CalibrationSubTypeViewID,
                Select2StandarClass = original.Select2StandarClass,
                SelectStandarClass = original.SelectStandarClass,
                NewClass = original.NewClass,
                Mandatory = false,
                inheritCalibrationSubTypeId =  original.inheritCalibrationSubTypeId,
                UnitOfMeasureTypeID = original.UnitOfMeasureTypeID,

             
            };

            // Debug log to verify the assignment
            Console.WriteLine($"DEBUG: Cloned CalibrationSubType {original.CalibrationSubTypeId} with TestPointGroupId: {testPointGroupId}");

            // If there are other properties that need to be copied, add them here
            // Make sure to create new instances for any reference types to avoid sharing data

            return cloned;
        }

        public bool LoadTestComponent { get; set; }

        public async Task ChangeAcc(object value, string name, string action)
        {


            if (1==1 || LoadTestComponent)
            {
               //await CloseGenericWindow2();
            }
            LoadTestComponent = true;

           // console.Log($"{name} item with index {value} {action}");
        }



    }
}
