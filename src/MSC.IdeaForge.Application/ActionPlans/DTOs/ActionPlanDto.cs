using System;
using System.Collections.Generic;
using MSC.IdeaForge.Domain.ValueObjects;

namespace MSC.IdeaForge.Application.ActionPlans.DTOs;

/// <summary>
/// Aksiyon planı verilerini taşımak için kullanılan DTO.
/// </summary>
public class ActionPlanDto
{
    public Guid Id { get; set; }
    public Guid ProblemId { get; set; }
    public List<ActionItem> Day30 { get; set; } = new();
    public List<ActionItem> Day60 { get; set; } = new();
    public List<ActionItem> Day90 { get; set; } = new();
    public List<string> SuccessMetrics { get; set; } = new();
    public List<string> QuickWins { get; set; } = new();
    public DateTime GeneratedAt { get; set; }
}
