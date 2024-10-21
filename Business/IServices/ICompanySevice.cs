using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface ICompanySevice
    {
        Task<bool> ValidateCompanyAsync(string companyName);
    }
}
