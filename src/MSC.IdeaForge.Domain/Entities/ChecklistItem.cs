using System;
using MSC.IdeaForge.Domain.Enums;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Girişim başlangıç kontrol listesindeki tek bir yapılacak öğesini temsil eden entity.
/// </summary>
public class ChecklistItem : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public ChecklistCategory Category { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public Guid? ProblemId { get; private set; }
    public int Priority { get; private set; }   // 1 (düşük) - 5 (yüksek)
    public DateTime? DueDate { get; private set; }

    // EF Core için gizli varsayılan yapıcı metot
    private ChecklistItem() { }

    /// <summary>
    /// Yeni bir kontrol listesi öğesi oluşturur.
    /// </summary>
    public static ChecklistItem Create(
        string title,
        string? description,
        ChecklistCategory category,
        int priority,
        Guid? problemId,
        DateTime? dueDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        return new ChecklistItem
        {
            Title = title,
            Description = description,
            Category = category,
            Priority = Math.Clamp(priority, 1, 5),
            ProblemId = problemId,
            DueDate = dueDate,
            IsCompleted = false
        };
    }

    /// <summary>
    /// Öğenin tamamlanma durumunu değiştirir.
    /// </summary>
    public void SetCompleted(bool isCompleted)
    {
        IsCompleted = isCompleted;
        CompletedAt = isCompleted ? DateTime.UtcNow : null;
        SetUpdated();
    }

    /// <summary>
    /// Öğeyi silinmiş olarak işaretler (soft delete).
    /// </summary>
    public void Delete()
    {
        MarkAsDeleted();
    }
}
