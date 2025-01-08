using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace TitanLibrary.Presistence;

public class DatabaseSeeder
{
    private readonly string _connectionString;

    public DatabaseSeeder(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void SeedDatabase()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            if (IsTableEmpty(connection, "Books"))
            {
                SeedBooks(connection);
            }

            if (IsTableEmpty(connection, "Users"))
            {
                SeedUsers(connection);
            }
        }
    }

    private bool IsTableEmpty(SqlConnection connection, string tableName)
    {
        var command = new SqlCommand($"SELECT COUNT(*) FROM {tableName}", connection);
        var count = (int)command.ExecuteScalar();
        return count == 0;
    }

    private void SeedBooks(SqlConnection connection)
    {
        var command = new SqlCommand(
        @"INSERT INTO Books (Title, Author, ISBN, PublishedYear, Genre, Availability) VALUES
            ('The Great Gatsby', 'F. Scott Fitzgerald', '9780743273565', 1925, 'Fiction', 1),
            ('1984', 'George Orwell', '9780451524935', 1949, 'Dystopian', 1),
            ('To Kill a Mockingbird', 'Harper Lee', '9780061120084', 1960, 'Fiction', 1),
            ('Pride and Prejudice', 'Jane Austen', '9781503290563', 1813, 'Romance', 1),
            ('Moby-Dick', 'Herman Melville', '9781503280786', 1851, 'Adventure', 1),
            ('The Catcher in the Rye', 'J.D. Salinger', '9780316769488', 1951, 'Fiction', 1),
            ('The Hobbit', 'J.R.R. Tolkien', '9780547928227', 1937, 'Fantasy', 1),
            ('War and Peace', 'Leo Tolstoy', '9780307266934', 1869, 'Historical Fiction', 1),
            ('Crime and Punishment', 'Fyodor Dostoevsky', '9780486415871', 1866, 'Philosophical', 1),
            ('Brave New World', 'Aldous Huxley', '9780060850524', 1932, 'Dystopian', 1),
            ('The Odyssey', 'Homer', '9780140268867', -800, 'Epic', 1),
            ('The Chronicles of Narnia', 'C.S. Lewis', '9780066238500', 1950, 'Fantasy', 1),
            ('The Alchemist', 'Paulo Coelho', '9780062315007', 1988, 'Adventure', 1),
            ('Anna Karenina', 'Leo Tolstoy', '9781853262715', 1877, 'Realist Fiction', 1),
            ('The Picture of Dorian Gray', 'Oscar Wilde', '9780141439570', 1890, 'Gothic Fiction', 1);",
        connection);
        command.ExecuteNonQuery();
    }

    private void SeedUsers(SqlConnection connection)
    {
        var hash = HashPassword("password");

        var command = new SqlCommand(
            @"INSERT INTO Users (FirstName, LastName, Email, PhoneNumber, MembershipDate, PasswordHash) VALUES
                ('John', 'Doe', 'j@e.com', '123-456-7890', GETDATE(), @Hash)," +
                "('Jane', 'Smith', 'janesmith@example.com', '987-654-3210', GETDATE(), @Hash);",
            connection);

        command.Parameters.AddWithValue("@Hash", hash);

        command.ExecuteNonQuery();
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
