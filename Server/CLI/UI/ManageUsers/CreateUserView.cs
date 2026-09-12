namespace CLI.UI.ManageUsers;

using Entities;
using RepositoryContracts;

public class CreateUserView
{
    private readonly IUserRepository _userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        Console.Write("Enter username: ");
        string? username = Console.ReadLine();

        Console.Write("Enter password: ");
        string? password = Console.ReadLine();

        User user = new User
        {
            Username = username!,
            Password = password!
        };

        User created = await _userRepository.AddAsync(user);
        Console.WriteLine($"User created with Id {created.Id}");
    }
}