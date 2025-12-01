using Bogus;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Helpers;
using Helpers.Models;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Threading.Tasks;
using static Blazed.Controls.MultiComponent.CalibrationSubTypeForm;


namespace CalibrationSaaS.Infraestructure.Blazor.GenericMethods
{
    public class CreateBase<Entity, CalibrationItem, CalibrationItemResult> : ComponentBase, ICreateItems<Entity, CalibrationItem> 
    where CalibrationItem : class, ICalibrationSubType2, IGenericCalibrationStandard, IResultComp, IResultTesPointGroup, new()
    where CalibrationItemResult : class, IResult2, IResultComp, IResultGen2, IUpdated, IDynamic, ISelect, IAggregate, ICreate, IResultTesPointGroup, new() //IResult2, IResultComp, IResultGen2, IDynamic, IUpdated,ISelect,ICreate, new()
    where Entity : class, //IGenericCalibrationCollection<CalibrationItemResult>,
        IGenericCalibrationCollection<GenericCalibrationResult2>,
        IResolution, ITolerance,IComponentS, new()
    {

        public AppState AppState { get; set; }
        //public IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
        public Func<dynamic, Application.Services.IPieceOfEquipmentService<CallContext>> PoeServices { get; set; }
        public CreateModel Parameter { get; set; }

        public CreateBase()
        {

        }


        public virtual async Task<List<CalibrationItem>> CreateItems(Entity eq, int CalibrationSubTypeId, string GroupName = null)
        {
            List<CalibrationItem> list = new List<CalibrationItem>();

            //var uom = eq.UnitofmeasurementID.GetUoM(AppState.UnitofMeasureList).;


            if (Parameter == null || (Parameter?.Data == null && string.IsNullOrEmpty(Parameter.NumberTestpoints)))
            {
                return list;
            }


            if (Parameter != null && Parameter.Header != null)
            {
                int cont = 1;
                if (Parameter.Header.Data != null && Parameter.Header.Data.Count > 0)
                {
                   

                    CalibrationItemResult gcr = new CalibrationItemResult();

                    gcr.GroupName = GroupName;
                    dynamic gce = new ExpandoObject();


                    var dic = Parameter.Header.Data;

                    if (1==2 && dic.ElementAtOrDefault(0).Key == "genericcalibrationresult2")
                    {
                        gcr.CreateNew(cont, CalibrationSubTypeId,
                               Parameter?.Component != null ? Parameter?.Component : "WorkOrderItem", eq.ComponentID, null,
                        (GenericCalibrationResult2)dic.ElementAtOrDefault(0).Value);

                        
                    }
                    else
                    {
                        //GenericCalibrationResult2 gcr = new GenericCalibrationResult2();

                        //gcr.GroupName = GroupName;

                        if (Parameter.Header.Data.ContainsKey("SequenceID"))
                        {
                            var gvr =Parameter.Header.Data.Map<GenericCalibrationResult2>();
                            //gce.TorqueSettings = item;
                            //gce.Standard = 0;
                            //gce.Result = 0;
                            //gce.AccuracySpecification = 0;
                            gcr.CreateNew(-1, CalibrationSubTypeId, Parameter?.Component != null ? Parameter?.Component : "WorkOrderItem", eq.ComponentID,
                                eq.Resolution, gvr);
                        }
                        else
                        {
                            //gce.TorqueSettings = item;
                            //gce.Standard = 0;
                            //gce.Result = 0;
                            //gce.AccuracySpecification = 0;
                            gcr.CreateNew(-1, CalibrationSubTypeId, Parameter?.Component != null ? Parameter?.Component : "WorkOrderItem", eq.ComponentID,
                                eq.Resolution, Parameter.Header.Data);
                        }

                            


                        gcr.Updated = DateTime.Now.Ticks;

                        var gc = gcr.GenericCalibration2 as CalibrationItem;

                        if (gc is null)
                        {
                            throw new Exception("CalibrationItem no must be null");
                        }


                        list.Add(gc);

                        gcr.Position = -1;

                        
                    }
                    cont++;
                    //}


                }


                if (Parameter.Header.Data == null && Parameter.Header.DefaultHeader.HasValue && Parameter.Header.DefaultHeader.Value)
                {

                    CalibrationItemResult gcr = new CalibrationItemResult();


                   
                    dynamic gce = new ExpandoObject();
                    gcr.GroupName = GroupName;

                    //gce.TorqueSettings = item;
                    //gce.Standard = 0;
                    //gce.Result = 0;
                    //gce.AccuracySpecification = 0;
                    gcr.CreateNew(-1, CalibrationSubTypeId, Parameter?.Component != null ? Parameter?.Component : "WorkOrderItem", eq.ComponentID,
                        eq.Resolution, Parameter.DefaultData);

                    gcr.Updated = DateTime.Now.Ticks;

                    gcr.Position = -1;

                    var gc = gcr.GenericCalibration2 as CalibrationItem;

                    if (gc is null)
                    {
                        throw new Exception("CalibrationItem no must be null");
                    }


                    list.Add(gc);

                }
            }

            if (Parameter.Poe == "1") //Datos del Poe
            {
                PieceOfEquipment poe = new PieceOfEquipment();
                if (eq is WorkOrderDetail wod && wod.PieceOfEquipment != null)
                {
                    poe = wod.PieceOfEquipment;
                    
                }

                dynamic gce = new ExpandoObject();
                GenericCalibrationResult2 gcr = new GenericCalibrationResult2();

                gcr.GroupName = GroupName;
                gce.PlatformManufacturer = poe?.EquipmentTemplate?.Manufacturer1?.Name ;
                gce.PlatformModel = poe?.EquipmentTemplate?.Model ;
                gce.PlatformSerial = poe?.SerialNumber;
                gce.Capacity = poe?.Capacity;
                //gce.AccuracySpecification = 0;
                gcr.CreateNew(1, CalibrationSubTypeId,"WorkOrderItem", eq.ComponentID, eq.Resolution, gce);
                gcr.Updated = DateTime.Now.Ticks;

                var gc = gcr.GenericCalibration2 as CalibrationItem;

                if (gc is null)
                {
                    throw new Exception("CalibrationItem no must be null");
                }


                list.Add(gc);
            }
            else if (Parameter.Poe == "2") //Datos Poe Indicator
            {
                PieceOfEquipment poe = new PieceOfEquipment();
                if (eq is WorkOrderDetail wod && wod.PieceOfEquipment != null)
                {
                    PieceOfEquipment poe1 = wod.PieceOfEquipment;

                    poe = await GetPieceOfEquipmentIndicator(poe1);

                }
                dynamic gce = new ExpandoObject();
                GenericCalibrationResult2 gcr = new GenericCalibrationResult2();
                gcr.GroupName = GroupName;
                gce.Manufacturer = poe?.EquipmentTemplate?.Manufacturer1?.Name;
                gce.Model = poe?.EquipmentTemplate?.Model;
                gce.SerialNumber = poe?.SerialNumber;
                gcr.CreateNew(1, CalibrationSubTypeId, "WorkOrderItem", eq.ComponentID, eq.Resolution, gce);
                gcr.Updated = DateTime.Now.Ticks;

                var gc = gcr.GenericCalibration2 as CalibrationItem;

                if (gc is null)
                {
                    throw new Exception("CalibrationItem no must be null");
                }


                list.Add(gc);
            }
            else if (Parameter.Poe == "3") //Datos Poe Micro
            {
                var workOrderDetail = eq as WorkOrderDetail;
                if (workOrderDetail != null && workOrderDetail.WOD_Weights != null && workOrderDetail.WOD_Weights.Count > 0)
                {
                    if (workOrderDetail.WOD_Weights != null && workOrderDetail.WOD_Weights.Count > 0)
                    {
                        int cont = 1;
                        foreach (var item in workOrderDetail.WOD_Weights)
                        {

                            var poe = item.WeightSet.PieceOfEquipment;

                            if (poe.TypeMicro.HasValue &&  poe.TypeMicro.Value ==1)
                            {



                                GenericCalibrationResult2 gcr = new GenericCalibrationResult2();
                                dynamic gce = new ExpandoObject();
                                gcr.GroupName = GroupName;
                                gce.UM = poe.MicronValue;
                                gce.Tol_HM = poe.Tolerance.ToleranceValue;
                                gce.Tol_HV = poe.ToleranceHV;
                                gce.Load_KFG = poe.LoadKGF;
                                gce.UncertantyBlock = poe.UncertaintyValue;
                                gce.HardnessValue = poe.Hardness;


                                gcr.CreateNew(cont, CalibrationSubTypeId, Parameter?.Component != null ? Parameter?.Component : "WorkOrderItem", eq.ComponentID,
                                eq.Resolution, gce);

                                gcr.Updated = DateTime.Now.Ticks;


                                var gc = gcr.GenericCalibration2 as CalibrationItem;

                                if (gc is null)
                                {
                                    throw new Exception("CalibrationItem no must be null");
                                }

                                list.Add(gc);

                                cont++;

                            }

                        }

                    }
                }
            
            }
            else if (Parameter.Poe == "4") //Datos Poe Vicker
            {
                var workOrderDetail = eq as WorkOrderDetail;
                if (workOrderDetail != null && workOrderDetail.WOD_Weights != null && workOrderDetail.WOD_Weights.Count > 0)
                {
                    if (workOrderDetail.WOD_Weights != null && workOrderDetail.WOD_Weights.Count > 0)
                    {
                        int cont = 1;
                        foreach (var item in workOrderDetail.WOD_Weights)
                        {

                            var poe = item.WeightSet.PieceOfEquipment;

                            if (poe.TypeMicro.HasValue && poe.TypeMicro.Value == 2)
                            {



                                GenericCalibrationResult2 gcr = new GenericCalibrationResult2();
                                dynamic gce = new ExpandoObject();
                                gcr.GroupName = GroupName;
                                gce.UM = poe.MicronValue;
                                gce.Tol_HM = poe.Tolerance.ToleranceValue;
                                gce.Tol_HV = poe.ToleranceHV;
                                gce.Load_KFG = poe.LoadKGF;
                                gce.UncertantyBlock = poe.UncertaintyValue;
                                gce.HardnessValue = poe.Hardness;


                                gcr.CreateNew(cont, CalibrationSubTypeId, Parameter?.Component != null ? Parameter?.Component : "WorkOrderItem", eq.ComponentID,
                                eq.Resolution, gce);

                                gcr.Updated = DateTime.Now.Ticks;


                                var gc = gcr.GenericCalibration2 as CalibrationItem;

                                if (gc is null)
                                {
                                    throw new Exception("CalibrationItem no must be null");
                                }

                                list.Add(gc);

                                cont++;

                            }

                        }

                    }
                }

            }

            if (Parameter != null && Parameter?.Data?.Count > 0)
            {

                int cont = 1;

                for (int i = 0; i < Parameter.Data.Count; i++)
                {

                    CalibrationItemResult gcr = new CalibrationItemResult();

                    gcr.GroupName = GroupName;
                    dynamic gce = new ExpandoObject();


                    var dic = Parameter.Data.ElementAtOrDefault(i);

                    if (dic.ElementAtOrDefault(0).Key == "genericcalibrationresult2")
                    {
                        gcr.CreateNew(cont, CalibrationSubTypeId,
                               Parameter?.Component != null ? Parameter?.Component : "WorkOrderItem", eq.ComponentID, null,
                        (GenericCalibrationResult2)dic.ElementAtOrDefault(0).Value);
                    }
                    else
                    {
                        gcr.CreateNew(cont, CalibrationSubTypeId, Parameter?.Component != null ? Parameter?.Component : "WorkOrderItem", eq.ComponentID,
                        eq.Resolution, Parameter.Data.ElementAtOrDefault(i));

                    }


                    gcr.Updated = DateTime.Now.Ticks;


                    var gc = gcr.GenericCalibration2 as CalibrationItem;

                    if (gc is null)
                    {
                        throw new Exception("CalibrationItem no must be null");
                    }

                    list.Add(gc);

                    cont++;
                }


            }
            else if (Parameter != null
             && !string.IsNullOrWhiteSpace(Parameter.NumberTestpoints)
             && Parameter.DefaultData != null
             && string.IsNullOrWhiteSpace(Parameter.Property)
             && string.IsNullOrWhiteSpace(Parameter.Operator)
             && string.IsNullOrWhiteSpace(Parameter.Poe))
            {

                SetNumberTestPoints(eq);

                int cont = 1;

                for (int i = 0; i < Convert.ToInt32(Parameter.NumberTestpoints); i++)
                {

                    CalibrationItemResult gcr = new CalibrationItemResult();
                    
                    gcr.GroupName = GroupName;

                    dynamic gce = new ExpandoObject();


                    //gce.TorqueSettings = item;
                    //gce.Standard = 0;
                    //gce.Result = 0;
                    //gce.AccuracySpecification = 0;
                    gcr.CreateNew(cont, CalibrationSubTypeId, Parameter?.Component != null ? Parameter?.Component : "WorkOrderItem", eq.ComponentID,
                        eq.Resolution, Parameter.DefaultData);

                    gcr.Updated = DateTime.Now.Ticks;


                    var gc = gcr.GenericCalibration2 as CalibrationItem;

                    if (gc is null)
                    {
                        throw new Exception("CalibrationItem no must be null");
                    }

                    list.Add(gc);

                    cont++;
                }




            }
            else
              if (Parameter != null && !string.IsNullOrEmpty(Parameter.NumberTestpoints)
               && Parameter.DefaultData != null && string.IsNullOrEmpty(Parameter.Property) && !string.IsNullOrEmpty(Parameter.Operator))
            {
                SetNumberTestPoints(eq);
                int operator_ = Convert.ToInt16(Parameter.Operator);
                int cont = 1;
                int numberTestpoints = Convert.ToInt32(Parameter.NumberTestpoints);



                List<string> listseccions = new List<string>();
                if (operator_ == 2)
                { 
                    for (int i = 0; i < numberTestpoints; i++)
                    {
                        int sectionNumber = i + 1;
                        listseccions.Add($"Section {sectionNumber} Inside");
                        listseccions.Add($"Section {sectionNumber} Outside");
                    }
                }
                else if (operator_ == 4)
                {

                    for (int i = 0; i < numberTestpoints; i++)
                    {
                        int sectionNumber = i + 1;
                        int contRow = 0 ;
                        listseccions.Add($"Section {sectionNumber} Row {cont++}");
                        listseccions.Add($"Section {sectionNumber} Row {cont++} ");
                        listseccions.Add($"Section {sectionNumber} Row {cont++} ");
                        listseccions.Add($"Section {sectionNumber} Row {cont++} ");
                    }
                }

                foreach (var item in listseccions)
                {

                    GenericCalibrationResult2 gcr = new GenericCalibrationResult2();

                    dynamic gce = new ExpandoObject();


                    //gce.TorqueSettings = item;
                    gce.Section = item;
                    gce.TestWeight = 0;
                    gce.AsFound = 0;
                    gce.AsLeft = 0;
                    gcr.GroupName = GroupName;
                    gcr.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.ComponentID, eq.Resolution, gce);

                    var gc = gcr.GenericCalibration2 as CalibrationItem;

                    if (gc is null)
                    {
                        throw new Exception("CalibrationItem no must be null");
                    }

                    list.Add(gc);

                    cont++;
                   

                }

             

            }

            else if (Parameter.Range != null)
            {



            }

            //Capacity and testpoints
            if (Parameter != null && !string.IsNullOrEmpty(Parameter.NumberTestpoints) 
                && !string.IsNullOrEmpty(Parameter.Property) && Parameter.DefaultData != null && string.IsNullOrEmpty(Parameter.Poe))
            {
                properties = new List<list>();

                GetProperty(eq, Parameter.Property);
                var prop=properties.ElementAtOrDefault(0);

                if(prop.Value == null)
                {
                    prop.Value = 0;
                }

                SetNumberTestPoints(eq);


                int i = 1;

                var value1 = Convert.ToDouble(prop.Value);

                var value2 = Convert.ToInt32(Parameter.NumberTestpoints);

                var row0 = Convert.ToInt32(Parameter.row0 ?? "0");

                foreach (var item in GetFS(value1, value2, row0))
                {

                    CalibrationItemResult gcr = new CalibrationItemResult();
                    gcr.GroupName = GroupName;
                    dynamic gce = new ExpandoObject();
                    var dicto = new Dictionary<string, object>();
                    if (Parameter.DefaultData != null)
                    {

                        foreach (KeyValuePair<string, object> dictItem in Parameter.DefaultData.Where(x => x.Key != "Nominal"))
                        {
                            if (dictItem.Value != null && dictItem.Value.ToString() == "step")
                            {
                                dicto.Add(dictItem.Key, item);
                            }
                            else if(dictItem.Value != null && dictItem.Value.ToString().Contains("context."))
                            {

                            }
                            else
                            {
                                dicto.Add(dictItem.Key, dictItem.Value);
                            }
                        }
                    }
                    //gce.Weight = item;
                    //gce.UoM = uom.Abbreviation;
                    //gce[""] = item;


                    gcr.CreateNew(i, CalibrationSubTypeId, Parameter?.Component != null ? Parameter?.Component : "WorkOrderItem", eq.ComponentID,
                        eq.Resolution, dicto);

                    gcr.Updated = DateTime.Now.Ticks;


                    var gc = gcr.GenericCalibration2 as CalibrationItem;

                    if (gc is null)
                    {
                        throw new Exception("CalibrationItem no must be null");
                    }

                    i++;
                    list.Add(gc);                 
                   
                }
            }

            foreach (var itemgen in list) 
            {
                itemgen.GroupName = GroupName;

            }


            return list;
        }


