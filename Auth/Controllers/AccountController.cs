using System.Threading.Tasks;
using Auth.Models;
using Auth.Services.Interfaces;
using Auth.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IPasswordHasher _passwordHasher;
		private readonly IUnitOfWork _uow;
		private readonly IAuthenticationHelper _authService;

		public AccountController(IPasswordHasher hasher,
								IUnitOfWork uow,
								IAuthenticationHelper authService)
		{
			_passwordHasher = hasher;
			_uow = uow;
			_authService = authService;
		}


		[HttpPost("Register")]
		public async Task<IActionResult> Register(RegisterViewModel regVm)
		{
			if (regVm == null) return BadRequest();

			if (!ModelState.IsValid) return BadRequest(regVm);

			User user = await _uow.UserRepository.GetAsync(u => u.UserName == regVm.UserName);

			if (user != null)
			{
				ModelState.AddModelError("", "User already exists");
				return BadRequest(regVm);
			}

			user = new User
			{
				UserName = regVm.UserName,
				Email = regVm.Email,
				PasswordHash = _passwordHasher.Hash(regVm.Password)
			};

			await _uow.UserRepository.AddAsync(user);
			await _uow.SaveChangesAsync();

			var userVm = new UserViewModel
			{
				Username = user.UserName,
				Email = user.Email
			};

			_authService.Authenticate(userVm);

			return Ok(userVm);
		}

		[HttpPost("LogIn")]
		public async Task<IActionResult> LogIn(LoginViewModel logVm)
		{
			if (logVm == null) return BadRequest("Invalid credentials");

			if (!ModelState.IsValid) return BadRequest(logVm);

			var hashedPwd = _passwordHasher.Hash(logVm.Password);

			var user = await _uow.UserRepository.GetAsync(x => x.UserName == logVm.UserName && x.PasswordHash == hashedPwd);

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

			_authService.Authenticate(userVm);

			return Ok(userVm);
		}

		[HttpGet("LogOut")]
		public IActionResult LogOut()
		{
			return Ok();
		}
	}
}
