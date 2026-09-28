using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string _filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public async Task<Post> AddAsync(Post post)
    {
        List<Post> posts = await FileHelper.LoadAsync<Post>(_filePath);
        post.Id = posts.Any()
            ? posts.Max(p => p.Id) + 1
            : 1;
        posts.Add(post);
        await FileHelper.SaveAsync(_filePath, posts);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        List<Post> posts = await FileHelper.LoadAsync<Post>(_filePath);
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.Id}' not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);
        await FileHelper.SaveAsync(_filePath, posts);
    }

    public async Task DeleteAsync(int id)
    {
        List<Post> posts = await FileHelper.LoadAsync<Post>(_filePath);
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);
        await FileHelper.SaveAsync(_filePath, posts);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        List<Post> posts = await FileHelper.LoadAsync<Post>(_filePath);
        Post? postToGet = posts.SingleOrDefault(p => p.Id == id);
        if (postToGet is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        return postToGet;
    }

    public IQueryable<Post> GetMany()
    {
        List<Post> posts = FileHelper.LoadAsync<Post>(_filePath).Result;
        return posts.AsQueryable();
    }
}
