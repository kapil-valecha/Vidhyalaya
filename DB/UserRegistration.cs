//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vidhyalaya.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class UserRegistration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserRegistration()
        {
            this.TeacherInSubjects = new HashSet<TeacherInSubject>();
            this.UserInRoles = new HashSet<UserInRole>();
        }
    
        public int UserId { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Gender")]
        public string Gender { get; set; }
        [DisplayName("Hobby")]
        public string Hobby { get; set; }
        [DisplayName("Email Id")]
        public string EmailId { get; set; }
        [DisplayName("Password")]
        public string Password { get; set; }
        [DisplayName("Date Of Birth")]
        public System.DateTime DOB { get; set; }
        [DisplayName("Address")]
        public int AddressId { get; set; }
        [DisplayName("Course")]
        public int CourseId { get; set; }
        [DisplayName("Role")]
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        [DisplayName("Date Create")]
        public System.DateTime DateCreated { get; set; }
        [DisplayName("Date Modified")]
        public System.DateTime DateModified { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual Course Course { get; set; }
        public virtual Role Role { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TeacherInSubject> TeacherInSubjects { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserInRole> UserInRoles { get; set; }
    }
}
