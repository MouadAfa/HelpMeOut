namespace HelpMeOut.Repository.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
    public int AddressId { get; set; }
    public bool IsHelper  { get; set; }
}