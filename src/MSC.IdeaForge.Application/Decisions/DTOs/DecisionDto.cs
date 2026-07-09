using System;

namespace MSC.IdeaForge.Application.Decisions.DTOs;

/// <summary>
/// Karar verilerini taşımak için kullanılan DTO.
/// </summary>
public class DecisionDto
{
    public Guid Id { get; set; }
    public Guid ProblemId { get; set; }
    public string ProblemTitle { get; set; } = string.Empty;
    public string DecisionType { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime DecisionDate { get; set; }
}
