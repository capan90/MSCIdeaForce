using System;

namespace MSC.IdeaForge.Application.Researches.DTOs;

/// <summary>
/// Araştırma verilerini taşımak için kullanılan DTO.
/// </summary>
public class ResearchDto
{
    public Guid Id { get; set; }
    public Guid ProblemId { get; set; }
    public string? MarketAnalysis { get; set; }
    public string? CompetitorSummary { get; set; }
    public string? TechnologyNotes { get; set; }
    public string? TrendNotes { get; set; }
    public string? Sources { get; set; }
    public DateTime? ResearchedAt { get; set; }
}