        public async Task<PieceOfEquipment> GetPieceOfEquipmentIndicator(PieceOfEquipment poe)
        {
            var poeService = PoeServices?.Invoke(null); // o el contexto adecuado
            if (poeService == null)
                return null;


            var children = await poeService.GetPieceOfEquipmentChildrenAll(poe, CallContext.Default);
           

            var childrenIndicator = children.Where(x => x.EquipmentTemplate?.EquipmentTypeObject?.HasIndicator == true);

            return childrenIndicator?.FirstOrDefault();
        }


        public bool IsDictionary(string line)
        {
            return true;
        }

        public bool IsCSV(string line)
        {
            return true;
        }
        public static List<double> GetFS(double capacity, double resol1, int? row0)
        {
            try
            {
                List<double> list = new List<double>();
                var NumberTestPoint = resol1;

                if (NumberTestPoint == 0)
                {
                    NumberTestPoint = 1;
                }
                int initRow = 1;
                int step = ((int)capacity / (int)NumberTestPoint);
                if (row0 > 0)
                {

                    for (int i = 1; i < row0 + 1; i++)
                    {
                        var s = 0;
                        list.Add(s);
                    }
                }

                for (int i = initRow; i < NumberTestPoint + 1; i++)
                {
                    var s = ((i) * step);

                    list.Add(s);

                }



                return list;
            }
            catch (Exception ex) 
            { 

               
              Console.WriteLine("GetFS: " + ex.Message);

               List<double> lst=  new List<double>();
                lst.Add(0);
                return lst;
            }
          
        }

