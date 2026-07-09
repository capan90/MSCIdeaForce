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
    /// </summary>
    Task<OpportunityScoreResult> ScoreOpportunityAsync(string title, string description, string? sector);

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
}
