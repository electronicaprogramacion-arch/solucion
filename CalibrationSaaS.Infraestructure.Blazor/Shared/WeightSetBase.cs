using Blazored.Modal;
using Blazored.Modal.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helpers.Controls.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Order;
using System.Text.RegularExpressions;


namespace CalibrationSaaS.Infraestructure.Blazor.Shared
{
    public class WeightSetComponentBase : KavokuComponentBase<WeightSet>
    {


        [Parameter]
        public Status CurrentStatus { get; set; }


        [Parameter]
        public Func<ChangeEventArgs,Task> ChangeAdd { get; set; }

        [Inject] public CalibrationSaaS.Domain.Aggregates.Shared.Basic.AppState AppState { get; set; }

        public EquipmentType EquipmentTypeObject { get; set; }

        [Parameter]
        public int? CalibrationTypeID { get; set; }


        [Parameter]
        public bool Dynamic { get; set; }

        [CascadingParameter] 
        public IModalService Modal { get; set; }

        public string _message { get; set; }


        public string Description { get; set; } = "";

        [Parameter]
        public string JsonFields { get; set; }
        public Dictionary<string, GridColumnBase> Fields { get; set; }

        [Parameter]
        public string Option { get; set; } = "";

        [Parameter]
        public string ID { get; set; }

        public List<WeightSet> Group2 { get; set; }
        public ResponsiveTable<WeightSet> Grid { get; set; } //= new ResponsiveTable<WeightSet>();

        [CascadingParameter(Name = "CascadeParam1")]
        public IWorkOrderItemCreate WorkOrderItemCreate { get; set; }

        [CascadingParameter(Name = "CascadeParamWO")]
        public WorkOrder WorkOrder { get; set; }
        /// <summary>
        /// properi in weightsets or poe and title 
        /// </summary>




        [Parameter]
        public List<WeightSet> lstWeightSet { get; set; } = new List<WeightSet>();

        public List<WeightSet> _Group;
        public List<WeightSet> Group
        {

            get
            {
                return _Group;
            }
            set
            {

                _Group = value;


            }
        }

        //[Parameter]
        //public EventCallback<ChangeEventArgs> ChangeDescription { get; set; }


        [Parameter]
        public EventCallback<ChangeEventArgs> ClearGrid { get; set; }

        public async Task<bool> Delete(Domain.Aggregates.Entities.WeightSet DTO)
        {
            //await searchComponent.ShowModalAction();
            //Group.Remove(DTO);

            //lstWeightSet.Remove(DTO);

            //Group2.Remove(DTO);

            //if (DeleteFunction != null)
            //{
            //    await DeleteFunction(DTO);
            //}

            PieceOfEquipment PieceOfEquipment = new PieceOfEquipment();

            PieceOfEquipment.WeightSets = lstWeightSet;

            ChangeEventArgs arg = new ChangeEventArgs();

            var trr = WorkOrderItemCreate.eq.WOD_Weights;

            WOD_Weight ww = null;
            foreach (var item in trr)
            {
                if (item.WeightSetID == DTO.WeightSetID)
                {
                    ww = item;
                }
            }

            if (ww != null)
            {
                List<WeightSet> list2 = new List<WeightSet>();

                WorkOrderItemCreate.eq.WOD_Weights.Remove(ww);

                foreach (var weightSet in WorkOrderItemCreate.eq.WOD_Weights)
                {
                    list2.Add(weightSet.WeightSet);
                }
                PieceOfEquipment.WeightSets = list2;
                PieceOfEquipment.PieceOfEquipmentID= DTO.PieceOfEquipmentID;
                arg.Value = PieceOfEquipment;

                ClearWeightSet().ConfigureAwait(false).GetAwaiter().GetResult(); ;

                ChangeAdd(arg).ConfigureAwait(false).GetAwaiter().GetResult(); 

                //StateHasChanged();



            }

            return false;

            //searchComponent.ShowResult();
        }
        public List<PieceOfEquipment> _listPoEDueDate = new List<PieceOfEquipment>();

        public List<PieceOfEquipment> ListPOE { get; set; } = new List<PieceOfEquipment>();


        public async Task SelectWeightSet()
        {

            var parameters = new ModalParameters();
            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);
            parameters.Add("Checkbox", false);
            parameters.Add("Indicator", true);
            parameters.Add("Component", Component);
            if (!string.IsNullOrEmpty(Option))
            {
                parameters.Add("Parameter1", Option);
            }
            
