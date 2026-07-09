using System;
using System.Collections.Generic;

namespace MSC.IdeaForge.Application.MVPPlans.DTOs;

/// <summary>
/// MVP Planı verilerini taşımak için kullanılan DTO.
/// </summary>
public class MVPPlanDto
{
    public Guid Id { get; set; }
    public Guid ProblemId { get; set; }
    public string? Scope { get; set; }
    public List<string> UserStories { get; set; } = new();
    public List<string> SprintPlan { get; set; } = new();
    public string? TechStack { get; set; }
    public string? Risks { get; set; }
    public string? FirstSalesPlan { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
