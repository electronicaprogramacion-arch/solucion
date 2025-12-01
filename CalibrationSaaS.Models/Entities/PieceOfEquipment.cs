using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using static CalibrationSaaS.Domain.Aggregates.Querys.QueryableExtensions2;
using static System.Formats.Asn1.AsnWriter;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    public abstract class Genericcondition<T>
    {

        public T Entity { get; set; }

        public abstract bool DisplayCondition(T parameter);

        public abstract object ValueCondition(T parameter);


    }


    public class viewModel<T>
    {

        public viewModel(T parametro)
        {
            Entity=parametro;   
        }
        public bool IsVisible { get; set; }

        public T Entity { get; set; }
    }
   
    [DataContract]
    public class PieceOfEquipment :  IGenericCalibrationCollection<GenericCalibrationResult2>, IGeneric,IResolution,IConfiguration, ITolerance, IUncetainty, IComponentS
    {

        public class SerialNumberCondition : Genericcondition<PieceOfEquipment>
        {
            public override bool DisplayCondition(PieceOfEquipment parameter)
            {
                return true;// parameter.SelectSingle(x => x.);
            }


            public override object ValueCondition(PieceOfEquipment parameter)
            {
                return null; // parameter.SelectSingle(x => x.);
            }





        }


        // public PieceOfEquipment(EquipmentTemplate et)
        //{
        //    this.EquipmentTemplate = et;    
        //    //WorkOrderDetail = new HashSet<WorkOrderDetail>();
           
        //    //EquipmentTemplate = new EquipmentTemplate();
        //}


        public PieceOfEquipment()
        {
            
            //WorkOrderDetail = new HashSet<WorkOrderDetail>();
           
            //EquipmentTemplate = new EquipmentTemplate();
        }



        [Key]        
        //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        //[Range(1, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        [DataMember(Order = 1)]
        public string PieceOfEquipmentID { get; set; }

        [CustomDisplayAttribute("", Condition= typeof(SerialNumberCondition))]
        [DataMember(Order = 2)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required SerialNumber")]
        public string SerialNumber { get; set; }

        private double _Capacity = 0;
        [DataMember(Order = 3)]
        public double Capacity { 
            get {
                
                
                return _Capacity;
            } 
            set
            { 
                _Capacity=value;    
            
            } }

        

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Required AddressId")]
        //[Range(1, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        [DataMember(Order = 5)]
        public int AddressId { get; set; }

        

        [DataMember(Order = 6)]
        public virtual EquipmentTemplate EquipmentTemplate { get; set; }

        //[DataMember(Order = 7)]
        //public int EquipmentId { get; set; }

       
        [DataMember(Order = 7)]
        public int Status { get; set; }

        
        [DataMember(Order = 8)]
        public string InstallLocation { get; set; }

       
        [DataMember(Order = 9)]
        public int TenantId { get; set; }

       
        //[DataMember(Order = 11)]
        //public int ClientId { get; set; }

        
        //[DataMember(Order = 12)]
        //public int UnitOfMeasurementId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required DueDate")]
        [DataMember(Order = 10)]
        public DateTime DueDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Required CustomerId")]
        [Range(1, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        [DataMember(Order = 11)]
        public int CustomerId { get; set; }       
        
       

        [DataMember(Order = 14)]
        public string Class { get; set; }

        [NotMapped]
        [DataMember(Order = 15)]
        public bool IsStandard { get; set; } = false;

        [NotMapped]
        [DataMember(Order = 16)]
        public bool IsWeigthSet { get; set; } = false;

        [DataMember(Order = 17)]
        public bool IsForAccreditedCal { get; set; }


        [DataMember(Order = 18)]
        public DateTime CalibrationDate { get; set; } = DateTime.Today;

   
        
        [DataMember(Order = 19)]
        public virtual Customer Customer { get; set; }

        
        
        [DataMember(Order = 24)]
        public virtual IList<User> Users { get; set; }


        [DataMember(Order = 25)]
        public virtual  ICollection<WeightSet> WeightSets { get; set; }

        
        [DataMember(Order = 26)]
        public int EquipmentTemplateId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        //[DataMember(Order = 27)]
        public virtual ICollection<WorkOrderDetail> WorOrderDetails { get; set; }

        //[DataMember(Order = 28)]
        //public int? WorOrderDetailID { get; set; }

        //[DataMember(Order = 29)]
        //public virtual ICollection<WorkOrder> WorOrder { get; set; }


        //[DataMember(Order = 29)]
        //public int? WorkOrderID { get; set; }


        [DataMember(Order = 30)]
        public virtual ICollection<POE_WorkOrder> POE_WorkOrders { get; set; }

        [DataMember(Order = 31)]
        [ForeignKey("IndicatorPieceOfEquipmentID")]
        [System.Text.Json.Serialization.JsonIgnore]
        [IgnoreDataMember]
        public virtual PieceOfEquipment Indicator { get; set; }


        [DataMember(Order = 32)]
        public string IndicatorPieceOfEquipmentID { get; set; }


        [DataMember(Order = 33)]
        
        public int? UnitOfMeasureID { get; set; }


        [DataMember(Order = 34)]
        
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }

        [IgnoreDataMember]
        
        public virtual ICollection<POE_User> POE_Users { get; set; }

       

        [DataMember(Order = 36)]
        public virtual ICollection<POE_POE> POE_POE { get; set; }


        [DataMember(Order = 37)]
        public virtual ICollection<RangeTolerance> Ranges { get; set; }



        private int? _ToleranceTypeID;
        [DataMember(Order = 38)]

        public int? ToleranceTypeID { get { return _ToleranceTypeID; } set { _ToleranceTypeID = value; } }


        //[DataMember(Order = 9)]       
        //public string ToleranceType { get; set; } = "";


        //[DataMember(Order = 39)]       
        //public double AccuracyPercentage { get; set; }

        [IsVisible(true)]
        [DataMember(Order = 40)]       
        public double Resolution { get; set; }


        [DataMember(Order = 41)]      
        public int DecimalNumber { get; set; }



        [DataMember(Order = 42)]
        public bool IsToleranceImport { get; set; }

        [DataMember(Order = 43)]
        public virtual ICollection<PieceOfEquipment> Peripherals { get; set; }

        [DataMember(Order = 44)]
        public bool IsDelete { get; set; }

        [DataMember(Order = 45)]
        public virtual ICollection<TestPointGroup> TestGroups { get; set; }

        [DataMember(Order = 46)]
        public bool IsTestPointImport { get; set; }

        [DataMember(Order = 47)]
        public int ClassHB44 { get; set; }

        [DataMember(Order = 48)]
        public string OfflineID { get; set; }

        [IsVisible(true)]
        [DataMember(Order = 49)]
        public string Notes { get; set; }

        [DataMember(Order = 50)]
        public string CustomerToolId { get; set; }

        [NotMapped]
        [DataMember(Order = 51)]
        public virtual ICollection<CertificatePoE> CertificatePoEs { get; set; }

         
        [NotMapped]
        [DataMember(Order = 69)]
        public virtual ICollection<Uncertainty> Uncertainty { get; set; }

        [NotMapped]
        [DataMember(Order = 52)]
        public bool IsNew { get; set; }


        [NotMapped]
        [DataMember(Order = 53)]
        public string JobComments { get; set; }

        [DataMember(Order = 54)]
        public virtual ICollection<POE_Scale> POE_Scale { get; set; }


        [DataMember(Order = 55)]
        public double UncertaintyValue { get; set; }

        //[DataMember(Order = 56)]
        //public double ToleranceValue { get; set; }

        [DataMember(Order = 57)]
        public string Scale { get; set; }


        [DataMember(Order = 58)]
        public virtual ICollection<CalibrationSubType_Standard> CalibrationSubType_Standard { get; set; }

        [DataMember(Order = 59)]
        public double ToleranceValueISO { get; set; }


        [DataMember(Order = 60)]
        public int? TypeMicro { get; set; }

        [DataMember(Order = 61)]
        public  double MicronValue { get; set; }


        [DataMember(Order = 62)]
        public double Hardness { get; set; }


        [IgnoreDataMember]
        public virtual ICollection<WOD_Standard> WOD_Standard { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<WO_Standard> WO_Standard { get; set; }

        //        Uncertainty Value(Double)
        //Tolerance Value(Double)
        //Scale to Apply

        [DataMember(Order = 63)]
        public double? LoadKGF { get; set; }

        [DataMember(Order = 64)]
        public double? ToleranceHV { get; set; }

        [DataMember(Order = 65)]
        public int? TestCodeID { get; set; }

        [NotMapped]
        [DataMember(Order = 66)]
        public virtual List<GenericCalibrationResult2> TestPointResult { get; set; }

        [NotMapped]
        [DataMember(Order = 67)]
        public int ToleranceUoM { get; set; }


        [DataMember(Order = 68)]
        public string Configuration { get; set; }

        //[DataMember(Order = 69)]
        //public bool? FullScale { get ; set ; }

        [DataMember(Order = 70)]
        public double? TolerancePercentage { get; set; }

        [DataMember(Order = 71)]
        double? ToleranceValue { get; set; }

        [DataMember(Order = 72)]
        public double? ToleranceFixedValue { get; set; }

        // Dynamic pricing is now handled through PriceTypePrice junction table
        // This allows flexible pricing based on configurable price types

        [NotMapped]
        [DataMember(Order = 75)]
        public string Component { get; set; }

        [NotMapped]
        [DataMember(Order = 76)]
        public string ComponentID
        {
            get
            {
                return PieceOfEquipmentID;
            }

            set
            {
                PieceOfEquipmentID = value;
            }
        }

        private string _ParentID;
        [DataMember(Order = 77)]
        public string ParentID
        {
            get
            {
                return _ParentID;
            }

            set
            {
                _ParentID = value;
            }
        }

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
                Tolerance tol1 = new Tolerance();
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


                    //var str2 = Newtonsoft.Json.JsonConvert.SerializeObject(tol);

                    //this._JsonTolerance = str2;

                    _Tolerance = tol;

                }

                if (_Tolerance != null)
                {
                   



                    _Tolerance = tol1.GetTolerance(_Tolerance.Json, this.JsonTolerance, _Tolerance);

                }             


                return _Tolerance;

            }
            set
            {


                _Tolerance = value;

                if(_Tolerance != null)
                {
                    JsonTolerance = _Tolerance.Json;
                }
                else
                {
                    JsonTolerance="";
                }

                


            }
        }





        public Dictionary<string, string> GetVirtualPoE()
        {

            if(this.TestPointResult != null)
            {




            }

            return null;

        }

        [NotMapped]
        [DataMember(Order = 103)]
        public bool IsOffline { get; set; }

        
        [DataMember(Order = 104)]
        public string Description { get; set; }

    }

    [DataContract]
    public class POE_WorkOrder
    {
        [DataMember(Order = 1)]
        public string PieceOfEquipmentID { get; set; }

        [DataMember(Order = 2)]
        public virtual PieceOfEquipment PieceOfEquipment { get; set; }
        [DataMember(Order = 3)]
        public int WorkOrderID { get; set; }
        [DataMember(Order = 4)]
        public virtual WorkOrder WorkOrder { get; set; }

    }


}
