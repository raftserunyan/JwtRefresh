using System;

namespace Auth.Services.Interfaces
{
    public interface IPasswordHasher
    {
        bool Compare(string hashedPassword, string password);
        string Hash(string password);
    }
}