using System.Data;
using HelpMeOut.Repository.DTOs;

namespace HelpMeOut.Repository.Repositories;

public class SkillRepository : Repository<SkillDto>, ISkillRepository
{
    public SkillRepository(IDbConnection connection) : base(connection, "Skills") { }
}