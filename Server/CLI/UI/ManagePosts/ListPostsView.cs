namespace CLI.UI.ManagePosts;

using RepositoryContracts;

public class ListPostsView
{
    private readonly IPostRepository _postRepository;

    public ListPostsView(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public Task ShowAsync()
    {
        Console.WriteLine("\n--- All Posts ---");
        foreach (var post in _postRepository.GetMany())
        {
            Console.WriteLine($"[{post.Id}] {post.Title}");
        }
        return Task.CompletedTask;
    }
}