using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vidhyalaya.DB;

namespace Vidhyalaya.Models
{
    public class SubjectInCoursesViewModel
    {
        

        //added due to mapping

        public int Id { get; set; }

        [Required(ErrorMessage = "please select Subject")]

        [Display(Name = " Subject")]

        public int SubjectId { get; set; }

        [Required(ErrorMessage = "please select Course")]

        [Display(Name = " Courses")]

        public int CourseId { get; set; }

        [Required(ErrorMessage = "please select Course")]

        [Display(Name = " Course")]



        public virtual Course Courses { get; set; }

        [Required(ErrorMessage = "please select Subject")]

        [Display(Name = " Subject")]



        public virtual Subject Subjects { get; set; }

    }
}
