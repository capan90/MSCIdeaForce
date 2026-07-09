using System;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Bir probleme ait yapay zeka rakip analizi sonuçlarını temsil eden entity.
/// Rakip listesi <see cref="MSC.IdeaForge.Domain.ValueObjects.Competitor"/> nesnelerinin JSON serileştirilmiş hali olarak saklanır.
/// </summary>
public class CompetitorAnalysis : BaseEntity
{
    public Guid ProblemId { get; private set; }
    public string CompetitorsJson { get; private set; } = "[]";
    public DateTime AnalyzedAt { get; private set; }

    // EF Core için gizli varsayılan yapıcı metot
    private CompetitorAnalysis() { }

    /// <summary>
    /// Yeni bir rakip analizi kaydı oluşturur.
    /// </summary>
    public static CompetitorAnalysis Create(Guid problemId, string competitorsJson)
    {
        if (problemId == Guid.Empty)
        {
            throw new ArgumentException("Problem ID boş olamaz.", nameof(problemId));
        }

        return new CompetitorAnalysis
        {
            ProblemId = problemId,
            CompetitorsJson = string.IsNullOrWhiteSpace(competitorsJson) ? "[]" : competitorsJson,
            AnalyzedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Mevcut rakip analizi kaydını günceller.
    /// </summary>
    public void Update(string competitorsJson)
    {
        CompetitorsJson = string.IsNullOrWhiteSpace(competitorsJson) ? "[]" : competitorsJson;
        AnalyzedAt = DateTime.UtcNow;
        SetUpdated();
    }
}
