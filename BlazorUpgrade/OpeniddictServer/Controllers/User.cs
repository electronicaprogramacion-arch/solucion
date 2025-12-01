
using OpeniddictServer.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;


namespace CalibrationSaaS.Domain.Aggregates.Entities
{


    [DataContract]
    public class User
    {
        public User()
        {

        }

        [Key]
        [DataMember(Order = 1)]
        public int UserID { get; set; }

        [DataMember(Order = 2)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "Name is too long.")]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "Name is too long.")]
        public string LastName { get; set; }


        //[DataMember(Order = 4)]       
        //[StringLength(50, ErrorMessage = "Name is too long.")]
        //public string TechnicianCode { get; set; }


        [DataMember(Order = 4)]
        [StringLength(50, ErrorMessage = "Name is too long.")]
        public string UserTypeID { get; set; }

        [Display(Name = "PasswordReset")]
        [DataMember(Order = 5)]
        public bool PasswordReset { get; set; }


        //[DataMember(Order = 7)]
        //public string  RoleID { get; set; }

        [DataMember(Order = 6)]
        public bool IsEnabled { get; set; }

        [DataMember(Order = 7)]
        public string UserName { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "Ivalid Email")]
        [DataMember(Order = 8)]
        public string Email { get; set; }


        [NotMapped]
        [DataMember(Order = 9)]
        public IList<Rol> RolesList { get; set; }



        //[Required(AllowEmptyStrings = false, ErrorMessage = "Rol Required")]
        [DataMember(Order = 10)]
        public string Roles { get; set; }

        //[DataMember(Order = 13)]
        //public IEnumerable<Rol> Roles1 { get; set; }


        [DataMember(Order = 11)]
        public string Occupation { get; set; }

        [IgnoreDataMember]
        public string Description { get; set; }


        [DataMember(Order = 14)]
        public string PassWord { get; set; }

        [DataMember(Order = 15)]
        public string IdentityID { get; set; }


        [DataMember(Order = 16)]
        public string Message { get; set; }

        [NotMapped, IgnoreDataMember]
        public ApplicationUser UserIdentity { get; set; }

        [DataMember(Order = 17)]
        public string OldPassWord { get; set; }


    }

    [DataContract]
    public class Rol
    {
        public Rol()
        {

        }


        public Rol(int id, string _Name)
        {
            RolID = id;
            Name = _Name;
        }


        [Required]
        [DataMember(Order = 1)]
        public int RolID { get; set; }

        [Required]
        [DataMember(Order = 2)]
        public string Name { get; set; }

        [Required]
        [DataMember(Order = 3)]
        public string Description { get; set; }

        [DataMember(Order = 4)]
        public virtual ICollection<User_Rol> UserRoles { get; set; }





    }
    [DataContract]
    public class User_Rol
    {
        [DataMember(Order = 1)]
        public int UserID { get; set; }

        [DataMember(Order = 2)]
        public int RolID { get; set; }

        [DataMember(Order = 3)]
        public User User { get; set; }

        [DataMember(Order = 4)]
        public Rol Rol { get; set; }



    }
}