using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Models;
using Auth.Services.Interfaces;
using Auth.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
		private readonly ITokenGenerator _tokenGenerator;
		private readonly IMapper _mapper;

		public AccountController(IPasswordHasher hasher,
								IUnitOfWork uow,
								IAuthenticationHelper authService,
								ITokenGenerator tokenGenerator,
								IMapper mapper)
		{
			_passwordHasher = hasher;
			_uow = uow;
			_authService = authService;
			_mapper = mapper;
			_tokenGenerator = tokenGenerator;
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

			// mapps username and email
			user = _mapper.Map<User>(regVm);
			user.PasswordHash = _passwordHasher.Hash(regVm.Password);
			//how does a user get its Role?
			user.RoleId = (await _uow.RoleRepository.GetAsync(r => r.Name == "User")).Id;

			await _uow.UserRepository.AddAsync(user);

			var userDto = _authService.Authenticate(user);

			user.RefreshToken = userDto.RefreshToken;

			await _uow.SaveChangesAsync();

			var userVm = _mapper.Map<UserViewModel>(userDto);

			return Ok(userVm);
		}

		[HttpPost("LogIn")]
		public async Task<IActionResult> LogIn(LoginViewModel logVm)
		{
			if (logVm == null) return BadRequest("Invalid credentials");

			if (!ModelState.IsValid) return BadRequest(logVm);

			var hashedPwd = _passwordHasher.Hash(logVm.Password);

			var user = await _uow.UserRepository.GetAsync(x => x.UserName == logVm.UserName && x.PasswordHash == hashedPwd, u => u.Role, u => u.RefreshToken);

			if (user == null)
			{
				ModelState.AddModelError("", "Invalid credentials");
				return BadRequest(logVm);
			}

			var userDto = _authService.Authenticate(user);

			if (user.RefreshToken != null)
			{
				user.RefreshToken.Token = userDto.RefreshToken.Token;
				user.RefreshToken.ExpiryDate = userDto.RefreshToken.ExpiryDate;
				await _uow.RefreshTokenRepository.UpdateAsync(user.RefreshToken);
			}
			else
			{
				user.RefreshToken = userDto.RefreshToken;
				await _uow.UserRepository.UpdateAsync(user);
			}

			await _uow.SaveChangesAsync();

			var userVm = _mapper.Map<UserViewModel>(userDto);

			return Ok(userVm);
		}

		[HttpPost("RefreshAccessToken")]
		public async Task<IActionResult> RefreshAccessToken(TokensToRefreshVM tokensVm)
		{
			if (tokensVm == null)
			{
				return BadRequest("Invalid client request");
			}

			string accessToken = tokensVm.AccessToken;
			string refreshToken = tokensVm.RefreshToken;

			ClaimsPrincipal principal;
			try
			{
				principal = _authService.GetPrincipalFromExpiredToken(accessToken);
			}
			catch (ArgumentException e)
			{
				return BadRequest("Invalid access token");
			}

			var username = principal.Identity.Name; //this is mapped to the Name claim by default

			var user = await _uow.UserRepository.GetAsync(u => u.UserName == username, u => u.Role, u => u.RefreshToken);
			if (user == null
				|| user.RefreshToken?.Token != refreshToken 
				|| user.RefreshToken?.ExpiryDate <= DateTime.Now)
			{
				return BadRequest("Invalid client request");
			}

			var newAccessToken = _tokenGenerator.GenerateAccessToken(user);
			var newRefreshToken = _tokenGenerator.GenerateRefreshToken();

			user.RefreshToken.Token = newRefreshToken.Token;
			user.RefreshToken.ExpiryDate = newRefreshToken.ExpiryDate;
			await _uow.RefreshTokenRepository.UpdateAsync(user.RefreshToken);
			await _uow.SaveChangesAsync();

			return new ObjectResult(new
			{
				accessToken = newAccessToken,
				refreshToken = newRefreshToken.Token
			});
		}

		[Authorize]
		[HttpPost("LogOut")]
		public async Task<IActionResult> LogOut()
		{
			var username = User.Identity.Name;

			var user = await _uow.UserRepository.GetAsync(u => u.UserName == username, u => u.RefreshToken);

			if (user == null) 
				return BadRequest();

			_uow.RefreshTokenRepository.Remove(user.RefreshToken);
			await _uow.SaveChangesAsync();

			return NoContent();
		}
	}
}
