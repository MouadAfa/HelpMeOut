using System.Data;
using HelpMeOut.Repository.DTOs;

namespace HelpMeOut.Repository.Repositories;

public class SeekerRepository : Repository<SeekerDto>, ISeekerRepository
{
    public SeekerRepository(IDbConnection connection) : base(connection, "Seekers") { }
}