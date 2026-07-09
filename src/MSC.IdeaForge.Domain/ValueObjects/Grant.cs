namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Bir hibe/destek/teşvik programını temsil eden değer nesnesi (Value Object).
/// Kalıcı saklanmaz; yalnızca yapay zeka sonucu olarak taşınır.
/// </summary>
public class Grant
{
    public string Name { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
    public string Deadline { get; set; } = string.Empty;
    public string EligibilityCriteria { get; set; } = string.Empty;
    public string ApplicationUrl { get; set; } = string.Empty;
    public int MatchScore { get; set; }   // 0-100 uygunluk skoru
}
