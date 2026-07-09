using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using MSC.IdeaForge.Domain.Interfaces;
using MSC.IdeaForge.Domain.ValueObjects;
using MSC.IdeaForge.Domain.Enums;

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

    /// <summary>
    /// Verilen problem hakkında detaylı pazar, rakip ve teknoloji araştırması yapar.
    /// </summary>
    public async Task<ResearchResult> ResearchProblemAsync(string title, string description, string? sector)
    {
        var apiKey = _configuration["AI:Gemini:ApiKey"];
        var model = _configuration["AI:Gemini:Model"] ?? "gemini-2.5-flash";

        // API Key kontrolü
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Gemini API Key bulunamadı. Lütfen appsettings.json dosyasındaki 'AI:Gemini:ApiKey' alanını güncelleyin.");
        }

        // Gemini promptunu hazırlıyoruz
        var prompt = $@"Aşağıdaki problemi araştır, rakipleri bul, pazar analizi yap, teknoloji notları yaz, trend notları çıkar ve kaynakları belirt. Yanıtı mutlaka JSON formatında döndür. JSON formatı şu alanları içermelidir:
- MarketAnalysis (Pazar analizi detayları, Türkçe)
- CompetitorSummary (Rakip analizi özeti, Türkçe)
- TechnologyNotes (Kullanılabilecek teknolojiler ve mimari notları, Türkçe)
- TrendNotes (Sektörel trendler ve gelecek projeksiyonları, Türkçe)
- Sources (Araştırma kaynakları veya referans olabilecek linkler/dokümanlar, Türkçe)

Problem Detayları:
Başlık: {title}
Açıklama: {description}
Sektör: {sector ?? "Belirtilmemiş"}

Yanıt şeması tam olarak şu şekilde olmalıdır:
{{
  ""MarketAnalysis"": ""pazar analizi detayları"",
  ""CompetitorSummary"": ""rakip analizi özeti"",
  ""TechnologyNotes"": ""teknoloji notları"",
  ""TrendNotes"": ""trend notları"",
  ""Sources"": ""kaynaklar""
}}

Sen profesyonel bir problem analisti ve pazar araştırmacısı yapay zekasın. Çıktı olarak sadece ve sadece saf, geçerli bir JSON döndürmelisin. JSON dışında hiçbir açıklama veya markdown biçimlendirmesi ekleme.";

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

        var result = JsonSerializer.Deserialize<ResearchResult>(jsonText, options);

        if (result is null)
        {
            throw new InvalidOperationException("Gemini yanıtı beklenen şemaya göre çözümlenemedi.");
        }

        return result;
    }

    /// <summary>
    /// Probleme yönelik olası 3 farklı çözüm önerisi sunar.
    /// </summary>
    public async Task<List<SolutionSuggestion>> SuggestSolutionsAsync(string title, string description, string? sector)
    {
        var apiKey = _configuration["AI:Gemini:ApiKey"];
        var model = _configuration["AI:Gemini:Model"] ?? "gemini-2.5-flash";

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Gemini API Key bulunamadı. Lütfen appsettings.json dosyasındaki 'AI:Gemini:ApiKey' alanını güncelleyin.");
        }

        // Gemini promptunu hazırlıyoruz
        var prompt = $@"Aşağıdaki problem için olası 3 farklı alternatif çözüm yöntemi öner. Yanıtı mutlaka JSON formatında döndür.
Yanıt, bir JSON array olmalıdır. Her eleman şu alanları içermelidir:
- SolutionType (Şu değerlerden biri olmalıdır: WebApp, MobileApp, DesktopApp, SaaS, AIAgent, BrowserExtension, APIService, ERPModule, IoT, PhysicalProduct, ProcessImprovement, Education, Consulting, Hybrid)
- Summary (Çözümün Türkçe kısa özeti)
- Complexity (Şu değerlerden biri olmalıdır: Low, Medium, High, VeryHigh)
- EstimatedDurationMonths (Tahmini yapım süresi, ay bazında tamsayı int)
- EstimatedCost (Tahmini maliyet seviyesi / açıklaması, Türkçe)
- Pros (Çözümün avantajları / artıları, Türkçe)
- Cons (Çözümün dezavantajları / eksileri, Türkçe)
- IsRecommended (Önerilen ana çözüm ise true, diğer alternatifler için false)

Problem Detayları:
Başlık: {title}
Açıklama: {description}
Sektör: {sector ?? "Belirtilmemiş"}

