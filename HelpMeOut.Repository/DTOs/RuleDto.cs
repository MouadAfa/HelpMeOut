namespace HelpMeOut.Repository.DTOs;

public class RuleDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public int Type { get; set; } // Maps to RuleType enum
}