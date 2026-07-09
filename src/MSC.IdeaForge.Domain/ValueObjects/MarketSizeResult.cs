namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Yapay zeka tarafından hesaplanan pazar büyüklüğü (TAM/SAM/SOM) sonuçlarını temsil eden değer nesnesi (Value Object).
/// </summary>
public class MarketSizeResult
{
    public string TAM { get; set; } = string.Empty;              // Total Addressable Market
    public string SAM { get; set; } = string.Empty;              // Serviceable Addressable Market
    public string SOM { get; set; } = string.Empty;              // Serviceable Obtainable Market
    public string TAMDescription { get; set; } = string.Empty;
    public string SAMDescription { get; set; } = string.Empty;
    public string SOMDescription { get; set; } = string.Empty;
    public string GrowthRate { get; set; } = string.Empty;
    public string KeyAssumptions { get; set; } = string.Empty;
    public string DataSources { get; set; } = string.Empty;
}
