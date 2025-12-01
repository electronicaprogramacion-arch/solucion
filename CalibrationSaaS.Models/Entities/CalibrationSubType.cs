using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Bogus;
using System.ComponentModel.DataAnnotations.Schema;
using Helpers.Models;
using System.Linq;
using System.Text.Json.Serialization;
using System.Runtime.CompilerServices;
using CalibrationSaaS.Domain.Aggregates.Querys;
using System.Reflection;
using Helpers;
using System.Dynamic;
using System.Reflection.Metadata;
using Newtonsoft.Json;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public partial class CalibrationSubType_DynamicProperty
    {

        [DataMember(Order = 1)]
        public int CalibrationSubTypeId { get; set; }

        [DataMember(Order = 2)]
        public int DynamicPropertyID { get; set; }

        //[DataMember(Order = 3)]
        [NotMapped]
        public virtual CalibrationSubType CalibrationSubType { get; set; }

        [DataMember(Order = 4)]
        [NotMapped]
        public virtual DynamicProperty DynamicProperty { get; set; }

    }


    [DataContract]
    public partial class CalibrationSubType_ViewProperty
    {

        [DataMember(Order = 1)]
        public int CalibrationSubTypeId { get; set; }

        [DataMember(Order = 2)]
        public int ViewPropertyID { get; set; }

        [NotMapped]
        public virtual CalibrationSubType CalibrationSubType { get; set; }

        [DataMember(Order = 3)]
        [NotMapped]
        public virtual ViewPropertyBase ViewProperty { get; set; }

    }


    [DataContract]
    public partial class CalibrationSubType
    {

        [DataMember(Order = 1)]
        public int CalibrationSubTypeId { get; set; }

        [DataMember(Order = 2)]
        public int CalibrationTypeId { get; set; }

        [DataMember(Order = 3)]
        public string Name { get; set; }

        //[DataMember(Order = 4)]
        //public string Description { get; set; }

        [System.ComponentModel.DataAnnotations.Editable(false)]
        [NotMapped]
        [IgnoreDataMember]
        public virtual ICollection<CalibrationSubType_Weight> CalibrationSubType_Weights { get; set; }

        [System.ComponentModel.DataAnnotations.Editable(false)]
        [NotMapped]
        [IgnoreDataMember]
        public virtual ICollection<CalibrationSubType_Standard> CalibrationSubType_Standard { get; set; }

        [System.ComponentModel.DataAnnotations.Editable(false)]
        [NotMapped]
        [DataMember(Order = 5)]
        public virtual ICollection<CalibrationSubType_DynamicProperty> DynamicProperties { get; set; }

       

        [System.ComponentModel.DataAnnotations.Editable(false)]
        [NotMapped]
        [DataMember(Order = 6)]
        public virtual ICollection<CalibrationSubType_ViewProperty> RenderProperties { get; set; }

        [System.ComponentModel.DataAnnotations.Editable(false)]
        [NotMapped]
        [DataMember(Order = 7)]
        public virtual ICollection<DynamicProperty> DynamicPropertiesSchema { get; set; }

        [DataMember(Order = 8)]
        public string NameToShow { get; set; }

        [DataMember(Order = 9)]
        public string CreateClass { get; set; }

        [DataMember(Order = 10)]
        public string GetClass { get; set; }

        [System.ComponentModel.DataAnnotations.Editable(false)]
        [DataMember(Order = 11)]
        public virtual CalibrationSubTypeView CalibrationSubTypeView { get; set; }


        [DataMember(Order = 12)]
        public int? CalibrationSubTypeViewID { get; set; }


        [DataMember(Order = 13)]
        public bool Enabled { get; set; }


        [DataMember(Order = 14)]
        public int Position { get; set; }

        [DataMember(Order = 15)]
        public string SelectStandarClass { get; set; }



        [DataMember(Order = 16)]
        public string OnChangeClass { get; set; }


        [DataMember(Order = 17)]
        public string NewClass { get; set; }

        [DataMember(Order = 18)]
        public string Select2StandarClass { get; set; }
        //[DataMember(Order = 11)]
        //public string GetClass { get; set; }

        [DataMember(Order = 19)]
        public string StandardAssignComponent { get; set; }


        [DataMember(Order = 20)]
        public string StandardAssignComponent2 { get; set; }

        //[NotMapped]
        [DataMember(Order = 21)]
        public bool Mandatory { get; set; }


        
        //[NotMapped]
        [DataMember(Order = 23)]
        public bool? SupportCSV { get; set; }

        [System.ComponentModel.DataAnnotations.Editable(false)]
        [NotMapped]
        [DataMember(Order = 24)]
        public virtual ICollection<DynamicProperty> DynamicPropertiesObject { get; set; }

        
        [DataMember(Order = 25)]
        public int inheritCalibrationSubTypeId { get; set; }

        [DataMember(Order = 26)]
        public int? UnitOfMeasureTypeID { get; set; }

        [DataMember(Order = 27)]
        public bool? CreateGroups { get; set; }

        [NotMapped]
        [DataMember(Order = 28)]
        public string? GroupName { get; set; }

        //public string ErrorMessage { get; set; }
        [NotMapped]
        [System.Text.Json.Serialization.JsonIgnore]
        public bool HasAction
        {
            get
            {

                if (!string.IsNullOrEmpty(this.SelectStandarClass) || !string.IsNullOrEmpty(this.Select2StandarClass))
                {
                    return true;
                }

                if (this.CalibrationSubTypeView.EnabledDelete || this.CalibrationSubTypeView.HasNewButton)
                {

                    return true;
                }


                return false;

            }
        }

        [NotMapped]
        [DataMember(Order = 29)]
        public List<CalibrationSubType> TestPointGroupList { get; set; }

        public CreateModel CreateModel()
        {


            return GetModel(this.CreateClass, "Create");


        }

        public CreateModel GetModel()
        {

            return GetModel(this.GetClass, "Get");

        }

        public CreateModel SelectModel()
        {

            return GetModel(this.SelectStandarClass, "Select");

        }

        public CreateModel Select2Model()
        {

            return GetModel(this.Select2StandarClass, "Select2");

        }

        public CreateModel NewModel()
        {

            return GetModel(this.NewClass, "newitems");

        }

        public  CalibrationSubType GetStyle(CalibrationSubType calibrationSubType, int calCount = 1)
        {
            

            //return calibrationSubType;

            CreateModel createModel = calibrationSubType.CreateModel();

            var headerRow = calibrationSubType?.DynamicPropertiesSchema?.Where(x => x.GridLocation == "new").FirstOrDefault();

            bool HasHeaderRow = false;

            if (headerRow != null)
            {
                calibrationSubType.CalibrationSubTypeView.EnabledNew = true;
            }


            //ErrorMessage += createModel.ErrorMessage;

            var styleform = createModel.Style;

            if (string.IsNullOrEmpty(styleform))
            {
                return calibrationSubType;
            }

            styleform= styleform.ToLower(); 

            if (calCount > 1)
            {
                calibrationSubType.CalibrationSubTypeView.ShowCardButton = true;

            }
            else if (calCount == 1)
            {
                calibrationSubType.CalibrationSubTypeView.ShowCardButton = false;
            }

            if (!string.IsNullOrEmpty(createModel.Component))
            {
                calibrationSubType.CalibrationSubTypeView.Component = createModel.Component;
            }
          


            calibrationSubType.CalibrationSubTypeView.CSSRow = "row justify-content-center w-100";

            calibrationSubType.CalibrationSubTypeView.CSSRowSeparator = "CSSRowSeparator";


            if (styleform.ToLower().Contains("modal"))
            {
                calibrationSubType.CalibrationSubTypeView.Style = "Modal";

                calibrationSubType.CalibrationSubTypeView.ShowCardButton = true;

                calibrationSubType.CalibrationSubTypeView.FullScreen = true;


            }
            if (styleform.ToLower().Contains("min"))
            {
                calibrationSubType.CalibrationSubTypeView.Style = "Modal";

                calibrationSubType.CalibrationSubTypeView.ShowCardButton = true;

                calibrationSubType.CalibrationSubTypeView.FullScreen = false;
            }

            if (styleform.ToLower().Contains("panel"))
            {
                calibrationSubType.CalibrationSubTypeView.Style = "Panel";

                if (string.IsNullOrEmpty(calibrationSubType.CalibrationSubTypeView.CSSGrid))
                {
                    calibrationSubType.CalibrationSubTypeView.CSSGrid = "CSSGridPanel";
                }

               

            }


            if (!string.IsNullOrEmpty(createModel.Title))
            {
                calibrationSubType.NameToShow = createModel.Title;
            }

            bool HasNew = false;
            if (!string.IsNullOrEmpty(calibrationSubType.NewClass))
            {
                calibrationSubType.CalibrationSubTypeView.EnabledNew = true;

                calibrationSubType.CalibrationSubTypeView.HasNewButton = true;

                calibrationSubType.CalibrationSubTypeView.EnabledDelete = true;
                               

                HasNew =true;
            }
            else
            {
                calibrationSubType.CalibrationSubTypeView.HasNewButton = false;

                calibrationSubType.CalibrationSubTypeView.EnabledDelete = false;
            }

            //if (createModel.NewDeleteRow.HasValue && createModel.NewDeleteRow.Value)
            //{
            //    calibrationSubType.CalibrationSubTypeView.EnabledNew = true;

            //    calibrationSubType.CalibrationSubTypeView.HasNewButton = true;

            //    calibrationSubType.CalibrationSubTypeView.EnabledDelete = true;
            //}

            if (!string.IsNullOrEmpty(calibrationSubType.SelectStandarClass))
            {
                calibrationSubType.CalibrationSubTypeView.EnabledSelect = true;

                calibrationSubType.CalibrationSubTypeView.ColActionCSS = "col-1";
            }
            else
            {
                calibrationSubType.CalibrationSubTypeView.EnabledSelect = false;
            }


            if (string.IsNullOrEmpty(calibrationSubType.CalibrationSubTypeView.CSSWidth))
            {
                calibrationSubType.CalibrationSubTypeView.CSSWidth = "col-12";
            }

            if (styleform.Contains("form"))
            {
                calibrationSubType.CalibrationSubTypeView.EnableButtonBar = true;

                calibrationSubType.CalibrationSubTypeView.ShowHeader = false;

                calibrationSubType.CalibrationSubTypeView.IsRowView = false;

                calibrationSubType.CalibrationSubTypeView.CSSRowSeparator = "CSSRowSeparatorForm";

                if (calibrationSubType.DynamicPropertiesSchema != null) 
                {
                    foreach (var itm in calibrationSubType.DynamicPropertiesSchema)
                    {

                        itm.ViewPropertyBase.ShowControl = true;
                        itm.ViewPropertyBase.ShowLabel = true;
                        if (string.IsNullOrEmpty(itm.ViewPropertyBase.CSSCol))
                        {
                            itm.ViewPropertyBase.CSSCol = "col-3";
                        }
                    }
                }

               
                



            }

            if (styleform.ToLower().Contains("grid"))
            {
                

                calibrationSubType.CalibrationSubTypeView.ColActionCSS = "col-1";
                calibrationSubType.CalibrationSubTypeView.ColButtonActionCSS = "col-2";

                if (string.IsNullOrEmpty(calibrationSubType.CalibrationSubTypeView.CSSGrid))
                {
                    calibrationSubType.CalibrationSubTypeView.CSSGrid = "CSSGrid row justify-content-center";
                }
                
                calibrationSubType.CalibrationSubTypeView.IsCollection = true;

                if (calibrationSubType.HasAction)
                {
                    calibrationSubType.CalibrationSubTypeView.CSSForm = "col-10";

                    calibrationSubType.CalibrationSubTypeView.CSSRowHeader = "row col-10 justify-content-center w-100";
                }
                else
                {
                    calibrationSubType.CalibrationSubTypeView.CSSForm = "col-12";

                    calibrationSubType.CalibrationSubTypeView.CSSRowHeader = "col-12";
                }

                calibrationSubType.CalibrationSubTypeView.IsRowView = true;

                calibrationSubType.CalibrationSubTypeView.ShowHeader = true;

                calibrationSubType.CalibrationSubTypeView.EnableButtonBar = false;


                //calibrationSubType.CalibrationSubTypeView.CSSRowHeader = "";
                if (calibrationSubType.DynamicPropertiesSchema != null)
                {

                    foreach (var itm in calibrationSubType.DynamicPropertiesSchema.Where(x => x.GridLocation != null && x.GridLocation.ToLower() == "row"))
                    {

                        itm.ViewPropertyBase.ShowControl = true;
                        itm.ViewPropertyBase.ShowLabel = false;

                        if (string.IsNullOrEmpty(itm.ViewPropertyBase.CSSCol))
                        {
                            itm.ViewPropertyBase.CSSCol = "col";
                        }

                        if(itm.DataType== "System.Boolean")
                        {
                            itm.ViewPropertyBase.CSSCol = "col-1";
                        }

                    }

                    foreach (var itm in calibrationSubType.DynamicPropertiesSchema.Where(x => x.GridLocation != null && x.GridLocation.ToLower() == "new"))
                    {

                        itm.ViewPropertyBase.ShowControl = true;
                        itm.ViewPropertyBase.ShowLabel = true;
                        if (string.IsNullOrEmpty(itm.ViewPropertyBase.CSSCol))
                        {
                            itm.ViewPropertyBase.CSSCol = "col-3";
                        }

                    }

                }
                 
                //foreach (var item in calibrationSubType.DynamicPropertiesSchema)
                //{
                //    item.ViewPropertyBase.isr
                //}

            }




            if (styleform.ToLower() == "modal-form-many")
            {
                return calibrationSubType;
            }
            else
            if (styleform.ToLower() == "modal-grid-many")
            {
                return calibrationSubType;
            }
            else
            if (styleform.ToLower() == "panel-grid-many")
            {
                return calibrationSubType;
            }
            else
            {
                return calibrationSubType;
            }

            return calibrationSubType;
        }


        private CreateModel GetModel(string CreateClass2, string pre)
        {

            CreateModel createModel = new CreateModel();


            createModel.ErrorMessage = "";

            if (string.IsNullOrEmpty(CreateClass2))
            {
                createModel.ErrorMessage = "Empty json or class - name";
            }
            string _createClass = "";
            if (CreateClass2 != null)
             _createClass = CreateClass2.Trim();

            if (string.IsNullOrEmpty(_createClass))
            {
                createModel.ErrorMessage = "Empty or poorly formed json or class - name";
            }


            string pathclass = "";
            if (_createClass != CreateClass2)
            {
                createModel.ErrorMessage = "poorly formed json or class - name";
            }
            //string pre = "Create";
            try
            {
               
                pathclass = GetClassName(_createClass, pre);
                if (string.IsNullOrEmpty(pathclass))

                {
                    CreateModel param = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateModel>(_createClass);
                    if (param != null)
                    {
                        createModel = param;
                        createModel.ClassName = GetClassName(createModel.ClassName, pre);
                        return createModel;

                    }
                }

                createModel.ClassName = pathclass;

                return createModel;


            }
            catch (Exception ex)
            {
                createModel.ErrorMessage += "Error in configuration Class in " + pre + " " + ex.Message;
                return createModel;
            }


        }

        public string GetClassName(string _createClass, string pre)
        {
            string pathclass = "";

            if(string.IsNullOrEmpty(_createClass))
            {
                return "";
            }

            if (_createClass.Contains(".") && !_createClass.Contains("{"))
            {
                pathclass = _createClass;
            }
            else
               if (_createClass.ToLower() == "capacity")
            {
                pathclass = "CalibrationSaaS.Infraestructure.Blazor.GenericMethods." + pre + "CapacityBased";
            }
            else
               if (_createClass.ToLower() == "fixed")
            {
                pathclass = "CalibrationSaaS.Infraestructure.Blazor.GenericMethods." + pre + "TestPointsFixed";
            }
            else
               if (_createClass.ToLower() == "user")
            {
                pathclass = "CalibrationSaaS.Infraestructure.Blazor.GenericMethods." + pre + "UserBased";
            }
            else
               if (_createClass.ToLower() == "generic")
            {
                pathclass = "CalibrationSaaS.Infraestructure.Blazor.GenericMethods." + pre + "Standard";
            }
            else

               if (_createClass.ToLower() == "newitems")
            {
                pathclass = "CalibrationSaaS.Infraestructure.Blazor.GenericMethods." + pre + "";
            }
            return pathclass;
        }

        //public string createJSONProperties()
        //{

        //    List<list> properties = new List<list>();


        //    foreach (var item in this.DynamicPropertiesSchema.Where(x => x.Enable == true).OrderBy(x => x.ColPosition))
        //    {

        //        if (item.ViewPropertyBase == null)
        //        {
        //            Console.WriteLine(item.Name + " View Propeerty not configured");

        //        }




        //        if (!string.IsNullOrEmpty(item.DefaultValue) && string.IsNullOrEmpty(item.Map))
        //        {
        //            properties.Add(new list(item.Name, (object)item.DefaultValue, item));
        //        }

                
        //        else
        //        {

        //            properties.Add(new list(item.Name, null, item));

        //        }
        //    }




        //    dynamic Model = new ExpandoObject();


        //    IDictionary<string, object> dic = new Dictionary<string, object>();

        //    foreach (var item in properties)
        //    {

             
        //        if (item.HasBoolValue)
        //        {
        //            dic.Add(item.DynamicProperty.Name, item.BoolValue);
        //        }
        //        else
        //        {
        //            dic.Add(item.DynamicProperty.Name, item.Value);
        //        }
        //    }

        //    Model = dic.ToExpando();


        //    string json = Newtonsoft.Json.JsonConvert.SerializeObject(Model);

        //    return json;


        //}
    }

    [DataContract]
    public partial class CalibrationSubTypeView
    {

        [DataMember(Order = 1)]
        public int CalibrationSubTypeViewID { get; set; }

        [DataMember(Order = 2)]
        public int CalibrationSubTypeId { get; set; }


        
        [System.Text.Json.Serialization.JsonIgnore]
        [DataMember(Order = 3)]
        public virtual CalibrationSubType CalibrationSubType { get; set; }


        [DataMember(Order = 4)]
        public bool EnabledDrag { get; set; }


        [DataMember(Order = 5)]
        public bool EnabledDelete { get; set; }

        [DataMember(Order = 6)]
        public bool EnabledDuplicate { get; set; }


        //[DataMember(Order = 7)]
        //public bool EnabledHeader { get; set; }


        [DataMember(Order = 8)]
        public bool EnabledSelect { get; set; }

        [DataMember(Order = 9)]
        public bool EnabledNew { get; set; }

        [DataMember(Order = 10)]
        public int PageSize { get; set; }

        [DataMember(Order = 11)]
        public string CSSRow { get; set; }

        [DataMember(Order = 12)]
        public string CSSRowSeparator { get; set; } = "CSSRowSeparator";

        [DataMember(Order = 13)]
        public bool EnableButtonBar { get; set; }


        //[DataMember(Order = 14)]
        //public bool ActionBeginRow { get; set; }


        [DataMember(Order = 15)]
        public string ColButtonActionCSS { get; set; } = "btnWidth";

        [DataMember(Order = 16)]
        public string ColActionCSS { get; set; } = "btnWidth";

        [DataMember(Order = 17)]
        public string AlingActionCSS { get; set; } = "AlingLeft";


        [DataMember(Order = 18)]
        public string Key { get; set; } = "SequenceID";

        [DataMember(Order = 19)]
        public string NoDataMessage { get; set; } = "No Data or Not configured";

        
        [DataMember(Order = 20)]
        public string CSSGrid { get; set; } 



        [DataMember(Order = 21)]
        public string SortField { get; set; }

        //[DataMember(Order = 22)]
        //public string ResolutionField { get; set; }


        [DataMember(Order = 22)]
        public bool? BlockIfInvalid { get; set; }


        [DataMember(Order = 23)]
        public string FilterField { get; set; }

        [DataMember(Order = 24)]
        public string FilterValue { get; set; }



        [DataMember(Order = 25)]
        public bool ShowHeader { get; set; }


        [DataMember(Order = 26)]
        public string CSSForm { get; set; } = "col-10";

        [DataMember(Order = 27)]
        public string CSSRowHeader { get; set; }

        //[DataMember(Order = 28)]
        //public bool OnlyNextPrevious { get; set; }

        [DataMember(Order = 29)]
        public bool? IsVisible { get; set; } = true;

        //[DataMember(Order = 30)]
        //public string NavigateTo { get; set; }

        //[DataMember(Order = 31)]
        //public string NavigateClass { get; set; }

        [DataMember(Order = 32)]
        public bool? SaveButton { get; set; }

        [DataMember(Order = 33)]
        public bool? CloseButton { get; set; }

        [DataMember(Order = 34)]
        public string Style { get; set; } = "Modal";


        /// <summary>
        /// config if is a par generic and genericcalibrationresult like in wod
        /// </summary>
        [DataMember(Order = 35)]
        public bool? UseResult { get; set; }

        /// <summary>
        /// config if the grid has many records if only a record is false like POE
        /// </summary>
        [DataMember(Order = 36)]
        public bool IsCollection { get; set; }


        [DataMember(Order = 37)]
        public string Component { get; set; }

        [DataMember(Order = 38)]
        public bool ShowCardButton { get; set; }

        [DataMember(Order = 39)]
        public bool EnableEdit { get; set; }


        [DataMember(Order = 40)]
        public string ColFormActionCSS { get; set; }

        [DataMember(Order = 41)]
        public bool IsRowView { get; set; }


        [DataMember(Order = 42)]
        public string NewButtonTitle { get; set; }


        [DataMember(Order = 43)]
        public string NewButtonCSS { get; set; }


        [DataMember(Order = 44)]
        public bool? EnableSelect2 { get; set; }


        [DataMember(Order = 45)]
        public string SelectTitle { get; set; } = "Standard";

        [DataMember(Order = 46)]
        public string Select2Title { get; set; }


       /// <summary>
       /// for all calibration (Form)
       /// </summary>
        [DataMember(Order = 47)]
        public bool HasDeleteSubType { get; set; }


        [DataMember(Order = 48)]
        public bool HasNewButton { get; set; }



        //private string _JSONConfiguration;
        //[NotMapped]
        //[System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        //[DataMember(Order = 49)]
        //public string JSONConfiguration
        //{
        //    get
        //    {
        //        string Options;
        //        var errors = new List<string>();

        //        Options = Newtonsoft.Json.JsonConvert.SerializeObject(this, new JsonSerializerSettings()
        //        {

        //            Error = (sender, error) =>
        //            {
        //                errors.Add(error.ErrorContext.Error.Message);
        //                error.ErrorContext.Handled = true;

        //                Console.WriteLine("Select error " + error.ErrorContext.Error.Message);


        //            }//error.ErrorContext.Handled = true


        //        });

        //        if (errors.Count > 0)
        //        {
        //            foreach (var item in errors)
        //            {
        //                Console.WriteLine("CalibrationSubTypeView JSONConfiguration" + item);
        //            }
        //        }

        //        _JSONConfiguration = Options;

        //        return _JSONConfiguration;

        //    }
        //    set
        //    {
        //        _JSONConfiguration = value;
        //    }
        //}

        [DataMember(Order = 49)]
        public string JSONConfiguration { get; set; }

        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Newtonsoft.Json.JsonIgnore]
        [DataMember(Order = 50)]
        public string CSVValidator { get; set; }

        [DataMember(Order = 51)]
        public bool? UseContext { get; set; }

        [DataMember(Order = 52)]
        public string CSSWidth { get; set; }

        [DataMember(Order = 53)]
        public int DefaultDecimalNumber { get; set; }

        [DataMember(Order = 54)]
        public string UncertaintyDescription { get; set; }

        [NotMapped]
        [DataMember(Order = 55)]
        public bool FullScreen { get; set; } = true;

        [DataMember(Order = 56)]
        public string AccordionType { get; set; }
    }

    public class CreateModel : Helpers.Models.CreateModelMin
    {

        public ICollection<DynamicProperty> Properties { get; set; }

    }

}
