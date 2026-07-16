using System;

namespace MSC.IdeaForge.Application.AI.DTOs;

public class AIProviderSettingDto
{
    public Guid Id { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsDefault { get; set; }
}
