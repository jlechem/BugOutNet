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
    
    public partial class BugAttachment
    {
        public int Id { get; set; }
        public int BugId { get; set; }
        public byte[] Attachment { get; set; }
        public string FileName { get; set; }
        public int UploadedById { get; set; }
        public System.DateTime Created { get; set; }
    
        public virtual Bug Bug { get; set; }
        public virtual User User { get; set; }
    }
}
