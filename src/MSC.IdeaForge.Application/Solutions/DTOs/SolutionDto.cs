using System;

namespace MSC.IdeaForge.Application.Solutions.DTOs;

/// <summary>
/// Çözüm önerisi verilerini taşımak için kullanılan DTO.
/// </summary>
public class SolutionDto
{
    public Guid Id { get; set; }
    public Guid ProblemId { get; set; }
    public string SolutionType { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Complexity { get; set; } = string.Empty;
    public int EstimatedDurationMonths { get; set; }
    public string? EstimatedCost { get; set; }
    public string? Pros { get; set; }
    public string? Cons { get; set; }
    public bool IsRecommended { get; set; }
    public DateTime CreatedAt { get; set; }
}
