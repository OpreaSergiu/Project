﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectPlatform.Models
{
    public class AccountInfoModels
    {
        [Key]
        public int Id { get; set; }
        public string AccountId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Address { get; set; }
        public string CNP { get; set; }
    }
}