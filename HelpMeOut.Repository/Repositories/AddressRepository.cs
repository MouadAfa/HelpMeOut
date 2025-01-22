using System.Data;
using HelpMeOut.Repository.DTOs;

namespace HelpMeOut.Repository.Repositories;

public class AddressRepository : Repository<AddressDto>, IAddressRepository
{
    public AddressRepository(IDbConnection connection) : base(connection, "Addresses") { }
    
    public IEnumerable<AddressDto> GetBy(Func<AddressDto, bool> predicate)
    {
        return GetAll().Where(predicate);
    }
}