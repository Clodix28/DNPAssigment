using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcUserRepository : IUserRepository
{
    private readonly AppContext ctx;

    public EfcUserRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<User> AddAsync(User user)
    {
        EntityEntry<User> entityEntry = await ctx.Users.AddAsync(user);
        await ctx.SaveChangesAsync();
        // the database sets the id, entityEntry.Entity has it after saving
        return entityEntry.Entity;
    }

    public async Task<User> UpdateAsync(User user)
    {
        if (!await ctx.Users.AnyAsync(u => u.Id == user.Id))
        {
            throw new InvalidOperationException($"User with ID '{user.Id}' not found");
        }

        ctx.Users.Update(user);
        await ctx.SaveChangesAsync();
        return user;
    }

    public async Task<User> DeleteAsync(int userId)
    {
        User? existing = await ctx.Users.SingleOrDefaultAsync(u => u.Id == userId);
        if (existing == null)
        {
            throw new InvalidOperationException($"User with ID '{userId}' not found");
        }

        ctx.Users.Remove(existing);
        await ctx.SaveChangesAsync();
        return existing;
    }

    public async Task<User> GetSingleAsync(int userId)
    {
        User? user = await ctx.Users.SingleOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID '{userId}' not found");
        }

        return user;
    }

    public IQueryable<User> GetMany()
    {
        return ctx.Users.AsQueryable();
    }
}
