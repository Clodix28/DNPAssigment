namespace CLI.UI.ManagePosts;

using RepositoryContracts;

public class SinglePostView
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICommentRepository _commentRepository;

    public SinglePostView(IPostRepository postRepository, IUserRepository userRepository, ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _commentRepository = commentRepository;
    }

    public async Task ShowAsync()
    {
        Console.Write("Enter post id: ");
        int postId = int.Parse(Console.ReadLine()!);

        var post = await _postRepository.GetSingleAsync(postId);
        var author = await _userRepository.GetSingleAsync(post.UserId);

        Console.WriteLine($"\nTitle: {post.Title}");
        Console.WriteLine($"By: {author.Username}");
        Console.WriteLine($"Body: {post.Body}");

        Console.WriteLine("\nComments:");
        var comments = _commentRepository.GetMany().Where(c => c.PostId == postId);
        foreach (var comment in comments)
        {
            Console.WriteLine($"- {comment.Body}");
        }
    }
}