namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Yapay zeka tarafından yapılan fırsat skorlamasının sonuçlarını ve her bir kriterin gerekçesini barındıran değer nesnesi (Value Object).
/// </summary>
public record OpportunityScoreResult(
    int ProblemSeverity,
    string ProblemSeverityReason,
    int MarketSize,
    string MarketSizeReason,
    int FounderFit,
    string FounderFitReason,
    int RiskScore,
    string RiskScoreReason,
    int TechnicalFeasibility,
    string TechnicalFeasibilityReason
);
