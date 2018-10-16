using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IdentityModel;

namespace Vidhyalaya.Models
{
    public class UserSearchViewModel
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        public string Gender { get; set; }
        public string Hobby { get; set; }
        [Required]
        [DisplayName("Email Id")]
        public string EmailId { get; set; }
        [Display(Name = "Role")]
        public int RoleId { get; set; }
        [Display(Name = "Course")]
        public int CourseId { get; set; }
        [Display(Name = "Address")]
        public int AddressId { get; set; }
        [Display(Name = "Address Line 01")]
        public string AddAddressTextBox1 { get; set; }
        [Display(Name = "Address Line 02")]
        public string AddAddressTextBox2 { get; set; }
        [Display(Name = "Country")]
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        [Display(Name = "State")]
        public int StateId { get; set; }
        public string StateName { get; set; }
        [Display(Name = "City")]
        public int CityId { get; set; }
        public string CityName { get; set; }
        public IEnumerable<CityModel> CityList { get; set; }
        public IEnumerable<StateModel> StateList { get; set; }
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}