using CalibrationSaaS.Domain.Aggregates.Interfaces;
using Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using static CalibrationSaaS.Domain.Aggregates.Querys.Querys;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{

    [Serializable]
    [DataContract]
    public class EquipmentTemplate : IGenericCalibrationCollection<GenericCalibrationResult2>, 
        IGeneric, IResolution, IConfiguration, ITolerance, IUncetainty, IComponentS,INew<EquipmentTemplate>

    {

        public EquipmentTemplate()
        {

            //PieceOfEquipment = new HashSet<PieceOfEquipment>();

            //if(Tolerance== null)
            //{
            //    Tolerance = new Tolerance();
            //}
            //Tolerance.PropertyChanged += Tolerance_PropertyChanged;

        }



        [Key]
        [DataMember(Order = 1)]
        public int EquipmentTemplateID { get; set; }

        [DataMember(Order = 2)]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        //[StringLength(100, ErrorMessage = "field text is too long.")]
        public string Name { get; set; } = "";

        [DataMember(Order = 3)]
        [StringLength(100, ErrorMessage = "Name is too long.")]
        public string Description { get; set; } = "";

        [DataMember(Order = 4)]
        [StringLength(500, ErrorMessage = "Name is too long.")]
        public string ImageUrl { get; set; } = "";

        //[DataMember(Order = 5)]       
        //public string Abbreviation { get; set; } = "";

        //[Required(ErrorMessage = "Required EquipmentTypeID")]
        [DataMember(Order = 5)]
        //[Range(1, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int? EquipmentTypeID
        {
            get
            {

                return GetEquipmentTypeId();

            }


            set
            {
                SetEquipmentTypeID(value);
            }

        }





        [Required(ErrorMessage = "Required ManufacturerID")]
        [DataMember(Order = 6)]
        [Range(1, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int ManufacturerID { get; set; }

        ////[Range(1, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        //[DataMember(Order = 7)]       
        //public double Tolerance { get; set; }

        private int? _ToleranceTypeID;
        [DataMember(Order = 8)]

        public int? ToleranceTypeID { get { return _ToleranceTypeID; } set { _ToleranceTypeID = value; } }


        //[DataMember(Order = 9)]       
        //public string ToleranceType { get; set; } = "";


        //[DataMember(Order = 10)]

        //public double AccuracyPercentage { get; set; }

        //[Range(0.00000001, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        [DataMember(Order = 11)]
        //[Range(0.1, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        public double Resolution { get; set; }


        [DataMember(Order = 12)]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        //[Range(1, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int DecimalNumber { get; set; }

        [DataMember(Order = 13)]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        //[Range(1, Double.PositiveInfinity, ErrorMessage = "Select a Unitofmeasure please")]
        public int? UnitofmeasurementID { get; set; }


        //[DataMember(Order = 14)]       
        //public string Unitofmeasurement { get; set; } = "";


        [DataMember(Order = 14)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required StatusID")]
        [Range(1, Double.PositiveInfinity, ErrorMessage = "Select a Status please")]
        public string StatusID { get; set; }

        //[DataMember(Order = 15)]
        //public string Status { get; set; } = "";


        [DataMember(Order = 15)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public string Model { get; set; }


        [DataMember(Order = 16)]
        public double Capacity { get; set; }

        [DataMember(Order = 17)]
        public bool IsEnabled { get; set; } = true;

        [DataMember(Order = 18)]
        public string Manufacturer { get; set; } = "";


        [DataMember(Order = 19)]
        public string EquipmentType { get; set; }


        [DataMember(Order = 20)]
        public virtual ICollection<TestPointGroup> TestGroups { get; set; }


        public EquipmentType _equipmentTypeObject;
        private EquipmentType _EquipmentType22;

        [NotMapped]
        [DataMember(Order = 21)]
        //public virtual EquipmentType EquipmentTypeObject { get; set; }
        public   virtual EquipmentType EquipmentTypeObject
        {

            get
            {
                if (NullableCollections)
                {
                    return null;
                }
                else
                {
                   
                    return GetEquipmentType(); 
                }
               
            }
            set
            {
                //SetEquipmentType(value);

                _equipmentTypeObject = value;
            }

        }

        [DataMember(Order = 22)]
        public virtual ICollection<RangeTolerance> Ranges { get; set; }
        //public object Equipments { get; set; }


        //[NotMapped]
        [System.Text.Json.Serialization.JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<PieceOfEquipment> PieceOfEquipments { get; set; }


        private UnitOfMeasure _UnitOfMeasure;
        [IgnoreDataMember]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public virtual UnitOfMeasure UnitOfMeasure { get { if (NullableCollections) { return null; } else { return _UnitOfMeasure; } } set { _UnitOfMeasure = value; }  }


        [DataMember(Order = 23)]
        public virtual Manufacturer Manufacturer1 { get; set; }

        [DataMember(Order = 24)]
        public int ClassHB44 { get; set; }

        [DataMember(Order = 25)]
        public bool IsComercial { get; set; }

        [DataMember(Order = 26)]
        public bool IsDelete { get; set; }

        [DataMember(Order = 27)]
        public bool IsGeneric { get; set; }


        [DataMember(Order = 28)]
        public string DeviceClass { get; set; }


        [DataMember(Order = 29)]
        public string PlatformSize { get; set; }


        [NotMapped]
        [DataMember(Order = 34)]
        public virtual ICollection<Uncertainty> Uncertainty { get; set; }


        private List<GenericCalibrationResult2> _Lstcal;

        [NotMapped]
        [DataMember(Order = 35)]
        public List<GenericCalibrationResult2> TestPointResult
        {
            get
            {
                if (NullableCollections)
                {
                    return null;
                }
                else
                {
                    return _Lstcal;
                }
            }

            set
            {
                _Lstcal = value;

            }
        }

        [NotMapped]
        public string Configuration { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tolerance section
        /// </summary>
        [DataMember(Order = 30)]
        public bool? FullScale { get; set; }

        [DataMember(Order = 61)]
        public double? TolerancePercentage { get; set; }

        [DataMember(Order = 62)]
        public double? ToleranceValue { get; set; }
        [DataMember(Order = 63)]
        public double? ToleranceFixedValue { get; set; }

        [DataMember(Order = 64)]
        [NotMapped]
        public bool? AddFromPoe { get; set; }

        [DataMember(Order = 65)]
        public int? EquipmentTemplateParent { get; set; }

        


        [NotMapped]
        [DataMember(Order = 76)]
        public string ComponentID
        {
            get
            {
                return EquipmentTemplateID.ToString();
            }
            set
            {
                var val = Convert.ToInt32(value);
                EquipmentTemplateID = val;
            }


        }

        //[DataMember(Order = 34)]
        //public string JsonTolerance { get ; set ; }

        //[DataMember(Order = 35)]
        //public Tolerance Tolerance { get ; set; }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        private string _JsonTolerance;
        [DataMember(Order = 101)]
        public string JsonTolerance
        {
            get
            {

                return this._JsonTolerance;

            }

            set
            {
                this._JsonTolerance = value;
            }


        }




        private Tolerance _Tolerance = new Tolerance();

        [NotMapped]
        [DataMember(Order = 102)]
        public virtual Tolerance Tolerance
        {
            get
            {
                Tolerance tol1 = new Tolerance();
                Tolerance tol = new Tolerance();
                if (string.IsNullOrEmpty(_Tolerance.Json) && string.IsNullOrEmpty(this._JsonTolerance) && this._ToleranceTypeID.HasValue==true)
                {
                    tol.ToleranceTypeID = this.ToleranceTypeID;
                    tol.Resolution = this.Resolution;
                    if (this.ToleranceFixedValue.HasValue)
                        tol.ToleranceFixedValue = this.ToleranceFixedValue.Value;

                    if (this.ToleranceValue.HasValue)
                        tol.ToleranceValue = this.ToleranceValue.Value;

                    if (this.TolerancePercentage.HasValue)
                        tol.TolerancePercentage = this.TolerancePercentage.Value;

                    _Tolerance = tol;

                    //var str2 = Newtonsoft.Json.JsonConvert.SerializeObject(tol);

                    //this._JsonTolerance = str2;

                }

                

                _Tolerance = tol1.GetTolerance(_Tolerance.Json, this.JsonTolerance, _Tolerance);


                return _Tolerance;

            }
            set
            {


                _Tolerance = value;



                JsonTolerance = _Tolerance.Json;


            }
        }

        [NotMapped]
        //[DataMember(Order = 35)]
        public virtual ICollection<EquipmentType> AditionalEquipmentTypes { get; set; }

        
        [DataMember(Order = 36)]
        public string AditionalEquipmentTypesJSON { get; set; }

        private int? _EquipmentTypeID;

        private int? _EquipmentTypeGroupID;

        
        [DataMember(Order = 37)]
        public int? EquipmentTypeGroupID { get 
            {

                if (this.NullableCollections)
                {
                    return null;
                }

                return _EquipmentTypeGroupID;
                //if (EquipmentTypeObject !=null)
                //{
                //    return EquipmentTypeObject.EquipmentTypeGroupID;
                //}
                //else
                //{
                //    return null;
                //}

            }
            set
            {
                //if(EquipmentTypeObject== null)
                //{
                //    _equipmentTypeObject = new EquipmentType();
                //}

                //EquipmentTypeObject.EquipmentTypeGroupID = value;
                _EquipmentTypeGroupID = value;
            }
        }


        private EquipmentTypeGroup? _EquipmentTypeGroup;
        [NotMapped]
        [DataMember(Order = 38)]
        public virtual EquipmentTypeGroup? EquipmentTypeGroup
        {
            get
            {


                if (this.NullableCollections)
                {
                    return null;
                }

                return _EquipmentTypeGroup;

                //if (this.NullableCollections)
                //{
                //    return null;
                //}
                //else if(EquipmentTypeObject != null)
                //{
                //    return EquipmentTypeObject.EquipmentTypeGroup;
                //}
                //else
                //{
                //    return new EquipmentTypeGroup();
                //}
            }
            set
            {
                //_EquipmentTypeGroup = value;
                //if (EquipmentTypeObject!=null)
                //{
                //    EquipmentTypeObject.EquipmentTypeGroup = _EquipmentTypeGroup;
                //}
                _EquipmentTypeGroup = value;


            }
        }
    


        [NotMapped]
        [DataMember(Order = 39)]
        public bool NullableCollections { get; set; } = false;

        [NotMapped]
        [DataMember(Order = 75)]
        public string Component { get; set; }
       


        //[NotMapped]
        //public string ParentID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        //public required string Component { get; init; }

        public static EquipmentTemplate New(string component)
        {
            return new EquipmentTemplate() { Component = component };
        }



        //private EquipmentType _EquipmentType2;
        //[NotMapped]
        //[DataMember(Order = 40)]
        //public  virtual EquipmentType EquipmentTypeObjectTemp {
        //    get
        //    {
        //        if (this.NullableCollections)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            return _EquipmentType2;
        //        }

        //    }
        //    set
        //    {
        //        _EquipmentType2 = value;
        //    }
        //}

        public int? GetEquipmentTypeId()
        {

            var value = "";
            var valueint = 0;
            if (!string.IsNullOrEmpty(this?._equipmentTypeObject?.ETCalculatedAlgorithm) && this?._equipmentTypeObject?.ETCalculatedAlgorithm == "Parent")
            {
                return this._EquipmentTypeID;
            }
            
            if (!string.IsNullOrEmpty(this.EquipmentType))
            {
                var aaa= this.EquipmentType.Split(','); 

                if (aaa.Length > 0)
                {
                    value = aaa[0];
                }
            }

            if(!string.IsNullOrEmpty(value))
            {
                return Convert.ToInt32(value);
            }

            return this._EquipmentTypeID;
        }

        public void SetEquipmentTypeID(int? Value)
        {
            this._EquipmentTypeID = Value;
        }

        public EquipmentType GetEquipmentType(WorkOrderDetail? wod = null)
        {

            EquipmentType et = null;

            
            //if (this?.AditionalEquipmentTypes?.Count > 0)
            if (!string.IsNullOrEmpty(this?.AditionalEquipmentTypesJSON) 
                && (this.AditionalEquipmentTypes == null || this.AditionalEquipmentTypes.Count == 0))
            {
               
                this.AditionalEquipmentTypes = Newtonsoft.Json.JsonConvert.DeserializeObject<ICollection<EquipmentType>>(this.AditionalEquipmentTypesJSON);

            }
            
            if(string.IsNullOrEmpty(this?.AditionalEquipmentTypesJSON) && this?.AditionalEquipmentTypes?.Count == 0 && _equipmentTypeObject != null)
            {
                return _equipmentTypeObject;
            }

            //if (string.IsNullOrEmpty(this?.AditionalEquipmentTypesJSON) && this?.AditionalEquipmentTypes?.Count == 0 && _equipmentTypeObject == null && (EquipmentTypeGroup != null))
            //{
            //    return EquipmentTypeGroup.;
            //}

            if (this.AditionalEquipmentTypes != null && this.AditionalEquipmentTypes.Count == 1)
            {

                return this.AditionalEquipmentTypes.ElementAtOrDefault(0);

            }
            //else if (this.AditionalEquipmentTypes != null && this.AditionalEquipmentTypes.Count > 1 && wod !=null)
            //{
            //    et = new EquipmentType();

            //    et =  AditionalEquipmentTypes.Where(x => x.CalibrationTypeID == wod.CalibrationTypeID && x.EquipmentTypeGroupID == wod.PieceOfEquipment.EquipmentTemplate.EquipmentTypeGroupID && x.HasStandard == false).FirstOrDefault();

               
            //    return et;

            //}
            else if (this.AditionalEquipmentTypes != null && this.AditionalEquipmentTypes.Count > 1)
            {
               
                
                et = new EquipmentType();

                var dtr = AditionalEquipmentTypes.Where(x => x.HasWorkOrderDetail == true).FirstOrDefault();

                if(dtr != null)
                {
                    et.CalibrationTypeID = dtr.CalibrationTypeID;
                }
                else
                {
                    var etr=AditionalEquipmentTypes.FirstOrDefault();

                    et.CalibrationTypeID = etr.CalibrationTypeID;
                }

               
                
                var ettemp= AditionalEquipmentTypes.Where(x =>  x.HasWorkOrderDetail == true).FirstOrDefault();
                
                if(ettemp != null)
                {

                    //et.CalibrationType = ettemp.CalibrationType;
                    //et.StandardComponent = ettemp.StandardComponent;
                    //et.CalibrationInstrucctions = ettemp.CalibrationInstrucctions;
                    et = ettemp;
                }
               
                
                foreach (var item in this?.AditionalEquipmentTypes)
                {
                    //NumericExtensions.CombineBoolPropertiesFrom(et, item);

                    et.DynamicConfiguration = et.DynamicConfiguration || item.DynamicConfiguration;
                    et.DynamicConfiguration2 = et.DynamicConfiguration2 || item.DynamicConfiguration2;
                    et.HardnessTestBlockInformation = item.HardnessTestBlockInformation || et.HardnessTestBlockInformation;
                    et.HasAccredited = item.HasAccredited || et.HasAccredited;
                    et.HasCapacity = item.HasCapacity || et.HasCapacity;
                    et.HasCertificate = item.HasCertificate || et.HasCertificate;
                    et.HasClass = item.HasClass || et.HasClass;
                    et.HasIndicator = item.HasIndicator || et.HasIndicator;
                    et.HasPeripheral = item.HasPeripheral || et.HasPeripheral;
                    et.HasRepeateabilityAndEccentricity = et.HasRepeateabilityAndEccentricity || item.HasRepeateabilityAndEccentricity;
                    et.HasResolution = et.HasResolution || item.HasResolution;
                    et.HasReturnToZero = et.HasReturnToZero || item.HasReturnToZero;
                    et.HasScales = et.HasScales || item.HasScales;
                    et.HasStandard = et.HasStandard || item.HasStandard;
                    et.HasTestpoint = et.HasTestpoint || item.HasTestpoint;
                    et.HasTolerance = et.HasTolerance || item.HasTolerance;
                    et.HasUncertanity = et.HasUncertanity || item.HasUncertanity;
                    et.HasStandardRange = et.HasStandardRange || item.HasStandardRange;
                    et.HasStandardConfiguration  = et.HasStandardConfiguration || item.HasStandardConfiguration;
                    //et.CalibrationType
                    
                    
                }

                _equipmentTypeObject = et;
            }

            

            return _equipmentTypeObject;

        }

        public void SetEquipmentType(EquipmentType _equipmentType)
        {


            _equipmentTypeObject = _equipmentType;

        }


    }


    [DataContract]
    public class TestPointGroup
    {
        public TestPointGroup()
        {
            TestPoints= new HashSet<TestPoint>();
        }

        [Key]
        [Required(ErrorMessage = "Required TestPoitGroupID")]
        [DataMember(Order = 1)]
        public int TestPoitGroupID { get; set; }

        [DataMember(Order = 2)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required OutUnitOfMeasurementID")]
        [Range(1, Double.PositiveInfinity, ErrorMessage = "Select a Status please")]
        public int OutUnitOfMeasurementID { get; set; }

        [NotMapped]
        [DataMember(Order = 3)]        
        public string OutUnitOfMeasurement { get; set; } = "";

        [DataMember(Order = 4)]
        //[Range(1, Double.PositiveInfinity, ErrorMessage = "Select a Status please")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public string TypeID { get; set; }

        [NotMapped]
        [DataMember(Order = 5)]       
        public string Type { get; set; } = "";

        [DataMember(Order = 6)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required Name")]
        public string Name { get; set; } = "";

      
        //public int EquipmentTemplateID { get; set; } 

        [DataMember(Order = 7)]
        public virtual ICollection<TestPoint> TestPoints { get; set; }

        [DataMember(Order = 8)]
        public virtual UnitOfMeasure UnitOfMeasurementOut { get; set; }

        [DataMember(Order = 9)]
        public string Description { get; set; }


        [DataMember(Order = 10)]
        public int? WorkOrderDetailID { get; set; }

        [DataMember(Order = 11)]
        public int? EquipmentTemplateID { get; set; }


        [DataMember(Order = 12)]
        public string PieceOfEquipmentID { get; set; }

       

    }

    [DataContract]
    public  class TestPoint
    {
        public TestPoint()
        {
            //UnitOfMeasurement = new UnitOfMeasure();

            //UnitOfMeasurementOut = new UnitOfMeasure();
        }


        [Required(ErrorMessage = "Required TestPointID")]
        [DataMember(Order = 1)]
        public int TestPointID { get; set; }

        [Required(ErrorMessage = "Required NominalTestPoit")]
        [DataMember(Order = 2)]
        [Range(0, Double.PositiveInfinity, ErrorMessage = "Range NominalTestPoit")]
        public double NominalTestPoit { get; set; }

        

        //[ForeignKey("UnitOfMeasurementID"),Column(Order =1)]
        [DataMember(Order = 3)]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public virtual UnitOfMeasure UnitOfMeasurement { get; set; } 


        [DataMember(Order = 4)]        
        public double LowerTolerance { get; set; }

        [DataMember(Order = 5)]
        public double UpperTolerance { get; set; }

        [DataMember(Order = 6)]
        [Range(0.0000001, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        //[Column(TypeName = "decimal(10,6)")]
        public double Resolution  { get; set; }

        [DataMember(Order = 7)]
        public int DecimalNumber { get; set; }

        //[ForeignKey("UnitOfMeasurementOutID"),Column(Order =0)]
        [DataMember(Order = 8)]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public virtual UnitOfMeasure UnitOfMeasurementOut { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [IgnoreDataMember]
        public virtual TestPointGroup TestPointGroup { get; set; }

        [DataMember(Order = 9)]
        [Range(1, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int UnitOfMeasurementID { get; set; }


        [DataMember(Order = 10)]
        public int UnitOfMeasurementOutID { get; set; }


        [DataMember(Order = 11)]
        public string CalibrationType { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Linearity> Linearities { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Repeatability> Repeatabilities { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Eccentricity> Eccentricities { get; set; }
        [DataMember(Order = 12)]
        public string Description { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<WOD_TestPoint> WOD_TestPoints { get; set; }

        [DataMember(Order = 13)]
        public int? TestPointGroupTestPoitGroupID { get; set; }


        [DataMember(Order = 14)]
        public int TestPointTarget { get; set; } = 1;

        [DataMember(Order = 15)]
        public bool IsDescendant { get; set; }

        
        [DataMember(Order = 16)]
        public int Position { get; set; }


    }


}
