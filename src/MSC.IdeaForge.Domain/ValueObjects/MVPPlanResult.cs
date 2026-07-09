using System.Collections.Generic;

namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Yapay zeka tarafından üretilen MVP plan sonuçlarını temsil eden değer nesnesi (Value Object).
/// </summary>
public class MVPPlanResult
{
    public string Scope { get; set; } = string.Empty;
    public List<string> UserStories { get; set; } = new();
    public List<string> SprintPlan { get; set; } = new();
    public string TechStack { get; set; } = string.Empty;
    public string Risks { get; set; } = string.Empty;
    public string FirstSalesPlan { get; set; } = string.Empty;
}
