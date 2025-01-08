namespace TitanLibrary.Presistence.Domain.Entities;

public class User
{
    public int UserID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime MembershipDate { get; set; }
    public string PasswordHash { get; set; }
}
