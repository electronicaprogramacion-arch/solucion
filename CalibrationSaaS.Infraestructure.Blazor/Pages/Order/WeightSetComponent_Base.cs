using Blazored.Modal;
using Blazored.Modal.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Order
{
    public class WeightSetComponent_Base : CalibrationSaaS.Infraestructure.Blazor.KavokuComponentBase<WeightSet>
    {

        [CascadingParameter(Name = "CascadeParam1")]
        public IWorkOrderItemCreate WorkOrderItemCreate { get; set; }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task<IEnumerable<WeightSet>> Filter33(WeightSet w)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            ResultSet<WeightSet> r = new ResultSet<WeightSet>();

            var a = lstWeightSet.Where(x => x.PieceOfEquipmentID == w.PieceOfEquipmentID).ToList();

            r.List = a;
            r.PageTotal = a.Count;

            return a;

        }

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

        [Parameter]
        public bool? Accredited { get; set; }

        [Parameter]
        public Status CurrentStatus { get; set; }


        [Parameter]
        public bool Certified { get; set; }

        [Parameter]
        public Func<ChangeEventArgs, Task> ChangeAdd { get; set; }


        [Parameter]
        public EventCallback<ChangeEventArgs> ClearGrid { get; set; }


        public string Description { get; set; } = "";

        [CascadingParameter] public IModalService Modal { get; set; }

        public WeightSet DefultTestPoint = new WeightSet();

        [Inject] public AppState AppState { get; set; }

        List<WeightSet> _Group;

        [Parameter]
        public List<WeightSet> lstWeightSet { get; set; } = new List<WeightSet>();


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

        public ResponsiveTable<WeightSet>
            Grid
        { get; set; } //= new ResponsiveTable<WeightSet>();

        //public void NewItem()
        //{

        //}
        [Parameter]
        public bool Master { get; set; } = false;

        public string POE { get; set; }

        public List<WeightSet> Group2;


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

        public List<WeightSet> Format()
        {

            return _Group;
        }

        [Parameter]
        public int GroupTestPointID { get; set; }

        public EditContext CurrentEditContext { get; set; }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnInitializedAsync()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            if (Group != null)
            {
                CurrentEditContext = new EditContext(Group);
            }



        }




        public async Task ClearWeightSet()
        {

            Group = new List<WeightSet>();

            Grid.DeleteAll();

            lstWeightSet.Clear();

            Group2.Clear();

            StateHasChanged();

            await ClearGrid.InvokeAsync(default);


        }



        public List<PieceOfEquipment> ListPOE { get; set; } = new List<PieceOfEquipment>();

        public string _message { get; set; }
        //Logger.LogDebug(Group.Name);
        public async Task SelectWeightSet()
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
            parameters.Add("Parameter1", 0);
             parameters.Add("CalibrationTypeID", WorkOrderItemCreate.eq.CalibrationTypeID);
            parameters.Add("Accredited", WorkOrderItemCreate.eq.IsAccredited);
            ModalOptions op = new ModalOptions();
            op.ContentScrollable = true;
            op.Class = "blazored-modal " + ModalSize.MediumWindow;
            //PieceOfEquipmentDueDate_Search
            //var messageForm = Modal.Show<CalibrationSaaS.Infraestructure.Blazor.Pages.AssetsBasics.POEWeightSet_Select>("Select", parameters, op);
            var messageForm = Modal.Show<CalibrationSaaS.Infraestructure.Blazor.GenericMethods.POEWeightSet_Select>("Select", parameters, op);
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


            PieceOfEquipment PieceOfEquipment = (PieceOfEquipment)result.Data;

            int conteo = lstWeightSet.Count;
            //eq.PieceOfEquipment.EquipmentTemplate = Template;
            //eq.PieceOfEquipment.EquipmentTemplateId = Template.EquipmentTemplateID;
            if (PieceOfEquipment.WeightSets != null && PieceOfEquipment.DueDate >= DateTime.Now)
            {

                Description = PieceOfEquipment.PieceOfEquipmentID + " " + PieceOfEquipment.SerialNumber;



                foreach (var item in PieceOfEquipment.WeightSets)
                {
                    var a = lstWeightSet.Where(x => x.WeightSetID == item.WeightSetID).FirstOrDefault();

                    if (a == null)
                    {
                        item.PieceOfEquipmentID = PieceOfEquipment.PieceOfEquipmentID;
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


                PieceOfEquipment.WeightSets = lstWeightSet;
                ChangeEventArgs arg = new ChangeEventArgs();

                arg.Value = PieceOfEquipment;

                await ChangeAdd(arg);


                StateHasChanged();
            }

            //_EquipmentTemplateID = EmptyValidationDictionary["EquipmentTemplateID"].ToString();
            //_Model = EmptyValidationDictionary["Model"].ToString();
            //_Name = EmptyValidationDictionary["Name"].ToString();
            //_Manufacturer = EmptyValidationDictionary["Manufacturer"].ToString();


            //eq.EquipmentTemplateId = Convert.ToInt32(EmptyValidationDictionary["EquipmentTemplateID"].ToString());
            //_message = "";


        }

        public void ChangeCheckBox(ChangeEventArgs e)
        {
            if (Accredited.HasValue && Accredited.Value)
            {
                Accredited = false;
            }
            else
            if (!Accredited.HasValue )
            
            {
                Accredited = true;
            }

        }

    }
}