            if(!CalibrationTypeID.HasValue)
            {
                parameters.Add("CalibrationTypeID", WorkOrderItemCreate.eq.GetCalibrationTypeID());
            }
            else
            {
                parameters.Add("CalibrationTypeID", CalibrationTypeID.Value);
            }

            if (WorkOrderItemCreate != null)
            {

                parameters.Add("Accredited", WorkOrderItemCreate.eq.IsAccredited);
                _listPoEDueDate.Clear();

                if (WorkOrderItemCreate.eq?.WOD_Weights?.Count > 0)
                {
                    foreach (var item in WorkOrderItemCreate.eq.WOD_Weights)
                    {

                        if (item.WeightSet != null)
                        {
                            PieceOfEquipment _poe = new PieceOfEquipment();
                            _poe = new PieceOfEquipment()
                            {
                                //PieceOfEquipmentID = ID,
                                //SerialNumber = listPoe.SerialNumber
                                PieceOfEquipmentID = item.WeightSet.PieceOfEquipmentID,
                                SerialNumber = item.WeightSet.Serial,
                                EquipmentTemplate = item?.WeightSet?.PieceOfEquipment?.EquipmentTemplate

                            };


                            _listPoEDueDate.Add(_poe);
                        }


                    }
                }


                if (WorkOrderItemCreate.eq?.WOD_Standard?.Count > 0)
                {
                    foreach (var item in WorkOrderItemCreate.eq.WOD_Standard)
                    {

                        if (item.Standard != null)
                        {
                            PieceOfEquipment _poe = new PieceOfEquipment();
                            _poe = new PieceOfEquipment()
                            {
                                //PieceOfEquipmentID = ID,
                                //SerialNumber = listPoe.SerialNumber
                                PieceOfEquipmentID = item.PieceOfEquipmentID,
                                EquipmentTemplate = item?.Standard?.EquipmentTemplate,
                                SerialNumber = item?.Standard?.SerialNumber

                            };


                            _listPoEDueDate.Add(_poe);
                        }


                    }


                }

                parameters.Add("pieceOfEquipmentsCh", _listPoEDueDate);
            }
            else if (WorkOrder != null)
            {
                _listPoEDueDate.Clear();
                if (WorkOrder?.WO_Standard?.Count > 0)
                {
                    foreach (var item in WorkOrder.WO_Standard)
                    {

                        if (item.Standard != null)
                        {
                            PieceOfEquipment _poe = new PieceOfEquipment();
                            _poe = new PieceOfEquipment()
                            {
                                //PieceOfEquipmentID = ID,
                                //SerialNumber = listPoe.SerialNumber
                                PieceOfEquipmentID = item.PieceOfEquipmentID,
                                EquipmentTemplate = item?.Standard?.EquipmentTemplate,
                                SerialNumber = item?.Standard?.SerialNumber

                            };


                            _listPoEDueDate.Add(_poe);
                        }


                    }


                }

            }



            ModalOptions op = new ModalOptions();
            op.ContentScrollable = true;
            op.Class = "blazored-modal " + ModalSize.LargeWindow;
            //PieceOfEquipmentDueDate_Search
            var messageForm = Modal.Show<CalibrationSaaS.Infraestructure.Blazor.GenericMethods.POEWeightSet_Select>("Select Standard", parameters, op);
            var result = await messageForm.Result;
            Console.Write("result from weigth set base yyy" + result);


            if (!result.Cancelled)
                _message = result.Data?.ToString() ?? string.Empty;

      
            if (result.Data == null)
            {
                return;
            }

            
            var _listPoEDueDates = (List<PieceOfEquipment>)result.Data;

            //_listPoEDueDates= _listPoEDueDates.Where()

            int conteo = 0;

            if(lstWeightSet != null)
            {
                conteo= lstWeightSet.Count;
            }
                




            if (_listPoEDueDates == null || _listPoEDueDates?.Count == 0)
            {
                //Console.WriteLine("Empty return pop");

                throw new Exception("No weights configured");
            }

            

            //await ClearWeightSet2();
            var iswe = _listPoEDueDates.Where(x => x.WeightSets != null && x.WeightSets.Count > 0).FirstOrDefault();

