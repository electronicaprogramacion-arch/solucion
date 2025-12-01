using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Runtime.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    public class UserInformation
    {
        [DataMember(Order = 1)]
        public int UserInformationID { get; set; }

        [Required]
        [DataMember(Order = 2)]
        [StringLength(20, ErrorMessage = "Too long (20 character limit).")]
        public string Title { get; set; }

        [Required]
        [DataMember(Order = 3)]
        [Display(Name = "First Name")]
        [StringLength(40, ErrorMessage = "Too long (40 character limit).")]
        public string FirstName { get; set; }

        [Required]
        [DataMember(Order = 4)]
        [Display(Name = "Last Name")]
        [StringLength(40, ErrorMessage = "Too long (40 character limit).")]
        public string LastName { get; set; }

        [Required]
        [DataMember(Order = 5)]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        [DataMember(Order = 6)]
        public string Email { get; set; }

        [Required]

        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        [DataMember(Order = 7)]
        public string Password { get; set; }

        [Required]
        //[CompareProperty("Password")]
        [Display(Name = "Confirm Password")]
        [DataMember(Order = 8)]
        public string ConfirmPassword { get; set; }

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Accept Ts & Cs is required")]
        [DataMember(Order = 9)]
        public bool AcceptTerms { get; set; }
    }
}
