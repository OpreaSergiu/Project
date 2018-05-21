using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectPlatform.Models
{
    public class ProjectCommentsViewModels
    {
        public ProjectStudentModels Project { get; set; }
        public IEnumerable<CommentModels> Comments { get; set; }
    }
}