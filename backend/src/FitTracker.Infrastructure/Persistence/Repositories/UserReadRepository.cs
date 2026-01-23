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
    private static readonly Func<FitTrackerDbContext, Guid, Task<UserEf?>> GetUserEfByIdCompiled =
        EF.CompileAsyncQuery((FitTrackerDbContext dbContext, Guid userId) =>
            dbContext.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == userId));

    private static readonly Func<FitTrackerDbContext, string, Task<UserEf?>> GetUserEfByUsernameCompiled =
        EF.CompileAsyncQuery((FitTrackerDbContext dbContext, string username) =>
            dbContext.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Username == username));

    private static readonly Func<FitTrackerDbContext, string, Task<UserEf?>> GetUserEfByEmailCompiled =
        EF.CompileAsyncQuery((FitTrackerDbContext dbContext, string email) =>
            dbContext.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Email == email));

    private static readonly Func<FitTrackerDbContext, string, Task<UserEf?>> GetUserEfByGoogleTokenCompiled =
        EF.CompileAsyncQuery((FitTrackerDbContext dbContext, string token) =>
            dbContext.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.GoogleProviderId == token));

    /// <inheritdoc />
    public async Task<User?> FindByIdReadonlyAsync(Guid id, CancellationToken cancellationToken)
    {
        var userEf = await GetUserEfByIdCompiled(context, id);

        if (userEf == null)
        {
            return null;
        }

        var user = mapper.Map<User>(userEf);
        return user;
    }

    /// <inheritdoc />
    public async Task<User?> FindByUsernameReadonlyAsync(string username, CancellationToken cancellationToken)
    {
        var userEf = await GetUserEfByUsernameCompiled(context, username);

        if (userEf == null)
        {
            return null;
        }

        var user = mapper.Map<User>(userEf);
        return user;
    }

    /// <inheritdoc />
    public async Task<User?> FindByEmailReadonlyAsync(string email, CancellationToken cancellationToken)
    {
        var userEf = await GetUserEfByEmailCompiled(context, email);

        if (userEf == null)
        {
            return null;
        }

        var user = mapper.Map<User>(userEf);
        return user;
    }

    /// <inheritdoc />
    public async Task<User?> FindByGoogleTokenReadonlyAsync(string token, CancellationToken cancellationToken)
    {
        var userEf = await GetUserEfByGoogleTokenCompiled(context, token);

        if (userEf == null)
        {
            return null;
        }

        var user = mapper.Map<User>(userEf);
        return user;
    }
}
