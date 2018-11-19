using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Vidhyalaya.Models
{
    public class UserRegistrationViewModel
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required (ErrorMessage ="Please Select Gender")]
        public string Gender { get; set; }
        public string Hobby { get; set; }

        [Required]
        [Remote("CheckEmailExists", "Student", HttpMethod = "POST", ErrorMessage = "Email address already registered.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string EmailId { get; set; }

        public bool? IsEmailVerified { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
               
        [Display(Name = "Role")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "please choose required course.")]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Display(Name = "Address")]
        public int AddressId { get; set; }

        [Required(ErrorMessage = "Atleast one Address Line is required.")]
        [Display(Name = "Address Line 01")]
        public string AddAddressTextBox1 { get; set; }

        [Display(Name = "Address Line 02")]
        public string AddAddressTextBox2 { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        public string CountryName { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [Display(Name = "State")]
        public int StateId { get; set; }

        public string StateName { get; set; }

        [Display(Name = "City")]
        public int CityId { get; set; }

        public string CityName { get; set; }

        public IEnumerable<CityModel> CityList { get; set; }
        public IEnumerable<StateModel> StateList { get; set; }
     
        [Display(Name = "Pincode")]
        [RegularExpression(@"[0-9]{6}", ErrorMessage = "Please enter 6 digit valid Pincode")]
        public int? Pincode { get; set; }

        [Required]
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        public bool? IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
    public class EditRegistrationViewModel
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please Select Gender")]
        public string Gender { get; set; }
        public string Hobby { get; set; }

        [Required]
        
        [EmailAddress]
        [Display(Name = "Email")]
        public string EmailId { get; set; }

        public bool? IsEmailVerified { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Role")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "please choose required course.")]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Display(Name = "Address")]
        public int AddressId { get; set; }

        [Required(ErrorMessage = "Atleast one Address Line is required.")]
        [Display(Name = "Address Line 01")]
        public string AddAddressTextBox1 { get; set; }

        [Display(Name = "Address Line 02")]
        public string AddAddressTextBox2 { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        public string CountryName { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [Display(Name = "State")]
        public int StateId { get; set; }

        public string StateName { get; set; }

        [Display(Name = "City")]
        public int CityId { get; set; }

        public string CityName { get; set; }

        public IEnumerable<CityModel> CityList { get; set; }
        public IEnumerable<StateModel> StateList { get; set; }

        [Display(Name = "Pincode")]
        [RegularExpression(@"[0-9]{6}", ErrorMessage = "Please enter 6 digit valid Pincode")]
        public int? Pincode { get; set; }

        [Required]
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        public bool? IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
    public class StateModel
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
    }
    public class CityModel
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
}