        /// <summary>
        /// can came from a 
        /// </summary>
        /// <param name="eq"></param>
        /// <exception cref="Exception"></exception>
        public void SetNumberTestPoints(Entity eq)
        {
            if (!Helpers.NumericExtensions.IsNumeric(Parameter.NumberTestpoints))
            {
                properties = new List<list>();
                int? calsubtype = null;
                var prop = Parameter.NumberTestpoints;
                if (Parameter.NumberTestpoints.Contains(","))
                {
                    var ap = Parameter.NumberTestpoints.Split(",");

                    prop = ap[0];
                    calsubtype = Convert.ToInt32(ap[1]);
                }

                GetProperty(eq, prop, calsubtype);
                var prop1 = properties.ElementAtOrDefault(0);
                if (prop1 == null || prop1.Value == null)
                {
                    throw new Exception("Parameter NumberTestpoints not found review configuration");
                }
                Parameter.NumberTestpoints = prop1.Value.ToString();

            }
        }


        public List<list> properties { get; set; } = new List<list>();
        
        public void GetProperty(Entity Context,string DefaultValue,int? subtype=null)
        {
            if (Context != null && !string.IsNullOrEmpty(DefaultValue) && DefaultValue.Contains("context"))
            {

                //&& item.DefaultValue.Contains("context")
                var map = DefaultValue.Replace("context.", "");

                //if (item.DefaultValue.Contains())
                if (map.Contains("TestPointResult"))
                {

                    var asp = map.Split("[");

                    var propi = asp[0];

                    var otroprop = asp[1].Split("]");

                    var indice = otroprop[0];

                    var propertyYes = otroprop[1].Replace(".", "").Replace("]", "");

                    var cxx = ((object)Context).GetPropValue(propi);



                    List<GenericCalibrationResult2> cx2 = (List<GenericCalibrationResult2>)cxx;

                    if (subtype.HasValue && cx2 != null )
                    {
                        cx2 = cx2.Where(x => x.CalibrationSubTypeId == subtype).ToList();
                    }

                    var cx = cx2[Convert.ToInt32(indice)];

                   if(cx2 != null)
                    {
                        if (!string.IsNullOrEmpty(cx.Object))
                        {
                            var obj1 = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(cx.Object);

                            dynamic result;

                            var assss = Helpers.DynamicHelper.PropertyExists(obj1, propertyYes, out result);

                            if (assss == true)
                            {
                                properties.Add(new list(DefaultValue, result));
                            }
                            else
                            {
                                properties.Add(new list(DefaultValue, ((object)Context).GetPropValue(map))); //  [item.Map], item));
                            }



                        }
                        else
                        {
                            properties.Add(new list(DefaultValue, ((object)Context).GetPropValue(map))); //  [item.Map], item));
                        }

                    }






                }
                else
                {
                    properties.Add(new list(DefaultValue, ((object)Context).GetPropValue(map))); //  [item.Map], item));

                }

                //((object)Context).GetPropValue(map)
            }
        }



    }


    public class GetBase<Entity, CalibrationItem> : ComponentBase, IGetItems<Entity, CalibrationItem> where CalibrationItem : class, new()
   //where CalibrationItemResult : class, IResult2, IResultComp, IResultGen2, IDynamic, IUpdated, ISelect, ICreate, new()
   where Entity : class, //IGenericCalibrationCollection<CalibrationItemResult>,
       IGenericCalibrationCollection<GenericCalibrationResult2>,
       IResolution, ITolerance, IComponentS, new()
    {



        public async Task<List<CalibrationItem>> GetItems(Entity eq, int CalibrationSubTypeId, int ihneritCalSubtypeId)
        {

            if (eq == null
                || eq?.TestPointResult == null || eq?.TestPointResult?.Count == 0
                || eq?.TestPointResult?.Where(x => x.CalibrationSubTypeId == CalibrationSubTypeId).FirstOrDefault() == null)
            {
                return null;
            }



            var list = eq.TestPointResult
                 .Where(x => x.CalibrationSubTypeId == CalibrationSubTypeId).ToList();


            List<CalibrationItem> ListGC3 = new List<CalibrationItem>();
            foreach (var itemGC in list)
            {
                if (itemGC.GenericCalibration2 == null)
                {

                    ListGC3.Add(itemGC.CreateNew(itemGC) as CalibrationItem);
                }
                else
                {
                    //if(itemGC.GenericCalibration2.BasicCalibrationResult == null)
                    //{
                    itemGC.GenericCalibration2.TestPointResult = new List<GenericCalibrationResult2>();
                    //}

                    itemGC.GenericCalibration2.TestPointResult.Add(itemGC);

                    ListGC3.Add(itemGC.GenericCalibration2 as CalibrationItem);
                }
                //itemGC.GenericCalibration2.BasicCalibrationResult.Add(itemGC);


            }
            return ListGC3;
        }
    }

   

    }
