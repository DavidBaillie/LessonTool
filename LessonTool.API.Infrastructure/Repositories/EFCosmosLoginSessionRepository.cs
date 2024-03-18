using LessonTool.API.Authentication.Models;
using LessonTool.API.Domain.Interfaces;
using LessonTool.API.Infrastructure.EntityFramework;
using LessonTool.API.Infrastructure.Extensions;
using LessonTool.Common.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LessonTool.API.Infrastructure.Repositories;

public class EFCosmosLoginSessionRepository(CosmosDbContext _context) : ILoginSessionRepository
{
    public async Task<UserLoginSession> CreateAsync(UserLoginSession entity, CancellationToken cancellationToken = default)
    {
        var session = entity.ToCosmosLoginSession();
        session.Id = Guid.NewGuid().ToString();

        var entry = await _context.LoginSessions.AddAsync(session, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entry.Entity.ToLoginSession();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var session = await _context.LoginSessions.FirstOrDefaultAsync(x => x.Id == id.ToString(), cancellationToken);

        if (session is not null)
        {
            _context.LoginSessions.Remove(session);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public Task DeleteExpiredSessions()
    {
        throw new NotImplementedException();
    }

    public async Task<UserLoginSession> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var session = await _context.LoginSessions.FirstOrDefaultAsync(x => x.Id == id.ToString(), cancellationToken);
        return session.ToLoginSession();
    }

    public async Task<UserLoginSession> UpdateAsync(UserLoginSession entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id == Guid.Empty)
            throw new DataAccessException($"Cannot update a sessions when no Id provided!");

        var entry = _context.LoginSessions.Update(entity.ToCosmosLoginSession());
        await _context.SaveChangesAsync(cancellationToken);

        return entry.Entity.ToLoginSession();
    }
}
