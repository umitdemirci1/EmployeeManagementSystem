﻿using Core.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Company : EntityBase
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Domain { get; set; }

        public ICollection<ApplicationUser> Employees { get; set; } 
    }
}
