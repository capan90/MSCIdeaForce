using System;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Takip edilen bir trend başlığına ait yapay zeka trend analizi sonuçlarını temsil eden entity.
/// </summary>
public class TrendAnalysis : BaseEntity
{
    public string TrendName { get; private set; } = null!;
    public string Summary { get; private set; } = null!;
    public string Opportunities { get; private set; } = null!;
    public string Actions { get; private set; } = null!;
    public DateTime AnalyzedAt { get; private set; }

    // EF Core için gizli varsayılan yapıcı metot
    private TrendAnalysis() { }

    /// <summary>
    /// Yeni bir trend analizi kaydı oluşturur.
    /// </summary>
    public static TrendAnalysis Create(
        string trendName,
        string summary,
        string opportunities,
        string actions)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(trendName);
        ArgumentException.ThrowIfNullOrWhiteSpace(summary);
        ArgumentException.ThrowIfNullOrWhiteSpace(opportunities);
        ArgumentException.ThrowIfNullOrWhiteSpace(actions);

        return new TrendAnalysis
        {
            TrendName = trendName,
            Summary = summary,
            Opportunities = opportunities,
            Actions = actions,
            AnalyzedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Mevcut trend analizi kaydını günceller.
    /// </summary>
    public void Update(string summary, string opportunities, string actions)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(summary);
        ArgumentException.ThrowIfNullOrWhiteSpace(opportunities);
        ArgumentException.ThrowIfNullOrWhiteSpace(actions);

        Summary = summary;
        Opportunities = opportunities;
        Actions = actions;
        AnalyzedAt = DateTime.UtcNow;
        SetUpdated();
    }
}
