using System.Security.Cryptography;
using System.Text;

namespace Asistencia.Application.Common.Security;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    public bool Verify(string password, string hash)
    {
        var newHash = Hash(password);
        return newHash == hash;
    }
}
