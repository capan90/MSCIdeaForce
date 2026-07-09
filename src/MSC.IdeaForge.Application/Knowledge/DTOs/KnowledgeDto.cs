using System;

namespace MSC.IdeaForge.Application.Knowledge.DTOs;

/// <summary>
/// Bilgi bankası girdisi verilerini taşımak için kullanılan DTO.
/// </summary>
public class KnowledgeDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Tags { get; set; }
    public Guid? ProblemId { get; set; }
    public string? Source { get; set; }
    public DateTime LearnedAt { get; set; }
}
