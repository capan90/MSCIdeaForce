using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using MSC.IdeaForge.Domain.Interfaces;
using MSC.IdeaForge.Domain.ValueObjects;

namespace MSC.IdeaForge.Infrastructure.AI;

/// <summary>
/// Gemini API kullanarak problemleri analiz eden sağlayıcı sınıf.
/// </summary>
public class GeminiAIProvider : IAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public GeminiAIProvider(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    /// <summary>
    /// Problemi Gemini API'sine göndererek JSON formatında analiz sonuçlarını alır.
    /// </summary>
    public async Task<ProblemAnalysisResult> AnalyzeProblemAsync(string title, string description, string? sector)
    {
        var apiKey = _configuration["AI:Gemini:ApiKey"];
        var model = _configuration["AI:Gemini:Model"] ?? "gemini-2.5-flash";

        // API Key kontrolü
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Gemini API Key bulunamadı. Lütfen appsettings.json dosyasındaki 'AI:Gemini:ApiKey' alanını güncelleyin.");
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
            }
        };

        // HTTP POST isteği gönderiyoruz
        var requestUrl = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";
        Console.WriteLine($"Gemini API URL: {requestUrl}");

        var response = await _httpClient.PostAsJsonAsync(
            requestUrl,
            requestBody
        );
        response.EnsureSuccessStatusCode();

        var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>();
        var jsonText = geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

        if (string.IsNullOrWhiteSpace(jsonText))
        {
            throw new InvalidOperationException("Gemini API boş veya geçersiz bir yanıt döndürdü.");
        }

        // Eğer yapay zeka çıktıyı markdown kod bloğu olarak döndürdüyse temizliyoruz
        if (jsonText.StartsWith("```json"))
        {
            jsonText = jsonText.Substring(7);
        }
        else if (jsonText.StartsWith("```"))
        {
            jsonText = jsonText.Substring(3);
        }

        if (jsonText.EndsWith("```"))
        {
            jsonText = jsonText.Substring(0, jsonText.Length - 3);
        }

        jsonText = jsonText.Trim();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Gelen JSON metnini DTO nesnesine dönüştürüyoruz
        var result = JsonSerializer.Deserialize<ProblemAnalysisResultDto>(jsonText, options);

        if (result is null)
        {
            throw new InvalidOperationException("Gemini yanıtı beklenen şemaya göre çözümlenemedi.");
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

    /// <summary>
    /// Problemi fırsat skorlama kriterlerine göre analiz edip her kriter için 1-10 puan ve gerekçe üretir.
    /// </summary>
    public async Task<OpportunityScoreResult> ScoreOpportunityAsync(string title, string description, string? sector)
    {
        var apiKey = _configuration["AI:Gemini:ApiKey"];
        var model = _configuration["AI:Gemini:Model"] ?? "gemini-2.5-flash";

        // API Key kontrolü
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Gemini API Key bulunamadı. Lütfen appsettings.json dosyasındaki 'AI:Gemini:ApiKey' alanını güncelleyin.");
        }

        // Gemini promptunu hazırlıyoruz
        var prompt = $@"Aşağıdaki problemi fırsat skorlama kriterlerine göre analiz et. Yanıtı mutlaka JSON formatında döndür. Her bir kriter için 1 ile 10 arasında bir puan (int) ve Türkçe bir gerekçe (Reason) üret.

Kriterler:
1. ProblemSeverity: Problemin şiddeti / aciliyeti nedir?
2. MarketSize: Pazar büyüklüğü nedir? Bu problemi yaşayan kişi sayısı ne kadar fazla?
3. FounderFit: Kurucu ekibin bu probleme uyumu nedir? (Bizim yapabilme kapasitemiz/kabiliyetimiz)
4. RiskScore: Risk skoru nedir? (Burada 1 en riskli/kötü, 10 en az riskli/en iyi.)
5. TechnicalFeasibility: Teknik uygulanabilirlik / yapılabilirlik nedir? (10 yapması en kolay/en fizibil, 1 yapması en zor olan)

Problem Detayları:
Başlık: {title}
Açıklama: {description}
Sektör: {sector ?? "Belirtilmemiş"}

Yanıt şeması tam olarak şu şekilde olmalıdır:
{{
  ""ProblemSeverity"": 8,
  ""ProblemSeverityReason"": ""gerekçe"",
  ""MarketSize"": 7,
  ""MarketSizeReason"": ""gerekçe"",
  ""FounderFit"": 6,
  ""FounderFitReason"": ""gerekçe"",
  ""RiskScore"": 5,
  ""RiskScoreReason"": ""gerekçe"",
  ""TechnicalFeasibility"": 8,
  ""TechnicalFeasibilityReason"": ""gerekçe""
}}

Sen profesyonel bir problem analisti yapay zekasın. Çıktı olarak sadece ve sadece saf, geçerli bir JSON döndürmelisin. JSON dışında hiçbir açıklama veya markdown biçimlendirmesi ekleme.";

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
            }
        };

        // HTTP POST isteği gönderiyoruz
        var requestUrl = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";
        Console.WriteLine($"Gemini API URL: {requestUrl}");

        var response = await _httpClient.PostAsJsonAsync(
            requestUrl,
            requestBody
        );
        response.EnsureSuccessStatusCode();

        var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>();
        var jsonText = geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

        if (string.IsNullOrWhiteSpace(jsonText))
        {
            throw new InvalidOperationException("Gemini API boş veya geçersiz bir yanıt döndürdü.");
        }

        // Eğer yapay zeka çıktıyı markdown kod bloğu olarak döndürdüyse temizliyoruz
        if (jsonText.StartsWith("```json"))
        {
            jsonText = jsonText.Substring(7);
        }
        else if (jsonText.StartsWith("```"))
        {
            jsonText = jsonText.Substring(3);
        }

        if (jsonText.EndsWith("```"))
        {
            jsonText = jsonText.Substring(0, jsonText.Length - 3);
        }

        jsonText = jsonText.Trim();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Gelen JSON metnini DTO nesnesine dönüştürüyoruz
        var result = JsonSerializer.Deserialize<OpportunityScoreResultDto>(jsonText, options);

        if (result is null)
        {
            throw new InvalidOperationException("Gemini yanıtı beklenen şemaya göre çözümlenemedi.");
        }

        return new OpportunityScoreResult(
            result.ProblemSeverity,
            result.ProblemSeverityReason ?? string.Empty,
            result.MarketSize,
            result.MarketSizeReason ?? string.Empty,
            result.FounderFit,
            result.FounderFitReason ?? string.Empty,
            result.RiskScore,
            result.RiskScoreReason ?? string.Empty,
            result.TechnicalFeasibility,
            result.TechnicalFeasibilityReason ?? string.Empty
        );
    }

    private class OpportunityScoreResultDto
    {
        public int ProblemSeverity { get; set; }
        public string? ProblemSeverityReason { get; set; }
        public int MarketSize { get; set; }
        public string? MarketSizeReason { get; set; }
        public int FounderFit { get; set; }
        public string? FounderFitReason { get; set; }
        public int RiskScore { get; set; }
        public string? RiskScoreReason { get; set; }
        public int TechnicalFeasibility { get; set; }
        public string? TechnicalFeasibilityReason { get; set; }
    }
}
