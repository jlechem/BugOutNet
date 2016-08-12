﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugOutNetLibrary.Models.GridModels
{
    public class ProjectGridModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required(AllowEmptyStrings =false, ErrorMessage ="Name is required.")]
        [StringLength( 100 )]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [Required( AllowEmptyStrings = false, ErrorMessage = "Description is required." )]
        [StringLength( 2000 )]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        [DisplayFormat( ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}" )]
        public DateTime? Created { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectGridModel"/> class.
        /// </summary>
        public ProjectGridModel()
        {

        }

    }
}