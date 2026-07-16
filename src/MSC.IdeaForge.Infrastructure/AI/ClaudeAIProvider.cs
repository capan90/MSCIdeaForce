using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using MSC.IdeaForge.Domain.Interfaces;
using MSC.IdeaForge.Domain.ValueObjects;
using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Infrastructure.AI;

/// <summary>
/// Anthropic Claude API kullanarak problemleri analiz eden sağlayıcı sınıf.
/// </summary>
public class ClaudeAIProvider : IAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ClaudeAIProvider(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    /// <summary>
    /// Problemi Claude API'sine göndererek JSON formatında analiz sonuçlarını alır.
    /// </summary>
    public async Task<ProblemAnalysisResult> AnalyzeProblemAsync(string title, string description, string? sector)
    {
        var apiKey = _configuration["AI:Claude:ApiKey"];
        var model = _configuration["AI:Claude:Model"] ?? "claude-haiku-4-5-20251001";

        // API Key kontrolü
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Claude API Key bulunamadı. Lütfen appsettings.json dosyasındaki 'AI:Claude:ApiKey' alanını güncelleyin.");
        }

        // Claude promptunu hazırlıyoruz
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
}}

Sen profesyonel bir problem analisti yapay zekasın. Çıktı olarak sadece ve sadece saf, geçerli bir JSON döndürmelisin. JSON dışında hiçbir açıklama veya markdown biçimlendirmesi ekleme.";

        var requestBody = new
        {
            model = model,
            max_tokens = 1024,
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        // HTTP isteğini hazırlıyoruz
        var requestUrl = "https://api.anthropic.com/v1/messages";
        Console.WriteLine($"Claude API URL: {requestUrl}");

        using var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
        request.Headers.Add("x-api-key", apiKey);
        request.Headers.Add("anthropic-version", "2023-06-01");
        request.Content = JsonContent.Create(requestBody);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // HTTP POST isteği gönderiyoruz
        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Claude API Hata: {errorContent}");
            throw new HttpRequestException($"Claude API hatası: {errorContent}");
        }

        response.EnsureSuccessStatusCode();

        var claudeResponse = await response.Content.ReadFromJsonAsync<ClaudeResponse>();
        var jsonText = claudeResponse?.Content?.FirstOrDefault()?.Text;

        if (string.IsNullOrWhiteSpace(jsonText))
        {
            throw new InvalidOperationException("Claude API boş veya geçersiz bir yanıt döndürdü.");
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Gelen JSON metnini DTO nesnesine dönüştürüyoruz
        var result = JsonSerializer.Deserialize<ProblemAnalysisResultDto>(jsonText, options);

        if (result is null)
        {
            throw new InvalidOperationException("Claude yanıtı beklenen şemaya göre çözümlenemedi.");
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

    // Anthropic Claude API yanıt yapısı için dahili sınıflar
    private class ClaudeResponse
    {
        [JsonPropertyName("content")]
        public ContentItem[]? Content { get; set; }
    }

    private class ContentItem
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

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

    public Task<List<Competitor>> AnalyzeCompetitorsAsync(string title, string description, string? sector)
    {
        throw new NotImplementedException();
    }

    public Task<InvestorBriefResult> GenerateInvestorBriefAsync(string title, string description, string? sector, string? solutionType, string? revenueModel, double? opportunityScore)
    {
        throw new NotImplementedException();
    }

    public Task<List<Grant>> FindGrantsAsync(string title, string description, string? sector)
    {
        throw new NotImplementedException();
    }

    public Task<MarketSizeResult> CalculateMarketSizeAsync(string title, string description, string? sector)
    {
        throw new NotImplementedException();
    }

    public Task<BusinessCanvasResult> GenerateBusinessCanvasAsync(string title, string description, string? sector, string? solutionType)
    {
        throw new NotImplementedException();
    }

    public Task<List<ChecklistItemSuggestion>> GenerateStartupChecklistAsync(string title, string description, string? sector)
    {
        throw new NotImplementedException();
    }

    public Task<List<TeamRole>> AnalyzeTeamNeedsAsync(string title, string description, string? sector, string? solutionType)
    {
        throw new NotImplementedException();
    }

    public Task<List<RelatedProblem>> FindRelatedProblemsAsync(string title, string description, List<string> allProblemTitles)
    {
        throw new NotImplementedException();
    }

    public Task<PricingStrategyResult> GeneratePricingStrategyAsync(string title, string description, string? sector, string? solutionType, string? targetCustomer)
    {
        throw new NotImplementedException();
    }

    public Task<List<RiskItemSuggestion>> GenerateRiskMatrixAsync(string title, string description, string? sector)
    {
        throw new NotImplementedException();
    }

    public Task<List<CustomerPersona>> GenerateCustomerPersonaAsync(string title, string description, string? sector)
    {
        throw new NotImplementedException();
    }

    public Task<ActionPlanResult> GenerateActionPlanAsync(string title, string description, string? sector, string? solutionType)
    {
        throw new NotImplementedException();
    }

    public Task<List<SimilarProblemResult>> FindSimilarProblemsAsync(string query, List<Problem> problems)
    {
        throw new NotImplementedException();
    }

    public Task<PatentAnalysisResult> AnalyzePatentsAsync(string title, string description, string? sector)
    {
        throw new NotImplementedException();
    }
}
