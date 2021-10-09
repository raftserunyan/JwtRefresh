using Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Data.Interfaces;

namespace Auth.Data
{
    public class RoleRepository : BaseRepository<Role> , IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {

        }
    }
}
