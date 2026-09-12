using CLI.UI.ManageComments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;

namespace CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;

public class CliApp
{
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;

    public CliApp(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository)
    {
        _userRepository = userRepository;
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }

    public async Task StartAsync()
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("/n--- Forum Application ---");
            Console.WriteLine("1. Manage Users");
            Console.WriteLine("2. Manage Posts");
            Console.WriteLine("3. Manage Comments");
            Console.WriteLine("0. Exit"); 
            Console.WriteLine("Choose an option:");
            
            string? choice = Console.ReadLine();
            switch (choice)
            {
                 case "1":
                     ManageUsersView usersView = new ManageUsersView(_userRepository);
                     await usersView.ShowAsync();
                     break;
                 
                 case "2":
                     ManagePostsView postsView = new ManagePostsView(_postRepository, _userRepository, _commentRepository);
                     await postsView.ShowAsync();
                     break;
                 
                 case "3":
                     ManageCommentsView commentsView = new ManageCommentsView(_commentRepository, _postRepository, _userRepository);
                     await commentsView.ShowAsync();
                     break;
                 
                 case "0":
                     exit = true;
                     break;
                 
                 default:
                     Console.WriteLine("Please select a valid option.");
                     break;
                     
            }
        }
    }
}
