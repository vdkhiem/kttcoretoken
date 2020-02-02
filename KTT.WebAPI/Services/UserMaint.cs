using KTT.WebAPI.Models;
using KTT.WebAPI.Shared;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KTT.WebAPI.Services
{
    public class UserMaint : EntityMaint<User>, IUserMaint
    {
        private readonly AppSettings appSettings;
        private readonly IJwtTokenManager jwtTokenManager;

        public UserMaint (AppSettings appSettings, IJwtTokenManager jwtTokenManager)
        {
            this.appSettings = appSettings;
            this.jwtTokenManager = jwtTokenManager;
        }

        public User Authenticate(string userName, string password)
        {
            var user = new User() { UserName= userName, Password = password};
            // TODO: validate userName and password

            user.Token = jwtTokenManager.GenerateJwt(user.Id);

            user.Password = null;

            return user;
        }
    }
}
