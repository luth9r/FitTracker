using AutoMapper;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FitTracker.Infrastructure.Persistence.Repositories
{
    internal sealed class UserWriteRepository(
        FitTrackerDbContext context,
        IMapper mapper) : IUserWriteRepository
    {
        /// <inheritdoc/>
        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            var userEf = mapper.Map<UserEf>(user);

            _ = await context.Users.AddAsync(userEf, cancellationToken);
        }

        /// <inheritdoc/>
        public void Update(User user)
        {
            var userEf = mapper.Map<UserEf>(user);

            context.Users.Attach(userEf);
            context.Entry(userEf).State = EntityState.Modified;

            context.Entry(userEf).Property(x => x.CreatedAt).IsModified = false;
        }
    }
}
