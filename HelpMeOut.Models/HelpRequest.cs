namespace HelpMeOut.Models;

public class HelpRequest
{
    public required string SeekerEmail { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public RequestType Type { get; set; }
    public List<Rule> Rules { get; set; } = [];
}