using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Xml;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    
    //[DataContract]
    public partial class EntityBasic
    {
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



       
        private Tolerance _Tolerance= new Tolerance();

        [NotMapped]
        [DataMember(Order = 102)]
        public Tolerance Tolerance { 
            get 
            {
                
                //var errors = new List<string>();

                //if (!string.IsNullOrEmpty(this._JsonTolerance))
                //{
                //    _Tolerance = Newtonsoft.Json.JsonConvert.DeserializeObject<Tolerance>(this.JsonTolerance, new JsonSerializerSettings()
                //    {
                //        Error = (sender, error) =>
                //        {
                //            errors.Add(error.ErrorContext.Error.Message);
                //            error.ErrorContext.Handled = true;

                //            Console.WriteLine("Tolerance " + error.ErrorContext.Error.Message);


                //        }//error.ErrorContext.Handled = true

                //    });

                //    if (errors.Count > 0)
                //    {
                //        _Tolerance = new Tolerance();


                //    }
                //}
                //else
                //{
                //    //_Tolerance = new Tolerance();
                //}

                return _Tolerance; 
            } 
            set 
            { 


                _Tolerance = value;

                var str2 = Newtonsoft.Json.JsonConvert.SerializeObject(_Tolerance);

                JsonTolerance = str2;


            } 
        }


        public void Tolerance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //var args = e as PropertyChangedExtendedEventArgs;
            //do whatever you want


//            Console.WriteLine("Tolerance_PropertyChanged");
        }

        //[IgnoreDataMember]
        //public DateTime CreateDate { get; set; } = DateTime.Now;

        //[IgnoreDataMember]
        //public DateTime UpdateDate { get; set; } = DateTime.Now;

        //[IgnoreDataMember]
        //public string UserCreate { get; set; } = "test";

        //[IgnoreDataMember]
        //public string UserUpdate { get; set; } = "testupdate";

    }
}
