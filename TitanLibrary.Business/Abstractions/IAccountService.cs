using TitanLibrary.Presistence.Domain.Entities;
using TitanLibrary.Business.Models;

namespace TitanLibrary.Business.Abstractions;

public interface IAccountService
{
    public bool RegisterUser(RegisterModel model);

    public User? AuthenticateUser(LoginModel model);
}
