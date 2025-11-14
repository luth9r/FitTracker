using FitTracker.Domain.Entities;

namespace FitTracker.Domain.Abstract.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameReadonlyAsync(string username, CancellationToken cancellationToken);

        Task<User?> GetByEmailReadonlyAsync(string email, CancellationToken cancellationToken);

        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<User?> GetByIdReadonlyAsync(Guid id, CancellationToken cancellationToken);


        Task AddAsync(User user, CancellationToken cancellationToken);

        void Update(User user);



    }
}
