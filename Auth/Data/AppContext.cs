using Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data
{
	public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
