using System.Collections.Generic;

namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Yapay zeka tarafından üretilen 30-60-90 günlük aksiyon planını temsil eden değer nesnesi.
/// </summary>
public class ActionPlanResult
{
    public List<ActionItem> Day30 { get; set; } = new();
    public List<ActionItem> Day60 { get; set; } = new();
    public List<ActionItem> Day90 { get; set; } = new();
    public List<string> SuccessMetrics { get; set; } = new();
    public List<string> QuickWins { get; set; } = new();
}

/// <summary>
/// Aksiyon planındaki tek bir aksiyonu temsil eden değer nesnesi.
/// </summary>
public class ActionItem
{
    public string Action { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
