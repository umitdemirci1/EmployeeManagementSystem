﻿using Core.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Strategies
{
    public interface IApplicationManagerJwtTokenGenerator
    {
        Task<string> GenerateJwtToken(ApplicationUser user);
    }
}
