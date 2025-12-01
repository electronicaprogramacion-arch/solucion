using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Helper;
using CalibrationSaaS.Infraestructure.Blazor.Services;

using Grpc.Core;
using CalibrationSaaS.Domain.Aggregates.Querys;
using Helpers;
using System.Linq.Expressions; 
using CalibrationSaaS.Domain.Aggregates.Entities;

using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System.Threading.Tasks;
using System;
using Blazed.Controls;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Forms;
using Blazed.Controls.Toast;

namespace BlazorApp1.Client.Pages.Basics.TestPoints
{
    public partial class GroupTestPoint:CalibrationSaaS.Infraestructure.Blazor.KavokuComponentBase<int>
    {

        [Parameter]
        public bool ActionBar { get; set; }

        [Parameter]
        public bool ShowResolution { get; set; } = true;

        public int OutUnitOfMeasureId { get; set; }
        private double _Capacity;
        public double Capacity 
        { get 
            
            {
                if (inici)
                {
                    return _Capacity;
                }
                else
                {
                    return Eq.Capacity;
                }
                
            
            
            }


            set
            {
                _Capacity = value;
            } 
        
        
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (Eq != null && Eq?.UnitofmeasurementID.HasValue==true && Group != null && Eq?.EquipmentTemplateID == 0 && Eq?.UnitofmeasurementID.HasValue==true)
        {
           
            //Group.OutUnitOfMeasurementID = Eq.UnitofmeasurementID.Value;
            //    OutUnitOfMeasureId = Eq.UnitofmeasurementID.Value;

        }
        if (firstRender)
        {
            Load();

            }



    }


    protected TestPoint RowExceute(TestPoint t)
    {
        //TestPoint t = new TestPoint();

        if (RT.IsDragAndDrop)
        {
            int posi = 100;
            var tmpg = RT.ItemList;
            foreach (var item in tmpg)
            {
                item.Position = posi;
                posi++;
            }



            return t;
        }

        if (t.Position >= 100)
        {
            return t;
        }


        var a = t.NominalTestPoit.ToStringNoTruncate((int)t.DecimalNumber);

        t.NominalTestPoit = a;

        int cont = 100;

        var aarrl = new List<TestPoint>();

        aarrl = RT.Items.Where(x => x.CalibrationType.ToLower() == "linearity" && x.IsDescendant==false)
            .OrderBy(x => x.NominalTestPoit).ToList();

        aarrl.ForEach(item22 =>
        {

            item22.Position = cont;
            cont++;

        });


        var aarr2 = RT.Items.Where(x => x.CalibrationType.ToLower() == "linearity" && x.IsDescendant == true)
            .OrderByDescending(x => x.NominalTestPoit).ToList();

        aarr2.ForEach(item22 =>
        {

            item22.Position = cont;
            cont++;

        });

        foreach(var item in aarr2)
        {
            aarrl.Add(item);
        }


        var arr3 = RT.Items.Where(x => x.CalibrationType.ToLower() == "eccentricity").OrderBy(x => x.NominalTestPoit).OrderBy(x => x.IsDescendant).ToList();
        arr3.ForEach(x =>
        {
            x.Position = cont;
            cont++;
        });

        var arr4 = RT.Items.Where(x => x.CalibrationType.ToLower() == "repeatability").OrderBy(x => x.NominalTestPoit).OrderBy(x => x.IsDescendant).ToList();
        arr4.ForEach(y =>
        {
            y.Position = cont;
            cont++;
        });



        foreach(var it in arr3)
        {
            aarrl.Add(it);
        }

        foreach (var it in arr4)
        {
            aarrl.Add(it);
        }


        RT.Clear();

            //var maxTestPointIDEecc = aarrl.Where(tp => tp.CalibrationType.ToLower() == "eccentricity").Max(tp => tp.TestPointID);

            //var ecc = aarrl.Where(tp => tp.CalibrationType.ToLower() == "eccentricity" && tp.TestPointID != maxTestPointIDEecc);

            //if (ecc.Any())
            //   {
            //foreach (var it in ecc)
            //    { 
            //        aarrl.Remove(it);
            //    }
            //}

            //var maxTestPointIDErep = aarrl.Where(tp => tp.CalibrationType.ToLower() == "repeatability").Max(tp => tp.TestPointID);

            //var rep = aarrl.Where(tp => tp.CalibrationType.ToLower() == "repeatability" && tp.TestPointID != maxTestPointIDErep);

            //if (ecc.Any())
            //{
            //    foreach (var it in rep)
            //    {
            //        aarrl.Remove(it);
            //    }
            //}
            RT.Items = aarrl;

        //cont++;
        return t;

    }


