using TitanLibrary.Presistence.Domain.Entities;
using TitanLibrary.Presistence.Abstractions;
using TitanLibrary.Business.Abstractions;

namespace TitanLibrary.Business;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IBorrowingRepository _borrowingRepository;

    public BookService(IBookRepository bookRepository, IBorrowingRepository borrowingRepository)
    {
        _bookRepository = bookRepository;
        _borrowingRepository = borrowingRepository;
    }

    public void Borrow(int userId, int bookId)
    {
        _borrowingRepository.BorrowBook(userId, bookId);
    }

    public void Return(int userId, int bookId)
    {
        _borrowingRepository.ReturnBook(userId, bookId);
    }

    public IEnumerable<Book> GetAll()
    {
        return _bookRepository.GetAllBooks();
    }

    public IEnumerable<Book> GetAllBorrowed(int userId)
    {
        return _bookRepository.GetUserBorrowedBooks(userId);
    }

    public Book? GetById(int id)
    {
        return _bookRepository.GetBookById(id);
    }

    public IEnumerable<Book> Search(string title, string author, string isbn)
    {
        return _bookRepository.SearchBooks(title, author, isbn);
    }
}
