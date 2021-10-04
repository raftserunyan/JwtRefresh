using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Models
{
	public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }

       
        public int? RoleId { get; set; } //foreign key

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; } //navigation property
    }
}
