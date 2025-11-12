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
        public async Task<Result<User, ValidationResult>> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            var userEf = await context.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

            if (userEf == null)
            {
                var errors = new ValidationResult(new[]
                {
                    new ValidationFailure(nameof(username), "User with such username not found")
                });
                return Result.Failure<User, ValidationResult>(errors);
            }

            var user = mapper.Map<User>(userEf);
            return Result.Success<User, ValidationResult>(user);
        }

        public async Task<Result<User, ValidationResult>> AddAsync(User user, CancellationToken cancellationToken)
        {
            var existingUserEf = await GetByUsernameAsync(user.Username, cancellationToken);
            if (existingUserEf.IsSuccess)
            {
                var errors = new ValidationResult(new[]
                {
                    new ValidationFailure(nameof(user), "User with such username already exists")
                });
                return Result.Failure<User, ValidationResult>(errors);
            }

            try
            {
                var userEf = mapper.Map<UserEf>(user);
                await context.Users.AddAsync(userEf, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                var addedUser = mapper.Map<User>(userEf);
                return Result.Success<User, ValidationResult>(addedUser);
            }
            catch (DbUpdateException exception)
            {
                var errors = new ValidationResult(new[]
                {
                    new ValidationFailure(nameof(user), "Database update error")
                });
                return Result.Failure<User, ValidationResult>(errors);
            }
        }

    }
}
