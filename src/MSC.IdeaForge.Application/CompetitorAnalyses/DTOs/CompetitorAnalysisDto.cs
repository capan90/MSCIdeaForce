using System;
using System.Collections.Generic;
using MSC.IdeaForge.Domain.ValueObjects;

namespace MSC.IdeaForge.Application.CompetitorAnalyses.DTOs;

/// <summary>
/// Rakip analizi verilerini taşımak için kullanılan DTO.
/// </summary>
public class CompetitorAnalysisDto
{
    public Guid Id { get; set; }
    public Guid ProblemId { get; set; }
    public List<Competitor> Competitors { get; set; } = new();
    public DateTime AnalyzedAt { get; set; }
}
