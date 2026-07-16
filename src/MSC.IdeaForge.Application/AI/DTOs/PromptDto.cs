using System;
using MSC.IdeaForge.Domain.Enums;

namespace MSC.IdeaForge.Application.AI.DTOs;

public class PromptDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PromptType PromptType { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int Version { get; set; }
    public DateTime CreatedAt { get; set; }
}
