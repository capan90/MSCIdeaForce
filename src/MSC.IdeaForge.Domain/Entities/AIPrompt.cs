using System;
using MSC.IdeaForge.Domain.Enums;

namespace MSC.IdeaForge.Domain.Entities;

public class AIPrompt : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PromptType PromptType { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int Version { get; set; } = 1;

    public AIPrompt()
    {
    }

    public AIPrompt(string name, string description, PromptType promptType, string content, bool isActive = true, int version = 1)
    {
        Name = name;
        Description = description;
        PromptType = promptType;
        Content = content;
        IsActive = isActive;
        Version = version;
    }
}
