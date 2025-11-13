using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Shared.ValidationErrors;
using FitTracker.Infrastructure.Persistence.Data;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Infrastructure.Persistence.Repositories
{
    internal class UserRepository(FitTrackerDbContext context, IMapper mapper, ILogger<UserRepository> logger) : IUserRepository
    {
        public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            var userEf = await context.Users
                              .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

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

    }
}
