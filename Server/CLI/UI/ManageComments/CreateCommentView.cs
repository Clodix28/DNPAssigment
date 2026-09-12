namespace CLI.UI.ManageComments;

using Entities;
using RepositoryContracts;

public class CreateCommentView
{
    private readonly ICommentRepository _commentRepository;

    public CreateCommentView(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task ShowAsync()
    {
        Console.Write("Enter post id: ");
        int postId = int.Parse(Console.ReadLine()!);

        Console.Write("Enter your user id: ");
        int userId = int.Parse(Console.ReadLine()!);

        Console.Write("Enter comment body: ");
        string? body = Console.ReadLine();

        Comment comment = new Comment
        {
            PostId = postId,
            UserId = userId,
            Body = body!
        };

        await _commentRepository.AddAsync(comment);
        Console.WriteLine("Comment added.");
    }
}