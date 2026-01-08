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
    private static readonly Func<FitTrackerDbContext, string, IAsyncEnumerable<UserEf>> GetUserEfByEmailCompiled =
        EF.CompileAsyncQuery((FitTrackerDbContext dbContext, string email) =>
            dbContext.Users
                .Where(u => u.Email == email));

    /// <inheritdoc />
    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        var userEf = mapper.Map<UserEf>(user);

        _ = await context.Users.AddAsync(userEf, cancellationToken);
    }

    /// <inheritdoc />
    public void Update(User user)
    {
        var userEf = mapper.Map<UserEf>(user);

        context.Users.Attach(userEf);
        context.Entry(userEf).State = EntityState.Modified;

        context.Entry(userEf).Property(x => x.CreatedAt).IsModified = false;
    }

    /// <inheritdoc />
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
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
}