    protected TestPoint NewItem()
    {
        TestPoint t = new TestPoint();

        if (Eq.UnitofmeasurementID.HasValue)
        {
            t.UnitOfMeasurementID = Eq.UnitofmeasurementID.Value;
        }

        t.CalibrationType = "Linearity";
        t.Resolution = Eq.Resolution;
        t.DecimalNumber = Eq.DecimalNumber;
            t.UnitOfMeasurementOutID = OutUnitOfMeasureId;//Group.OutUnitOfMeasurementID;
        t.Position = RT.Items.Count;
        t.TestPointID = NumericExtensions.GetUniqueID(t.TestPointID);

        return t;

    }



    public void Changetype(ChangeEventArgs test)
    {

        var testpoints = RT.Items;

        var a = testpoints.Where(Querys.WOD.GetEccentricityTestPoint().Compile()).FirstOrDefault();

        var b = testpoints.Where(Querys.WOD.GetRepeatibilityTestPoint().Compile()).FirstOrDefault();

        if (a != null || b != null)
        {

        }


    }

    public bool HasResolution { get; set; } = true;


    [Parameter]
    public string ContainerCss { get; set; } = "";


    public EditContext editContext { get; set; }


   


    [Parameter]
    public bool Enabled { get; set; }

    public string Message { get; set; }

    public int NumberTestPoint { get; set; } = 5;

    //[CascadingParameter(Name ="EquipmentTemplate")]
    [Parameter]
    public EquipmentTemplate Eq { get; set; }


    public TestPoint TestPoint { get; set; }


        public bool inici { get; set; }
        protected async void Change2(ChangeEventArgs arg)
        {

            inici = true;
            //var value2 = Eq.Capacity;
            var val = arg.Value.ToString();
            decimal value;
            var obj = decimal.TryParse(arg.Value.ToString(), out value);
            Capacity = Convert.ToDouble(value);

            //Eq.Capacity = value2;

        }

    public void ChangeUoM(ChangeEventArgs arg)
    {
        var val = arg.Value.ToString();
        if (int.TryParse(val, out int uomId))
        {
            Group.OutUnitOfMeasurementID = uomId;
            OutUnitOfMeasureId = uomId;

            // Actualizar el UoM en todos los TestPoints existentes
            if (Group?.TestPoints != null && Group.TestPoints.Count > 0)
            {
                foreach (var testPoint in Group.TestPoints)
                {
                    testPoint.UnitOfMeasurementID = uomId;
                    testPoint.UnitOfMeasurementOutID = uomId;

                    // Actualizar el objeto UnitOfMeasurement
                    var uom = AppState.UnitofMeasureList?.FirstOrDefault(x => x.UnitOfMeasureID == uomId);
                    if (uom != null)
                    {
                        testPoint.UnitOfMeasurement = uom;
                        testPoint.UnitOfMeasurementOut = uom;
                    }
                }
            }

            StateHasChanged();
        }
    }

    public void Change(TestPoint TestPoint, ChangeEventArgs arg)
    {
        var val = arg.Value.ToString();
        decimal value;
        var obj = decimal.TryParse(arg.Value.ToString(), out value);

        if (obj && value > 0)
        {

            var tp = CalibrationSaaS.Infraestructure.Blazor.Helper.Calculate.CalculateResolution((decimal)Capacity, TestPoint, value);//CalibrationSaaS.Infraestructure.Blazor.Helper.Calculate.CalculateTestPoint(TestPoint, null, 0, 0,0);

            TestPoint.DecimalNumber = (Int32)tp;
        }

    }


    async Task ChangeTolerance(TestPoint TestPoint, ChangeEventArgs arg)
    {


        var val = arg.Value.ToString();

        if (string.IsNullOrEmpty(val))
        {
            return;
        }

        decimal value;
        var obj = decimal.TryParse(arg.Value.ToString(), out value);

        if (obj && value > 0)
        {

            var tp = CalibrationSaaS.Infraestructure.Blazor.Helper.Calculate.CalculateTestPoint((double)value, TestPoint, Eq.Ranges?.ToList(), Eq.Tolerance.AccuracyPercentage, Eq.DecimalNumber, 0);

            TestPoint.CopyPropertiesFrom(tp);
        }

        ///StateHasChanged();
    }



    TestPoint DefultTestPoint = new TestPoint();

    [Inject] public AppState AppState { get; set; }

    [Inject] public CalibrationSaaS.Application.Services.IBasicsServices<CallContext> Client { get; set; }

    TestPointGroup _Group;


    public TestPointGroup Group
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

