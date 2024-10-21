using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IAccountService
    {
        Task<string> LoginAsync(string tenantId, string email, string password);
    }
}
