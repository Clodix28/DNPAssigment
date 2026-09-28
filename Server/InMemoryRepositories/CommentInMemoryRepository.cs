using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private readonly List<Comment> _comments = new()
    {
        new Comment { Id = 1, PostId = 1, UserId = 1, Body = "Really enjoyed reading this."},
        new Comment { Id = 2, PostId = 2, UserId = 2, Body = "I don't think that's quite right."},
        new Comment { Id = 3, PostId = 3, UserId = 3, Body = "This helped me a lot, appreciate it." },
    };

    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = _comments.Any()
            ? _comments.Max(c => c.Id) + 1
            : 1;
        _comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task<Comment> UpdateAsync(Comment comment)
    {
        Comment? existingComment = _comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.Id}' not found");
        }

        _comments.Remove(existingComment);
        _comments.Add(comment);

        return Task.FromResult(comment);
    }

    public Task<Comment> DeleteAsync(int id)
    {
        Comment? commentToRemove = _comments.SingleOrDefault(c => c.Id == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }

        _comments.Remove(commentToRemove);
        return Task.FromResult(commentToRemove);
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? commentToGet = _comments.SingleOrDefault(c => c.Id == id);
        if (commentToGet is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }
        return Task.FromResult(commentToGet);
    }

    public IQueryable<Comment> GetMany()
    {
        return _comments.AsQueryable();
    }
}
