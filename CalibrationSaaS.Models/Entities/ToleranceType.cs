using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using Helpers;
using Newtonsoft.Json;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public partial class ToleranceType:IGeneric
    {
       
        [DataMember(Order = 1)]
        public string Key { get; set; }
        
        [DataMember(Order = 2)]
        public string Value { get; set; }

         [DataMember(Order = 3)]
        public int CalibrationTypeId { get; set; }

        [NotMapped]
        [DataMember(Order = 5)]
        public virtual CalibrationType CalibrationType { get; set; }

        [DataMember(Order = 6)]
        public bool? Enable { get; set; }

    }

    //[DataContract]
    //public partial class ToleranceType_EquipmentType : IGeneric
    //{
        
    //    [DataMember(Order = 1)]
    //    public string Key { get; set; }

    //    [DataMember(Order = 2)]
    //    public int EquipmentTypeID { get; set; }

    //    [DataMember(Order = 3)]
    //    public virtual ToleranceType ToleranceType { get; set; }

    //    [DataMember(Order = 4)]
    //    public virtual EquipmentType EquipmentType { get; set; }



    //}


    [DataContract]
    public partial class Tolerance: INotifyPropertyChanged//: ITolerance
    {

        private int? _ToleranceTypeID;

        double _TolerancePercentage;

        bool? _FullScale;

        bool? _UsePreviousCalibrationData;

        [DataMember(Order = 1)]
        public int? ToleranceTypeID 
        {
            get { return _ToleranceTypeID; 
            }
            set {
                
                SetPropertyField(() => this.ToleranceTypeID, ref _ToleranceTypeID, value); 

                //OnPropertyChanged(() => this.ToleranceTypeID);
            }

        }

        private void OnPropertyChanged(Expression<Func<int?>> value)
        {
            string propertyName = (value.Body as MemberExpression).Member.Name;
        }

        [DataMember(Order = 2)]
        public bool? FullScale 
        {
            get
            {
                return _FullScale;
            }
            set { 
                SetPropertyField(() => this.FullScale, ref _FullScale, value); 
            }


        }

        
        [DataMember(Order = 3)]
        public double TolerancePercentage {
            get
            {
                return _TolerancePercentage;
            }
            set
            {
                SetPropertyField(() => this.TolerancePercentage, ref _TolerancePercentage, value);
            }
        }


        double _ToleranceValue;

        [DataMember(Order = 4)]
        public double ToleranceValue
        {
            get
            {
                return _ToleranceValue;
            }
            set
            {
                SetPropertyField(() => this.ToleranceValue, ref _ToleranceValue, value);
            }
        }


        double _ToleranceFixedValue;
        [DataMember(Order = 5)]
        public double ToleranceFixedValue
        {
            get
            {
                return _ToleranceFixedValue;
            }
            set
            {
                SetPropertyField(() => this.ToleranceFixedValue, ref _ToleranceFixedValue, value);
            }
        }


        //double _ResolutionValue;

        //[DataMember(Order = 6)]
        //public double ResolutionValue
        //{
        //    get
        //    {
        //        return _ResolutionValue;
        //    }
        //    set
        //    {
        //        SetPropertyField(() => this.ResolutionValue, ref _ResolutionValue, value);
        //    }
        //}

        double _AccuracyPercentage;

        [DataMember(Order = 7)]
        public double AccuracyPercentage
        {
            get
            {
                return _AccuracyPercentage;
            }
            set
            {
                SetPropertyField(() => this.AccuracyPercentage, ref _AccuracyPercentage, value);
            }
        }


        double _MaxValue;
        [System.Text.Json.Serialization.JsonIgnore]
        public double MaxValue
        {
            get
            {
                return _MaxValue;
            }
            set
            {
                SetPropertyField(() => this.MaxValue, ref _MaxValue, value);
            }
        }


        double _Resolution;

        [DataMember(Order = 8)]       
        public double Resolution
        {
            get
            {
                return _Resolution;
            }
            set
            {
                SetPropertyField(() => this.Resolution, ref _Resolution, value);
            }
        }


        [DataMember(Order = 9)]
      
        public int DecimalNumber { get; set; }


        [DataMember(Order = 10)]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public string Json { get; set; }


        [DataMember(Order = 11)]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool Changed { get; set; }

        [DataMember(Order = 12)]
        public bool? UsePreviousCalibrationData
        {
            get
            {
                return _UsePreviousCalibrationData;
            }
            set
            {
                SetPropertyField(() => this.UsePreviousCalibrationData, ref _UsePreviousCalibrationData, value);
            }


        }

        public Tolerance GetTolerance(string Json, string JsonTolerance, Tolerance _tolerance)
        {
            Tolerance _Tolerance = new Tolerance();

            var errors = new List<string>();

            string JsonValue = string.Empty;

            if ((!string.IsNullOrEmpty(Json)) || (string.IsNullOrEmpty(JsonTolerance) && !string.IsNullOrEmpty(Json)))
            {
                JsonValue = _tolerance.Json;
            }

            else if (!string.IsNullOrEmpty(JsonTolerance) && !_tolerance.ToleranceTypeID.HasValue)
            {
                JsonValue = JsonTolerance;
            }

            if (!string.IsNullOrEmpty(JsonValue))
            {
                _Tolerance = Newtonsoft.Json.JsonConvert.DeserializeObject<Tolerance>(JsonValue, new JsonSerializerSettings()
                {
                    Error = (sender, error) =>
                    {
                        errors.Add(error.ErrorContext.Error.Message);
                        error.ErrorContext.Handled = true;

//                        Console.WriteLine("Tolerance " + error.ErrorContext.Error.Message);


                    }

                });

                if (errors.Count > 0)
                {
                    _Tolerance = new Tolerance();


                }
            }


            return _Tolerance;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        //private static PropertyChangedEventArgs ValuePropertyChangedEventArgs = new PropertyChangedEventArgs("Value");
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {

           

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }

            var str2 = Newtonsoft.Json.JsonConvert.SerializeObject(this);

            Json = str2;

            //Console.WriteLine("tolerance jason" + Json);


        }


        private bool onchange;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgsCustom e)
        {

            onchange = true;

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }

            //9503
            string valueString = e.Value != null  ? e.Value.ToString()  : "null";
