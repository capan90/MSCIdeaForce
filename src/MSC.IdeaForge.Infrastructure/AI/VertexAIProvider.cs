using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using MSC.IdeaForge.Domain.Interfaces;
using MSC.IdeaForge.Domain.ValueObjects;

namespace MSC.IdeaForge.Infrastructure.AI;

/// <summary>
/// Google Cloud Vertex AI Gemini API kullanarak problemleri analiz eden sağlayıcı sınıf.
/// </summary>
public class VertexAIProvider : IAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public VertexAIProvider(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    /// <summary>
    /// Problemi Vertex AI Gemini API'sine göndererek JSON formatında analiz sonuçlarını alır.
    /// </summary>
    public async Task<ProblemAnalysisResult> AnalyzeProblemAsync(string title, string description, string? sector)
    {
        var projectId = _configuration["AI:VertexAI:ProjectId"];
        var accessToken = _configuration["AI:VertexAI:AccessToken"];
        var region = _configuration["AI:VertexAI:Region"] ?? "us-central1";
        var model = _configuration["AI:VertexAI:Model"] ?? "gemini-1.5-flash";

        // Proje ID kontrolü
        if (string.IsNullOrWhiteSpace(projectId))
        {
            throw new InvalidOperationException("Vertex AI ProjectId bulunamadı. Lütfen appsettings.json dosyasındaki 'AI:VertexAI:ProjectId' alanını güncelleyin.");
        }

        // Access Token kontrolü
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new InvalidOperationException("Vertex AI AccessToken bulunamadı. Lütfen appsettings.json dosyasındaki 'AI:VertexAI:AccessToken' alanını güncelleyin.");
        }

        // Gemini promptunu hazırlıyoruz
        var prompt = $@"Aşağıdaki problemi analiz et. Yanıtı mutlaka JSON formatında döndür. JSON formatı şu alanları içermelidir:
- Summary (Problemin kısa Türkçe özeti)
- SuggestedCategory (Önerilen Türkçe kategori)
- SuggestedTags (Önerilen Türkçe etiketler, virgülle ayrılmış tek bir string)
- RiskLevel (Low, Medium, High veya Critical değerlerinden biri)
- SolutionTypeSuggestion (Önerilen Türkçe çözüm yöntemi veya tipi)
- ConfidenceScore (0.0 ile 1.0 arasında bir güven skoru, double)

Problem Detayları:
Başlık: {title}
Açıklama: {description}
Sektör: {sector ?? "Belirtilmemiş"}

Yanıt şeması tam olarak şu şekilde olmalıdır:
{{
  ""Summary"": ""açıklama"",
  ""SuggestedCategory"": ""kategori"",
  ""SuggestedTags"": ""etiket1, etiket2"",
  ""RiskLevel"": ""Medium"",
  ""SolutionTypeSuggestion"": ""çözüm önerisi"",
  ""ConfidenceScore"": 0.85
}}";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            },
            generationConfig = new
            {
                responseMimeType = "application/json"
            }
        };

        // Vertex AI REST API endpoint URL'ini hazırlıyoruz
        var requestUrl = $"https://{region}-aiplatform.googleapis.com/v1/projects/{projectId}/locations/{region}/publishers/google/models/{model}:generateContent";
        Console.WriteLine($"Vertex AI API URL: {requestUrl}");

        using var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Content = JsonContent.Create(requestBody);

        // HTTP POST isteği gönderiyoruz
        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Vertex AI API Hata: {errorContent}");
            throw new HttpRequestException($"Vertex AI API hatası: {errorContent}");
        }

        response.EnsureSuccessStatusCode();

        var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>();
        var jsonText = geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

        if (string.IsNullOrWhiteSpace(jsonText))
        {
            throw new InvalidOperationException("Vertex AI API boş veya geçersiz bir yanıt döndürdü.");
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Gelen JSON metnini DTO nesnesine dönüştürüyoruz
        var result = JsonSerializer.Deserialize<ProblemAnalysisResultDto>(jsonText, options);

        if (result is null)
        {
            throw new InvalidOperationException("Vertex AI yanıtı beklenen şemaya göre çözümlenemedi.");
        }

        return new ProblemAnalysisResult(
            result.Summary ?? string.Empty,
            result.SuggestedCategory ?? string.Empty,
            result.SuggestedTags ?? string.Empty,
            result.RiskLevel ?? "Medium",
            result.SolutionTypeSuggestion ?? string.Empty,
            result.ConfidenceScore
        );
    }

    // Gemini API yanıt yapısı için dahili sınıflar
    private class GeminiResponse
    {
        [JsonPropertyName("candidates")]
        public Candidate[]? Candidates { get; set; }
    }

    private class Candidate
    {
        [JsonPropertyName("content")]
        public Content? Content { get; set; }
    }

    private class Content
    {
        [JsonPropertyName("parts")]
        public Part[]? Parts { get; set; }
    }

    private class Part
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }

    private class ProblemAnalysisResultDto
    {
        public string? Summary { get; set; }
        public string? SuggestedCategory { get; set; }
        public string? SuggestedTags { get; set; }
        public string? RiskLevel { get; set; }
        public string? SolutionTypeSuggestion { get; set; }
        public double ConfidenceScore { get; set; }
    }

    public Task<OpportunityScoreResult> ScoreOpportunityAsync(string title, string description, string? sector, string? founderContext = null)
    {
        throw new NotImplementedException();
    }

    public Task<ResearchResult> ResearchProblemAsync(string title, string description, string? sector)
    {
        throw new NotImplementedException();
    }

    public Task<List<SolutionSuggestion>> SuggestSolutionsAsync(string title, string description, string? sector)
    {
        throw new NotImplementedException();
    }

    public Task<MVPPlanResult> GenerateMVPPlanAsync(string title, string description, string? sector, string? solutionType)
    {
        throw new NotImplementedException();
    }

    // Sektör analizi bu sağlayıcıda henüz uygulanmadı; şu an yalnızca Gemini destekler.
    public Task<SectorAnalysisResult> AnalyzeSectorAsync(string sector)
    {
        throw new NotImplementedException();
    }

    // Trend, gelir ve doğrulama soruları analizleri bu sağlayıcıda henüz uygulanmadı; şu an yalnızca Gemini destekler.
    public Task<TrendAnalysisResult> AnalyzeTrendAsync(string trend)
    {
        throw new NotImplementedException();
    }

    public Task<RevenueAnalysisResult> AnalyzeRevenueAsync(string title, string description, string? sector, string? solutionType)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>> GenerateValidationQuestionsAsync(string title, string description)
    {
        throw new NotImplementedException();
    }
}
