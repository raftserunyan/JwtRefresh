using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.DTOs;
using Auth.Models;
using Auth.Services.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Services
{
	public class AuthenticationHelper : IAuthenticationHelper
	{
		private readonly ITokenGenerator _tokenGenerator;
		private readonly IMapper _mapper;
		private readonly JwtSettings _jwtsettings;

		public AuthenticationHelper(ITokenGenerator tokenGenerator, 
									IMapper mapper, 
									IOptions<JwtSettings> jwtsettings)
		{
			_tokenGenerator = tokenGenerator;
			_mapper = mapper;
			_jwtsettings = jwtsettings.Value;
		}

		public UserDto Authenticate(User user)
		{
			var userDto = _mapper.Map<UserDto>(user);
			userDto.AccessToken = _tokenGenerator.GenerateAccessToken(user);
			userDto.RefreshToken = _tokenGenerator.GenerateRefreshToken();

			return userDto;
		}

		//checks if the jwt is valid and returns a principal
		public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
				ValidateIssuer = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtsettings.Key)),
				ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken securityToken;

			ClaimsPrincipal principal;
			try
			{
				principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
			}
			catch (ArgumentException e)
			{
				throw;
			}

			var jwtSecurityToken = securityToken as JwtSecurityToken;
			if (jwtSecurityToken == null
				|| !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
			{
				throw new SecurityTokenException("Invalid token");
			}

			return principal;
		}
	}
}
