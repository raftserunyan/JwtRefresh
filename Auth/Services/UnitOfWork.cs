using Auth.Data;
using Auth.Data.Interfaces;
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

        private IRefreshTokenRepository _refreshTokenRepo;
        public IRefreshTokenRepository RefreshTokenRepository
        {
            get
            {
                if (_refreshTokenRepo == null)
                    _refreshTokenRepo = new RefreshTokenRepository(_context);
                return _refreshTokenRepo;
            }
        }

        private IEmailConfirmationRepository _emailConfirmRepo;
        public IEmailConfirmationRepository EmailConfirmationRepository
        {
            get
            {
                if (_emailConfirmRepo == null)
                    _emailConfirmRepo = new EmailConfirmationRepository(_context);
                return _emailConfirmRepo;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