//            Console.WriteLine(e.PropertyName + " : " + valueString);

            this.SetPropertyValue(e.PropertyName, e.Value);

            var str2 = Newtonsoft.Json.JsonConvert.SerializeObject(this);

            Json = str2;

            //Console.WriteLine("tolerance jason " + Json);

            Changed = true; 

            onchange = false;

        }

        protected virtual void OnPropertyChanged(MemberExpression vali)
        {
            //string propertyName = (vali.Body as MemberExpression).Member.Name;

            //MemberExpression body = vali.Body as MemberExpression;

            //if (body == null)
            //{
            //    UnaryExpression ubody = (UnaryExpression)vali.Body;
            //    body = ubody.Operand as MemberExpression;
            //}

            //var propertyName = body.Member.Name;


            //OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected void SetPropertyField<T>(string propertyName, ref T field, T newValue)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                OnPropertyChanged(new PropertyChangedEventArgsCustom(propertyName, newValue));
            }
        }


        protected void SetPropertyField<T>(Expression<Func<int?>> prop, ref T field, T newValue)
        {

            string propertyName = (prop.Body as MemberExpression).Member.Name;

            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                if (!onchange)
                {
                    OnPropertyChanged(new PropertyChangedEventArgsCustom(propertyName, newValue));
                }
               
            }
        }

        protected void SetPropertyField<T>(Expression<Func<bool?>> prop, ref T field, T newValue)
        {

            string propertyName = (prop.Body as MemberExpression).Member.Name;

            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                OnPropertyChanged(new PropertyChangedEventArgsCustom(propertyName, newValue));
            }
        }

        protected void SetPropertyField<T>(Expression<Func<double>> prop, ref T field, T newValue)
        {

            string propertyName = (prop.Body as MemberExpression).Member.Name;

            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                OnPropertyChanged(new PropertyChangedEventArgsCustom(propertyName, newValue));
            }
        }

    }


    public class PropertyChangedEventArgsCustom : PropertyChangedEventArgs
    {

        public object Value { get; set; }
        public PropertyChangedEventArgsCustom(string propertyName,object value) : base(propertyName)
        {

            Value = value;
        }
    }

}
