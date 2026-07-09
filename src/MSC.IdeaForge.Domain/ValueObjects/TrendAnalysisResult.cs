namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Yapay zeka tarafından yapılan trend analizi sonuçlarını temsil eden değer nesnesi (Value Object).
/// </summary>
public class TrendAnalysisResult
{
    public string Summary { get; set; } = string.Empty;
    public string Opportunities { get; set; } = string.Empty;
    public string Actions { get; set; } = string.Empty;
}
