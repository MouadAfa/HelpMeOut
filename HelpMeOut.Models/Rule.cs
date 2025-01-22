namespace HelpMeOut.Models;

public class Rule
{
    public required string Name { get; set; } // e.g., "Distance", "Experience", "Skill"
    public required string Value { get; set; } // e.g., "10km", "5 years", "Javascript"
    public RuleType Type { get; set; }

    public enum RuleType
    {
        Distance,
        Experience,
        Skill,
        Location,
        Other
    }
}