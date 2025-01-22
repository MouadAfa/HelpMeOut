using HelpMeOut.Repository.DTOs;

namespace HelpMeOut.Repository.Repositories;

public interface IUserRepository : IRepository<UserDto>
{
    IEnumerable<UserDto> GetBy(Func<UserDto, bool> predicate);
}