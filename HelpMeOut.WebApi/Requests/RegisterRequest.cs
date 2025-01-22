using HelpMeOut.Models;

namespace HelpMeOut.WebApi.Requests;

public class RegisterRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public DateTime Birthday { get; set; }
    public Address Address { get; set; }
    public string Password { get; set; }
    public bool IsHelper  { get; set; }
}