    public ResponsiveTable<TestPoint>
            RT
    { get; set; } = new ResponsiveTable<TestPoint>();



    public void Show(TestPointGroup group)
    {

        if (group != null)
        {
            _Group = group;

            StateHasChanged();
        }


    }

    public TestPointGroup Format()
    {

        return _Group;
    }

    [Parameter]
    public int GroupTestPointID { get; set; }


    public EditContext ec;

        //Dictionary<string, string> ToleranceList;

        [Parameter]
    public List<ToleranceType> ToleranceList { get; set; }

    public void Load()
    {
        DefultTestPoint.UnitOfMeasurement = new UnitOfMeasure();
        DefultTestPoint.UnitOfMeasurementOut = new UnitOfMeasure();
        //ToleranceList = AppState.ToleranceList;

        SwitchList = new List<SwitchItem>();

        SwitchItem switchItem = new SwitchItem();

        switchItem.Label = "Repeatability";

        SwitchItem switchItem2 = new SwitchItem();

        switchItem2.Label = "Eccentricity";

        SwitchList.Add(switchItem);
        SwitchList.Add(switchItem2);

        if ((Eq.TestGroups?.ElementAtOrDefault(0) == null))
        {
            HasResolution = false;

            Group = new TestPointGroup();

                Group.TestPoitGroupID = NumericExtensions.GetUniqueID(Group.TestPoitGroupID);


        }
        else if ((Eq.TestGroups?.ElementAtOrDefault(0) != null))
        {

            Group = Eq.TestGroups?.ElementAtOrDefault(0);

                Group.TestPoitGroupID = NumericExtensions.GetUniqueID(Group.TestPoitGroupID);



            }
        if (Eq.Tolerance.ToleranceTypeID == 0)
        {
            HasResolution = false;
        }


        Group.TypeID = "Linearity";
        if (Eq.UnitofmeasurementID.HasValue)
        {
            Group.OutUnitOfMeasurementID = Eq.UnitofmeasurementID.Value;
                OutUnitOfMeasureId = Eq.UnitofmeasurementID.Value;
        }

            //if ((!IsOnline || 1 == 1) && Group.TestPoitGroupID == 0)
            //{
                Group.TestPoitGroupID = NumericExtensions.GetUniqueID(Group.TestPoitGroupID);
            //}



            editContext = new EditContext(Group);
    }




    protected override async Task OnInitializedAsync()
    {

        //Load();

        await base.OnInitializedAsync();

            
    }



    public bool IsGenerate = false;

    protected async Task SaveGrid(ChangeEventArgs arg)
    {

        if (IsGenerate)
        {
            return;
        }

        var list = RT.Items;


        TestPoint obj = (TestPoint)arg.Value;

        if (obj.IsDescendant && list != null)
        {
            obj.Position = list.Count + 100;
        }

        if (obj.NominalTestPoit < 0)
        {
            await ShowError("Negative Value");
            throw new Exception("Negative Value");

        }

        if (obj.NominalTestPoit != 0 && obj.IsDescendant == false)
        {
            var a = list.Where(x => x.NominalTestPoit == obj.NominalTestPoit && obj.CalibrationType != x.CalibrationType).ToList();

            if (a != null && a.Count > 1)
            {
                await ShowError("Already Testpoint value");

                throw new Exception("Already Testpoint value");
            }
        }
            
            ChangeEventArgs arg2 = new ChangeEventArgs();
        arg2.Value = obj.NominalTestPoit;

        await ChangeTolerance(obj, arg2);

        ChangeEventArgs arg3 = new ChangeEventArgs();
        arg3.Value = obj.Resolution;

        Change(obj, arg3);

    }

    [Parameter]
    public bool AlwaysRender { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await  base.OnParametersSetAsync();
        if (AlwaysRender)
        {
            Load();
        }



    }


    protected async Task EditGrid(ChangeEventArgs arg)
    {


        var list = RT.Items;

        var c = RT.oldCurrentEditItem;

        TestPoint obj = RT.currentEdit;//(TestPoint)arg.Value;

        if (obj.IsDescendant && list!= null)
        {
            obj.Position = list.Count + 100;
        }

        if (obj.NominalTestPoit < 0)
        {
            await ShowError("Negative Value");
            throw new Exception("Negative Value");

        }

        if (obj.NominalTestPoit != 0 && obj.IsDescendant==false)
        {
            var a = list.Where(x => x.NominalTestPoit == obj.NominalTestPoit && x.CalibrationType==obj.CalibrationType).ToList();
            //&& c.NominalTestPoit == obj.NominalTestPoit
            if (a != null && a.Count > 1 )
            {
                await ShowError("Already Testpoint value");

                throw new Exception("Already Testpoint value");
            }
        }


        ChangeEventArgs arg2 = new ChangeEventArgs();
        arg2.Value = obj.NominalTestPoit;

        await ChangeTolerance(obj, arg2);

        ChangeEventArgs arg3 = new ChangeEventArgs();
        arg3.Value = obj.Resolution;

        Change(obj, arg3);

    }




