using System;

namespace MSC.IdeaForge.Application.Agents;

/// <summary>
/// Koordinatör adım durumlarını temsil eden enum.
/// </summary>
public enum AgentStepStatus
{
    Pending,
    Running,
    Completed,
    Failed
}

/// <summary>
/// Koordinatör içerisindeki her bir yapay zeka adımının ilerleme detayını tutan sınıf.
/// </summary>
public class AgentProgress
{
    public string Step { get; set; } = string.Empty;
    public AgentStepStatus Status { get; set; } = AgentStepStatus.Pending;
    public string? ErrorMessage { get; set; }
    public DateTime? CompletedAt { get; set; }
}