            var poeconfig = _listPoEDueDates.Where(x => x.EquipmentTemplate?.EquipmentTypeObject?.HasStandardConfiguration == true).FirstOrDefault();   //ElementAtOrDefault(0);

            if(iswe == null && poeconfig== null)
            {

            }


            if (

                (EquipmentTypeObject?.HasStandardConfiguration == true
                //|| poeconfig?.EquipmentTemplate?.EquipmentTypeObject?.HasWorkOrdeDetailStandard==true
                ) && iswe == null)
            {

           
                await SelectStandard2(_listPoEDueDates, conteo);

                return;
            
            }
            else if (iswe == null && EquipmentTypeObject?.HasStandardConfiguration == false)
            {

                await ShowToast("Cannot assign standard please check the configuration", Blazed.Controls.Toast.ToastLevel.Warning);
            }




            foreach (PieceOfEquipment PieceOfEquipment in _listPoEDueDates.DistinctBy(x=>x.PieceOfEquipmentID))
            {
                if (PieceOfEquipment.WeightSets != null)
                {

                    Description = PieceOfEquipment.PieceOfEquipmentID + " " + PieceOfEquipment.SerialNumber;

                    if (lstWeightSet == null)
                    {
                        lstWeightSet = new List<WeightSet>();
                    }

                    foreach (var item in PieceOfEquipment.WeightSets.DistinctBy(x=>x.WeightSetID))
                    {
                        var a = lstWeightSet?.Where(x => x.WeightSetID == item.WeightSetID && (string.IsNullOrEmpty(Option) == string.IsNullOrEmpty(x.Option) || x.Option == Option)).FirstOrDefault();


                        //var exist1 = lstWeightSet.Where(x => x.PieceOfEquipmentID == PieceOfEquipment.PieceOfEquipmentID && (string.IsNullOrEmpty(Option) == string.IsNullOrEmpty(x.Option) || x.Option == Option)).FirstOrDefault();


                        if (a == null)
                        {
                            item.PieceOfEquipmentID = PieceOfEquipment.PieceOfEquipmentID;

                            item.Option = Option;
                            lstWeightSet.Add(item);
                        }
                        else
                        {

                        }

                    }


                    //if (lstWeightSet.Count <= conteo)
                    //{

                    //    return;
                    //}


                    ListPOE.Add(PieceOfEquipment);
                    var queryLastNames =
                    from student in lstWeightSet
                    group student by student.PieceOfEquipmentID into newGroup
                    orderby newGroup.Key
                    select newGroup;

                    if (Group2 == null)
                    {
                        Group2 = new List<WeightSet>();
                    }
                    Group2.Clear();
                    foreach (var item3 in queryLastNames)
                    {
                        int cont = 0;
                        foreach (var item4 in item3)
                        {
                            if (cont == 0)
                            {
                                Group2.Add(item4);
                            }
                            cont = cont + 1;

                        }
                    }

                  
                   
                }
                //else
                //{
                //    throw new Exception("No weights configured");
                //}

                foreach (var item45 in lstWeightSet)
                {
                    if(!string.IsNullOrEmpty(Option))
                    {
                        item45.Option = Option;
                    }
                  
                }

                PieceOfEquipment.WeightSets = lstWeightSet;
                ChangeEventArgs arg = new ChangeEventArgs();

                arg.Value = PieceOfEquipment;

                

                if(lstWeightSet.Count == 0)
                {
                    await ShowToast("Unconfigured weight sets", Blazed.Controls.Toast.ToastLevel.Warning);
                }
                else
                {
                    if(WorkOrderItemCreate != null && ChangeAdd != null)
                    {
                        //WorkOrderItemCreate.StandardsToAssing = arg;
                        await ChangeAdd(arg);//WorkOrderItemCreate.ChangeDescripcion(arg);
                    }
                    
                    //await ChangeDescription.InvokeAsync(arg);
                }
            }
            //PieceOfEquipment PieceOfEquipment = (PieceOfEquipment)result.Data;



            StateHasChanged();



        }


        public async Task SelectWeightSet333()
        {


            //var parameters = new ModalParameters();
            //parameters.Add("SelectOnly", true);
            //parameters.Add("IsModal", true);
            //parameters.Add("Accredited", Accredited);

            //var messageForm = Modal.Show<CalibrationSaaS.Infraestructure.Blazor.Pages.AssetsBasics.POEWeightSets_Search>("Weight Sets", parameters);
            //var result = await messageForm.Result;
            //Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();


            var parameters = new ModalParameters();
            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);
            parameters.Add("Checkbox", false);
            parameters.Add("Indicator", true);
            if (!string.IsNullOrEmpty(Option))
            {
                parameters.Add("Parameter1",Option);
            }

