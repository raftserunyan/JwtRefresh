using System;

namespace Auth.ViewModel
{
    public class UserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
		public string RefreshToken { get; set; }
		public RoleViewModel Role { get; set; }
    }
}
