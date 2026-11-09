using ApiContracts.Posts;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<PostDto> AddPostAsync(CreatePostDto request);
    Task UpdatePostAsync(int id, UpdatePostDto request);
    Task<PostDto> GetPostAsync(int id);
    Task<List<PostDto>> GetPostsAsync(string? titleContains = null, int? userId = null, string? userName = null);
    Task DeletePostAsync(int id);
}