            parameters.Add("CalibrationTypeID", WorkOrderItemCreate.eq.GetCalibrationTypeID());
            parameters.Add("Accredited", WorkOrderItemCreate.eq.IsAccredited);
            parameters.Add("pieceOfEquipmentsCh", ListPOE);
            ModalOptions op = new ModalOptions();
            op.ContentScrollable = true;
            op.Class = "blazored-modal " + ModalSize.MediumWindow;
            //PieceOfEquipmentDueDate_Search
            
            var messageForm = Modal.Show<CalibrationSaaS.Infraestructure.Blazor.GenericMethods.POEWeightSet_Select>("Select Standard", parameters, op);
            var result = await messageForm.Result;
            Console.Write("result " + result);


            if (!result.Cancelled)
                _message = result.Data?.ToString() ?? string.Empty;

            //EmptyValidationDictionary = result.Data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            //      .ToDictionary(prop => prop.Name, prop => prop.GetValue(result.Data, null));

            if (result.Data == null)
            {
                return;
            }


            //PieceOfEquipment PieceOfEquipment = (PieceOfEquipment)result.Data;

            _listPoEDueDate.Clear();
            var _listPoEDueDates = (List<PieceOfEquipment>)result.Data;

            int conteo = lstWeightSet.Count;

            


            if(_listPoEDueDates== null || _listPoEDueDates?.Count == 0)
            {
                //Console.WriteLine("Empty return pop");

                throw new Exception("No weights configured");
            }

            
            foreach (PieceOfEquipment PieceOfEquipment in _listPoEDueDates)
            {
                if (PieceOfEquipment.WeightSets != null)
                {

                    Description = PieceOfEquipment.PieceOfEquipmentID + " " + PieceOfEquipment.SerialNumber;



                    foreach (var item in PieceOfEquipment.WeightSets)
                    {
                        var a = lstWeightSet.Where(x => x.WeightSetID == item.WeightSetID && x.Option == Option).FirstOrDefault();

                        if (a == null)
                        {
                            item.PieceOfEquipmentID = PieceOfEquipment.PieceOfEquipmentID;

                            item.Option = Option;
                            lstWeightSet.Add(item);
                        }
                        else
                        {

                        }

                    }
                    if (lstWeightSet.Count <= conteo)
                    {

                        return;
                    }


                    ListPOE.Add(PieceOfEquipment);
                    var queryLastNames =
                    from student in lstWeightSet
                    group student by student.PieceOfEquipmentID into newGroup
                    orderby newGroup.Key
                    select newGroup;

                    if (Group2 == null)
                    {
                        Group2 = new List<WeightSet>();
                    }
                    Group2.Clear();
                    foreach (var item3 in queryLastNames)
                    {
                        int cont = 0;
                        foreach (var item4 in item3)
                        {
                            if (cont == 0)
                            {
                                Group2.Add(item4);
                            }
                            cont = cont + 1;

                        }
                    }

                    //lstWeightSet = lstWeightSet.Where(x => x.PieceOfEquipmentID == POE).ToList();

                    //foreach (var item in lstWeightSet)
                    //{
                    //    //item.PieceOfEquipmentID = PieceOfEquipment.PieceOfEquipmentID;

                    //    await Grid.SaveNewItem(item); //Add(item);
                    //}

                    //foreach (var item45 in lstWeightSet)
                    //{
                    //    item45.Option = Option;
                    //}
                    PieceOfEquipment.WeightSets = lstWeightSet;
                    ChangeEventArgs arg = new ChangeEventArgs();

                    arg.Value = PieceOfEquipment;

                    await ChangeAdd(arg);


                    
                }
                else
                {
                    //throw new Exception("No weights configured");
                }

            }

