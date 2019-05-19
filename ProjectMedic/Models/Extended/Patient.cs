using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProjectMedic.Models
{
    [MetadataType(typeof(PatientMetadata))]
    public partial class Patient
    {
        public string ConfirmPassword { get; set; }
    }

    public class PatientMetadata
    {
        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required!")]
        public string Patient_Firstname { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required!")]
        public string Patient_Lastname { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Patient_DoB { get; set; }

        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required!")]
        public string Patient_Email { get; set; }

        [Display(Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required!")]
        public string Patient_Username { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]
        [MinLength(10, ErrorMessage = "Minimum 10 characters are required!")]
        public string Patient_Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Patient_Password", ErrorMessage = "Passwords are not match!")]
        public string ConfirmPassword { get; set; }
    }
}