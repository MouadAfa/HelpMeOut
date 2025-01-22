using System.Data;
using System.Security.Authentication;
using HelpMeOut.Repository.DTOs;
using HelpMeOut.Repository.Repositories;
using LanguageExt;
using Microsoft.Extensions.Logging;

namespace HelpMeOut.Services;

public interface IUsersService
{
    Either<Error, UserDto> CreateUser(UserDto userDto);
    Either<Error, UserDto> UpdateUser(UserDto userDto);
    Either<Error, UserDto> Login(string email, string password);
    Either<Error, UserDto> Get(string email);
}

public class UsersService : IUsersService
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<UsersService> _logger;
    
    public UsersService(
        IUserRepository userRepository,
        IAccountRepository accountRepository,
        ILogger<UsersService> logger)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _logger = logger;
    }

    public Either<Error, UserDto> CreateUser(UserDto userDto)
    {
        return Tryable<UserDto>.Try(CreateUserInternal, userDto, ex => _logger.LogError(ex, ex.Message));
    }
    
    public Either<Error, UserDto> UpdateUser(UserDto userDto)
    {
        return Tryable<UserDto>.Try(UpdateUserInternal, userDto, ex => _logger.LogError(ex, ex.Message));
    }

    public Either<Error, UserDto> Login(string email, string password)
    {
        var accountDto = new AccountDto { Email = email, Password = password };
        return Tryable<AccountDto>.Try(LoginInternal, accountDto, ex => _logger.LogError(ex, ex.Message));
    }

    public Either<Error, UserDto> Get(string email)
    {
        var user = _userRepository.GetBy(u => u.Email == email).SingleOrDefault();
        if (user == null)
        {
            var msg = $"User with email {email} could not be found.";
            _logger.LogError(msg);
            return Either<Error, UserDto>.Left(new Error
                { Message = msg, Exception = new InvalidCredentialException(msg) });
        }
        
        return Either<Error, UserDto>.Right(user);
    }

    #region Private methods

    private Either<Error, UserDto> CreateUserInternal(UserDto userDto)
    {
        var exists = _userRepository.GetBy(u => u.Email == userDto.Email).Any();
        if (exists)
        {
            var msg = $"User with email {userDto.Email} already exists.";
            _logger.LogError(msg);
            return Either<Error, UserDto>.Left(new Error
                { Message = msg, Exception = new InvalidCredentialException(msg) });
        }
        
        _userRepository.Create(userDto);
        
        var user = _userRepository.GetBy(u => u.Email == userDto.Email).SingleOrDefault();

        if (user == null)
        {
            var msg = $"User with email {userDto.Email} could not be added.";
            _logger.LogError(msg);
            return Either<Error, UserDto>.Left(new Error
                { Message = msg, Exception = new InvalidCredentialException(msg) });
        }
        
        return Either<Error, UserDto>.Right(user);
    }
    
    private Either<Error, UserDto> UpdateUserInternal(UserDto userDto)
    {
        var user = _userRepository.GetBy(u => u.Email == userDto.Email).SingleOrDefault();
        if (user == null)
        {
            var msg = $"User with email {userDto.Email} could not be found.";
            _logger.LogError(msg);
            return Either<Error, UserDto>.Left(new Error
                { Message = msg, Exception = new InvalidCredentialException(msg) });
        }
        
        userDto.Id = user.Id;
        _userRepository.Update(userDto);
        
        var newUser = _userRepository.GetBy(u => u.Email == userDto.Email).SingleOrDefault();

        if (newUser == null)
        {
            var msg = $"User with email {userDto.Email} could not be updated.";
            _logger.LogError(msg);
            return Either<Error, UserDto>.Left(new Error
                { Message = msg, Exception = new InvalidOperationException(msg) });
        }
        
        return Either<Error, UserDto>.Right(newUser);
    }

    private Either<Error, UserDto> LoginInternal(AccountDto accountDto)
    {
        var account = _accountRepository.GetAll().SingleOrDefault(u => u.Email == accountDto.Email && u.Password == accountDto.Password);
        if (account == null)
        {
            var msg = $"Email or Password is incorrect.";
            _logger.LogError(msg);
            return Either<Error, UserDto>.Left(new Error
                { Message = msg, Exception = new InvalidCredentialException(msg) });
        }

        if (!account.Active)
        {
            var msg = $"This account is not active. Please activate your account.";
            _logger.LogError(msg);
            return Either<Error, UserDto>.Left(new Error
                { Message = msg, Exception = new InvalidConstraintException(msg) });
        }
        
        var user = _userRepository.GetBy(u => u.Email == accountDto.Email).SingleOrDefault();

        if (user == null)
        {
            var msg = $"The Email {accountDto.Email} is not attached to an user.";
            _logger.LogError(msg);
            return Either<Error, UserDto>.Left(new Error
                { Message = msg, Exception = new InvalidCredentialException(msg) });
        }

        _logger.LogInformation("Logged in successfully.");
        
        return Either<Error, UserDto>.Right(user);
    }
    
    #endregion
}