using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TitanLibrary.Presistence.Domain.Entities;
using TitanLibrary.Presistence.Abstractions;
using Microsoft.Extensions.Logging;

namespace TitanLibrary.Presistence;

public class BookRepository : IBookRepository
{
    private readonly string _connectionString;
    private readonly ILogger

    public BookRepository(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionStrings:DefaultConnection"]!;
    }

    public IEnumerable<Book> GetAllBooks()
    {
        var books = new List<Book>();
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("SELECT BookID, Title, Author, ISBN, PublishedYear, Genre, Availability FROM Books", connection);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        BookID = (int)reader["BookID"],
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        ISBN = reader["ISBN"].ToString(),
                        PublishedYear = reader["PublishedYear"] as int?,
                        Genre = reader["Genre"].ToString(),
                        Availability = (bool)reader["Availability"]
                    });
                }
            }
        }
        return books;
    }

    public Book? GetBookById(int bookId)
    {
        Book book = null;
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("SELECT * FROM Books WHERE BookID = @BookID", connection);
            command.Parameters.AddWithValue("@BookID", bookId);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    book = new Book
                    {
                        BookID = (int)reader["BookID"],
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        ISBN = reader["ISBN"].ToString(),
                        PublishedYear = reader["PublishedYear"] as int?,
                        Genre = reader["Genre"].ToString(),
                        Availability = (bool)reader["Availability"]
                    };
                }
            }
        }
        return book;
    }

    public IEnumerable<Book> GetUserBorrowedBooks(int userId)
    {
        var books = new List<Book>();
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("SELECT Books.BookID, Title, Author, ISBN, PublishedYear, Genre, Availability " +
                "FROM TitanLibrary.dbo.Books INNER JOIN Borrowings on Books.BookID = Borrowings.BookId " +
                "WHERE UserId = @UserId AND ReturnDate is null", connection);
            command.Parameters.AddWithValue("@UserId", userId);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        BookID = (int)reader["BookID"],
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        ISBN = reader["ISBN"].ToString(),
                        PublishedYear = reader["PublishedYear"] as int?,
                        Genre = reader["Genre"].ToString(),
                        Availability = (bool)reader["Availability"]
                    });
                }
            }
        }
        return books;
    }

    public IEnumerable<Book> SearchBooks(string title, string author, string isbn)
    {
        var books = new List<Book>();
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand(
                "SELECT * FROM Books WHERE (@Title IS NULL OR UPPER(Title) LIKE UPPER(@Title)) AND (@Author IS NULL OR UPPER(Author) LIKE UPPER(@Author)) AND (@ISBN IS NULL OR ISBN = @ISBN)",
                connection);
            command.Parameters.AddWithValue("@Title", string.IsNullOrEmpty(title) ? DBNull.Value : $"%{title}%");
            command.Parameters.AddWithValue("@Author", string.IsNullOrEmpty(author) ? DBNull.Value : $"%{author}%");
            command.Parameters.AddWithValue("@ISBN", string.IsNullOrEmpty(isbn) ? DBNull.Value : isbn);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        BookID = (int)reader["BookID"],
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        ISBN = reader["ISBN"].ToString(),
                        PublishedYear = reader["PublishedYear"] as int?,
                        Genre = reader["Genre"].ToString(),
                        Availability = (bool)reader["Availability"]
                    });
                }
            }
        }
        return books;
    }
}