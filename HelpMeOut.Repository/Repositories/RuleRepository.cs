using System.Data;
using HelpMeOut.Repository.DTOs;

namespace HelpMeOut.Repository.Repositories;

public class RuleRepository : Repository<RuleDto>, IRuleRepository
{
    public RuleRepository(IDbConnection connection) : base(connection, "Rules") { }
}