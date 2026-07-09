using System;

namespace MSC.IdeaForge.Application.Trends.DTOs;

/// <summary>
/// Trend analizi verilerini taşımak için kullanılan DTO.
/// </summary>
public class TrendAnalysisDto
{
    public Guid Id { get; set; }
    public string TrendName { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Opportunities { get; set; } = string.Empty;
    public string Actions { get; set; } = string.Empty;
    public DateTime AnalyzedAt { get; set; }
}
