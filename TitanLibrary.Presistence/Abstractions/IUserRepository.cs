using TitanLibrary.Presistence.Domain.Entities;

namespace TitanLibrary.Presistence.Abstractions;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    User GetUserById(int userId);
    User GetUserByEmail(string email);
    bool Register(User user);
}
