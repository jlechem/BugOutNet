using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BugOutNetLibrary.Models.ViewModels
{
    public class BugViewModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set;}

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the creator.
        /// </summary>
        /// <value>
        /// The creator.
        /// </value>
        public string Creator { get; set; }

        /// <summary>
        /// Gets or sets the project identifier.
        /// </summary>
        /// <value>
        /// The project identifier.
        /// </value>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>
        /// The project.
        /// </value>
        public string Project { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the status identifier.
        /// </summary>
        /// <value>
        /// The status identifier.
        /// </value>
        public int StatusId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the priority identifier.
        /// </summary>
        /// <value>
        /// The priority identifier.
        /// </value>
        public int PriorityId { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public string Priority { get; set; }

        /// <summary>
        /// Gets or sets the assignted to identifier.
        /// </summary>
        /// <value>
        /// The assignted to identifier.
        /// </value>
        public int? AssigntedToId { get; set; }

        /// <summary>
        /// Gets or sets the assigned to.
        /// </summary>
        /// <value>
        /// The assigned to.
        /// </value>
        public string AssignedTo { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        public DateTime? Created { get; set; }

        /// <summary>
        /// Gets or sets the last updated.
        /// </summary>
        /// <value>
        /// The last updated.
        /// </value>
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the last updated identifier.
        /// </summary>
        /// <value>
        /// The last updated identifier.
        /// </value>
        public int? LastUpdatedId { get; set; }

        /// <summary>
        /// Gets or sets the last upated by.
        /// </summary>
        /// <value>
        /// The last upated by.
        /// </value>
        public string LastUpatedBy { get; set; }

        /// <summary>
        /// Gets or sets the new comment.
        /// </summary>
        /// <value>
        /// The new comment.
        /// </value>
        public string NewComment { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public List<BugCommentViewModel> Comments { get; set; }

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        /// <value>
        /// The attachments.
        /// </value>
        public List<BugAttachmentViewModel> Attachments { get; set; }

        /// <summary>
        /// Gets or sets the file upload.
        /// </summary>
        /// <value>
        /// The file upload.
        /// </value>
        public HttpPostedFileBase FileUpload { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BugViewModel"/> class.
        /// </summary>
        public BugViewModel()
        {
            Attachments = new List<BugAttachmentViewModel>();
            Comments = new List<BugCommentViewModel>();
        }

    }
}