Yanıt şeması tam olarak şu şekilde olmalıdır:
[
  {{
    ""SolutionType"": ""WebApp"",
    ""Summary"": ""Çözüm özeti..."",
    ""Complexity"": ""Medium"",
    ""EstimatedDurationMonths"": 4,
    ""EstimatedCost"": ""Düşük bütçeli sunucu maliyetleri"",
    ""Pros"": ""Hızlı canlıya alma imkanı"",
    ""Cons"": ""Çevrimdışı çalışma desteği olmaması"",
    ""IsRecommended"": true
  }}
]

Sen profesyonel bir ürün yöneticisi yapay zekasın. Çıktı olarak sadece ve sadece saf, geçerli bir JSON array döndürmelisin. JSON dışında hiçbir açıklama veya markdown biçimlendirmesi ekleme.";

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

        var resultDto = JsonSerializer.Deserialize<List<SolutionSuggestionDto>>(jsonText, options);

        if (resultDto is null)
        {
            throw new InvalidOperationException("Gemini yanıtı beklenen şemaya göre çözümlenemedi.");
        }

        var suggestions = new List<SolutionSuggestion>();
        foreach (var dto in resultDto)
        {
            if (!Enum.TryParse<SolutionType>(dto.SolutionType, true, out var type))
            {
                type = SolutionType.Hybrid;
            }

            if (!Enum.TryParse<ComplexityLevel>(dto.Complexity, true, out var comp))
            {
                comp = ComplexityLevel.Medium;
            }

            suggestions.Add(new SolutionSuggestion
            {
                SolutionType = type,
                Summary = dto.Summary ?? string.Empty,
                Complexity = comp,
                EstimatedDurationMonths = dto.EstimatedDurationMonths,
                EstimatedCost = dto.EstimatedCost,
                Pros = dto.Pros,
                Cons = dto.Cons,
                IsRecommended = dto.IsRecommended
            });
        }

        return suggestions;
    }

    /// <summary>
    /// Problem ve çözüm türüne göre detaylı bir MVP planı üretir.
    /// </summary>
    public async Task<MVPPlanResult> GenerateMVPPlanAsync(string title, string description, string? sector, string? solutionType)
    {
        var apiKey = _configuration["AI:Gemini:ApiKey"];
        var model = _configuration["AI:Gemini:Model"] ?? "gemini-2.5-flash";

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Gemini API Key bulunamadı. Lütfen appsettings.json dosyasındaki 'AI:Gemini:ApiKey' alanını güncelleyin.");
        }

        // Gemini promptunu hazırlıyoruz
        var prompt = $@"Aşağıdaki problem ve önerilen çözüm tipine göre detaylı bir MVP (Minimum Viable Product) planı oluştur. Yanıtı mutlaka JSON formatında döndür.
JSON formatı şu alanları içermelidir:
- Scope (MVP kapsamı özeti, Türkçe)
- UserStories (Kullanıcı hikayeleri listesi, en az 5 adet, Türkçe, JSON string dizisi şeklinde)
- SprintPlan (3 sprintlik plan, her sprint için hedefler ve teslimatlar, en az 3 elemanlı Türkçe JSON string dizisi şeklinde)
- TechStack (Önerilen teknolojiler, Türkçe)
- Risks (MVP sürecindeki potansiyel riskler, Türkçe)
- FirstSalesPlan (İlk satış / pazara giriş planı, Türkçe)

Problem Detayları:
Başlık: {title}
Açıklama: {description}
Sektör: {sector ?? "Belirtilmemiş"}
Önerilen Çözüm Türü: {solutionType ?? "Belirtilmemiş"}

Yanıt şeması tam olarak şu şekilde olmalıdır:
{{
  ""Scope"": ""kapsam detayı"",
  ""UserStories"": [
    ""Kullanıcı olarak şunu yapmak istiyorum böylece şu faydayı sağlarım..."",
    ""Kullanıcı olarak...""
  ],
  ""SprintPlan"": [
    ""Sprint 1: Altyapının kurulması ve kullanıcı girişi..."",
    ""Sprint 2: ..."",
    ""Sprint 3: ...""
  ],
  ""TechStack"": ""React, .NET, PostgreSQL"",
  ""Risks"": ""Pazar entegrasyon riskleri..."",
  ""FirstSalesPlan"": ""İlk 10 müşteriye ücretsiz beta sunulması...""
}}

