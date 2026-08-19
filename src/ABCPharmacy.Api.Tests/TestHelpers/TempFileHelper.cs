using System.Text.Json;

namespace ABCPharmacy.Api.Tests.TestHelpers;

internal static class TempFileHelper
{
    public static string CreateTempJsonFile<T>(T content)
    {
        var path = Path.Combine(Path.GetTempPath(), $"abcpharmacy_test_{Guid.NewGuid():N}.json");
        var json = JsonSerializer.Serialize(content, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
        return path;
    }

    public static void DeleteIfExists(string path)
    {
        try { if (File.Exists(path)) File.Delete(path); } catch { /* ignore */ }
    }
}