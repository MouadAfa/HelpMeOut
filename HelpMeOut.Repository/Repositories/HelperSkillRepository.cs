using System.Data;
using HelpMeOut.Repository.DTOs;

namespace HelpMeOut.Repository.Repositories;

public class HelperSkillRepository : Repository<HelperSkillDto>, IHelperSkillRepository
{
    public HelperSkillRepository(IDbConnection connection) : base(connection, "HelperSkills") { }
}