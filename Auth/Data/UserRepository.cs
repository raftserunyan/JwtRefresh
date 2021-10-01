using Auth.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Data
{
    public class UserRepository : BaseRepository<User> , IUserRepository
    {
        public UserRepository(AppContext context) : base(context)
        {

        }
    }
}
