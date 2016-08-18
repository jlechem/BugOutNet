using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugOutNetLibrary.Models.ViewModels
{
    public class BugAttachmentViewModel
    {
        public int Id { get; set; }
        public int BugId { get; set; }
        public byte[] Attachment { get; set; }

    }
}