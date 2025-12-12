using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FitTracker.Infrastructure.Persistence.Repositories
{
    internal sealed class UserReadRepository(
        FitTrackerDbContext context,
        IMapper mapper,
        ILogger<UserReadRepository> logger) : IUserReadRepository
    {
        private static readonly Func<FitTrackerDbContext, Guid, IAsyncEnumerable<UserEf>> GetUserEfByIdCompiled =
        EF.CompileAsyncQuery(
            (FitTrackerDbContext dbContext, Guid userId) =>
                dbContext.Users
                         .AsNoTracking()
                         .Where(u => u.Id == userId));

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<User?> GetByUsernameReadonlyAsync(string username, CancellationToken cancellationToken)
        {
            var userEf = await context.Users
                              .AsNoTracking()
                              .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

            if (userEf == null)
            {
                return null;
            }

            var user = mapper.Map<User>(userEf);
            return user;
        }

        /// <inheritdoc/>
        public async Task<User?> GetByEmailReadonlyAsync(string email, CancellationToken cancellationToken)
        {
            var userEf = await context.Users
                              .AsNoTracking()
                              .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            if (userEf == null)
            {
                return null;
            }

            var user = mapper.Map<User>(userEf);
            return user;
        }

        /// <inheritdoc/>
        public async Task<User?> GetByGoogleTokenReadonlyAsync(string token, CancellationToken cancellationToken)
        {
            var userEf = await context.Users
                              .AsNoTracking()
                              .FirstOrDefaultAsync(u => u.GoogleProviderId == token, cancellationToken);

            if (userEf == null)
            {
                return null;
            }

            var user = mapper.Map<User>(userEf);
            return user;
        }
    }
}
