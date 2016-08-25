using BugOutNetLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugOutNetLibrary.Models
{
    public class ExceptionModel
    {
        public int Id { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public int? CreatedByID { get; set; }
        public ErrorStatus ErrorStatus { get; set; }
        public DateTime Created { get; set; }
    }
}
