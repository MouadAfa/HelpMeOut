using HelpMeOut.Repository.DTOs;

namespace HelpMeOut.Repository.Repositories;

public interface IAddressRepository : IRepository<AddressDto>
{
    IEnumerable<AddressDto> GetBy(Func<AddressDto, bool> predicate);
}