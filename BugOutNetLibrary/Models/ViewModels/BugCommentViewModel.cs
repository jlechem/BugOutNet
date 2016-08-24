using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugOutNetLibrary.Models.ViewModels
{
    public class BugCommentViewModel
    {
        public int Id { get; set; }
        public int BugId { get; set; }
        public string Comment { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }

    }
}