namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Yapay zeka tarafından önerilen bir risk öğesini temsil eden değer nesnesi.
/// </summary>
public class RiskItemSuggestion
{
    public string RiskName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;   // Teknik/Pazar/Finansal/Operasyonel/Hukuki/Rekabet
    public int Probability { get; set; }                    // 1-5
    public int Impact { get; set; }                         // 1-5
    public int RiskScore { get; set; }                      // Probability * Impact
    public string MitigationStrategy { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;      // Açık/İzleniyor/Azaltıldı/Kapalı
}
