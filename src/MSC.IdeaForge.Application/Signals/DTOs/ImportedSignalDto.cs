using System;

namespace MSC.IdeaForge.Application.Signals.DTOs;

/// <summary>
/// İçe aktarılmış ham sinyal verilerini taşımak için kullanılan DTO.
/// </summary>
public class ImportedSignalDto
{
    public Guid Id { get; set; }
    public string SourceType { get; set; } = string.Empty;
    public string? SourceUrl { get; set; }
    public string RawContent { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public DateTime ImportedAt { get; set; }
    public bool IsProcessed { get; set; }
    public Guid? ProblemId { get; set; }
}
