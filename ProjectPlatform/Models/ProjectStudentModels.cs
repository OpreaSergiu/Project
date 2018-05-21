using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectPlatform.Models
{
    public class ProjectStudentModels
    {
        [Key]
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Subject { get; set; }
        public float Grade { get; set; }
        public string StudentEmail { get; set; }
        public string FolderPath { get; set; }
        public string Class { get; set; }
        public string StudentName { get; set; }
    }
}