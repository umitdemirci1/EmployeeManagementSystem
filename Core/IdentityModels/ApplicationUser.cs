using Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IdentityModels
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsDeleted { get; set; }
        public User User { get; set; }
        public string? CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
