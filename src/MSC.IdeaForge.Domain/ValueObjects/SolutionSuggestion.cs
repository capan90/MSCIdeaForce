using MSC.IdeaForge.Domain.Enums;

namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Yapay zekanın önerdiği çözüm alternatifi değer nesnesi (Value Object).
/// </summary>
public class SolutionSuggestion
{
    public SolutionType SolutionType { get; set; }
    public string Summary { get; set; } = string.Empty;
    public ComplexityLevel Complexity { get; set; }
    public int EstimatedDurationMonths { get; set; }
    public string? EstimatedCost { get; set; }
    public string? Pros { get; set; }
    public string? Cons { get; set; }
    public bool IsRecommended { get; set; }
}
