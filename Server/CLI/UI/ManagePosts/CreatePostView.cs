namespace CLI.UI.ManagePosts;

using Entities;
using RepositoryContracts;

public class CreatePostView
{
    private readonly IPostRepository _postRepository;

    public CreatePostView(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task ShowAsync()
    {
        Console.Write("Enter title: ");
        string? title = Console.ReadLine();

        Console.Write("Enter body: ");
        string? body = Console.ReadLine();

        Console.Write("Enter your user id: ");
        int userId = int.Parse(Console.ReadLine()!);

        Post post = new Post
        {
            Title = title!,
            Body = body!,
            UserId = userId
        };

        Post created = await _postRepository.AddAsync(post);
        Console.WriteLine($"Post created with Id {created.Id}");
    }
}