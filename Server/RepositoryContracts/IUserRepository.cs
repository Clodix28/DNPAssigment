using Entities;

namespace RepositoryContracts;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<User> DeleteAsync(int userId);
    Task<User> GetSingleAsync(int userId);
    IQueryable<User> GetMany();
}