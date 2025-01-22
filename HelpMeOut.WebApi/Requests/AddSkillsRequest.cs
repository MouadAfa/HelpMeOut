using HelpMeOut.Models;

namespace HelpMeOut.WebApi.Requests;

public class AddSkillsRequest
{
    public string Email { get; set; }
    public Skill[] Skills { get; set; }
}