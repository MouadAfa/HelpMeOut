namespace HelpMeOut.Repository.DTOs;

public class AccountDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool Active { get; set; }
}