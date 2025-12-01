using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Helpers.Controls
{


    [DataContract]
    public class KeyValueOption
    {
        public KeyValueOption()
        {

        }
        public KeyValueOption(string key, string value,string option)
        {

            Key= key;   
            Value= value;   
            Option= option; 


        }

        [NotMapped]
        [DataMember(Order = 1)]
        public string Key { get; set; }

        [NotMapped]
        [DataMember(Order = 2)]
        public string Value { get; set; }


        [NotMapped]
        [DataMember(Order = 3)]
        public string Option { get; set; }

        [NotMapped]
        [DataMember(Order = 4)]
        public int? CalibrationTypeID { get; set; }


        [NotMapped]
        [DataMember(Order = 5)]
        public string Fields { get; set; }

        [NotMapped]
        [DataMember(Order = 6)]
        public string Component { get; set; }


    }




    [DataContract]
    public class KeyValue2
    {
        public KeyValue2()
        {

        }
        public KeyValue2(int Key, string value)
        {

        }

        [NotMapped]
        [DataMember(Order = 2)]
        public int Key { get; set; }

        [NotMapped]
        [DataMember(Order = 1)]
        public string Value { get; set; }

       
    }



    [DataContract]
    public class KeyValue 
    {
        public KeyValue()
        {

        }
        public KeyValue(string Key, string value)
        {
          
        }
        [NotMapped]
        [DataMember(Order = 1)]
        public string Key { get; set; }

        [NotMapped]
        [DataMember(Order = 2)]
        public int Value { get; set; }
    }
    [DataContract]
    public class KeyValueDate 
    {
        public KeyValueDate()
        {

        }
        public KeyValueDate(string Key, string value)
        {

        }
        [NotMapped]
        [DataMember(Order = 1)]
        public DateTime Key { get; set; }

        [NotMapped]
        [DataMember(Order = 2)]
        public int Value { get; set; }
    }

    [DataContract]
    public class KeyValueSearch 
    {
        public KeyValueSearch()
        {

        }
        public KeyValueSearch(string Key, string value)
        {

        }
        [NotMapped]
        [DataMember(Order = 1)]
        public string Type { get; set; }

        [NotMapped]
        [DataMember(Order = 2)]
        public string Link { get; set; }

        [NotMapped]
        [DataMember(Order = 3)]
        public string Fields { get; set; }
    }

    [DataContract]
    public class CustomClaim 
    {
        public CustomClaim()
        {

        }
        public CustomClaim(string Key, string value)
        {

        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [DataMember(Order = 1)]
        public int CustomClaimID { get; set; }



        [DataMember(Order = 2)]
        public string Key { get; set; }



        [DataMember(Order = 3)]
        public string Value { get; set; }

        [DataMember(Order = 4)]
        public virtual CurrentUser User { get; set; }

        [DataMember(Order = 4)]
        public int CurrentUserID { get; set; }


    }

    public class CurrentUser
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        [DataMember(Order = 1)]
        public int CurrentUserID { get; set; }

        [DataMember(Order = 2)]
        public string Type { get; set; }


        [DataMember(Order = 3)]
        public virtual ICollection<CustomClaim> Claims { get; set; }

        [NotMapped]
        //[DataMember(Order = 4)]
        public  int? TechID { get; set; }

        [NotMapped]
        //[DataMember(Order = 4)]
        public string Name { get; set; }

        [NotMapped]
        //[DataMember(Order = 4)]
        public string Roles { get; set; }



    }



    public class SelectFilter {

        public string[] Filter { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
        public string Method { get; set; }
        public Dictionary<string,Dictionary<string,string>> Options { get; set; }


    }

}
