namespace CLI.UI.ManageUsers;

using RepositoryContracts;

public class ListUsersView
{
    private readonly IUserRepository _userRepository;

    public ListUsersView(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task ShowAsync()
    {
        Console.WriteLine("\n--- All Users ---");
        foreach (var user in _userRepository.GetMany())
        {
            Console.WriteLine($"[{user.Id}] {user.Username}");
        }
        return Task.CompletedTask;
    }
}