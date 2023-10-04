using System.Security.Cryptography;
using System.Text;

namespace OTP.Domain;

public sealed class Sha256HashService : IHashService
{
    public Hash Hash(string data)
    {
        using (var sha256Hash = SHA256.Create())
        {
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));

            var builder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return new Hash(builder.ToString());
        }
    }
}