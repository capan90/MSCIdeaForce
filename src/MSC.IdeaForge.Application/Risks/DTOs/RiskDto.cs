using System;

namespace MSC.IdeaForge.Application.Risks.DTOs;

/// <summary>
/// Risk verilerini taşımak için kullanılan DTO.
/// </summary>
public class RiskDto
{
    public Guid Id { get; set; }
    public Guid ProblemId { get; set; }
    public string RiskName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Probability { get; set; }
    public int Impact { get; set; }
    public int RiskScore { get; set; }
    public string MitigationStrategy { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
