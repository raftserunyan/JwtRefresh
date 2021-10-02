using System.Security.Cryptography;
using System.Text;
using Auth.Services.Interfaces;

namespace Auth.Services
{
	public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {

            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool Compare(string hashedPassword, string password)
        {
            return Hash(password) == hashedPassword;
        }
    }
}
