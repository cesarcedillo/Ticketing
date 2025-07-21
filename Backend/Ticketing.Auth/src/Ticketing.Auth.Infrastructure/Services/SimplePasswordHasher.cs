using Ticketing.Auth.Domain.Interfaces;

namespace Ticketing.Auth.Infrastructure.Services;
public class SimplePasswordHasher : IPasswordHasher
{
  public string Hash(string password)
  {
    if (string.IsNullOrWhiteSpace(password))
      return password;
    using var sha = System.Security.Cryptography.SHA256.Create();
    var bytes = System.Text.Encoding.UTF8.GetBytes(password);
    var hash = sha.ComputeHash(bytes);
    return Convert.ToBase64String(hash);
  }

  public bool Verify(string password, string passwordHash)
      => Hash(password) == passwordHash;
}
