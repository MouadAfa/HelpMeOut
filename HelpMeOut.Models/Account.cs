namespace HelpMeOut.Models;

public class Account
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public bool Active { get; set; }
}
