using Auth.Data.Interfaces;
using Auth.Models;

namespace Auth.Data
{
	public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
	{
		public RefreshTokenRepository(AppDbContext context) : base(context)
		{
		}
	}
}
