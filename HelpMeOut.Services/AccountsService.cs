using System.Security.Authentication;
using HelpMeOut.Repository.DTOs;
using HelpMeOut.Repository.Repositories;
using LanguageExt;
using Microsoft.Extensions.Logging;

namespace HelpMeOut.Services;

public interface IAccountsService
{
    Either<Error, AccountDto> Create(AccountDto accountDto);
    Either<Error, AccountDto> GetAccountByEmail(string email);
    Either<Error, AccountDto> Update(AccountDto accountDto);
}

public class AccountsService : IAccountsService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<AccountsService> _logger;

    public AccountsService(IAccountRepository accountRepository, ILogger<AccountsService> logger)
    {
        _accountRepository = accountRepository;
        _logger = logger;
    }

    public Either<Error, AccountDto> Create(AccountDto accountDto)
    {
        var exists = _accountRepository.GetAll().Any(a => a.Email == accountDto.Email);
        if (exists)
        {
            var msg = $"Account with email {accountDto.Email} already exists.";
            _logger.LogError(msg);
            return Either<Error, AccountDto>.Left(new Error
                { Message = msg, Exception = new InvalidCredentialException(msg) });
        }
        
        _accountRepository.Create(accountDto);
        
        var account = _accountRepository.GetAll().SingleOrDefault(u => u.Email == accountDto.Email);

        if (account == null)
        {
            var msg = $"Account with email {accountDto.Email} could not be added.";
            _logger.LogError(msg);
            return Either<Error, AccountDto>.Left(new Error
                { Message = msg, Exception = new InvalidCredentialException(msg) });
        }
        
        return Either<Error, AccountDto>.Right(account);
    }

    public Either<Error, AccountDto> GetAccountByEmail(string email)
    {
        var accountDto = _accountRepository.GetAll().SingleOrDefault(a => a.Email == email);
        if (accountDto == null)
        {
            var msg = $"Account with email {email} could not be found or multiple accounts exist.";
            _logger.LogError(msg);
            return Either<Error, AccountDto>.Left(new Error
                { Message = msg, Exception = new InvalidCredentialException(msg) });
        }
        
        return Either<Error, AccountDto>.Right(accountDto);
    }

    public Either<Error, AccountDto> Update(AccountDto accountDto)
    {
        var either = from acc in GetAccountByEmail(accountDto.Email)
            let accDto = new AccountDto
            {
                Id = acc.Id,
                Email = acc.Email,
                Password = acc.Password,
                Active = true
            }
            select accDto;

        var right = (AccountDto a) =>
        {
            _accountRepository.Update(a);
            return Either<Error, AccountDto>.Right(a);
        };

        var left = (Error e) =>
        {
            _logger.LogError(e.Message, e.Exception);
            return Either<Error, AccountDto>.Left(e);
        };

        return either.Match(right, left);
    }
}