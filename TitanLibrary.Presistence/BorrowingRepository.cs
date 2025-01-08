using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using TitanLibrary.Presistence.Domain.Entities;
using TitanLibrary.Presistence.Abstractions;
using Microsoft.Extensions.Configuration;

namespace TitanLibrary.Presistence;

public class BorrowingRepository : IBorrowingRepository
{
    private readonly string _connectionString;

    public BorrowingRepository(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionStrings:DefaultConnection"]!;
    }

    public void BorrowBook(int userId, int bookId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand(
                "INSERT INTO Borrowings (BookID, UserID, BorrowDate) VALUES (@BookID, @UserID, @BorrowDate)",
                connection);
            command.Parameters.AddWithValue("@BookID", bookId);
            command.Parameters.AddWithValue("@UserID", userId);
            command.Parameters.AddWithValue("@BorrowDate", DateTime.Now);
            connection.Open();
            command.ExecuteNonQuery();
        }

        UpdateBookAvailability(bookId, false);
    }

    public void ReturnBook(int userId, int bookId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("UPDATE Borrowings SET ReturnDate = @ReturnDate WHERE UserId = @UserId AND BookId = @BookId", connection);
            command.Parameters.AddWithValue("@ReturnDate", DateTime.Now);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@BookId", bookId);
            connection.Open();
            command.ExecuteNonQuery();
        }

        UpdateBookAvailability(bookId, true);
    }

    public IEnumerable<Borrowing> GetBorrowingsByUserId(int userId)
    {
        var borrowings = new List<Borrowing>();
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("SELECT * FROM Borrowings WHERE UserID = @UserID", connection);
            command.Parameters.AddWithValue("@UserID", userId);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    borrowings.Add(new Borrowing
                    {
                        BorrowingID = (int)reader["BorrowingID"],
                        BookID = (int)reader["BookID"],
                        UserID = (int)reader["UserID"],
                        BorrowDate = (DateTime)reader["BorrowDate"],
                        ReturnDate = reader["ReturnDate"] as DateTime?
                    });
                }
            }
        }
        return borrowings;
    }

    public void UpdateBookAvailability(int bookId, bool availability)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("UPDATE Books SET Availability = @Availability WHERE BookID = @BookID", connection);
            command.Parameters.AddWithValue("@Availability", availability);
            command.Parameters.AddWithValue("@BookID", bookId);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
