using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Models
{
    public class EmailConfirmation
    {
        public Guid Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
