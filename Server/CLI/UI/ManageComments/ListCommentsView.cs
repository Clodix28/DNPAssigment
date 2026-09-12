namespace CLI.UI.ManageComments;

using RepositoryContracts;

public class ListCommentsView
{
    private readonly ICommentRepository _commentRepository;

    public ListCommentsView(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public Task ShowAsync()
    {
        Console.WriteLine("\n--- All Comments ---");
        foreach (var comment in _commentRepository.GetMany())
        {
            Console.WriteLine($"[Post {comment.PostId}, User {comment.UserId}] {comment.Body}");
        }
        return Task.CompletedTask;
    }
}