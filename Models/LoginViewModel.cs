﻿using System.ComponentModel.DataAnnotations;
using System.IdentityModel;
namespace Vidhyalaya.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter your Email Address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Id")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Enter your password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}