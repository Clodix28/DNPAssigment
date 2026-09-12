namespace CLI.UI.ManageUsers;

using RepositoryContracts;

public class ManageUsersView
{
    private readonly IUserRepository _userRepository;

    public ManageUsersView(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine("\n--- Manage Users ---");
            Console.WriteLine("1. Create new user");
            Console.WriteLine("2. List all users");
            Console.WriteLine("0. Back");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    CreateUserView createView = new CreateUserView(_userRepository);
                    await createView.ShowAsync();
                    break;
                case "2":
                    ListUsersView listView = new ListUsersView(_userRepository);
                    await listView.ShowAsync();
                    break;
                case "0":
                    back = true;
                    break;
                default:
                    Console.WriteLine("Invalid option, try again.");
                    break;
            }
        }
    }
}