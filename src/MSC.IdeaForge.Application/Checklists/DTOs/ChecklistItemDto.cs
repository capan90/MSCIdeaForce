using System;

namespace MSC.IdeaForge.Application.Checklists.DTOs;

/// <summary>
/// Kontrol listesi öğesi verilerini taşımak için kullanılan DTO.
/// </summary>
public class ChecklistItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Guid? ProblemId { get; set; }
    public int Priority { get; set; }
    public DateTime? DueDate { get; set; }
}
