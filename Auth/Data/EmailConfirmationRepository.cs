using Auth.Data.Interfaces;
using Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Data
{
    public class EmailConfirmationRepository : BaseRepository<EmailConfirmation>, IEmailConfirmationRepository
    {
        public EmailConfirmationRepository(AppDbContext context) : base(context)
        {
        }
    }
}
