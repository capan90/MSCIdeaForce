namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Yapay zeka tarafından üretilen Business Model Canvas 9 bloğunu temsil eden değer nesnesi (Value Object).
/// </summary>
public class BusinessCanvasResult
{
    public string KeyPartners { get; set; } = string.Empty;
    public string KeyActivities { get; set; } = string.Empty;
    public string KeyResources { get; set; } = string.Empty;
    public string ValuePropositions { get; set; } = string.Empty;
    public string CustomerRelationships { get; set; } = string.Empty;
    public string Channels { get; set; } = string.Empty;
    public string CustomerSegments { get; set; } = string.Empty;
    public string CostStructure { get; set; } = string.Empty;
    public string RevenueStreams { get; set; } = string.Empty;
}
