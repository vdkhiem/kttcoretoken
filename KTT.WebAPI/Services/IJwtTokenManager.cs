using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTT.WebAPI.Services
{
    public interface IJwtTokenManager
    {
        string GenerateJwt(int userId);
    }
}
