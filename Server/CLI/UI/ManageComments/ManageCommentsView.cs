namespace CLI.UI.ManageComments;

using RepositoryContracts;

public class ManageCommentsView
{
    private readonly ICommentRepository _commentRepository;
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public ManageCommentsView(ICommentRepository commentRepository, IPostRepository postRepository, IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine("\n--- Manage Comments ---");
            Console.WriteLine("1. Add comment to a post");
            Console.WriteLine("2. List all comments");
            Console.WriteLine("0. Back");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    CreateCommentView createView = new CreateCommentView(_commentRepository);
                    await createView.ShowAsync();
                    break;
                case "2":
                    ListCommentsView listView = new ListCommentsView(_commentRepository);
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