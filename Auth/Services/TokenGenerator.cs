using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth.Models;
using Auth.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Services
{
	public class TokenGenerator : ITokenGenerator
	{
		private readonly JwtSettings _jwtsettings;

		public TokenGenerator(IOptions<JwtSettings> jwtsettings)
		{
			_jwtsettings = jwtsettings.Value;
		}

		public string GenerateAccessToken(User user)
		{
			//creates the token by combining the parts
			var tokenHandler = new JwtSecurityTokenHandler();

			//secret key of the server
			var key = Encoding.UTF8.GetBytes(_jwtsettings.Key);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.UserName),
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(ClaimTypes.Role, user.Role.Name)
				}),
				Expires = DateTime.UtcNow.AddDays(2),
				IssuedAt = DateTime.UtcNow,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
		public RefreshToken GenerateRefreshToken()
		{
			var refreshToken = new RefreshToken();

			var randomNumber = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
				refreshToken.Token = Convert.ToBase64String(randomNumber);
			}
			refreshToken.ExpiryDate = DateTime.UtcNow.AddDays(30);

			return refreshToken;
		}
	}
}
