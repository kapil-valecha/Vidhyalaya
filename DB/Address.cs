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

    public partial class Address
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Address()
        {
            this.UserRegistrations = new HashSet<UserRegistration>();
        }
    
        public int AddressId { get; set; }
        [DisplayName("Address Line 01")]
        public string AddressTextBox1 { get; set; }
        [DisplayName("Address Line 02")]
        public string AddressTextBox2 { get; set; }
        [DisplayName("Country")]
        public int CountryId { get; set; }
        [DisplayName("State")]
        public int StateId { get; set; }
        [DisplayName("City")]
        public int CityId { get; set; }
    
        public virtual City City { get; set; }
        public virtual Country Country { get; set; }
        public virtual State State { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRegistration> UserRegistrations { get; set; }
    }
}
