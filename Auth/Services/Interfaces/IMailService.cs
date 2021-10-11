using Auth.Models;
using System;
using System.Threading.Tasks;

namespace Auth.Services.Interfaces
{
    public interface IMailService
    {
        public void SendConfirmationEmailAsync(User user, Guid id, string domain);
    }
}
