using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Text;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    
    [DataContract]
    
    public class CustomerAggregate : IGeneric
    {
        public CustomerAggregate()
        {
            //Socials = new HashSet<Social>();
            //Addresses = new HashSet<Address>();
            //Contacts= new HashSet<Contact>();
            //PhoneNumbers = new HashSet<PhoneNumber>();

        }

        [Key]
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int AggregateID { get; set; }


        //[Required(ErrorMessage = "Required")]
        //[DataMember(Order = 2)]
        //public int CustomerID { get; set; }



        
        [DataMember(Order = 2)]
        public string Name { get; set; }

        
        [DataMember(Order = 3)]
        public virtual ICollection<Address> Addresses { get; set; }


        [DataMember(Order = 4)]
        public virtual ICollection<EmailAddress> EmailAddresses { get; set; }


        [DataMember(Order = 5)]
        public virtual ICollection<Contact> Contacts { get; set; }


        [DataMember(Order = 6)]
        public virtual  ICollection<Social> Socials { get; set; }


        [DataMember(Order =7)]
        public virtual  ICollection<PhoneNumber> PhoneNumbers { get; set; }

        [DataMember(Order = 8)]
        public  int CustomerID { get; set; }


        [IgnoreDataMember]
        public string Description { get ; set; }
    }
}
