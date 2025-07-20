namespace Ticketing.Auth.Domain.Interfaces;
public interface IPasswordHasher
{
  bool Verify(string password, string storedHash);
  string Hash(string password);
}
