using System.Text;
using System.Text.Json;

namespace PicPayPA.Infra;

public class GeminiSummarizer : ISummarizer
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeminiSummarizer(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = new HttpClient();
    }

    public string Summarize(string text, int maxChars = 120)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;

        var request = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = $"Resuma o seguinte texto em no máximo {maxChars} caracteres: {text}" }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = _httpClient.PostAsync($"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent?key={_apiKey}", content).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
            var summary = result.GetProperty("candidates")[0]
                               .GetProperty("content")
                               .GetProperty("parts")[0]
                               .GetProperty("text")
                               .GetString() ?? "";

            return summary.Length > maxChars ? summary[..maxChars] + "..." : summary;
        }
        catch
        {
            return text.Length > maxChars ? text[..maxChars] + "..." : text;
        }
    }
}