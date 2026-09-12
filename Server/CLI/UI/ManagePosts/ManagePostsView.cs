namespace CLI.UI.ManagePosts;

using RepositoryContracts;

public class ManagePostsView
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICommentRepository _commentRepository;

    public ManagePostsView(IPostRepository postRepository, IUserRepository userRepository, ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _commentRepository = commentRepository;
    }

    public async Task ShowAsync()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine("\n--- Manage Posts ---");
            Console.WriteLine("1. Create new post");
            Console.WriteLine("2. List all posts");
            Console.WriteLine("3. View single post");
            Console.WriteLine("0. Back");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    CreatePostView createView = new CreatePostView(_postRepository);
                    await createView.ShowAsync();
                    break;
                case "2":
                    ListPostsView listView = new ListPostsView(_postRepository);
                    await listView.ShowAsync();
                    break;
                case "3":
                    SinglePostView singleView = new SinglePostView(_postRepository, _userRepository, _commentRepository);
                    await singleView.ShowAsync();
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