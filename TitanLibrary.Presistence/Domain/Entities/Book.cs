namespace TitanLibrary.Presistence.Domain.Entities;

public class Book
{
    public int BookID { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public int? PublishedYear { get; set; }
    public string Genre { get; set; }
    public bool Availability { get; set; }
}
