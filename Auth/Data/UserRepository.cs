using Auth.Models;

namespace Auth.Data
{
	public class UserRepository : BaseRepository<User> , IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {

        }
    }
}
