using BugOutNetLibrary.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugOutNetLibrary.Models.GridModels
{
    public abstract class GridModel
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        protected Entities DB { get; set; }

        #endregion

        public GridModel() { }

    }
}
