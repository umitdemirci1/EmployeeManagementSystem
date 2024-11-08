using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class ApprovedCompaniesResponse
    {
        public string CompanyId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
