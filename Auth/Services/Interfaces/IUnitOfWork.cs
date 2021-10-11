using Auth.Data.Interfaces;
using System.Threading.Tasks;

namespace Auth.Services.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IEmailConfirmationRepository EmailConfirmationRepository { get; }

        Task SaveChangesAsync();
    }
}