namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Yapay zeka tarafından yapılan problem analizinin sonuçlarını barındıran değer nesnesi (Value Object).
/// </summary>
public record ProblemAnalysisResult(
    string Summary,
    string SuggestedCategory,
    string SuggestedTags,
    string RiskLevel,
    string SolutionTypeSuggestion,
    double ConfidenceScore
);
