using Auth.Data;
using Auth.Services.Interfaces;
using System.Threading.Tasks;

namespace Auth.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppContext _context;

        public UnitOfWork(AppContext dbcontext)
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

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
