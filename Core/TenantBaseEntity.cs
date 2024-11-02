using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class TenantBaseEntity : EntityBase, IMustHaveTenant
    {
        public string CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
