namespace Application.Interfaces.Repositories.UserInterfaces;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}