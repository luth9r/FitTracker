using AutoMapper;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Infrastructure.Persistence.Repositories;

internal sealed class UserReadRepository(
    FitTrackerDbContext context,
    IMapper mapper) : IUserReadRepository
{
    private static readonly Func<FitTrackerDbContext, Guid, IAsyncEnumerable<UserEf>> GetUserEfByIdCompiled =
        EF.CompileAsyncQuery((FitTrackerDbContext dbContext, Guid userId) =>
            dbContext.Users
                .AsNoTracking()
                .Where(u => u.Id == userId));

    private static readonly Func<FitTrackerDbContext, string, IAsyncEnumerable<UserEf>> GetUserEfByUsernameCompiled =
        EF.CompileAsyncQuery((FitTrackerDbContext dbContext, string username) =>
            dbContext.Users
                .AsNoTracking()
                .Where(u => u.Username == username));

    private static readonly Func<FitTrackerDbContext, string, IAsyncEnumerable<UserEf>> GetUserEfByEmailCompiled =
        EF.CompileAsyncQuery((FitTrackerDbContext dbContext, string email) =>
            dbContext.Users
                .AsNoTracking()
                .Where(u => u.Email == email));

    private static readonly Func<FitTrackerDbContext, string, IAsyncEnumerable<UserEf>> GetUserEfByGoogleTokenCompiled =
        EF.CompileAsyncQuery((FitTrackerDbContext dbContext, string token) =>
            dbContext.Users
                .AsNoTracking()
                .Where(u => u.GoogleProviderId == token));

    /// <inheritdoc />
    public async Task<User?> GetByIdReadonlyAsync(Guid id, CancellationToken cancellationToken)
    {
        var userEf = await GetUserEfByIdCompiled(context, id)
            .FirstOrDefaultAsync(cancellationToken);

        if (userEf == null)
        {
            return null;
        }

        var user = mapper.Map<User>(userEf);
        return user;
    }

    /// <inheritdoc />
    public async Task<User?> GetByUsernameReadonlyAsync(string username, CancellationToken cancellationToken)
    {
        var userEf = await GetUserEfByUsernameCompiled(context, username)
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

        if (userEf == null)
        {
            return null;
        }

        var user = mapper.Map<User>(userEf);
        return user;
    }

    /// <inheritdoc />
    public async Task<User?> GetByEmailReadonlyAsync(string email, CancellationToken cancellationToken)
    {
        var userEf = await GetUserEfByEmailCompiled(context, email)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (userEf == null)
        {
            return null;
        }

        var user = mapper.Map<User>(userEf);
        return user;
    }

    /// <inheritdoc />
    public async Task<User?> GetByGoogleTokenReadonlyAsync(string token, CancellationToken cancellationToken)
    {
        var userEf = await GetUserEfByGoogleTokenCompiled(context, token)
            .FirstOrDefaultAsync(u => u.GoogleProviderId == token, cancellationToken);

        if (userEf == null)
        {
            return null;
        }

        var user = mapper.Map<User>(userEf);
        return user;
    }
}
