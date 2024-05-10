using LessonTool.API.Authentication.Models;
using LessonTool.API.Infrastructure.EntityFramework;
using LessonTool.API.Infrastructure.Extensions;
using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.Common.Domain.Exceptions;
using LessonTool.Common.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LessonTool.API.Infrastructure.Repositories;

public class EFCosmosUserAccountRepository(CosmosDbContext _context) : IUserAccountRepository
{
    public async Task<UserAccount> GetAccountByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var account = await _context.UserAccounts.FirstOrDefaultAsync(x => x.Username == username);
        return account?.ToUserAccount();
    }

    public async Task<UserAccount> CreateAsync(UserAccount entity, CancellationToken cancellationToken = default)
    {
        var account = entity.ToCosmosUserAccount();
        account.Id = Guid.NewGuid().ToString();

        var entry = await _context.UserAccounts.AddAsync(account, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entry?.Entity?.ToUserAccount();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var account = await _context.UserAccounts.FirstOrDefaultAsync(x => x.Id == id.ToString(), cancellationToken);

        if (account is not null)
        {
            _context.UserAccounts.Remove(account);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<UserAccount> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var account = await _context.UserAccounts.FirstOrDefaultAsync(x => x.Id == id.ToString());
        return account?.ToUserAccount();
    }

    public async Task<UserAccount> UpdateAsync(UserAccount entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id == Guid.Empty)
            throw new DataAccessException($"Cannot update a user account when no Id provided!");

        var entry = _context.UserAccounts.Update(entity.ToCosmosUserAccount());
        await _context.SaveChangesAsync(cancellationToken);

        return entry?.Entity?.ToUserAccount();
    }
}
