using Blazor.IndexedDB.Framework;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics.TestPoints;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazed.Controls.Toast;
using System.Linq;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Order;
using Modal = Blazed.Controls.Modal;
using Google.Protobuf.WellKnownTypes;
using Type = System.Type;
using System.Security.Cryptography.X509Certificates;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using Helpers.Controls;
using Grpc.Core;
using Helpers;
using Blazored.Modal;
using Blazed.Controls.MultiComponent;
using Blazored.Modal.Services;
using Microsoft.FluentUI.AspNetCore.Components;

namespace CalibrationSaaS.Infraestructure.Blazor.Shared
{
    //WorkOrderItem
    //PieceOfEquipmentCreate
    public partial class CalibrationTypeComponent<Entity,CalibrationItem, CalibrationItemResult> : FComponentBase 
        where Entity: class, IResolution, IConfiguration,ITolerance ,
        IGenericCalibrationCollection<GenericCalibrationResult2>, 
        new()
        where CalibrationItem : class, IResultComp, IGenericCalibrationSubTypeCollection<GenericCalibrationResult2>, new()
        where CalibrationItemResult : class,IResultComp,IResultGen2, IResult2, IDynamic,IUpdated,ISelect, new()
    {



        [Parameter]
        public string ParentAccordionID { get; set; } = "accordionDynamic";

        public int CurrentSubTypes { get; set; }

        public int NumSubTypes { get; set; }


        public int ExistsCalibration { get; set; }

        public int NumGroup { get; set; }

        public int MaxNumberCopies { get; set; } = 160;

        [Inject] public Application.Services.IPieceOfEquipmentService<CallContext> _poeServices { get; set; }


        [CascadingParameter(Name = "CascadeParam3")]
        public Entity DomainEntityProp { get; set; }


        [CascadingParameter(Name = "CascadeParam2")]
        public ICalibrationType eq { get; set; }

        public EventCallback<ChangeEventArgs> OnChangeDescription { get; set; }


        //[Parameter]
        //public Helpers.Controls.Component Component { get; set; }

        [Parameter]
        public bool ChangeResolution { get; set; }

        [Parameter]
        public bool HasBeenCompleted { get; set; }


        

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (eq != null  && (eq.CalibrationSubTypes != null)  && eq.CalibrationTypeId >= 4)
            {
                Grids = new List<Grid>();

                foreach (var item in eq.CalibrationSubTypes)
                {
                    Grid grid = new Grid();
                    grid.Name = item.Name;
                    grid.GridObject = new CalibrationSaaS.Infraestructure.Blazor.Shared.GenericTestComponent<CalibrationItem, CalibrationItemResult,Entity>();
                    Grids.Add(grid);
                }

            }



        }

        [Parameter]
        public Func<ICalibrationType,int, Task> NewCalibrationFunc { get; set; }

        [Parameter]
       public Func<List<GenericCalibrationResult2>, string, Task<List<GenericCalibrationResult2>>> ResultCalibration { get; set; }


        [Parameter]
        public Func<Task> SaveAction { get; set; }

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
            }
            finally { 
            

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

                await CloseGenericWindow2(CalibrationSubType);


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
                    if(itemxx.CalibrationSubType== CalibrationType)
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
                else
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
                            if(itemsele.Position== item.SelectPosition)
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

//                    Console.WriteLine(item.Object);

                    GenericCalibrationResult2 genericCalibration2 = null; 
 
                    if (Selectlist?.Count > 0)
                    {
                        genericCalibration2 = Selectlist.Where(x => x.Position == item.Position).FirstOrDefault();

                        if(genericCalibration2 == null) 
                        {
                            //item.Updated = 0;
                        }
                        else
                        {
                            //item.KeyObject = genericCalibration2.KeyObject;
                        }
                        

                    }

                    //if (item?.GenericCalibration2.Position != null)
                    //{

                    //}


                }

            }

            if (ResultCalibration != null) 
            {

                 ResultCalibration(list.OrderBy(x=>x.CalibrationSubTypeId).ToList(), CalibrationType).ConfigureAwait(false).GetAwaiter().GetResult();
            }

            

            return Task.CompletedTask;
                //eq.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();
         

                //eq.BalanceAndScaleCalibration.GenericCalibration = list;

            //if(list != null && list.Count  > 0)
            //{
            //    foreach (var item in list)
            //    {
            //        if(item.BasicCalibrationResult.CurrentViews != null && item.BasicCalibrationResult.CurrentViews.Count > 0)
            //        {
            //            foreach (var dp in item.BasicCalibrationResult.CurrentViews)
            //            {

            //                var view = dp.ViewPropertyBase;

            //                var subtype = eq.CalibrationSubTypes.Where(x => x.CalibrationSubTypeId == item.CalibrationSubTypeId).FirstOrDefault();

            //                if (subtype != null && subtype.CalibrationSubTypeView != null && subtype.CalibrationSubTypeView.BlockIfInvalid.HasValue 
            //                    && view.IsValid.HasValue && !view.IsValid.Value)
            //                {
            //                    var error = "";
            //                    if (!string.IsNullOrEmpty(view.ErrorMesage))
            //                    {
            //                        error = view.ErrorMesage;
            //                    }
            //                    await ShowError("Error en " + view.Display + " " + error);

            //                    if(subtype.CalibrationSubTypeView.BlockIfInvalid.HasValue && subtype.CalibrationSubTypeView.BlockIfInvalid.Value)
            //                    {
            //                        throw new Exception("Error en " + view.Display + " " + error);
            //                    }

                                

            //                }    
                           
            //            }
            //        }
                    
            //    }
            //}


            //await ShowGenericCalibration(14);

            


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
        public async Task SaveAndCloseGeneric()
        {

            try
            {

                await ShowProgress();

                await CloseGenericWindow();

                if(SaveAction!= null) {

                    await SaveAction();
                
                }
                //await WorkOrderItemCreate.SetCurrentStatus(eq.CurrentStatus, true);


                foreach (var item in _refModals)
                {
                    await item.CancelItem("");
                }

            }
            catch(Exception ex) {

                await ExceptionManager(ex);
            
            
            }
            finally 
            {
                await CloseProgress();

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
                    if (item.CalibrationSubType == value.CalibrationSubType)
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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        { 


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

                var eq1 =(ICalibrationType) eq.CloneObject();

                eq1.CalibrationSubTypes = cals;

                DomainEntityProp.Configuration= CurrentSubTypes.ToString();


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




            string CalibrationSubTypeId = itemcs.CalibrationSubTypeId.ToString();
            //if (Param.Contains("Compression"))
            //{
            //    await ShowEccentricity();
            //}
            //if (Param.Contains("Tension") || Param.Contains("Universal"))
            //{
            //    await ShowLinearity();
            //}

            foreach (var item in _refModals)
            {
                if (item.Style == "Modal")
                {
                    item.Hide();
                }
            }

            foreach (var item in _refModals)
            {
                if(item.ID== CalibrationSubTypeId + "-modal" && item.Style == "Modal")
                {
                    item.Toggle();
                }
               
            }

            StateHasChanged();



        }


      
      

      

    

        public async Task ShowTests(string Param="")
        {
           

            
        }

       

    }
}
