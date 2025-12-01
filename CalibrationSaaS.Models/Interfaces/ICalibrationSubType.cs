using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


using CalibrationSaaS.Domain.Aggregates.Entities;


namespace CalibrationSaaS.Domain.Aggregates.Interfaces
{

    public interface IConfiguration
    {


        public string Configuration { get; set; }

        


    }


    public interface ITolerance
    {

        //public int? ToleranceTypeID { get; set; }

        public string JsonTolerance { get; set; }

        public  Tolerance Tolerance { get; set; }


        //public bool? FullScale { get; set; }

        //public double? TolerancePercentage { get; set; }

        //public double? ToleranceValue { get; set; }

        //public double? ToleranceFixedValue { get; set; }


        //public double ResolutionValue { get;  }


        //public double AccuracyPercentage { get; set; }







    }


    //public interface ITolerance
    //{

    //    public int? ToleranceTypeID { get; set; }

    //    public bool FullScale { get; set; }

    //    public double TolerancePercentage { get; set; }

    //    public double ToleranceValue { get; set; }

    //}

    public interface IResolution
    {

        
        public double Resolution { get; set; }
       
        public int DecimalNumber { get; set; }


    }


    public interface INew<T> where T : INew<T>
    {
        static abstract T New(string component);
    }
    public interface IComponentS
    {
        public string ComponentID { get; set; }
        public string Component { get; set; }


    }

    public interface IUncetainty
    {

        public ICollection<Uncertainty> Uncertainty { get; set; }

    }


    public interface IResultGen2
    {

        public GenericCalibration2 GenericCalibration2 { get; set; }


    }



        public interface IResultComp
    {
        public string ComponentID { get; set; }

        public string Component { get; set; }

        //public string GroupName { get; set; }

    }
    public interface IResultTesPointGroup
    {

        public string? GroupName { get; set; }

        public string? UncertaintyJSON { get; set; }
    }

    public interface IResultGen :  IResult, IResultPos
        {



        }



    public interface IResult3 : IResultComp, IResultGen, IResolution
    {
        


    }

    public interface IUpdated
    {

        public long? Updated { get; set; }

        

    }

  


    public interface ICreate
    {
        public GenericCalibrationResult2 CreateNew(int secuencie, int SubType, string Component, string ComponentId, GenericCalibration2 gen, GenericCalibrationResult2 genr);
        
        public GenericCalibrationResult2 CreateNew(int secuencie, int SubType, string Component, string ComponentId, double resolution, Dictionary<string, object> dic);


        public GenericCalibrationResult2 CreateNew(int secuencie, int SubType, string Component, string ComponentId, double resolution, dynamic gce);


        public GenericCalibration2 CreateNew(GenericCalibrationResult2 gcr);


    }



    public interface ISelect
    {

        public string KeyObject { get; set; }

        



    }


    public interface IAggregate
    {
        public string Aggregate { get; set; }
    }


    

    public interface IDynamic
    {
       
        public string Object { get; set; }
       
        public string ExtendedObject { get; set; }
    }

    public interface IResult2:  IResultGen, IResolution, IResult0
    {
        

    }


    public interface IResultPos
    {

        public int Position { get; set; }

    }



    public interface IResult0
    {

        public int WorkOrderDetailId { get; set; }

    }



        public interface IResult
    {
        public int SequenceID { get; set; }

        public int CalibrationSubTypeId { get; set; }

    }
    public interface ICalibrationSubType2 : ICalibrationSubType,IResultComp
    { 
    
    
    }

        public interface ICalibrationSubType: IResult, IResult0
    {
        //public int SequenceID { get; set; }

        //public int CalibrationSubTypeId { get; set; }

        //public int WorkOrderDetailId { get; set; }

        ICollection<WeightSet> WeightSets { get; set; }

         TestPoint TestPoint { get; set; }

        //BasicCalibrationResult BasicCalibrationResult { get; set; }
        double CalculateWeightValue { get; set; }

         ICollection<CalibrationSubType_Weight> CalibrationSubType_Weights { get; set; }


        //IResult BasicCalibrationResult { get; set; }

        int? TestPointID { get; set; }

        
        //ICollection<CalibrationSubType_Standard> Standards { get; set; }


    }

    public interface IGenericCalibrationStandard 
    {
        


        ICollection<CalibrationSubType_Standard> Standards { get; set; }


    }


    public interface IGenericCalibrationSubType<T> : ICalibrationSubType, IGenericCalibrationStandard where T:IResultGen,IResult0
    { 
           T  BasicCalibrationResult { get; set; }


          // ICollection<CalibrationSubType_Standard> Standards { get; set; }


    }


    //public interface IGenericCalibrationSubTypeGeneric<T> : IGenericCalibrationSubType<T>, ICalibrationSubType where T : IResult
    //{ 
    
    
    
    //}

        public interface IGenericCalibrationCollection<T> where T : IResult2
    {
        List<T> TestPointResult { get; set; }

       

    }


    //public interface IParentID<T> where T : IResult2
    //{
    //    List<T> TestPointResult { get; set; }

    //}




    public interface IGenericCalibrationSubTypeCollection<T> : IGenericCalibrationCollection<T>, IGenericCalibrationStandard, ICalibrationSubType where T : IResult2
    {
        //List<T> BasicCalibrationResult { get; set; }


    }



}
