namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Yapay zeka tarafından önerilen bir kontrol listesi öğesini temsil eden değer nesnesi.
/// </summary>
public class ChecklistItemSuggestion
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;   // ChecklistCategory adı
    public int Priority { get; set; }                       // 1-5
}
