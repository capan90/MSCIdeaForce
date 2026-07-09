using System;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Bir probleme ait yapay zeka gelir modeli analizi sonuçlarını temsil eden entity.
/// </summary>
public class RevenueAnalysis : BaseEntity
{
    public Guid ProblemId { get; private set; }
    public string RevenueModel { get; private set; } = null!;
    public string MonthlyRevenueEstimate { get; private set; } = null!;
    public string AnnualRevenueEstimate { get; private set; } = null!;
    public string PricingStrategy { get; private set; } = null!;
    public string TargetCustomer { get; private set; } = null!;
    public string SalesChannel { get; private set; } = null!;
    public string Scalability { get; private set; } = null!;
    public string Risks { get; private set; } = null!;
    public DateTime AnalyzedAt { get; private set; }

    // EF Core için gizli varsayılan yapıcı metot
    private RevenueAnalysis() { }

    /// <summary>
    /// Yeni bir gelir analizi kaydı oluşturur.
    /// </summary>
    public static RevenueAnalysis Create(
        Guid problemId,
        string revenueModel,
        string monthlyRevenueEstimate,
        string annualRevenueEstimate,
        string pricingStrategy,
        string targetCustomer,
        string salesChannel,
        string scalability,
        string risks)
    {
        if (problemId == Guid.Empty)
        {
            throw new ArgumentException("Problem ID boş olamaz.", nameof(problemId));
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(revenueModel);

        return new RevenueAnalysis
        {
            ProblemId = problemId,
            RevenueModel = revenueModel,
            MonthlyRevenueEstimate = monthlyRevenueEstimate ?? string.Empty,
            AnnualRevenueEstimate = annualRevenueEstimate ?? string.Empty,
            PricingStrategy = pricingStrategy ?? string.Empty,
            TargetCustomer = targetCustomer ?? string.Empty,
            SalesChannel = salesChannel ?? string.Empty,
            Scalability = scalability ?? string.Empty,
            Risks = risks ?? string.Empty,
            AnalyzedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Mevcut gelir analizi kaydını günceller.
    /// </summary>
    public void Update(
        string revenueModel,
        string monthlyRevenueEstimate,
        string annualRevenueEstimate,
        string pricingStrategy,
        string targetCustomer,
        string salesChannel,
        string scalability,
        string risks)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(revenueModel);

        RevenueModel = revenueModel;
        MonthlyRevenueEstimate = monthlyRevenueEstimate ?? string.Empty;
        AnnualRevenueEstimate = annualRevenueEstimate ?? string.Empty;
        PricingStrategy = pricingStrategy ?? string.Empty;
        TargetCustomer = targetCustomer ?? string.Empty;
        SalesChannel = salesChannel ?? string.Empty;
        Scalability = scalability ?? string.Empty;
        Risks = risks ?? string.Empty;
        AnalyzedAt = DateTime.UtcNow;
        SetUpdated();
    }
}
