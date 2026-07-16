using System;

namespace MSC.IdeaForge.Domain.Entities;

public class AIProviderSetting : BaseEntity
{
    public string ProviderName { get; set; } = string.Empty; // Gemini, Claude, OpenAI
    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsDefault { get; set; } = false;

    public AIProviderSetting()
    {
    }

    public AIProviderSetting(string providerName, string apiKey, string model, bool isActive = true, bool isDefault = false)
    {
        ProviderName = providerName;
        ApiKey = apiKey;
        Model = model;
        IsActive = isActive;
        IsDefault = isDefault;
    }
}
