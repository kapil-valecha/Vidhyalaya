using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidhyalaya.Areas.Admin.Models
{
    public class SubjectViewModel
    {
        public int SubjectId { get; set; }
        [Required]
        [Display(Name = "Add Subject ")]
        public string SubjectName { get; set; }
    }
}