    //Logger.LogDebug(Group.Name);

    public string CalibrationType { get; set; }


    void ChangeType(ChangeEventArgs arg)
    {

    }


        protected string Bound(TestPoint TestPoint)
    {
        if (TestPoint == null)
        {
            return "";
        }
        //TestPoint.CalibrationType = Group.TypeID;
        TestPoint.UnitOfMeasurementOutID = Group.OutUnitOfMeasurementID;
            TestPoint.UnitOfMeasurementOutID = OutUnitOfMeasureId;

        //TestPoint.Description = Group.Description;
        return "";
    }



    [Parameter]
    public Func<Task> funcGenerate { get; set; }


    [Parameter]
    public Func<Task> AfterGenerate { get; set; }

    public async Task Generate2(TestPointGroup test)
    {
        //await ShowProgress();
        await Generate(test);
        //await Generate(test);
        Console.WriteLine("Generate2 testpoints");
        //await CloseProgress();
        if (AfterGenerate != null)
        {
            await AfterGenerate();
        }

        //await RT.Reload();    

    }
    public async Task Generate(TestPointGroup test)
    {

            try
            {
                        IsGenerate = true;

        if (funcGenerate != null)
        {
            await funcGenerate();
        }



        if (NumberTestPoint == 0)
        {
            await ShowError("number of testpoints must be greater than zero");
            return;
        }

        if (Capacity == 0)
        {
            await ShowError("You must choose the capacity");
            return;
        }

        if (Eq.Resolution == 0)
        {
            await ShowToast("Resolution is Empty", ToastLevel.Warning);
            //return;
        }
        if (test.OutUnitOfMeasurementID == 0)
        {
            await ShowError("You must choose the UOM");
            return;
        }

        RT.Clear();
        test.TestPoints.Clear();

        List<TestPoint> lst = new List<TestPoint>();
        Linearity l = new Linearity();

        ChangeEventArgs arg = new ChangeEventArgs();
        arg.Value = test.OutUnitOfMeasurementID.ToString();
        test.UnitOfMeasurementOut = Helpers.NumericExtensions.Conversion<UnitOfMeasure>(test.UnitOfMeasurementOut,
            arg.Value, AppState.UnitofMeasureList, nameof(test.UnitOfMeasurementOut.UnitOfMeasureID));

                //Calculate.Conversion<UnitOfMeasure>(test., arg, AppState.UnitofMeasureList, nameof(test.UnitOfMeasurementOut.UnitOfMeasureID));

                var CapacityReal = Capacity;  /*QueryableExtensions2.ConversionUOM(Eq.UnitofmeasurementID, Capacity, test.OutUnitOfMeasurementID,AppState.UnitofMeasureList);*/

         

            if (NumberTestPoint == 0)
            {
                NumberTestPoint = 1;
            }

            int step = (int)(CapacityReal / NumberTestPoint);

                if (Eq?.EquipmentTypeObject?.HasReturnToZero==true)
                {
                    var t0 = CreateTestPoint(0, test, 0, "Linearity");

                    lst.Add(t0);
                }
       

        for (int i = 1; i < NumberTestPoint + 1; i++)
        {
            TestPoint t = new TestPoint();
            var s = (i) * step;
            t = CreateTestPoint(s, test,i, "Linearity");


            lst.Add(t);
        }
        
               
       
                if (Eq?.EquipmentTypeObject?.HasReturnToZero==true)
                {
                    int mid = (int)(Capacity / 2);

                    var tfinal0 = CreateTestPoint(mid, test, NumberTestPoint + 1, "Linearity", true);

                    var tfinal = CreateTestPoint(0, test, NumberTestPoint + 2, "Linearity", true);

                    lst.Add(tfinal0);
                    lst.Add(tfinal);
                }

                if (Eq?.EquipmentTypeObject?.HasRepeateabilityAndEccentricity==true)
                {

                    int v = (int)(Capacity / 2);

                    if (SwitchList[0].Value == true)
                    {
                        TestPoint t1 = CreateTestPoint(v, test, lst.Count, "Repeatability");
                        lst.Add(t1);
                    }
                    if (SwitchList[1].Value == true)
                    {
                        TestPoint t2 = CreateTestPoint(v, test, lst.Count, "Eccentricity");

                        lst.Add(t2);
                    }

                }
                                   

        //RT.Clear();

        for (int ii=0; ii<lst.Count;ii++)
        {
            if(ii < lst.Count - 1)
            {
                await RT.SaveNewItem(lst[ii],null,null, false);
            }
            else
            {
                await RT.SaveNewItem(lst[ii]);
            }

        }

        IsGenerate = false;
        //await RT.SaveNewItem(t);
        //Console.WriteLine(RT.Items.Count);

            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);
            }



    }



