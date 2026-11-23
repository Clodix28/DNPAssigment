using Entities;
using Microsoft.EntityFrameworkCore;

namespace EfcRepositories;

public class AppContext : DbContext
{
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // relative path that points at the same file whether you run "dotnet ef" from this folder
        // or start the Web API (which runs from Server/WebApplication)
        optionsBuilder.UseSqlite("Data Source=../EfcRepositories/app.db");
    }
}
