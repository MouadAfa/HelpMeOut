using System.Data;
using HelpMeOut.Repository.DTOs;

namespace HelpMeOut.Repository.Repositories;

public class UserRepository : Repository<UserDto>, IUserRepository
{
    public UserRepository(IDbConnection connection) : base(connection, "Users") { }
    
    public IEnumerable<UserDto> GetBy(Func<UserDto, bool> predicate)
    {
        var allUsers = base.GetAll().ToList();
        return allUsers.Where(predicate);
    }
}