using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string _filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public async Task<User> AddAsync(User user)
    {
        List<User> users = await FileHelper.LoadAsync<User>(_filePath);
        user.Id = users.Any()
            ? users.Max(u => u.Id) + 1
            : 1;
        users.Add(user);
        await FileHelper.SaveAsync(_filePath, users);
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        List<User> users = await FileHelper.LoadAsync<User>(_filePath);
        User? existingUser = users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.Id}' not found");
        }

        users.Remove(existingUser);
        users.Add(user);
        await FileHelper.SaveAsync(_filePath, users);

        return user;
    }

    public async Task<User> DeleteAsync(int userId)
    {
        List<User> users = await FileHelper.LoadAsync<User>(_filePath);
        User? userToRemove = users.SingleOrDefault(u => u.Id == userId);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{userId}' not found");
        }

        users.Remove(userToRemove);
        await FileHelper.SaveAsync(_filePath, users);

        return userToRemove;
    }

    public async Task<User> GetSingleAsync(int userId)
    {
        List<User> users = await FileHelper.LoadAsync<User>(_filePath);
        User? userToGet = users.SingleOrDefault(u => u.Id == userId);
        if (userToGet is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{userId}' not found");
        }

        return userToGet;
    }

    public IQueryable<User> GetMany()
    {
        List<User> users = FileHelper.LoadAsync<User>(_filePath).Result;
        return users.AsQueryable();
    }
}
