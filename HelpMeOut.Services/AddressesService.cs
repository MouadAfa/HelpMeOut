using HelpMeOut.Repository.DTOs;
using HelpMeOut.Repository.Repositories;
using LanguageExt;
using Microsoft.Extensions.Logging;

namespace HelpMeOut.Services;

public interface IAddressesService
{
    Either<Error, AddressDto> GetAddressById(int id);
    Either<Error, AddressDto> Create(AddressDto addressDto);
}

public class AddressesService : IAddressesService
{
    private readonly IAddressRepository _addressRepository;
    private readonly ILogger<AddressesService> _logger;

    public AddressesService(IAddressRepository addressRepository, ILogger<AddressesService> logger)
    {
        _addressRepository = addressRepository;
        _logger = logger;
    }

    public Either<Error, AddressDto> GetAddressById(int id)
    {
        return Tryable<int>.Try(GetAddressByIdInternal, id, ex => _logger.LogError(ex, ex.Message));
    }

    public Either<Error, AddressDto> Create(AddressDto addressDto)
    {
        return Tryable<AddressDto>.Try(CreateInternal, addressDto, ex => _logger.LogError(ex, ex.Message));
    }

    #region Private methods

    private Either<Error, AddressDto> GetAddressByIdInternal(int id)
    {
        return _addressRepository.Get(id);
    }
    
    private Either<Error, AddressDto> CreateInternal(AddressDto addressDto)
    {
        var all = _addressRepository.GetAll();
        var address = _addressRepository.GetBy(a => a.Equals(addressDto)).FirstOrDefault();

        if (address == null)
        {
            _addressRepository.Create(addressDto);
            return Either<Error, AddressDto>.Right(_addressRepository.GetBy(a => a.Equals(addressDto)).First());
        }
        
        return Either<Error, AddressDto>.Right(address);
    }

    #endregion
}