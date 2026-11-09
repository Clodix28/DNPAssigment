using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts.Comments;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<CommentDto> AddCommentAsync(CreateCommentDto request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("comments", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<CommentDto>(response, JsonOptions)!;
    }

    public async Task<CommentDto> UpdateCommentAsync(int id, UpdateCommentDto request)
    {
        HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"comments/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<CommentDto>(response, JsonOptions)!;
    }

    public async Task<CommentDto> GetCommentAsync(int id)
    {
        HttpResponseMessage httpResponse = await client.GetAsync($"comments/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<CommentDto>(response, JsonOptions)!;
    }

    public async Task<List<CommentDto>> GetCommentsAsync(int? postId = null, int? userId = null, string? userName = null)
    {
        List<string> filters = new();
        if (postId != null)
        {
            filters.Add($"postId={postId}");
        }
        if (userId != null)
        {
            filters.Add($"userId={userId}");
        }
        if (!string.IsNullOrWhiteSpace(userName))
        {
            filters.Add($"userName={Uri.EscapeDataString(userName)}");
        }

        string uri = filters.Count == 0 ? "comments" : "comments?" + string.Join("&", filters);

        HttpResponseMessage httpResponse = await client.GetAsync(uri);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<List<CommentDto>>(response, JsonOptions)!;
    }

    public async Task DeleteCommentAsync(int id)
    {
        HttpResponseMessage httpResponse = await client.DeleteAsync($"comments/{id}");
        if (!httpResponse.IsSuccessStatusCode)
        {
            string response = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception(response);
        }
    }
}
