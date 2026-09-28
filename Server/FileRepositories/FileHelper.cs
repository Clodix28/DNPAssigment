using System.Text.Json;

namespace FileRepositories;

internal static class FileHelper
{
    public static async Task<List<T>> LoadAsync<T>(string filePath)
    {
        if (!File.Exists(filePath))
        {
            await File.WriteAllTextAsync(filePath, "[]");
        }

        string json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
    }

    public static async Task SaveAsync<T>(string filePath, List<T> items)
    {
        string json = JsonSerializer.Serialize(items);
        await File.WriteAllTextAsync(filePath, json);
    }
}
