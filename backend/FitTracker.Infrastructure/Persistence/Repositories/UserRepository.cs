using AutoMapper;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FitTracker.Infrastructure.Persistence.Repositories
{
    internal class UserRepository(FitTrackerDbContext context, IMapper mapper, ILogger<UserRepository> logger) : IUserRepository
    {
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

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var userEf = await context.Users
                               .FindAsync(new object[] { id }, cancellationToken);

            if (userEf == null)
            {
                return null;
            }

            var user = mapper.Map<User>(userEf);
            return user;
        }

        public async Task<User?> GetByIdReadonlyAsync(Guid id, CancellationToken cancellationToken)
        {
            var userEf = await context.Users
                               .AsNoTracking()
                               .FirstOrDefaultAsync(u => u.Id == id);

            if (userEf == null)
            {
                return null;
            }

            var user = mapper.Map<User>(userEf);
            return user;
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            var userEf = mapper.Map<UserEf>(user);

            await context.Users.AddAsync(userEf, cancellationToken);
        }

        public void Update(User user)
        {
            var userEf = mapper.Map<UserEf>(user);

            context.Users.Update(userEf);
        }



    }
}
