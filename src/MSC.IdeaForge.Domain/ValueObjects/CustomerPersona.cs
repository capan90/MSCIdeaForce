namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Yapay zeka tarafından üretilen bir hedef müşteri personasını temsil eden değer nesnesi.
/// </summary>
public class CustomerPersona
{
    public string Name { get; set; } = string.Empty;
    public string Age { get; set; } = string.Empty;
    public string Occupation { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Income { get; set; } = string.Empty;
    public string Goals { get; set; } = string.Empty;
    public string Frustrations { get; set; } = string.Empty;
    public string Motivations { get; set; } = string.Empty;
    public int TechSavviness { get; set; }                 // 1-5
    public string BuyingBehavior { get; set; } = string.Empty;
    public string PreferredChannels { get; set; } = string.Empty;
    public string Quote { get; set; } = string.Empty;
}
