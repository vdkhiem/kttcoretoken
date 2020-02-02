using KTT.WebAPI.Models;
using KTT.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTT.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : BaseController<User>
    {
        private readonly IJwtTokenManager jwtTokenManager;
        public UserController(IEntityMaint<User> entityMaint, IJwtTokenManager jwtTokenManager) : base(entityMaint)
        {
            this.jwtTokenManager = jwtTokenManager;

            if (this.entityMaint.GetAll().Any()) return;
            this.entityMaint.Add(new User() { Id = 1, UserName = "khiemuser", Password = "khiempass" });
            this.entityMaint.Add(new User() { Id = 2, UserName = "tranuser", Password = "tranpass" });
            this.entityMaint.Add(new User() { Id = 3, UserName = "thyuser", Password = "thypass" });
        }

        [HttpGet("[action]/{userName}")]
        public IActionResult GetByName(string userName)
        {
            var user = entityMaint.Search(user => user.UserName == userName).FirstOrDefault();

            if (user != null)
                return Ok(user);
            else
                return NotFound();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(User user)
        {
            var token = jwtTokenManager.GenerateJwt(user.Id);
            if (string.IsNullOrEmpty(token)) NotFound();
            user.Password = null;
            user.Token = token;
            return Ok(user);
        }
    }
}
