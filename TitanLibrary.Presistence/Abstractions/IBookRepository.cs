using TitanLibrary.Presistence.Domain.Entities;

namespace TitanLibrary.Presistence.Abstractions;

public interface IBookRepository
{
    IEnumerable<Book> GetAllBooks();
    IEnumerable<Book> GetUserBorrowedBooks(int userId);
    Book? GetBookById(int bookId);
    IEnumerable<Book> SearchBooks(string title, string author, string isbn);
}
