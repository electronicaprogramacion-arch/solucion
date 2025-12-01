using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Collections;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using System.Collections.Specialized;
using System.Linq;
using Helpers.Controls;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public partial class ChildrenView
    {
        [NotMapped]
        [DataMember(Order = 2)]
        public List<GenericCalibrationResult2> TestPointResult { get; set; }

        [Key]
        [NotMapped]
        [DataMember(Order = 1)]
        public string ID { get; set; }


    }


    [DataContract]
    public partial class WorkOrderDetail :  IGenericCalibrationCollection<GenericCalibrationResult2>, IGeneric, IDisposable, INoteWOD, IResolution, IConfiguration,ITolerance,IUncetainty,IComponentS
    {
        public WorkOrderDetail()
        {

            //WorkOder = new WorkOrder();
            //CalibrationResult = new HashSet<CalibrationResult>();
            //EquipmentCondition = new HashSet<EquipmentCondition>();
            CurrentStatus = new Status();
            PreviusStatus = new List<Status>();


        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "Required WorkOrderDetailID")]
        [DataMember( Order = 1)]
        public int WorkOrderDetailID { get; set; }

        [Required(ErrorMessage = "Required WorkOrderID")]
        [DataMember(Order = 2)]
        public int WorkOrderID { get; set; }

        //[Required(ErrorMessage = "Required")]
        [DataMember(Order = 3)]
        public int TenantId { get; set; }


        [NotMapped]
        [Required(ErrorMessage = "Required PieceOfEquipmentId")]
        [DataMember(Order = 4)]
        public string PieceOfEquipmentId { get; set; }

        [Required(ErrorMessage = "Required IsAccredited")]
        [DataMember(Order = 5)]
        public bool? IsAccredited { get; set; } = false;


        //[Required(ErrorMessage = "Required IsService")]
        //[DataMember(Order = 6)]
        //public bool? IsService { get; set; } = true;


        //[Required(ErrorMessage = "Required IsRepair")]
        //[DataMember(Order = 7)]
        //public bool? IsRepair { get; set; } = false;

        [Required(ErrorMessage = "Required SelectedNewStatus")]
        [DataMember(Order = 8)]
        public short SelectedNewStatus { get; set; } = 1; //Ready for Calibration



        [DataMember(Order = 9)]
        public virtual Status CurrentStatus { get; set; }

        [NotMapped]

        [DataMember(Order = 10)]
        public virtual ICollection<Status> PreviusStatus { get; set; }


        //[DataMember(Order = 11)]
        //public string Comments { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[DataMember(Order = 12)]
        //public string TechnicianName { get; set; }
        // [Required(ErrorMessage = "Required")]
        [DataMember(Order = 11)]
        public string CertificateComment { get; set; }



        //[DataMember(Order = 13)]
        //public string StandardDescription { get; set; }


        [DataMember(Order = 12)]
        public double Humidity { get; set; }



        [DataMember(Order = 13)]
        public double Temperature { get; set; }

        [DataMember(Order = 14)]
        public string Description { get; set; }


        //[DataMember(Order = 17)]
        //public virtual EquipmentTemplate EquipmentTemplate { get; set; }

        [DataMember(Order = 15)]
        public int TemperatureUOMID { get; set; }


        //[NotMapped]
        [DataMember(Order = 16)]
        public virtual PieceOfEquipment PieceOfEquipment { get; set; }

        [DataMember(Order = 17)]
        public virtual User Technician { get; set; }

        //[DataMember(Order = 18)]
        //public int StandarID { get; set; }

        [DataMember(Order = 19)]
        public int CalibrationIntervalID { get; set; }


        //[DataMember(Order = 20)]
        //public int CalibrationIntervalName { get; set; }


        [DataMember(Order = 21)]
        public DateTime? CalibrationDate { get; set; }


        [DataMember(Order = 22)]
        public DateTime? CalibrationCustomDueDate { get; set; }


        [DataMember(Order = 23)]
        public DateTime? CalibrationNextDueDate { get; set; }


        [DataMember(Order = 24)]
        public virtual WorkOrder WorkOder { get; set; }

        [DataMember(Order = 25)]
        public virtual ICollection<EquipmentCondition> EquipmentCondition { get; set; }


        [NotMapped]

        [DataMember(Order = 26)]
        public virtual UnitOfMeasure HumidityUOM { get; set; }


        [NotMapped]

        [DataMember(Order = 27)]
        public virtual UnitOfMeasure TemperatureUOM { get; set; }

        [StringLength(500, ErrorMessage = "Note is too long.")]
        [DataMember(Order = 28)]
        public string TechnicianComment { get; set; } 


        [DataMember(Order = 29)]
        public int TestPointNumber { get; set; } = 5;

        [DataMember(Order = 30)]
        public int HumidityUOMID { get; set; }


        [DataMember(Order = 31)]
        public int? TechnicianID { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [DataMember(Order = 32)]
        public virtual ICollection<Status> PossibleNextStatus { get; set; }

        [DataMember(Order = 33)]
        public int CurrentStatusID { get; set; }


        //[DataMember(Order = 33)]
        //public int TemperatureUOMID { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[DataMember(Order = 36)]

        //public virtual ICollection<EquipmentCondition> EquipmentCondition { get; set; }

        //[DataMember(Order = 34)]
        //public int? ToleranceTypeID { get; set; }

        [DataMember(Order = 35)]
        public virtual BalanceAndScaleCalibration BalanceAndScaleCalibration { get; set; }

        //[DataMember(Order = 36)]
        //public double AccuracyPercentage { get; set; }

        [DataMember(Order = 37)]
        public int DecimalNumber { get; set; }

        [DataMember(Order = 38)]
        public double Resolution { get; set; }

        //[DataMember(Order = 39)]
        //public double Tolerance { get; set; }



        [DataMember(Order = 40)]
        public string Environment { get; set; }


        [DataMember(Order = 41)]
        public virtual ICollection<WOD_TestPoint> WOD_TestPoints { get; set; }


        [DataMember(Order = 42)]
        public virtual ICollection<WOD_Weight> WOD_Weights { get; set; }


        [DataMember(Order = 43)]
        public string WorkOrderDetailHash { get; set; }



        [DataMember(Order = 44)]
        public virtual ICollection<WorkDetailHistory> WorkDetailHistorys { get; set; }


        [DataMember(Order = 45)]
        public int? AddressID { get; set; }


        public virtual Address Address { get; set; }


        private int? calibrationTypeID;
        [DataMember(Order = 46)]
        public int? CalibrationTypeID 
        {
            get
            {
                if (this?.PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject != null)
                {
                    return this.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject.CalibrationTypeID;
                }

                return null;
            }
            set
            {
                calibrationTypeID = value;
            }

        }


        public CalibrationType calibrationType; //CalibrationType

        [NotMapped]
        [DataMember(Order = 47)]
        public virtual CalibrationType CalibrationType {
            get 
            {
                //if (this?.PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject != null && !NullableCollections)
                //{
                //    return this.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject.CalibrationType;
                //}

                //return null;
                return calibrationType ?? this?.PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject?.CalibrationType;
            }

            set
            {
                calibrationType = value;
            }
           
        }



        [DataMember(Order = 48)]
        public virtual ICollection<TestPointGroup> TestGroups { get; set; }


        [DataMember(Order = 49)]
        public int ClassHB44 { get; set; }

        [DataMember(Order = 50)]
        public bool IsComercial { get; set; }
        //AccuracyPercentage

        //    DecimalNumber
        //    Resolution

        //    Tolerance
        [DataMember(Order = 51)]
        public virtual ICollection<RangeTolerance> Ranges { get; set; }

        [IgnoreDataMember]
        public string Name { get; set; }

        [DataMember(Order = 52)]
        public int Multiplier { get; set; } = 1;

        [DataMember(Order = 53)]
        public string OfflineID { get; set; }

        [DataMember(Order = 54)]
        public int OfflineStatus { get; set; }

        [DataMember(Order = 55)]
        public DateTime? StatusDate { get; set; }

        [DataMember(Order = 56)]
        public bool HasBeenCompleted { get; set; }


         [DataMember(Order = 57)]
        public virtual Certificate Certificate { get; set; }

         [NotMapped] 
        public int? ModeID { get; set; }

         [DataMember(Order = 58)] 
        public bool IncludeASTM { get; set; }

       [DataMember(Order = 59)]
        public bool IsUniversal { get; set; }


        
         [DataMember(Order = 60)]
         public double TemperatureAfter { get; set; }

         [DataMember(Order = 61)]
         public int? CertificationID { get; set; }



         // Alias property for compatibility
         //[NotMapped]
         //public string PieceOfEquipmentID
         //{
         //    get => PieceOfEquipmentId;
         //    set => PieceOfEquipmentId = value;
         //}

         [DataMember(Order = 62)]
        public virtual Certification Certification { get; set; }

        [NotMapped]
        public bool NullableCollections { get; set; }

        //[DataMember(Order = 63)]
        //public  int CalibrationSubTypeID{ get; set; }


        [DataMember(Order = 64)]
        public string TemperatureStandardId { get; set; }

        private TestCode _TestCode;
         [DataMember(Order = 65)]
        public virtual TestCode TestCode { get { if (NullableCollections) { return null; } else { return _TestCode; } } set { _TestCode = value; } }


        [DataMember(Order = 66)]
        public int? TestCodeID  { get; set; }

        [DataMember(Order = 67)]
        public bool? EndOfMonth { get; set; }

        //[DataMember(Order = 68)]
        //public int? ExternalConditionId { get; set; }

        private ICollection<ExternalCondition> _EnviromentCondition;

        [DataMember(Order = 68)]
        public virtual ICollection<ExternalCondition> EnviromentCondition { get { if (NullableCollections) { return null; } else { return _EnviromentCondition; } } set { _EnviromentCondition = value; } }


        private ICollection<NoteWOD> _NotesWOD;

        [DataMember(Order = 69)]
        public virtual ICollection<NoteWOD> NotesWOD { get { if (NullableCollections) { return null; } else { return _NotesWOD; } } set { _NotesWOD = value; } }


        [DataMember(Order = 70)]
        public virtual ICollection<WOD_Standard> WOD_Standard { get; set; }

        [DataMember(Order = 71)]
        public string WorkOrderDetailUserID { get; set; }

        private ICollection<CalibrationSubType_Standard> _CalibrationSubType_Standards;
        [NotMapped]
        public virtual ICollection<CalibrationSubType_Standard> CalibrationSubType_Standards { get { if (NullableCollections) { return null; } else { return _CalibrationSubType_Standards; } } set { _CalibrationSubType_Standards = value; } }

        [NotMapped]
        [System.Text.Json.Serialization.JsonIgnore]
        public bool dispose { get; set; }


        [NotMapped]
        [System.Text.Json.Serialization.JsonIgnore]
        public bool Refresh { get; set; }


        [DataMember(Order = 72)]
        public string Configuration { get ; set ; }


        private ICollection<WOD_Procedure> _WOD_Procedure;

        [NotMapped]
        [DataMember(Order = 73)]
        public virtual ICollection<WOD_Procedure> WOD_Procedure { get { if (NullableCollections) { return null; } else { return _WOD_Procedure; } } set { _WOD_Procedure = value; } }

        
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool IsModifiedOff { get; set; }


       
        [DataMember(Order = 110)]
        public bool IsOffline { get; set; }

        [DataMember(Order = 74)]
        public int? ToleranceTypeID { get; set; }


        [NotMapped]
        [DataMember(Order = 111)]
        public bool IsSync { get; set; }

       


        [DataMember(Order = 112)]
        public bool? IsUSP41 { get; set; } =  false;

        [DataMember(Order = 113)]
        public string? JsonTestPointsGroups { get; set; }

        [DataMember(Order = 114)]
        public string? CompositeWodId { get; set; }

        public static Expression<Func<WorkOrderDetail, bool>> WorkOrderDetailFilter;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tolerance section
        /// </summary>
        //[DataMember(Order = 73)]
        //public bool? FullScale { get; set; }

        //[DataMember(Order = 74)]
        //public double? TolerancePercentage { get; set; }

        //[DataMember(Order = 75)]
        //public double? ToleranceValue { get; set; }

        //[DataMember(Order = 76)]
        //public double? ToleranceFixedValue { get ; set ; }


        [NotMapped]
        public virtual List<GenericCalibrationResult2> TestPointResult { get 
            {
                if(this?.BalanceAndScaleCalibration == null)
                {
                    return null;
                }
                if(this?.BalanceAndScaleCalibration?.TestPointResult == null)
                {
                    return null;
                }
                else
                {
                    return this.BalanceAndScaleCalibration.TestPointResult;
                }
                
            
            
            } set 
            {
                if(this.BalanceAndScaleCalibration != null)
                {
                    this.BalanceAndScaleCalibration.TestPointResult = value;
                }
                
            } 
         }

        //[DataMember(Order = 77)]
        //public string JsonTolerance { get ; set ; }

        //[DataMember(Order = 78)]
        //public Tolerance Tolerance { get ; set ; }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
                Tolerance tol = new Tolerance();

                _Tolerance = tol.GetTolerance(_Tolerance.Json, this.JsonTolerance, _Tolerance);


                return _Tolerance;
            }
            set
            {
                _Tolerance = value;

                JsonTolerance = _Tolerance.Json;
            }
        }


        [NotMapped]
        public virtual ICollection<Uncertainty> Uncertainty { get ; set ; }

        [NotMapped]
        [DataMember(Order=75)]
        public string Component { get; set; }

        [NotMapped]
        [DataMember(Order = 76)]
        public string ComponentID
        {
            get
            {
              return WorkOrderDetailID.ToString();
            }
            set
            {
                var val = Convert.ToInt32(value);
                WorkOrderDetailID = val;
            }
        }

        [DataMember(Order = 77)]
        public int? ParentID  { get; set; }

        [NotMapped]
        [DataMember(Order = 78)]
        public int? WorkOrderDetailIdPrevious { get; set; }


        [NotMapped]
        [DataMember(Order = 79)]
        public bool OnlyChngeStatus { get; set; }

     
        public void Dispose()
        {
            dispose2();


        }

        public void dispose2()
        {
            if (dispose)
            {
            this.BalanceAndScaleCalibration = null;
            this.PieceOfEquipment = null;
            this.Ranges = null;
            this.CurrentStatus = null;
            this.WorkOder = null;
            this.WorkDetailHistorys = null;
            this.WOD_Weights = null;
            this.TestGroups = null;
            }
           
        }

        public int? GetCalibrationTypeID()
        {
            if (this?.PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject != null)
            {
                var calibrationType =  this?.PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject.CalibrationTypeID;
                return calibrationType;

            }

            return null;
        }


        public ICalibrationType GetCalibrationType()
        {
            if (this?.PieceOfEquipment?.EquipmentTemplate?.GetEquipmentType(this).CalibrationType != null)
            {
                var result = this.PieceOfEquipment.EquipmentTemplate.GetEquipmentType(this);
                var result1 = this.PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject?.CalibrationType;
                if(result?.CalibrationType?.CalibrationSubTypes == null && result1.CalibrationSubTypes != null)
                {
                    return result1;
                }
                return result.CalibrationType;

            }

            ICalibrationType cal = new CalibrationType() as ICalibrationType;

            cal.CalibrationSubTypes = new List<CalibrationSubType>();
            
            return cal;
        }


    }




}
