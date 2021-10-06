﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackerProject.Models
{
    public class Category
    {
        public string CategoryId { get; set; }
        public string CtaegoryName { get; set; }
        [ForeignKey("ProjectId")]
        public string ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<Bug> Bugs { get; set; }
    }
}