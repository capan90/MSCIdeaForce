using System;

namespace MSC.IdeaForge.Application.FieldNotes.DTOs;

/// <summary>
/// Saha notu verilerini taşımak için kullanılan DTO.
/// </summary>
public class FieldNoteDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Location { get; set; }
    public DateTime ObservationDate { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string? Tags { get; set; }
    public Guid? ProblemId { get; set; }
    public bool HasPhoto { get; set; }
}
