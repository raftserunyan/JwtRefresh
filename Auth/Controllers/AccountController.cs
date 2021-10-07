using System.Threading.Tasks;
using Auth.Models;
using Auth.Services.Interfaces;
using Auth.ViewModel;
using AutoMapper;
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
		private readonly IMapper _mapper;

		public AccountController(IPasswordHasher hasher,
								IUnitOfWork uow,
								IAuthenticationHelper authService,
								IMapper mapper)
		{
			_passwordHasher = hasher;
			_uow = uow;
			_authService = authService;
			_mapper = mapper;
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

			user = _mapper.Map<User>(regVm);
			user.PasswordHash = _passwordHasher.Hash(regVm.Password);
			user.RoleId = (await _uow.RoleRepository.GetAsync(r => r.Name == "User")).Id;

			await _uow.UserRepository.AddAsync(user);
			await _uow.SaveChangesAsync();

			await _uow.UserRepository.LoadReferenceAsync(user, u => u.Role);

			var userVm = _mapper.Map<UserViewModel>(user);

			_authService.Authenticate(userVm);

			return Ok(userVm);
		}

		[HttpPost("LogIn")]
		public async Task<IActionResult> LogIn(LoginViewModel logVm)
		{
			if (logVm == null) return BadRequest("Invalid credentials");

			if (!ModelState.IsValid) return BadRequest(logVm);

			var hashedPwd = _passwordHasher.Hash(logVm.Password);

			var user = await _uow.UserRepository.GetAsync(x => x.UserName == logVm.UserName && x.PasswordHash == hashedPwd, u => u.Role);

			if (user == null)
			{
				ModelState.AddModelError("", "Invalid credentials");
				return BadRequest(logVm);
			}

			var userVm = _mapper.Map<UserViewModel>(user);

			_authService.Authenticate(userVm);

			return Ok(userVm);
		}
	}
}
