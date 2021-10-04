using Auth.Data;
using System.Threading.Tasks;

namespace Auth.Services.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        Task SaveChangesAsync();
    }
}