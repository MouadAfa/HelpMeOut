using System.Data;
using HelpMeOut.Repository.DTOs;

namespace HelpMeOut.Repository.Repositories;

public class AccountRepository : Repository<AccountDto>, IAccountRepository
{
    public AccountRepository(IDbConnection connection) : base(connection, "Accounts") { }
}