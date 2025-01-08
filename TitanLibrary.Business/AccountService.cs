using System;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using TitanLibrary.Business.Models;
using TitanLibrary.Presistence.Domain.Entities;
using TitanLibrary.Presistence.Abstractions;
using TitanLibrary.Business.Abstractions;

namespace TitanLibrary.Business;

public class AccountService : IAccountService
{
    private readonly IUserRepository _userRepository;

    public AccountService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public bool RegisterUser(RegisterModel model)
    {
        if (model.Password != model.ConfirmPassword)
            throw new ArgumentException("Passwords do not match.");

        string hashedPassword = HashPassword(model.Password);

        var user = new User
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            MembershipDate = DateTime.Now,
            PasswordHash = hashedPassword,
        };

        return _userRepository.Register(user);
    }

    public User? AuthenticateUser(LoginModel model)
    {
        var user = _userRepository.GetUserByEmail(model.Email);

        if (user is not null && VerifyPassword(model.Password, user.PasswordHash))
            return user;

        return null;
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }

    private bool VerifyPassword(string inputPassword, string storedHash)
    {
        string hashedInput = HashPassword(inputPassword);
        return hashedInput == storedHash;
    }
}
