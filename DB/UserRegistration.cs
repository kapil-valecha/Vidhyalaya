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
    
    public partial class UserRegistration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserRegistration()
        {
            this.TeacherInSubjects = new HashSet<TeacherInSubject>();
            this.UserInRoles = new HashSet<UserInRole>();
        }
    
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Hobby { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public System.DateTime DOB { get; set; }
        public int AddressId { get; set; }
        public int CourseId { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateModified { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual Course Course { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TeacherInSubject> TeacherInSubjects { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserInRole> UserInRoles { get; set; }
    }
}
