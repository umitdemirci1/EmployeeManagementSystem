using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IdentityModels
{
    public class ApplicationRole : IdentityRole
    {
        public bool IsDeleted { get; set; }
    }
}
