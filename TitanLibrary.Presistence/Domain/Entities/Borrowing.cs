namespace TitanLibrary.Presistence.Domain.Entities;

public class Borrowing
{
    public int BorrowingID { get; set; }
    public int BookID { get; set; }
    public int UserID { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}
