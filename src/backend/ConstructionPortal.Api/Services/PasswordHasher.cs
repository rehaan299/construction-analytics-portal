using System.Security.Cryptography;
using System.Text;

namespace ConstructionPortal.Api.Services;

public static class PasswordHasher
{
    // Prototype-only: do NOT use this in production.
    public static string Hash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }
}
