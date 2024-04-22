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

    public async Task DeleteExpiredSessionsAsync(CancellationToken cancellationToken = default)
    {
        var expired = await _context.LoginSessions
            .Where(x => x.ExpiresDateTime < DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        _context.LoginSessions.RemoveRange(expired);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserLoginSession> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var session = await _context.LoginSessions.FirstAsync(x => x.Id == id.ToString(), cancellationToken);
        return session.ToLoginSession();
    }

    public async Task<UserLoginSession> UpdateAsync(UserLoginSession entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id == Guid.Empty)
            throw new ArgumentException($"Cannot update a sessions when no Id provided!");

        var exisiting = await _context.LoginSessions.FirstAsync(x => x.Id == entity.Id.ToString(), cancellationToken);
        
        exisiting.RefreshToken = entity.RefreshToken;
        exisiting.ExpiresDateTime = entity.ExpiresDateTime;
        exisiting.AccessToken = entity.AccessToken;

        var entry = _context.LoginSessions.Update(exisiting);
        await _context.SaveChangesAsync(cancellationToken);

        return entry.Entity.ToLoginSession();
    }

    public async Task<UserLoginSession> GetSessionByUserIdAsync(string userId, string refreshToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(refreshToken))
            throw new ArgumentException($"Cannot get a user login session when the userID and token are not provided!");

        var entity = await _context.LoginSessions
            .FirstAsync(x => x.UserAccountId == userId && x.RefreshToken == refreshToken, cancellationToken);
        return entity.ToLoginSession();
    }
}