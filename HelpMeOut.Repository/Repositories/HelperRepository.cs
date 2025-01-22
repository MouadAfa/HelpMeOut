using System.Data;
using HelpMeOut.Repository.DTOs;

namespace HelpMeOut.Repository.Repositories;

public class HelperRepository : Repository<HelperDto>, IHelperRepository
{
    public HelperRepository(IDbConnection connection) : base(connection, "Helpers") { }
}