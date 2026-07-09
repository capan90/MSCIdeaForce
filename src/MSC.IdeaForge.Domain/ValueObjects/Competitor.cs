namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Bir rakibi ve rekabet analizini temsil eden değer nesnesi (Value Object).
/// CompetitorAnalysis entity içinde JSON olarak saklanır.
/// </summary>
public class Competitor
{
    public string Name { get; set; } = string.Empty;
    public string Strengths { get; set; } = string.Empty;      // Güçlü yönler
    public string Weaknesses { get; set; } = string.Empty;     // Zayıf yönler
    public string Pricing { get; set; } = string.Empty;        // Fiyatlandırma
    public string MarketShare { get; set; } = string.Empty;    // Pazar payı
    public string OurAdvantage { get; set; } = string.Empty;   // Bizim avantajımız
}
