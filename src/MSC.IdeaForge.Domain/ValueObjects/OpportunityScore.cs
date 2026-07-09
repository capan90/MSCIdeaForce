namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Fırsat puanlaması kriterlerini ve ağırlıklı toplam skoru barındıran değer nesnesi (Value Object).
/// </summary>
public record OpportunityScore
{
    public int ProblemSeverity { get; private set; }
    public int MarketSize { get; private set; }
    public int FounderFit { get; private set; }
    public int RiskScore { get; private set; }
    public int TechnicalFeasibility { get; private set; }

    // EF Core için varsayılan yapıcı metot
    private OpportunityScore() { }

    public OpportunityScore(int problemSeverity, int marketSize, int founderFit, int riskScore, int technicalFeasibility)
    {
        ValidateScore(problemSeverity, nameof(problemSeverity));
        ValidateScore(marketSize, nameof(marketSize));
        ValidateScore(founderFit, nameof(founderFit));
        ValidateScore(riskScore, nameof(riskScore));
        ValidateScore(technicalFeasibility, nameof(technicalFeasibility));

        ProblemSeverity = problemSeverity;
        MarketSize = marketSize;
        FounderFit = founderFit;
        RiskScore = riskScore;
        TechnicalFeasibility = technicalFeasibility;
    }

    /// <summary>
    /// Kriter ağırlıklarına göre hesaplanan ağırlıklı ortalama fırsat skoru.
    /// Ağırlıklar: ProblemSeverity x3, MarketSize x2, FounderFit x2, RiskScore x1, TechnicalFeasibility x2.
    /// Toplam Ağırlık = 10.
    /// </summary>
    public double TotalScore => (ProblemSeverity * 3 + MarketSize * 2 + FounderFit * 2 + RiskScore * 1 + TechnicalFeasibility * 2) / 10.0;

    private static void ValidateScore(int score, string paramName)
    {
        if (score < 1 || score > 10)
        {
            throw new ArgumentOutOfRangeException(paramName, "Skor değeri 1 ile 10 arasında olmalıdır.");
        }
    }
}
