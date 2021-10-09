using Auth.Models;
using Auth.Data.Interfaces;

namespace Auth.Data
{
	public class UserRepository : BaseRepository<User> , IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {

        }
    }
}
