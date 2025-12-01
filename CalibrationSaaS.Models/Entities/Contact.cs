using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;


namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public class Contact :  IGeneric
    {


        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [DataMember(Order = 1)]
        public int ContactID { get; set; }
        
        [DataMember(Order = 2)]        
        [Required(AllowEmptyStrings =false, ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "Name is too long.")]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Required")]        
        [StringLength(50, ErrorMessage = "Name is too long.")]
        public string LastName { get; set; }



        [DataMember(Order = 4)]
        public bool IsEnabled { get; set; }

        [DataMember(Order = 5)]
        public string UserName { get; set; }


        //[Required(AllowEmptyStrings = false, ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "Ivalid Email")]
        [DataMember(Order =6)]
        public string Email { get; set; }

       


        [DataMember(Order = 7)]
        public string Occupation { get; set; }

        [Display(Name = "PasswordReset")]
        [DataMember(Order = 8)]
        public bool PasswordReset { get; set; }

        [DataMember(Order = 11)]
        public int AggregateID { get; set; }
        [DataMember(Order = 12)]
        public string Description { get; set; }

        
        //[Required(AllowEmptyStrings = true, ErrorMessage = "Required")]
        [DataMember(Order = 13)]
        [StringLength(13, ErrorMessage = "Number is too long. (Max 13 digits")]
        public string PhoneNumber { get; set; }

        [Phone]
        [DataMember(Order = 14)]
        [StringLength(13, ErrorMessage = "Number is too long. (Max 13 digits")]
        public string CellPhoneNumber { get; set; }

        [DataMember(Order = 15)]
        public bool IsDelete { get; set; }
        //[DataMember(Order = 8)]
        //public string Roles { get; set; }

        //[DataMember(Order = 9)]
        //public IList<Rol> RolesList { get; set; }
        [DataMember(Order = 16)]
        public int? CustomerAggregateAggregateID { get; set; }

        [DataMember(Order = 17)]
        public string Note { get; set; }



    }

   
    

}
