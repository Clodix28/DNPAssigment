using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string _filePath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        List<Comment> comments = await FileHelper.LoadAsync<Comment>(_filePath);
        comment.Id = comments.Any()
            ? comments.Max(c => c.Id) + 1
            : 1;
        comments.Add(comment);
        await FileHelper.SaveAsync(_filePath, comments);
        return comment;
    }

    public async Task<Comment> UpdateAsync(Comment comment)
    {
        List<Comment> comments = await FileHelper.LoadAsync<Comment>(_filePath);
        Comment? existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.Id}' not found");
        }

        comments.Remove(existingComment);
        comments.Add(comment);
        await FileHelper.SaveAsync(_filePath, comments);

        return comment;
    }

    public async Task<Comment> DeleteAsync(int id)
    {
        List<Comment> comments = await FileHelper.LoadAsync<Comment>(_filePath);
        Comment? commentToRemove = comments.SingleOrDefault(c => c.Id == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }

        comments.Remove(commentToRemove);
        await FileHelper.SaveAsync(_filePath, comments);

        return commentToRemove;
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        List<Comment> comments = await FileHelper.LoadAsync<Comment>(_filePath);
        Comment? commentToGet = comments.SingleOrDefault(c => c.Id == id);
        if (commentToGet is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }

        return commentToGet;
    }

    public IQueryable<Comment> GetMany()
    {
        List<Comment> comments = FileHelper.LoadAsync<Comment>(_filePath).Result;
        return comments.AsQueryable();
    }
}
