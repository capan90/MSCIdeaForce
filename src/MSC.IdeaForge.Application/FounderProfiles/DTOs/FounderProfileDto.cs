using System;

namespace MSC.IdeaForge.Application.FounderProfiles.DTOs;

/// <summary>
/// Kurucu profili verilerini taşımak için kullanılan DTO.
/// </summary>
public class FounderProfileDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    public string Industries { get; set; } = string.Empty;
    public string Experience { get; set; } = string.Empty;
    public string Network { get; set; } = string.Empty;
    public string Capital { get; set; } = string.Empty;
    public string Interests { get; set; } = string.Empty;
    public string? Bio { get; set; }
}
