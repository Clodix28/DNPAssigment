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
        // TODO: build out the main menu loop here
    }
}
