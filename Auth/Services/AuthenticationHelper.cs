using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Models;
using Auth.ViewModel;
using Auth.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Services
{
	public class AuthenticationHelper : IAuthenticationHelper
	{
		private readonly JwtSettings _jwtsettings;

		public AuthenticationHelper(IOptions<JwtSettings> jwtsettings)
		{
			_jwtsettings = jwtsettings.Value;
		}

		public void Authenticate(UserViewModel user)
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