            StateHasChanged();




        }


        public async Task CancelItem()
        {
            if (Group2 != null && Group2.Count > 0)
            {
                await CancelItem(Group2.ElementAtOrDefault(0));
            }
        }
        public string POE { get; set; }
        public async Task CancelItem(WeightSet arg)
        {

            POE = arg.PieceOfEquipmentID;

            //lstWeightSet = lstWeightSet.Where(x => x.PieceOfEquipmentID == POE).ToList();
            //Grid.Clear();
            //foreach (var item in lstWeightSet)
            //{
            //    //item.PieceOfEquipmentID = PieceOfEquipment.PieceOfEquipmentID;

            //    await Grid.SaveNewItem(item); //Add(item);
            //}

            await Grid.FilterList(arg);


        }

        public async Task SelectStandard()
        {


            var parameters = new ModalParameters();
            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);
            parameters.Add("Checkbox", false);
            parameters.Add("Indicator", true);
            parameters.Add("Component", Component);
            if (!string.IsNullOrEmpty(Option))
            {
                parameters.Add("Parameter1", Option);
            }

            if (!CalibrationTypeID.HasValue)
            {
                parameters.Add("CalibrationTypeID", WorkOrderItemCreate.eq.GetCalibrationTypeID());
            }
            else
            {
                parameters.Add("CalibrationTypeID", CalibrationTypeID.Value);
            }
            parameters.Add("Accredited", WorkOrderItemCreate.eq.IsAccredited);

            if (WorkOrderItemCreate.eq?.WOD_Standard?.Count > 0)
            {
                foreach (var item in WorkOrderItemCreate.eq.WOD_Standard)
                {

                    if (item.Standard != null)
                    {
                        PieceOfEquipment _poe = new PieceOfEquipment();
                        _poe = new PieceOfEquipment()
                        {
                            //PieceOfEquipmentID = ID,
                            //SerialNumber = listPoe.SerialNumber
                            PieceOfEquipmentID = item.PieceOfEquipmentID,
                            
                        };


                        _listPoEDueDate.Add(_poe);
                    }


                }
            }


            parameters.Add("pieceOfEquipmentsCh", _listPoEDueDate);


            ModalOptions op = new ModalOptions();
            op.ContentScrollable = true;
            op.Class = "blazored-modal " + ModalSize.MediumWindow;
            //PieceOfEquipmentDueDate_Search
            //var messageForm = Modal.Show<CalibrationSaaS.Infraestructure.Blazor.Pages.AssetsBasics.POEWeightSet_Select>("Select", parameters, op);

            var messageForm = Modal.Show<CalibrationSaaS.Infraestructure.Blazor.GenericMethods.POEWeightSet_Select>("Select Standard", parameters, op);
            var result = await messageForm.Result;
            Console.Write("result " + result);


            if (!result.Cancelled)
                _message = result.Data?.ToString() ?? string.Empty;

            if (result.Data == null)
            {
                return;
            }


            var _listPoEDueDates = (List<PieceOfEquipment>)result.Data;

            int conteo = lstWeightSet.Count;

            if (_listPoEDueDates == null || _listPoEDueDates?.Count == 0)
            {
                //Console.WriteLine("Empty return pop");

                throw new Exception("No weights configured");
            }


            foreach (PieceOfEquipment PieceOfEquipment in _listPoEDueDates)
            {
                if (PieceOfEquipment.DueDate >= DateTime.Now)
                {

                    Description = PieceOfEquipment.PieceOfEquipmentID + " " + PieceOfEquipment.SerialNumber;

                    WeightSet item = new WeightSet();

                    item.PieceOfEquipmentID = PieceOfEquipment.PieceOfEquipmentID;

                    item.PieceOfEquipment = PieceOfEquipment;

                    var exist = lstWeightSet.Where(x => x.PieceOfEquipmentID == item.PieceOfEquipmentID && (string.IsNullOrEmpty(Option) == string.IsNullOrEmpty(x.Option) || x.Option == Option)).FirstOrDefault();




                    //if (exist != null)
                    //{
                    //    await ShowToast("Already Standard", Blazed.Controls.Toast.ToastLevel.Info);

                    //    return;
                    //}

                    if (!string.IsNullOrEmpty(Option)) 
                    {
                        item.Option = Option;
                    }
                


                    lstWeightSet.Add(item);
                    if (lstWeightSet.Count <= conteo)
                    {

                        return;
                    }


                    ListPOE.Add(PieceOfEquipment);
                    var queryLastNames =
                    from student in lstWeightSet
                    group student by student.PieceOfEquipmentID into newGroup
                    orderby newGroup.Key
                    select newGroup;

                    if (Group2 == null)
                    {
                        Group2 = new List<WeightSet>();
                    }
                    Group2.Clear();
                    foreach (var item3 in queryLastNames)
                    {
                        int cont = 0;
                        foreach (var item4 in item3)
                        {
                            if (cont == 0)
                            {
                                Group2.Add(item4);
                            }
                            cont = cont + 1;

                        }
                    }

                    //foreach (var item45 in lstWeightSet)
                    //{
                    //    item45.Option = Option;
                    //}
                    PieceOfEquipment.WeightSets = lstWeightSet;
                    ChangeEventArgs arg = new ChangeEventArgs();

                    arg.Value = PieceOfEquipment;

                    await ChangeAdd(arg);


                    
                }

                else
                {
                    await ShowToast("Not Valid Due Date", Blazed.Controls.Toast.ToastLevel.Info);

                    //return;

                }
            }
            StateHasChanged();

        }

        public async Task SelectStandard2(List<PieceOfEquipment> _listPoEDueDates, int conteo)
        {


            

            foreach (PieceOfEquipment PieceOfEquipment in _listPoEDueDates)
            {

                var exist1 = lstWeightSet.Where(x => x.PieceOfEquipmentID == PieceOfEquipment.PieceOfEquipmentID && (string.IsNullOrEmpty(Option)==string.IsNullOrEmpty(x.Option) || x.Option == Option)).FirstOrDefault();


                if (exist1 == null && PieceOfEquipment.DueDate >= DateTime.Now)
                {

                    Description = PieceOfEquipment.PieceOfEquipmentID + " " + PieceOfEquipment.SerialNumber;

                    WeightSet item = new WeightSet();

                    item.PieceOfEquipmentID = PieceOfEquipment.PieceOfEquipmentID;

                    item.PieceOfEquipment = PieceOfEquipment;

                    var exist = lstWeightSet.Where(x => x.PieceOfEquipmentID == item.PieceOfEquipmentID && x.Option == Option).FirstOrDefault();

                    //if (exist != null)
                    //{
                    //    await ShowToast("Already Standard", Blazed.Controls.Toast.ToastLevel.Info);

                    //    return;
                    //}

                    if (!string.IsNullOrEmpty(Option))
                    {
                        item.Option = Option;
                    }



                    lstWeightSet.Add(item);
                    if (lstWeightSet.Count <= conteo)
                    {

                        return;
                    }


                    ListPOE.Add(PieceOfEquipment);
                    var queryLastNames =
                    from student in lstWeightSet
                    group student by student.PieceOfEquipmentID into newGroup
                    orderby newGroup.Key
                    select newGroup;

                    if (Group2 == null)
                    {
                        Group2 = new List<WeightSet>();
                    }
                    Group2.Clear();
                    foreach (var item3 in queryLastNames)
                    {
                        int cont = 0;
                        foreach (var item4 in item3)
                        {
                            if (cont == 0)
                            {
                                Group2.Add(item4);
                            }
                            cont = cont + 1;

                        }
                    }

                    //foreach (var item45 in lstWeightSet)
                    //{
                    //    item45.Option = Option;
                    //}
                    PieceOfEquipment.WeightSets = lstWeightSet;
                    ChangeEventArgs arg = new ChangeEventArgs();

                    arg.Value = PieceOfEquipment;

                    await ChangeAdd(arg);



                }

                else if(exist1 == null)
                {
                    await ShowToast("Not Valid Due Date", Blazed.Controls.Toast.ToastLevel.Info);

                    //return;

                }


            }
            StateHasChanged();

        }

        public async Task ClearWeightSet2()
        {

            Group = new List<WeightSet>();

            Grid.DeleteAll();

            //lstWeightSet.Clear();

            Group2.Clear();

            await ClearGrid.InvokeAsync(default);

            //StateHasChanged();


        }

        public async Task ClearWeightSet()
        {

            Group = new List<WeightSet>();

            Grid.DeleteAll();

            lstWeightSet.Clear();

            Group2.Clear();

            await ClearGrid.InvokeAsync(default);

            //StateHasChanged();


        }


        public EditContext CurrentEditContext { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                EquipmentTypeObject = WorkOrderItemCreate?.eq?.PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject;
            }


        }
        protected override Task OnInitializedAsync()
        {


            if (Group != null)
            {
                CurrentEditContext = new EditContext(Group);
            }

            

            Dictionary<string, GridColumnBase> fields= new Dictionary<string, GridColumnBase>();


            if (!string.IsNullOrEmpty(JsonFields))
            {
                Fields = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, GridColumnBase>>(JsonFields);
            }


            GridColumnBase cb = new GridColumnBase();

            cb.Field = "Test";
            cb.Title = "TitleTest";
            cb.DefaultSort = true;
            cb.CSScol = "col-2";


            fields.Add("Test", cb);

            GridColumnBase cb2 = new GridColumnBase();

            cb2.Field = "Test2";
            cb2.Title = "TitleTest2";
            cb2.DefaultSort = true;
            cb2.CSScol = "col-2";


            fields.Add("Test2", cb2);

            string item = Newtonsoft.Json.JsonConvert.SerializeObject(fields);


            //'{"Test":{"Field":"Test","Title":"TitleTest","CSScol":"col-2","DefaultSort":true,"HasSorting":true,"Value":null,"Format":null,"IsVisble":true,"IsToolTip":false,"ToolTipField":null,"ControlType":null},"Test2":{"Field":"Test2","Title":"TitleTest2","CSScol":"col-2","DefaultSort":true,"HasSorting":true,"Value":null,"Format":null,"IsVisble":true,"IsToolTip":false,"ToolTipField":null,"ControlType":null}}'

            return base.OnInitializedAsync();   


        }



        public void GroupWeights(List<WeightSet> groupi)
        {

            if (groupi == null || groupi.Count == 0)
            {

                return;
            }
            if (Group2 == null)
            {
                Group2 = new List<WeightSet>();
            }
            //if (!string.IsNullOrEmpty(Option))
            //{
            //    groupi = groupi.Where(x => x.Option == Option).ToList();
            //}
            var queryLastNames =
            from student in groupi
            group student by student.PieceOfEquipmentID into newGroup
            orderby newGroup.Key
            select newGroup;
            Group2.Clear();
            foreach (var item3 in queryLastNames)
            {
                int cont = 0;
                foreach (var item4 in item3)
                {
                    if (cont == 0)
                    {
                        Group2.Add(item4);
                        cont = cont + 1;
                    }
                }

            }
        }


        public async Task Show(List<WeightSet> groupi)
        {


            if (groupi != null)
            {
                //if (!string.IsNullOrEmpty(Option))
                //{
                //    groupi= groupi.Where(x=>x.Option== Option).ToList();    
                //}


                if (Group2 == null)
                {
                    Group2 = new List<WeightSet>();
                }
                if (1 == 1)
                {
                    var queryLastNames =
                    from student in groupi
                    group student by student.PieceOfEquipmentID into newGroup
                    
                    orderby newGroup.Key
                    select newGroup;
                    Group2.Clear();
                    foreach (var item3 in queryLastNames)
                    {
                        int cont = 0;
                        foreach (var item4 in item3)
                        {
                            if (cont == 0)
                            {
                                Group2.Add(item4);
                                cont = cont + 1;
                            }
                        }
                    }
                }
                _Group = groupi;//.Where(x=>x.PieceOfEquipmentID==POE).ToList();


                if (Grid == null)
                {
                    return;
                }
                //RT.Items.ToList().Clear();
                //RT.ItemsDataSource.ToList();
                Grid.DeleteAll();
                Grid.Clear();

                var aaa = groupi.GroupBy(i => i.WeightSetID).Select(group => group.First());

                foreach (var item in aaa)
                {
                    await Grid.SaveNewItem(item); //Add(item);
                }


                // RT.Items = _Group;

                StateHasChanged();
            }


        }



        [Parameter]
        public bool Master { get; set; } = false;

       

        [Parameter]
        public bool Accredited { get; set; }
        public void ChangeCheckBox(ChangeEventArgs e)
        {
            if (Accredited)
            {
                Accredited = false;
            }
            else
            {
                Accredited = true;
            }

        }

        public async Task<IEnumerable<WeightSet>> Filter33(WeightSet w)
        {

            ResultSet<WeightSet> r = new ResultSet<WeightSet>();

            var a = lstWeightSet.Where(x => x.PieceOfEquipmentID == w.PieceOfEquipmentID).ToList();

            r.List = a;
            r.PageTotal = a.Count;

            return a;

        }
    }
}
