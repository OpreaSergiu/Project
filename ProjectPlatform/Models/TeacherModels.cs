using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectPlatform.Models
{
    public class TeacherModels
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Class { get; set; }
        public string Subject { get; set; }
        public int SubjectId { get; set; }
    }
}