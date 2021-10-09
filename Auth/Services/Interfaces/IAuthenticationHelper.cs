using System.Security.Claims;
using Auth.DTOs;
using Auth.Models;

namespace Auth.Services.Interfaces
{
	public interface IAuthenticationHelper
	{
		UserDto Authenticate(User user);
		public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
	}
}