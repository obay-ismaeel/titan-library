using TitanLibrary.Presistence.Domain.Entities;

namespace TitanLibrary.Presistence.Abstractions;

public interface IBorrowingRepository
{
    void BorrowBook(int userId, int bookId);
    void ReturnBook(int userId, int bookId);
    IEnumerable<Borrowing> GetBorrowingsByUserId(int userId);
    public void UpdateBookAvailability(int bookId, bool availability);
}
