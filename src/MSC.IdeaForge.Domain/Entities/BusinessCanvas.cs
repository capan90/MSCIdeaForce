using System;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Bir probleme ait Business Model Canvas (9 blok) kaydını temsil eden entity.
/// </summary>
public class BusinessCanvas : BaseEntity
{
    public Guid ProblemId { get; private set; }
    public string KeyPartners { get; private set; } = string.Empty;
    public string KeyActivities { get; private set; } = string.Empty;
    public string KeyResources { get; private set; } = string.Empty;
    public string ValuePropositions { get; private set; } = string.Empty;
    public string CustomerRelationships { get; private set; } = string.Empty;
    public string Channels { get; private set; } = string.Empty;
    public string CustomerSegments { get; private set; } = string.Empty;
    public string CostStructure { get; private set; } = string.Empty;
    public string RevenueStreams { get; private set; } = string.Empty;
    public DateTime GeneratedAt { get; private set; }

    // EF Core için gizli varsayılan yapıcı metot
    private BusinessCanvas() { }

    /// <summary>
    /// Yeni bir Business Model Canvas kaydı oluşturur.
    /// </summary>
    public static BusinessCanvas Create(
        Guid problemId,
        string keyPartners, string keyActivities, string keyResources,
        string valuePropositions, string customerRelationships, string channels,
        string customerSegments, string costStructure, string revenueStreams)
    {
        if (problemId == Guid.Empty)
        {
            throw new ArgumentException("Problem ID boş olamaz.", nameof(problemId));
        }

        var canvas = new BusinessCanvas { ProblemId = problemId };
        canvas.Apply(keyPartners, keyActivities, keyResources, valuePropositions,
            customerRelationships, channels, customerSegments, costStructure, revenueStreams);
        return canvas;
    }

    /// <summary>
    /// Mevcut Business Model Canvas kaydını günceller.
    /// </summary>
    public void Update(
        string keyPartners, string keyActivities, string keyResources,
        string valuePropositions, string customerRelationships, string channels,
        string customerSegments, string costStructure, string revenueStreams)
    {
        Apply(keyPartners, keyActivities, keyResources, valuePropositions,
            customerRelationships, channels, customerSegments, costStructure, revenueStreams);
        SetUpdated();
    }

    // Ortak alan atama yardımcı metodu
    private void Apply(
        string keyPartners, string keyActivities, string keyResources,
        string valuePropositions, string customerRelationships, string channels,
        string customerSegments, string costStructure, string revenueStreams)
    {
        KeyPartners = keyPartners ?? string.Empty;
        KeyActivities = keyActivities ?? string.Empty;
        KeyResources = keyResources ?? string.Empty;
        ValuePropositions = valuePropositions ?? string.Empty;
        CustomerRelationships = customerRelationships ?? string.Empty;
        Channels = channels ?? string.Empty;
        CustomerSegments = customerSegments ?? string.Empty;
        CostStructure = costStructure ?? string.Empty;
        RevenueStreams = revenueStreams ?? string.Empty;
        GeneratedAt = DateTime.UtcNow;
    }
}
