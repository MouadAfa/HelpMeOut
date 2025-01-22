namespace HelpMeOut.Repository.DTOs;

public class HelpRequestDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Type { get; set; } // Maps to RequestType enum
}