Sen profesyonel bir ürün müdürü yapay zekasın. Çıktı olarak sadece ve sadece saf, geçerli bir JSON döndürmelisin. JSON dışında hiçbir açıklama veya markdown biçimlendirmesi ekleme.";

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

        var result = JsonSerializer.Deserialize<MVPPlanResult>(jsonText, options);

        if (result is null)
        {
            throw new InvalidOperationException("Gemini yanıtı beklenen şemaya göre çözümlenemedi.");
        }

        return result;
    }

    /// <summary>
    /// Verilen sektör hakkında detaylı trend analizi yapar.
    /// </summary>
    public async Task<SectorAnalysisResult> AnalyzeSectorAsync(string sector)
    {
        var apiKey = _configuration["AI:Gemini:ApiKey"];
        var model = _configuration["AI:Gemini:Model"] ?? "gemini-2.5-flash";

        // API Key kontrolü
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Gemini API Key bulunamadı. Lütfen appsettings.json dosyasındaki 'AI:Gemini:ApiKey' alanını güncelleyin.");
        }

        // Gemini promptunu hazırlıyoruz
        var prompt = $@"Aşağıdaki sektör hakkında detaylı bir trend analizi yap. Yanıtı mutlaka JSON formatında döndür. JSON formatı şu alanları içermelidir:
- TrendSummary (Sektör trend özeti, Türkçe)
- Opportunities (Sektördeki fırsatlar, Türkçe)
- Threats (Sektördeki tehditler, Türkçe)
- GrowthPotential (Büyüme potansiyeli değerlendirmesi, Türkçe)
- KeyPlayers (Sektördeki ana oyuncular / önemli firmalar, Türkçe)
- Recommendation (Genel öneriler ve stratejik tavsiyeler, Türkçe)

Sektör: {sector}

Yanıt şeması tam olarak şu şekilde olmalıdır:
{{
  ""TrendSummary"": ""sektör trend özeti"",
  ""Opportunities"": ""fırsatlar"",
  ""Threats"": ""tehditler"",
  ""GrowthPotential"": ""büyüme potansiyeli"",
  ""KeyPlayers"": ""ana oyuncular"",
  ""Recommendation"": ""genel öneriler""
}}

Sen profesyonel bir sektör analisti yapay zekasın. Çıktı olarak sadece ve sadece saf, geçerli bir JSON döndürmelisin. JSON dışında hiçbir açıklama veya markdown biçimlendirmesi ekleme.";

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

        var result = JsonSerializer.Deserialize<SectorAnalysisResult>(jsonText, options);

        if (result is null)
        {
            throw new InvalidOperationException("Gemini yanıtı beklenen şemaya göre çözümlenemedi.");
        }

        return result;
    }

    /// <summary>
    /// Verilen trend başlığı hakkında açıklama, fırsat alanları ve önerilen aksiyonları içeren analiz yapar.
    /// </summary>
    public async Task<TrendAnalysisResult> AnalyzeTrendAsync(string trend)
    {
        var apiKey = _configuration["AI:Gemini:ApiKey"];
        var model = _configuration["AI:Gemini:Model"] ?? "gemini-2.5-flash";

        // API Key kontrolü
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Gemini API Key bulunamadı. Lütfen appsettings.json dosyasındaki 'AI:Gemini:ApiKey' alanını güncelleyin.");
        }

        // Gemini promptunu hazırlıyoruz
        var prompt = $@"Aşağıdaki teknoloji/iş trendi hakkında güncel bir analiz yap. Yanıtı mutlaka JSON formatında döndür. JSON formatı şu alanları içermelidir:
- Summary (Trendin ne olduğu ve neden önemli olduğuna dair Türkçe açıklama)
- Opportunities (Bu trendin açtığı girişim / ürün fırsat alanları, Türkçe)
- Actions (Bu trendi değerlendirmek için önerilen somut aksiyonlar, Türkçe)

Trend: {trend}

Yanıt şeması tam olarak şu şekilde olmalıdır:
{{
  ""Summary"": ""trend açıklaması"",
  ""Opportunities"": ""fırsat alanları"",
  ""Actions"": ""önerilen aksiyonlar""
}}

Sen profesyonel bir trend analisti yapay zekasın. Çıktı olarak sadece ve sadece saf, geçerli bir JSON döndürmelisin. JSON dışında hiçbir açıklama veya markdown biçimlendirmesi ekleme.";

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

        jsonText = CleanJsonText(jsonText);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<TrendAnalysisResult>(jsonText, options);

        if (result is null)
        {
            throw new InvalidOperationException("Gemini yanıtı beklenen şemaya göre çözümlenemedi.");
        }

        return result;
    }

    /// <summary>
    /// Verilen problem için gelir modeli analizi yapar.
    /// </summary>
    public async Task<RevenueAnalysisResult> AnalyzeRevenueAsync(string title, string description, string? sector, string? solutionType)
    {
        var apiKey = _configuration["AI:Gemini:ApiKey"];
        var model = _configuration["AI:Gemini:Model"] ?? "gemini-2.5-flash";

        // API Key kontrolü
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Gemini API Key bulunamadı. Lütfen appsettings.json dosyasındaki 'AI:Gemini:ApiKey' alanını güncelleyin.");
        }

        // Gemini promptunu hazırlıyoruz
        var prompt = $@"Aşağıdaki problem/ürün fikri için detaylı bir gelir modeli analizi yap. Yanıtı mutlaka JSON formatında döndür. JSON formatı şu alanları içermelidir:
