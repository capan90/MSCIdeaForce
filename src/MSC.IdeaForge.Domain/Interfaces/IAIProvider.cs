using MSC.IdeaForge.Domain.ValueObjects;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Yapay Zeka analiz hizmetleri için arayüz.
/// </summary>
public interface IAIProvider
{
    /// <summary>
    /// Verilen problem detaylarını analiz eder.
    /// </summary>
    /// <param name="title">Problem başlığı</param>
    /// <param name="description">Problem açıklaması</param>
    /// <param name="sector">İlgili sektör</param>
    /// <returns>Analiz sonuçlarını içeren ProblemAnalysisResult nesnesini döner.</returns>
    Task<ProblemAnalysisResult> AnalyzeProblemAsync(string title, string description, string? sector);

    /// <summary>
    /// Verilen problem detaylarını fırsat skoru kriterlerine göre analiz edip puanlar.
    /// Kurucu profili bağlamı (founderContext) verilirse Founder Fit skoru bu bilgiye göre daha isabetli hesaplanır.
    /// </summary>
    Task<OpportunityScoreResult> ScoreOpportunityAsync(string title, string description, string? sector, string? founderContext = null);

    /// <summary>
    /// Verilen problem hakkında pazar, rakip ve teknoloji araştırması yapar.
    /// </summary>
    Task<ResearchResult> ResearchProblemAsync(string title, string description, string? sector);

    /// <summary>
    /// Verilen problem için olası 3 farklı çözüm önerisi sunar.
    /// </summary>
    Task<List<SolutionSuggestion>> SuggestSolutionsAsync(string title, string description, string? sector);

    /// <summary>
    /// Problem ve çözüm türüne göre MVP planı oluşturur.
    /// </summary>
    Task<MVPPlanResult> GenerateMVPPlanAsync(string title, string description, string? sector, string? solutionType);

    /// <summary>
    /// Verilen sektör hakkında trend analizi yapar.
    /// </summary>
    /// <param name="sector">Analiz edilecek sektör adı</param>
    /// <returns>Sektör analiz sonuçlarını içeren SectorAnalysisResult nesnesini döner.</returns>
    Task<SectorAnalysisResult> AnalyzeSectorAsync(string sector);

    /// <summary>
    /// Verilen trend başlığı hakkında açıklama, fırsat alanları ve önerilen aksiyonları içeren analiz yapar.
    /// </summary>
    /// <param name="trend">Analiz edilecek trend başlığı (Örn: AI, LLM, MCP)</param>
    /// <returns>Trend analiz sonuçlarını içeren TrendAnalysisResult nesnesini döner.</returns>
    Task<TrendAnalysisResult> AnalyzeTrendAsync(string trend);

    /// <summary>
    /// Verilen problem için gelir modeli analizi yapar.
    /// </summary>
    Task<RevenueAnalysisResult> AnalyzeRevenueAsync(string title, string description, string? sector, string? solutionType);

    /// <summary>
    /// Verilen problem için sorulması gereken doğrulama (validation) sorularını üretir.
    /// </summary>
    /// <returns>Doğrulama sorularını içeren metin listesi döner.</returns>
    Task<List<string>> GenerateValidationQuestionsAsync(string title, string description);

    /// <summary>
    /// Verilen problem için detaylı rakip analizi yapar.
    /// </summary>
    /// <returns>Her rakibi ve rekabet avantajlarını içeren Competitor listesi döner.</returns>
    Task<List<Competitor>> AnalyzeCompetitorsAsync(string title, string description, string? sector);

    /// <summary>
    /// Verilen problem için yatırımcıya sunulacak kısa bir briefing üretir.
    /// </summary>
    Task<InvestorBriefResult> GenerateInvestorBriefAsync(string title, string description, string? sector, string? solutionType, string? revenueModel, double? opportunityScore);

    /// <summary>
    /// Verilen problem/proje için Türkiye'deki uygun hibe, destek ve teşvik programlarını bulur.
    /// </summary>
    Task<List<Grant>> FindGrantsAsync(string title, string description, string? sector);

    /// <summary>
    /// Verilen problem için pazar büyüklüğünü (TAM/SAM/SOM) hesaplar.
    /// </summary>
    Task<MarketSizeResult> CalculateMarketSizeAsync(string title, string description, string? sector);

    /// <summary>
    /// Verilen problem için Business Model Canvas 9 bloğunu üretir.
    /// </summary>
    Task<BusinessCanvasResult> GenerateBusinessCanvasAsync(string title, string description, string? sector, string? solutionType);
}
