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
using CalibrationSaaS.Domain.Aggregates.ValueObjects;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI
{
    public class CalibrationTypeComponent_Base : FComponentBase
    {

        [CascadingParameter(Name = "CascadeParam1")]
        public IWorkOrderItemCreate WorkOrderItemCreate { get; set; }

        [Parameter]
        public CalibrationType cmcValues { get; set; }

        public Modal ModalLinearity { get; set; }
        public Modal ModalLRepeatibility { get; set; }
        public Modal ModalLEccentricity { get; set; }

        public CalibrationType _persistentCalibrationType;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            
         
            if (WorkOrderItemCreate.eq != null && (WorkOrderItemCreate.eq?.calibrationType?.CalibrationSubTypes != null) && WorkOrderItemCreate.eq?.calibrationType?.CalibrationTypeId >= 4)
            {
                Grids = new List<Grid>();

                foreach (var item in WorkOrderItemCreate.eq?.calibrationType?.CalibrationSubTypes.DistinctBy(x => x.CalibrationTypeId).Where(x => x.CalibrationSubTypeView != null))
                {
                    Grid grid = new Grid();
                    grid.Name = item.Name;
                    grid.GridObject = new CalibrationSaaS.Infraestructure.Blazor.GenericTestComponent<GenericCalibration, GenericCalibrationResult>();
                    Grids.Add(grid);
                }

            }

            //if (WorkOrderItemCreate.eq.GetCalibrationTypeID() >= 4)
            //{
            //    Grids = new List<Grid>();


            //    if(WorkOrderItemCreate.eq.GetCalibrationType().CalibrationSubTypes != null && (WorkOrderItemCreate.eq.GetCalibrationType().CalibrationSubTypes.Count > 0))
            //    {

            //        foreach (var item in WorkOrderItemCreate.eq.GetCalibrationType().CalibrationSubTypes)
            //        {
            //            Grid grid = new Grid();
            //            grid.Name = item.Name;
            //            grid.GridObject = new CalibrationSaaS.Infraestructure.Blazor.GenericTestComponent<GenericCalibration, GenericCalibrationResult>();
            //            Grids.Add(grid);
            //        }


            //    }

            //}

            if (WorkOrderItemCreate.eq?.CalibrationType?.CalibrationSubTypes != null)
            {
                _persistentCalibrationType = WorkOrderItemCreate.eq.CalibrationType;
            }
            else
            {
                _persistentCalibrationType = WorkOrderItemCreate.eq.calibrationType;
            }

            StateHasChanged();
        }

        public async Task SaveRockwell()
        {

            try
            {

                await ShowProgress();

                await CloseRockwellWindow();


                await WorkOrderItemCreate.SetCurrentStatus(WorkOrderItemCreate.eq.CurrentStatus, true);


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


        public async Task SaveAndCloseRockwell()
        {

            try
            {

                await ShowProgress();

                await CloseRockwellWindow();

                await WorkOrderItemCreate.SetCurrentStatus(WorkOrderItemCreate.eq.CurrentStatus, true);

                if(ModalLinearity != null)
                {
                    ModalLinearity.Hide();
                }

                if (ModalLEccentricity != null)
                {
                    ModalLEccentricity.Hide();
                }

                if (ModalLRepeatibility != null)
                {

                    ModalLRepeatibility.Hide();
                }




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



        public async Task CloseRockwellWindow()
        {



            List<CalibrationSaaS.Domain.Aggregates.Entities.Rockwell> list = new List<CalibrationSaaS.Domain.Aggregates.Entities.Rockwell>();

            if (Linearity != null && Linearity.RT != null && Linearity?.RT?.Items?.Count > 0)
            {
                var items = Linearity.RT.Items;

                list.AddRange(items);


            }

            if (Compresion != null && Compresion.RT != null && Compresion?.RT?.Items?.Count > 0)
            {
                var items = Compresion.RT.Items;

                list.AddRange(items);
            }


            if (WorkOrderItemCreate.eq.BalanceAndScaleCalibration == null)
            {
                WorkOrderItemCreate.eq.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();
            }

            //Console.WriteLine("CloseLinearity XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX " + list.Count);

            WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Rockwells = list;
            foreach (var item in WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Rockwells)
            {
                //Console.WriteLine(item.BasicCalibrationResult.Nominal);
            }

            //if(WorkOrderItemCreate.eq.EnviromentCondition == null)
            //{
            WorkOrderItemCreate.eq.EnviromentCondition = new List<ExternalCondition>();
            //}

            if (Linearity.EnviromentComponent != null)
            {
                WorkOrderItemCreate.eq.EnviromentCondition.Add(Linearity.EnviromentComponent.EnviromentCondition);
            }

            if (Compresion.EnviromentComponent != null)
            {
                WorkOrderItemCreate.eq.EnviromentCondition.Add(Compresion.EnviromentComponent.EnviromentCondition);
            }


        }




        public async Task SaveForce()
        {

            try
            {

                await ShowProgress();

                await CloseWindow();


                await WorkOrderItemCreate.SetCurrentStatus(WorkOrderItemCreate.eq.CurrentStatus, true);


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


        public async Task SaveAndCloseForce()
        {

            try
            {

                await ShowProgress();

                await CloseWindow();



               
                await WorkOrderItemCreate.SetCurrentStatus(WorkOrderItemCreate.eq.CurrentStatus, true);


                if (ModalLinearity != null)
                {
                    ModalLinearity.Hide();
                }

                if (ModalLEccentricity != null)
                {
                    ModalLEccentricity.Hide();
                }

                if (ModalLRepeatibility != null)
                {

                    ModalLRepeatibility.Hide();
                }



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




        public async Task CloseWindow()
        {



            //Console.WriteLine("CloseLinearity XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            //Console.WriteLine("Linearity RT Count " + Linearity?.RT?.Items?.Count);

            List<Force> list = new List<Force>();

            if (Linearity != null && Linearity.RT != null && Linearity?.RT?.Items?.Count > 0)
            {
                var items = Linearity.RT.Items;

                list.AddRange(items);


            }

            bool iscopy = false;
            if (Linearity.HasCopy && Linearity.HasChange )
            {
                iscopy = true;
            }


            if (Compresion != null && Compresion.RT != null && Compresion?.RT?.Items?.Count > 0)
            {
                //if (1==1 && !iscopy)
                if (!iscopy)
                {
                    var items34 = Compresion.RT.Items as List<Force>;

                    var items33 = items34.OrderBy(x => x.BasicCalibrationResult.Position).ToList(); ;

                    list.AddRange(items33);
                }
                else if (iscopy && Linearity.TotalForcesCompt != null)
                {
                    int con = 1;
                    var TotalForcesCompt = new List<Force>();

                    var items34 =  Linearity.TotalForcesCompt as List<Force>;//Compresion.RT.Items as List<Force>;

                    //foreach (var item in items34.OrderBy(x => x.BasicCalibrationResult.Position).ToList())
                    //{

                    //    var resultCloned = new Force();
                    //    resultCloned.BasicCalibrationResult = new ForceResult();

                    //    resultCloned.UnitOfMeasureId = item.UnitOfMeasureId;
                    //    resultCloned.CalibrationUncertaintyValueUnitOfMeasureId = item.CalibrationUncertaintyValueUnitOfMeasureId;

                    //    resultCloned.CalibrationSubTypeId = item.CalibrationSubTypeId + 1;
                    //    resultCloned.BasicCalibrationResult.CalibrationSubTypeId = item.BasicCalibrationResult.CalibrationSubTypeId + 1;
                    //    resultCloned.SequenceID = con;
                    //    resultCloned.BasicCalibrationResult.SequenceID = con;
                    //    resultCloned.WorkOrderDetailId = item.WorkOrderDetailId;
                    //    resultCloned.BasicCalibrationResult.WorkOrderDetailId = item.WorkOrderDetailId;
                    //    resultCloned.BasicCalibrationResult.Position = item.BasicCalibrationResult.Position;

                    //    resultCloned.BasicCalibrationResult.FS = item.BasicCalibrationResult.FS;
                    //    resultCloned.BasicCalibrationResult.Nominal = item.BasicCalibrationResult.Nominal;
                    //    TotalForcesCompt.Add(resultCloned);
                    //    con++;

                    //}
                    list.AddRange(items34.OrderBy(x => x.BasicCalibrationResult.Position).ToList());
                    Linearity.TotalForcesCompt = null;
                    WorkOrderItemCreate.eq.Refresh = true;
                    //Compresion.RT.Clear();

                    //Compresion.RT.Clear2();


                }

            }


            if (WorkOrderItemCreate.eq.BalanceAndScaleCalibration == null)
            {
                WorkOrderItemCreate.eq.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();
            }

            //Console.WriteLine("CloseLinearity XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX " + list.Count);

            WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Forces = list;

           

            foreach (var itemm in list)
            {
                //Console.WriteLine(itemm.CalibrationSubTypeId);
            }

            if(Linearity?.RT != null)
            {
                //Linearity.RT.Clear2();
            }
            if (Compresion?.RT != null)
            {

                //Compresion.RT.Clear2();
            }
        }



        public async Task CloseGenericWindow()
        {
            List<GenericCalibration> list = new List<GenericCalibration>();

            foreach (var item in _refs)
            {

                if (item != null && item?.RT != null && item?.RT?.Items?.Count > 0)
                {
                    var lista = item.RT.Items;
                    
                    list.AddRange((IEnumerable<GenericCalibration>)lista);
                }


            }
           
           
                WorkOrderItemCreate.eq.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();
         

                WorkOrderItemCreate.eq.BalanceAndScaleCalibration.GenericCalibration = list;

            if(list != null && list.Count  > 0)
            {
                foreach (var item in list)
                {
                    if(item.BasicCalibrationResult.CurrentViews != null && item.BasicCalibrationResult.CurrentViews.Count > 0)
                    {
                        foreach (var dp in item.BasicCalibrationResult.CurrentViews)
                        {

                            var view = dp.ViewPropertyBase;

                            var subtype = WorkOrderItemCreate.eq.GetCalibrationType().CalibrationSubTypes.Where(x => x.CalibrationSubTypeId == item.CalibrationSubTypeId).FirstOrDefault();

                            if (subtype != null && subtype.CalibrationSubTypeView != null && subtype.CalibrationSubTypeView.BlockIfInvalid.HasValue 
                                && view.IsValid.HasValue && !view.IsValid.Value)
                            {
                                var error = "";
                                if (!string.IsNullOrEmpty(view.ErrorMesage))
                                {
                                    error = view.ErrorMesage;
                                }
                                await ShowError("Error en " + view.Display + " " + error);

                                if(subtype.CalibrationSubTypeView.BlockIfInvalid.HasValue && subtype.CalibrationSubTypeView.BlockIfInvalid.Value)
                                {
                                    throw new Exception("Error en " + view.Display + " " + error);
                                }


                            }    
                           
                        }
                    }
                    
                }
            }


            //await ShowGenericCalibration(14);

            WorkOrderItemCreate.eq.CalibrationType = _persistentCalibrationType;

            StateHasChanged();

        }

        public async Task SaveGeneric()
        {

            try
            {

                await ShowProgress();

                await CloseGenericWindow();


                await WorkOrderItemCreate.SetCurrentStatus(WorkOrderItemCreate.eq.CurrentStatus, true);
                              

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


                await WorkOrderItemCreate.SetCurrentStatus(WorkOrderItemCreate.eq.CurrentStatus, true);


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






        [Parameter]
        public EventCallback<ChangeEventArgs> OnChangeDescription { get; set; }


        public dynamic Linearity { get; set; }

        public dynamic Compresion { get; set; }






        public class Grid
        {

            public string Name { get; set; }
            public dynamic GridObject { get; set; }

        }


        public List<Grid> Grids { get; set; }


        public List<dynamic> _refs = new();
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

                //if (!inc ) 
                //{
                //    _refs.Add(value);
                //}
                //else
                //{
                   
                //    _refs.Add(value);
                //}
               
            }
                
                
        
        
        }



        public List<Modal> _refModals = new();
        public Modal RefModal { set => _refModals.Add(value); }





        public string Argument { get; set; } = "LTI";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            if (WorkOrderItemCreate.eq.CalibrationType != null)
            {
                _persistentCalibrationType = WorkOrderItemCreate.eq.CalibrationType;
            }
            else
            {
                _persistentCalibrationType = WorkOrderItemCreate.eq.calibrationType;
            }

           
        }


        public async Task ShowGenericCalibration(int CalibrationSubTypeId)
        {

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
                item.Hide();
            }

            foreach (var item in _refModals)
            {
                if(item.ID== CalibrationSubTypeId.ToString() + "-modal")
                {
                    item.Toggle();
                }
               
            }

            StateHasChanged();



        }


        public async  Task ShowCalibration(string Param = "")
        {

            if (Param.Contains("Compression"))
            {
                await ShowEccentricity();
            }
             if (Param.Contains("Tension")||Param.Contains("Universal"))
            {
                await ShowLinearity();
            }

            
        }
        public async Task ShowLinearity()
        {


            if (WorkOrderItemCreate.CalibrationInstructions != null && WorkOrderItemCreate.CalibrationInstructions.changeTolerance)
            {

                WorkOrderItemCreate.eq.Resolution = WorkOrderItemCreate.CalibrationInstructions.ResolutionComponent.eq.Resolution;
                WorkOrderItemCreate.eq.Tolerance.AccuracyPercentage = WorkOrderItemCreate.CalibrationInstructions.ResolutionComponent.eq.Tolerance.AccuracyPercentage;
                WorkOrderItemCreate.eq.DecimalNumber = WorkOrderItemCreate.CalibrationInstructions.ResolutionComponent.eq.DecimalNumber;
                WorkOrderItemCreate.eq.Tolerance.ToleranceFixedValue = WorkOrderItemCreate.CalibrationInstructions.ResolutionComponent.eq.Tolerance.ToleranceFixedValue;
                WorkOrderItemCreate.eq.IsAccredited = WorkOrderItemCreate.eq.IsAccredited;
                WorkOrderItemCreate.eq.Tolerance.ToleranceValue = WorkOrderItemCreate.CalibrationInstructions.ResolutionComponent.eq.Tolerance.ToleranceValue;


                if (WorkOrderItemCreate.CalibrationInstructions.ResolutionComponent.eq.Tolerance.ToleranceTypeID.HasValue)
                {
                     WorkOrderItemCreate.eq.Tolerance.ToleranceTypeID = WorkOrderItemCreate.CalibrationInstructions.ResolutionComponent.eq.Tolerance.ToleranceTypeID.Value;
                }
               
                WorkOrderItemCreate.eq.IsComercial = WorkOrderItemCreate.CalibrationInstructions.ResolutionComponent.eq.IsComercial;
                WorkOrderItemCreate.eq.ClassHB44 = WorkOrderItemCreate.CalibrationInstructions.ResolutionComponent.eq.ClassHB44;

                //Console.WriteLine("changeresolution");

                WorkOrderItemCreate.ChangeResolution = true;
            }
            else
            {
                //WorkOrderItemCreate.ChangeResolution = false;
            }

            //if (we.Ranges == null && (CalibrationInstructions?.ResolutionComponent?.RangeComponent?.RT?.Items?.Count > 0 || CalibrationInstructions?.ResolutionComponent?.RangeComponent?.RT?.Items?.Count > 0))
            //{
            //    we.Ranges = new List<RangeTolerance>();
            //}

            WorkOrderItemCreate.eq.Ranges = new List<RangeTolerance>();



            //visibleModal = "";
            //LinearityShow = true;
            //RepeatibilityShow = false;
            //EccentricityShow = false;
            if(ModalLEccentricity != null)
            {
                 ModalLEccentricity.Hide();
            }
           

            //ModalLRepeatibility.Hide();
           
            if (ModalLinearity != null)
            {
                ModalLinearity.Toggle();
            }

            StateHasChanged();

        }

      

        public async Task ShowEccentricity()
        {

            //LinearityShow = false;
            //RepeatibilityShow = false;
            //EccentricityShow = true;

            if(ModalLinearity != null)
            {
                ModalLinearity.Hide();
            }

            if (ModalLEccentricity != null)
            {
                ModalLEccentricity.Toggle();
            }
            //ModalLRepeatibility.Hide();
            StateHasChanged();

        }

        public async Task ShowTests(string Param="")
        {
            //if(Param == "LTI")
            //{
            
            //LinearityShow = true;
            //RepeatibilityShow = true;
            //EccentricityShow = true;
            ////await CloseProgress();
            //await ScrollPosition();
            //await ScrollPosition();
            //}
            //else
            //{
            //    //await ShowProgress();
            //LinearityShow = true;
            //RepeatibilityShow = true;
            //EccentricityShow = true;
            ////await CloseProgress();
            //await ScrollPosition();
            //await ScrollPosition();

            //}

            
        }

        public object GetInstance(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return Activator.CreateInstance(type);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }
            return null;
        }

    }
}
