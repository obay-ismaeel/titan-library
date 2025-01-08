using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TitanLibrary.Presistence.Domain.Entities;
using TitanLibrary.Presistence.Abstractions;

namespace TitanLibrary.Presistence;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionStrings:DefaultConnection"]!;
    }

    public IEnumerable<User> GetAllUsers()
    {
        var users = new List<User>();
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("SELECT * FROM Users", connection);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        UserID = (int)reader["UserID"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        MembershipDate = (DateTime)reader["MembershipDate"],
                        PasswordHash = reader["PasswordHash"].ToString()
                    });
                }
            }
        }
        return users;
    }

    public User GetUserById(int userId)
    {
        User user = null;
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("SELECT * FROM Users WHERE UserID = @UserID", connection);
            command.Parameters.AddWithValue("@UserID", userId);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new User
                    {
                        UserID = (int)reader["UserID"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        MembershipDate = (DateTime)reader["MembershipDate"],
                        PasswordHash = reader["PasswordHash"].ToString()
                    };
                }
            }
        }
        return user;
    }

    public User GetUserByEmail(string email)
    {
        User user = null;
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("SELECT * FROM Users WHERE Email = @Email", connection);
            command.Parameters.AddWithValue("@Email", email);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new User
                    {
                        UserID = (int)reader["UserID"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        MembershipDate = (DateTime)reader["MembershipDate"],
                        PasswordHash = reader["PasswordHash"].ToString()
                    };
                }
            }
        }
        return user;
    }

    public bool Register(User user)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO Users (Email, PasswordHash, PhoneNumber, FirstName, LastName, MembershipDate) VALUES (@Email, @PasswordHash, @PhoneNumber, @FirstName, @LastName, GETDATE())";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