- RevenueModel (Şu değerlerden en uygun olanı: SaaS, Enterprise, Lisans, Danışmanlık, Marketplace, Abonelik, Donanım, API, Hibrit)
- MonthlyRevenueEstimate (Tahmini aylık gelir aralığı, Türkçe string)
- AnnualRevenueEstimate (Tahmini yıllık gelir aralığı, Türkçe string)
- PricingStrategy (Önerilen fiyatlandırma stratejisi, Türkçe)
- TargetCustomer (Hedef müşteri segmenti, Türkçe)
- SalesChannel (Önerilen satış kanalları, Türkçe)
- Scalability (Ölçeklenebilirlik değerlendirmesi, Türkçe)
- Risks (Gelir modeline dair riskler, Türkçe)

Problem Detayları:
Başlık: {title}
Açıklama: {description}
Sektör: {sector ?? "Belirtilmemiş"}
Çözüm Tipi: {solutionType ?? "Belirtilmemiş"}

Yanıt şeması tam olarak şu şekilde olmalıdır:
{{
  ""RevenueModel"": ""SaaS"",
  ""MonthlyRevenueEstimate"": ""aylık gelir tahmini"",
  ""AnnualRevenueEstimate"": ""yıllık gelir tahmini"",
  ""PricingStrategy"": ""fiyatlandırma stratejisi"",
  ""TargetCustomer"": ""hedef müşteri"",
  ""SalesChannel"": ""satış kanalları"",
  ""Scalability"": ""ölçeklenebilirlik"",
  ""Risks"": ""riskler""
}}

Sen profesyonel bir iş modeli ve gelir stratejisi analisti yapay zekasın. Çıktı olarak sadece ve sadece saf, geçerli bir JSON döndürmelisin. JSON dışında hiçbir açıklama veya markdown biçimlendirmesi ekleme.";

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

        jsonText = CleanJsonText(jsonText);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<RevenueAnalysisResult>(jsonText, options);

        if (result is null)
        {
            throw new InvalidOperationException("Gemini yanıtı beklenen şemaya göre çözümlenemedi.");
        }

        return result;
    }

    /// <summary>
    /// Verilen problem için sorulması gereken 5 doğrulama sorusunu üretir.
    /// </summary>
    public async Task<List<string>> GenerateValidationQuestionsAsync(string title, string description)
    {
        var apiKey = _configuration["AI:Gemini:ApiKey"];
        var model = _configuration["AI:Gemini:Model"] ?? "gemini-2.5-flash";

        // API Key kontrolü
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Gemini API Key bulunamadı. Lütfen appsettings.json dosyasındaki 'AI:Gemini:ApiKey' alanını güncelleyin.");
        }

        // Gemini promptunu hazırlıyoruz
        var prompt = $@"Aşağıdaki problem/ürün fikrinin gerçek olup olmadığını doğrulamak (validation) için potansiyel müşterilere sorulması gereken 5 kritik soru üret.
Yanıtı mutlaka JSON formatında döndür. Yanıt, sadece Türkçe soru metinlerinden oluşan bir JSON array (string dizisi) olmalıdır.

Problem Detayları:
Başlık: {title}
Açıklama: {description}

Yanıt şeması tam olarak şu şekilde olmalıdır:
[
  ""Birinci doğrulama sorusu?"",
  ""İkinci doğrulama sorusu?""
]

Sen profesyonel bir müşteri geliştirme (customer development) uzmanı yapay zekasın. Çıktı olarak sadece ve sadece saf, geçerli bir JSON array döndürmelisin. JSON dışında hiçbir açıklama veya markdown biçimlendirmesi ekleme.";

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

        jsonText = CleanJsonText(jsonText);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<List<string>>(jsonText, options);

        if (result is null)
        {
            throw new InvalidOperationException("Gemini yanıtı beklenen şemaya göre çözümlenemedi.");
        }

        return result;
    }

    /// <summary>
    /// Yapay zeka çıktısını markdown kod bloğu işaretlerinden temizleyip saf JSON haline getirir.
    /// </summary>
    private static string CleanJsonText(string jsonText)
    {
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

        return jsonText.Trim();
    }

    private class SolutionSuggestionDto
    {
        public string? SolutionType { get; set; }
        public string? Summary { get; set; }
        public string? Complexity { get; set; }
        public int EstimatedDurationMonths { get; set; }
        public string? EstimatedCost { get; set; }
        public string? Pros { get; set; }
        public string? Cons { get; set; }
        public bool IsRecommended { get; set; }
    }
}
