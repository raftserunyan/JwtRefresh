using Auth.Models;
using Auth.Services;
using Auth.Services.Interfaces;
using Auth.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IPasswordHasher _passwordHasher;
        private IUnitOfWork _uow;

        public AuthenticationController(IPasswordHasher hasher , IUnitOfWork uow)
        {
            _passwordHasher = hasher;
            _uow = uow;

        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterViewModel regVm)
        {
            if (regVm == null) return BadRequest();

            if (!ModelState.IsValid) return BadRequest(regVm);

            User user = new User
            { 
                UserName = regVm.UserName,
                Email = regVm.Email , 
                PasswordHash = _passwordHasher.Hash(regVm.Password)
            };

            _uow.UserRepository.Add(user);
            _uow.SaveChangesAsync();
            return Ok(User);
            
        }

        [HttpPost("LogIn")]
        public IActionResult LogIn(LoginViewModel LogVm)
        {

        }

        [HttpGet("LogOut")]
        public IActionResult LogOut()
        {

        }

        private async Task Authenticate()
        {

        }
    }
}
