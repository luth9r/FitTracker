using FitTracker.Domain.Entities;
using FluentValidation.Results;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Domain.Abstract.Interfaces
{
    public interface IUserRepository
    {
        Task<Result<User, ValidationResult>> GetByUsernameAsync(string username, CancellationToken cancellationToken);

        Task<Result<User, ValidationResult>> AddAsync(User user, CancellationToken cancellationToken);

    }
}
