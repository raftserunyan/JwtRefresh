using Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Data
{
    public interface IUserRepository : IBaseRepository<User>
    {
    }
}
