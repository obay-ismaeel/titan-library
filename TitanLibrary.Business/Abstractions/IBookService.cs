using TitanLibrary.Presistence.Domain.Entities;

namespace TitanLibrary.Business.Abstractions;

public interface IBookService
{
    IEnumerable<Book> GetAll();

    Book? GetById(int id);

    IEnumerable<Book> Search(string title, string author, string isbn);

    void Borrow(int userId, int bookId);

    void Return(int userId, int bookId);

    IEnumerable<Book> GetAllBorrowed(int userId);
}
