using System;

namespace MSC.IdeaForge.Application.BusinessCanvases.DTOs;

/// <summary>
/// Business Model Canvas verilerini taşımak için kullanılan DTO.
/// </summary>
public class BusinessCanvasDto
{
    public Guid Id { get; set; }
    public Guid ProblemId { get; set; }
    public string KeyPartners { get; set; } = string.Empty;
    public string KeyActivities { get; set; } = string.Empty;
    public string KeyResources { get; set; } = string.Empty;
    public string ValuePropositions { get; set; } = string.Empty;
    public string CustomerRelationships { get; set; } = string.Empty;
    public string Channels { get; set; } = string.Empty;
    public string CustomerSegments { get; set; } = string.Empty;
    public string CostStructure { get; set; } = string.Empty;
    public string RevenueStreams { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
}
