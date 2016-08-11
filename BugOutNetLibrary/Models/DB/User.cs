//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BugOutNetLibrary.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Users_Projects = new HashSet<Users_Projects>();
            this.Users_Roles = new HashSet<Users_Roles>();
        }
    
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] Password { get; set; }
        public string EmailAddress { get; set; }
        public bool IsVerified { get; set; }
        public bool IsBlocked { get; set; }
        public int AccessFailedCount { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public byte[] Avatar { get; set; }
        public bool AutoLogin { get; set; }
        public bool IsAdmin { get; set; }
        public string Salt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Users_Projects> Users_Projects { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Users_Roles> Users_Roles { get; set; }
    }
}
