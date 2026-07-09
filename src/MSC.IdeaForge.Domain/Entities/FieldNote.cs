using System;
using MSC.IdeaForge.Domain.Enums;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Saha gözlemlerini ve gerçek hayat notlarını temsil eden entity.
/// </summary>
public class FieldNote : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string? Location { get; private set; }
    public DateTime ObservationDate { get; private set; }
    public string Content { get; private set; } = null!;
    public FieldNoteSource Source { get; private set; }
    public string? Tags { get; private set; }
    public Guid? ProblemId { get; private set; }
    public bool HasPhoto { get; private set; }

    // EF Core için gizli varsayılan yapıcı metot
    private FieldNote() { }

    /// <summary>
    /// Yeni bir saha notu oluşturur.
    /// </summary>
    public static FieldNote Create(
        string title,
        string? location,
        DateTime observationDate,
        string content,
        FieldNoteSource source,
        string? tags,
        Guid? problemId,
        bool hasPhoto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        return new FieldNote
        {
            Title = title,
            Location = location,
            ObservationDate = observationDate,
            Content = content,
            Source = source,
            Tags = tags,
            ProblemId = problemId,
            HasPhoto = hasPhoto
        };
    }
}
