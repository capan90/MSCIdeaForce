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
}
