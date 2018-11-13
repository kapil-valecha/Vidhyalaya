using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vidhyalaya.Areas.Admin.Models
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }

        [Required]
        public string CourseName { get; set; }
    }
}