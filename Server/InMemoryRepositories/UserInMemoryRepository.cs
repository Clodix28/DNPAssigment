using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private readonly List<User> _users = new()
    {
        new User { Id = 1, Email = "claudiu@ggg.com",  Password = "password" , Username = "claudiu" },
        new User { Id = 2, Email = "george@ggg.com", Password = "password", Username = "george" },
        new User { Id = 3, Email = "vikinka@ggg.com", Password = "password", Username = "viki" },    
    };

    public Task<User> AddAsync(User user)
    {
        user.Id = _users.Any()
            ? _users.Max(u => u.Id) + 1
            : 1;
        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task<User> UpdateAsync(User user)
    {
        User? existingUser = _users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.Id}' not found");
        }

        _users.Remove(existingUser);
        _users.Add(user);

        return Task.FromResult(user);
    }

    public Task<User> DeleteAsync(int id)
    {
        User? userToRemove = _users.SingleOrDefault(u => u.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }

        _users.Remove(userToRemove);
        return Task.FromResult(userToRemove);
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? userToGet = _users.SingleOrDefault(u => u.Id == id);
        if (userToGet is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }
        return Task.FromResult(userToGet);
    }

    public IQueryable<User> GetMany()
    {
        return _users.AsQueryable();
    }
}