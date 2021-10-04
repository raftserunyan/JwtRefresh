using Auth.Data;
using Auth.Services.Interfaces;
using System.Threading.Tasks;

namespace Auth.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext dbcontext)
        {
            _context = dbcontext;
        }

        private IUserRepository _userRepo;
        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepo == null)
                    _userRepo = new UserRepository(_context);
                return _userRepo;
            }
        }

        private IRoleRepository _roleRepo;
        public IRoleRepository RoleRepository 
        {
            get
            {
                if (_roleRepo == null)
                    _roleRepo = new RoleRepository(_context);
                return _roleRepo;
            } 
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
