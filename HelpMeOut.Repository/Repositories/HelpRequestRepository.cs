using System.Data;
using HelpMeOut.Repository.DTOs;

namespace HelpMeOut.Repository.Repositories;

public class HelpRequestRepository : Repository<HelpRequestDto>, IHelpRequestRepository
{
    public HelpRequestRepository(IDbConnection connection) : base(connection, "HelpRequests") { }
}