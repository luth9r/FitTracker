using AutoMapper;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Infrastructure.Persistence.Repositories;

internal sealed class UserWriteRepository(
    FitTrackerDbContext context,
    IMapper mapper) : IUserWriteRepository
{
    private static readonly Func<FitTrackerDbContext, Guid, Task<UserEf?>> FindUserEfByIdCompiled =
        EF.CompileAsyncQuery((FitTrackerDbContext dbContext, Guid id) =>
            dbContext.Users
                .FirstOrDefault(u => u.Id == id));

    private static readonly Func<FitTrackerDbContext, string, Task<UserEf?>> FindByEmailCompiled =
        EF.CompileAsyncQuery((FitTrackerDbContext db, string email) =>
            db.Users.FirstOrDefault(u => u.Email == email));

    private static readonly Func<FitTrackerDbContext, string, Task<UserEf?>> FindByGoogleTokenCompiled =
        EF.CompileAsyncQuery((FitTrackerDbContext db, string token) =>
            db.Users.FirstOrDefault(u => u.GoogleProviderId == token));

    /// <inheritdoc />
    public async Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var userEf = await FindUserEfByIdCompiled(context, id);

        if (userEf == null)
        {
            return null;
        }

        return mapper.Map<User>(userEf);
    }

    /// <inheritdoc />
    public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var userEf = await FindByEmailCompiled(context, email);

        if (userEf == null)
        {
            return null;
        }

        return mapper.Map<User>(userEf);
    }

    /// <inheritdoc />
    public async Task<User?> FindByGoogleTokenAsync(string token, CancellationToken cancellationToken)
    {
        var userEf = await FindByGoogleTokenCompiled(context, token);

        if (userEf == null)
        {
            return null;
        }

        return mapper.Map<User>(userEf);
    }

    /// <inheritdoc />
    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        var userEf = mapper.Map<UserEf>(user);

        _ = await context.Users.AddAsync(userEf, cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        var userEf = context.Users.Local.FirstOrDefault(u => u.Id == user.Id);

        if (userEf is null)
        {
            // if (userEf == null)
            // {
            //     userEf = await context.Users
            //         .FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);
            // }

            throw new KeyNotFoundException($"User {user.Id} not found for update");
        }

        mapper.Map(user, userEf);
    }
}
