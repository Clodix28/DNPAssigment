using ApiContracts.Comments;

namespace BlazorApp.Services;

public interface ICommentService
{
    Task<CommentDto> AddCommentAsync(CreateCommentDto request);
    Task<CommentDto> UpdateCommentAsync(int id, UpdateCommentDto request);
    Task<CommentDto> GetCommentAsync(int id);
    Task<List<CommentDto>> GetCommentsAsync(int? postId = null, int? userId = null, string? userName = null);
    Task DeleteCommentAsync(int id);
}
