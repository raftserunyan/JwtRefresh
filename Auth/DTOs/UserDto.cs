using Auth.Models;

namespace Auth.DTOs
{
	public class UserDto
	{
		public string UserName { get; set; }
		public string Email { get; set; }
		public Role Role { get; set; }
		public string AccessToken { get; set; }
		public RefreshToken RefreshToken { get; set; }
	}
}
