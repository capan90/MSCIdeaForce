using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using MSC.IdeaForge.Domain.Interfaces;
using MSC.IdeaForge.Domain.ValueObjects;

namespace MSC.IdeaForge.Infrastructure.AI;

/// <summary>
/// OpenAI API kullanarak problemleri analiz eden sağlayıcı sınıf.
/// </summary>
public class OpenAIProvider : IAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public OpenAIProvider(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    /// <summary>
    /// Problemi OpenAI API'sine göndererek JSON formatında analiz sonuçlarını alır.
    /// </summary>
    public async Task<ProblemAnalysisResult> AnalyzeProblemAsync(string title, string description, string? sector)
    {
        var apiKey = _configuration["AI:OpenAI:ApiKey"];
        var model = _configuration["AI:OpenAI:Model"] ?? "gpt-4o-mini";

        // API Key kontrolü
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("OpenAI API Key bulunamadı. Lütfen appsettings.json dosyasındaki 'AI:OpenAI:ApiKey' alanını güncelleyin.");
        }

        // OpenAI promptunu hazırlıyoruz
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
            model = model,
            messages = new[]
            {
                new { role = "user", content = prompt }
            },
            response_format = new { type = "json_object" }
        };

        // HTTP isteğini hazırlıyoruz
        var requestUrl = "https://api.openai.com/v1/chat/completions";
        Console.WriteLine($"OpenAI API URL: {requestUrl}");

        using var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        request.Content = JsonContent.Create(requestBody);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // HTTP POST isteği gönderiyoruz
        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"OpenAI API Hata: {errorContent}");
            throw new HttpRequestException($"OpenAI API hatası: {errorContent}");
        }

        response.EnsureSuccessStatusCode();

        var openAiResponse = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
        var jsonText = openAiResponse?.Choices?.FirstOrDefault()?.Message?.Content;

        if (string.IsNullOrWhiteSpace(jsonText))
        {
            throw new InvalidOperationException("OpenAI API boş veya geçersiz bir yanıt döndürdü.");
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Gelen JSON metnini DTO nesnesine dönüştürüyoruz
        var result = JsonSerializer.Deserialize<ProblemAnalysisResultDto>(jsonText, options);

        if (result is null)
        {
            throw new InvalidOperationException("OpenAI yanıtı beklenen şemaya göre çözümlenemedi.");
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

    // OpenAI API yanıt yapısı için dahili sınıflar
    private class OpenAIResponse
    {
        [JsonPropertyName("choices")]
        public Choice[]? Choices { get; set; }
    }

    private class Choice
    {
        [JsonPropertyName("message")]
        public Message? Message { get; set; }
    }

    private class Message
    {
        [JsonPropertyName("content")]
        public string? Content { get; set; }
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

    public Task<List<Competitor>> AnalyzeCompetitorsAsync(string title, string description, string? sector)
    {
        throw new NotImplementedException();
    }
}
