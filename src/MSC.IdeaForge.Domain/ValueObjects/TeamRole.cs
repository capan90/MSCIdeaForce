namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Yapay zeka tarafından önerilen bir ekip rolünü temsil eden değer nesnesi (Value Object).
/// Kalıcı saklanmaz; yalnızca analiz sonucu olarak taşınır.
/// </summary>
public class TeamRole
{
    public string Role { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public bool IsFounderCanHandle { get; set; }
    public string EstimatedCost { get; set; } = string.Empty;
    public string HiringTimeline { get; set; } = string.Empty;
}
