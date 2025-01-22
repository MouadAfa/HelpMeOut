namespace HelpMeOut.Models;

public abstract class AUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public DateTime Birthday { get; set; }
    public required Address Address { get; set; }
}
