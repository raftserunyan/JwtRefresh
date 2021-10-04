using Auth.Models;
using System;

namespace Auth.ViewModel
{
    public class UserViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public Role Role { get; set; }
    }
}
