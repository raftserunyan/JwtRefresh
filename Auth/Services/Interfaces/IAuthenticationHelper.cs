using Auth.ViewModel;

namespace Auth.Services.Interfaces
{
	public interface IAuthenticationHelper
	{
		void Authenticate(UserViewModel user);
	}
}