    private TestPoint CreateTestPoint(int step, TestPointGroup test, int pos, string Type = "Linearity" ,bool? IsDecendant=null )
    {
        TestPoint t = new TestPoint();

        if (IsDecendant.HasValue && IsDecendant==true)
        {
            t.IsDescendant = true;
        }

            int step2 = 0;
            
            if(pos > 0)
            {
                step2= step / pos;
            }
            
        
            t.Position = pos;
            if(step2 >= 10)
            {
                t.NominalTestPoit = step.Ceiling10();
            }
            else
            {
                t.NominalTestPoit = (double)Math.Ceiling(Convert.ToDecimal(step));
            }
        
        t.UnitOfMeasurement = test.UnitOfMeasurementOut;
        t.UnitOfMeasurementID = test.OutUnitOfMeasurementID;

        t.UnitOfMeasurementOut = test.UnitOfMeasurementOut;
        t.UnitOfMeasurementOutID = test.OutUnitOfMeasurementID;

        t.CalibrationType = Type;

        t.Resolution = Eq.Resolution;

        t.Description = test.Description;

        var tp = CalibrationSaaS.Infraestructure.Blazor.Helper.Calculate.CalculateTestPoint(t.NominalTestPoit, t, Eq.Ranges?.ToList(), Eq.Tolerance.AccuracyPercentage, Eq.DecimalNumber, 0);

        t.CopyPropertiesFrom(tp);

            //if ((!IsOnline || 1 == 1) && t.TestPointID == 0)
            //{
                t.TestPointID = NumericExtensions.GetUniqueID(t.TestPointID);
            //}

            return t;
    }


    public void MapObject()
    {
        //Console.WriteLine(RT.Items.Count);
        if(RT== null || RT?.Items == null || RT?.Items?.Count == 0)
        {
            return;
        }


        if (Eq == null)
        {
            //Console.WriteLine("GT Items22");
            Eq = new EquipmentTemplate();
            Eq.TestGroups = new List<TestPointGroup>();
        }
        Console.WriteLine("GT Items");
        Console.WriteLine(RT.ItemsDataSource.Count());
        Console.WriteLine(Group.TestPoints.Count);

        if (RT.Items.Count > 0)
        {
            var listtestpoint = RT.Items;


            var group = Group;



            //foreach (var item in Eq.TestGroups)
            //{
            //foreach (var item2 in listtestpoint)

            listtestpoint.ForEach(item2 =>
            {


                if (string.IsNullOrEmpty(item2.CalibrationType))
                {

                    Message = "Method cannot be empty";

                    return;
                }

                item2.UnitOfMeasurement = null;
                item2.UnitOfMeasurementOut = null;

            });

            //}


            group.TestPoints = listtestpoint;

            Eq.TestGroups = new List<TestPointGroup>();


                //if ((!IsOnline || 1 == 1 ) && group.TestPoitGroupID == 0)
                //{
                    group.TestPoitGroupID = NumericExtensions.GetUniqueID(group.TestPoitGroupID);
                //}

                Eq.TestGroups.Add(group);
        }
        else
        {
            Console.WriteLine("GT no load");
        }
    }


    public async Task FormSubmit(EditContext e)
    {
        await ShowProgress();

        //llena la propiedad base creo que no va

        MapObject();


        try
        {

            BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);


            var Result = (await basics.CreateEquipment(Eq));


            await ShowToast("The information has been saved successfully.", ToastLevel.Success);

            await CloseModal(Result);

        }

        catch (RpcException ex)
        {

            await ExceptionManager(ex);

        }
        catch (Exception ex)
        {
            await ExceptionManager(ex);

        }

    }



    public void ChangeSwitch(ChangeEventArgs arg)
    {
        var a = SwitchList.FirstOrDefault(x => x.ID == ((SwitchItem)arg.Value).ID);

        a.Value = !((SwitchItem)arg.Value).Value;
    }


    public List<SwitchItem> SwitchList { get; set; }

    public class SwitchItem
    {
        public int ID { get; set; }


        public string Label { get; set; }

        public bool Value { get; set; }




    }


    }
}
