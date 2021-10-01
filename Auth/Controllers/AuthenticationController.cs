using Auth.Models;
using Auth.Services.Interfaces;
using Auth.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _uow;
        private readonly JwtSettings _jwtsettings;

        public AuthenticationController(IPasswordHasher hasher , IOptions<JwtSettings> jwtsettings, IUnitOfWork uow)
        {
            _passwordHasher = hasher;
            _uow = uow;
            _jwtsettings = jwtsettings.Value;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel regVm)
        {
            if (regVm == null) return BadRequest();

            if (!ModelState.IsValid) return BadRequest(regVm);

            User user = new User
            { 
                UserName = regVm.UserName,
                Email = regVm.Email , 
                PasswordHash = _passwordHasher.Hash(regVm.Password)
            };

            await _uow.UserRepository.AddAsync(user);
            await _uow.SaveChangesAsync();

            var userVm = new UserViewModel
            {
                Username = user.UserName,
                Email = user.Email
            };

            Authenticate(userVm);

            return Ok(userVm); 
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn(LoginViewModel logVm)
        {
            if (logVm == null) return BadRequest();

            if (!ModelState.IsValid) return BadRequest(logVm);

            var hashedPwd = _passwordHasher.Hash(logVm.Password);

            var user = await _uow.UserRepository.GetAsync(x => x.PasswordHash == hashedPwd && x.UserName == logVm.UserName);           

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid credentials");
                return BadRequest(logVm);
            }

            var userVm = new UserViewModel
            {
                Username = user.UserName,
                Email = user.Email
            };

            Authenticate(userVm);

            return Ok(userVm);
        }

        [HttpGet("LogOut")]
        public IActionResult LogOut()
        {
            return Ok();
        }

        private void Authenticate(UserViewModel user)
        {
            //creates the token by combining the parts
            var tokenHandler = new JwtSecurityTokenHandler();
            
            //secret key of the server
            var key = Encoding.UTF8.GetBytes(_jwtsettings.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(2),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
        }
    }
}
