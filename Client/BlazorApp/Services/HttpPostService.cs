using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts.Posts;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("posts", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(response, JsonOptions)!;
    }

    public async Task UpdatePostAsync(int id, UpdatePostDto request)
    {
        // the API answers with 204 No Content here, so there's nothing to read back
        HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"posts/{id}", request);
        if (!httpResponse.IsSuccessStatusCode)
        {
            string response = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception(response);
        }
    }

    public async Task<PostDto> GetPostAsync(int id)
    {
        HttpResponseMessage httpResponse = await client.GetAsync($"posts/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(response, JsonOptions)!;
    }

    public async Task<List<PostDto>> GetPostsAsync(string? titleContains = null, int? userId = null, string? userName = null)
    {
        List<string> filters = new();
        if (!string.IsNullOrWhiteSpace(titleContains))
        {
            filters.Add($"titleContains={Uri.EscapeDataString(titleContains)}");
        }
        if (userId != null)
        {
            filters.Add($"userId={userId}");
        }
        if (!string.IsNullOrWhiteSpace(userName))
        {
            filters.Add($"userName={Uri.EscapeDataString(userName)}");
        }

        string uri = filters.Count == 0 ? "posts" : "posts?" + string.Join("&", filters);

        HttpResponseMessage httpResponse = await client.GetAsync(uri);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<List<PostDto>>(response, JsonOptions)!;
    }

    public async Task DeletePostAsync(int id)
    {
        HttpResponseMessage httpResponse = await client.DeleteAsync($"posts/{id}");
        if (!httpResponse.IsSuccessStatusCode)
        {
            string response = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception(response);
        }
    }
}
