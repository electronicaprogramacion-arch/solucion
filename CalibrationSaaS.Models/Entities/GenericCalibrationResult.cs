using Bogus;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class GenericCalibrationResult:IResult2, IUpdated
    {

        //[Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int SequenceID { get; set; } //Consecutive of the Calibration Result

        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 2)]
        public int CalibrationSubTypeId { get; set; }

        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 3)]
        public int WorkOrderDetailId { get; set; }

        [DataMember(Order = 4)]
        public int Position { get; set; }


        [DataMember(Order = 5)]
        public double Resolution { get; set; }

        
        [DataMember(Order = 6)]
        public int DecimalNumber { get; set; }


        [DataMember(Order = 7)]
        public string Object { get; set; }

        [DataMember(Order = 8)]
        public string ExtendedObject { get; set; }


        [JsonIgnore]
        [NotMapped]
        [IgnoreDataMember]
        public virtual GenericCalibration GenericCalibration { get; set; }

        [NotMapped]
        [DataMember(Order = 9)]
        public bool HasModified { get; set; }

        [NotMapped]
        public virtual List<DynamicProperty> CurrentViews { get; set; }
        public string ComponentID { get ; set ; }

        [JsonIgnore]
        [NotMapped]
        public GenericCalibration2 GenericCalibration2 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        [DataMember(Order = 10)]
        public long? Updated { get ; set ; }

        [NotMapped]
        [DataMember(Order = 11)]
        public ICollection<GenericCalibrationResult2Aggregate> Aggregates { get; set; }
        
        


        //[DataMember(Order = 10)]
        //public string ComponentID { get; set; }

    }


    [DataContract]
    public class GenericCalibrationResult2Aggregate
    {


        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int SequenceID { get; set; } //Consecutive of the Calibration Result


        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 2)]
        public int CalibrationSubTypeId { get; set; }


        [DataMember(Order = 11)]
        public string ComponentID { get; set; }

        [DataMember(Order = 12)]
        public string Component { get; set; }


        [DataMember(Order = 13)]
        public string Type { get; set; }



        [DataMember(Order = 14)]
        public string JSON { get; set; }

    }


    [DataContract]
    public class GenericCalibrationResult2 : IResult2, IResultComp,IResultGen2,IUpdated, IDynamic, ISelect, IAggregate,ICreate, IResultTesPointGroup
    {

        //[Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int SequenceID { get; set; } //Consecutive of the Calibration Result


        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 2)]
        public int CalibrationSubTypeId { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [DataMember(Order = 3)]
        public int WorkOrderDetailId { get; set; }

        [DataMember(Order = 4)]
        public int Position { get; set; }


        [DataMember(Order = 5)]
        public double Resolution { get; set; }


        [DataMember(Order = 6)]
        public int DecimalNumber { get; set; }


        [DataMember(Order = 7)]
        public string Object { get; set; }

        [DataMember(Order = 8)]
        public string ExtendedObject { get; set; }



        [NotMapped]
        [DataMember(Order = 9)]
        public virtual GenericCalibration2 GenericCalibration2 { get; set; }

        [NotMapped]
        [DataMember(Order = 10)]
        public bool HasModified { get; set; }

        [NotMapped]
        public virtual List<DynamicProperty> CurrentViews { get; set; }


        [DataMember(Order = 11)]
        public string ComponentID { get; set; }

        [DataMember(Order = 12)]
        public string Component { get; set; }

        [DataMember(Order = 13)]
        public long? Updated { get; set; } = 0;

        [DataMember(Order = 14)]
        public string KeyObject { get ; set ; }


        [NotMapped] 
        [DataMember(Order = 15)]
        public string Aggregate { get; set; }

        [DataMember(Order = 16)]
        public string? GroupName { get; set; } = "TestPointResult";

        [DataMember(Order = 17)]
        public string? UncertaintyJSON { get; set; }


        public GenericCalibrationResult2 CreateNew(int secuencie, int SubType, string _Component, string ComponentId, GenericCalibration2 gen,GenericCalibrationResult2 genr)
        {

            SequenceID = secuencie;
            CalibrationSubTypeId = SubType;
            Component = Component;
            ComponentID = ComponentId;
            Aggregate = genr.Aggregate;
            CurrentViews = genr.CurrentViews;
            DecimalNumber = genr.DecimalNumber;
            ExtendedObject = genr.ExtendedObject;
            HasModified = genr.HasModified;
            KeyObject = genr.KeyObject;
            Object = genr.Object;
            Position = secuencie;
            Resolution = genr.Resolution;
            GroupName = genr.GroupName;
            UncertaintyJSON = genr.UncertaintyJSON;

            if (genr.GenericCalibration2 != null)
            {
                GenericCalibration2 = genr.GenericCalibration2;
                


            }
            else if(gen != null)
            {
                GenericCalibration2 = gen;
            }
            else
            {
                GenericCalibration2 gc = new GenericCalibration2();
                gc.TestPointResult = new List<GenericCalibrationResult2>();

                //gc.WorkOrderDetailId = eq.WorkOrderDetailID;
                gc.CalibrationSubTypeId = SubType;
                gc.Component = Component;//"WorkOrderItem";
                gc.ComponentID = ComponentId;
                gc.SequenceID = secuencie;
                GenericCalibration2 = gc;
                GenericCalibration2.TestPointResult.Add(this);
            }
            
            GenericCalibration2.SequenceID = secuencie;
            //GenericCalibration2.TestPointResult = new List<GenericCalibrationResult2>();


            return this;




        }




        public GenericCalibrationResult2 CreateNew(int secuencie, int SubType, string Component, string ComponentId, double resolution, Dictionary<string,object> dic)
        {

            //dynamic gce = new ExpandoObject();

            var eo = new ExpandoObject();
            var eoColl = (ICollection<KeyValuePair<string, object>>)eo;

            foreach (var kvp in dic)
            {
                eoColl.Add(kvp);
            }

            dynamic eoDynamic = eo;


            return CreateNew(secuencie, SubType, Component, ComponentId, resolution, eoDynamic);


        }

        public GenericCalibrationResult2 CreateNew(int secuencie,int SubType, string Component,string ComponentId,double resolution,dynamic gce)
        {

            GenericCalibration2 gc = new GenericCalibration2();
            gc.TestPointResult = new List<GenericCalibrationResult2>();

            //gc.WorkOrderDetailId = eq.WorkOrderDetailID;
            gc.CalibrationSubTypeId = SubType;
            gc.Component = Component;//"WorkOrderItem";
            gc.ComponentID = ComponentId;
            gc.SequenceID = secuencie;
          

            //GenericCalibrationResult2 gcc = new GenericCalibrationResult2();
            //gcc.WorkOrderDetailId = eq.WorkOrderDetailID;
            this.CalibrationSubTypeId = SubType;
            this.SequenceID = secuencie;
            this.Resolution = resolution;//eq.Resolution;
            this.Component = Component; // "WorkOrderItem";
            this.ComponentID = ComponentId;
            this.SequenceID = secuencie;
            this.Position = secuencie;
            this.GenericCalibration2 = gc;

           
            if(gce != null)
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(gce);

                this.Object = json;
            }

            gc.TestPointResult.Add(this);
            return this;


        }

       
       

        public GenericCalibration2 CreateNew(GenericCalibrationResult2 gcr)
        {

            GenericCalibration2 gc = new GenericCalibration2();
            gc.TestPointResult = new List<GenericCalibrationResult2>();

          
            gc.CalibrationSubTypeId = gc.CalibrationSubTypeId;
            gc.SequenceID = gc.SequenceID;

            gc.Component = gc.Component; // "WorkOrderItem";
            gc.ComponentID = gc.ComponentID;
            gc.GroupName = gcr.GroupName;
            gc.UncertaintyJSON = gcr.UncertaintyJSON;
            //gc.BasicCalibrationResult.Add(gcr);
            this.GenericCalibration2 = gc;
            
            gc.TestPointResult.Add(this);
            return gc;


        }



    }


}