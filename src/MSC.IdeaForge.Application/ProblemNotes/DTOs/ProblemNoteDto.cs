using System;

namespace MSC.IdeaForge.Application.ProblemNotes.DTOs;

/// <summary>
/// Problem notu verilerini taşımak için kullanılan DTO.
/// </summary>
public class ProblemNoteDto
{
    public Guid Id { get; set; }
    public Guid ProblemId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string NoteType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
