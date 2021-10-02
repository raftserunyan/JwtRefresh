using Auth.Models;

namespace Auth.Data
{
	public class UserRepository : BaseRepository<User> , IUserRepository
    {
        public UserRepository(AppContext context) : base(context)
        {

        }
    }
}
