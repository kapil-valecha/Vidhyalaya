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

    public partial class UserInRole
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        [DisplayName("Role")]
        public int RoleId { get; set; }
    
        public virtual Role Role { get; set; }
        public virtual UserRegistration UserRegistration { get; set; }
    }
}
