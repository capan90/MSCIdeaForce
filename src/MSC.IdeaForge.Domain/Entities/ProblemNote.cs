using System;
using MSC.IdeaForge.Domain.Enums;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Bir probleme ait serbest notları temsil eden entity.
/// </summary>
public class ProblemNote : BaseEntity
{
    public Guid ProblemId { get; private set; }
    public string Content { get; private set; } = null!;
    public NoteType NoteType { get; private set; }

    // EF Core için gizli varsayılan yapıcı metot
    private ProblemNote() { }

    /// <summary>
    /// Yeni bir problem notu oluşturur.
    /// </summary>
    public static ProblemNote Create(Guid problemId, string content, NoteType noteType)
    {
        if (problemId == Guid.Empty)
        {
            throw new ArgumentException("Problem ID boş olamaz.", nameof(problemId));
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        return new ProblemNote
        {
            ProblemId = problemId,
            Content = content,
            NoteType = noteType
        };
    }

    /// <summary>
    /// Notu silinmiş olarak işaretler (soft delete).
    /// </summary>
    public void Delete()
    {
        MarkAsDeleted();
    }
}
