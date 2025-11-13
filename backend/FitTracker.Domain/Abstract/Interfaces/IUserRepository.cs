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
        Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);

        Task AddAsync(User user, CancellationToken cancellationToken);

    }
}
