using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectPlatform.Models
{
    public class CommentModels
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public DateTime PostDate { get; set; }
        public string Content { get; set; }

    }
}