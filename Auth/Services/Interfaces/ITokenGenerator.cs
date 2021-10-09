using Auth.Models;

namespace Auth.Services.Interfaces
{
	public interface ITokenGenerator
	{
		string GenerateAccessToken(User user);

		RefreshToken GenerateRefreshToken();
	}
}
