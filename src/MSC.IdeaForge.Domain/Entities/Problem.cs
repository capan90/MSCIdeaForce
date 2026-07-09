using MSC.IdeaForge.Domain.Enums;

namespace MSC.IdeaForge.Domain.Entities;

public class Problem : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ProblemStatus Status { get; private set; } = ProblemStatus.Draft;
    public PriorityLevel Priority { get; private set; } = PriorityLevel.Medium;
    public string? Sector { get; private set; }
    public string? Tags { get; private set; }
    public string? Source { get; private set; }

    private Problem() { }

    public static Problem Create(string title, string description, string? sector = null, string? source = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        return new Problem
        {
            Title = title,
            Description = description,
            Sector = sector,
            Source = source
        };
    }

    public void Update(string title, string description, string? sector = null, string? tags = null, string? source = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        Title = title;
        Description = description;
        Sector = sector;
        Tags = tags;
        Source = source;
        SetUpdated();
    }

    public void ChangeStatus(ProblemStatus newStatus)
    {
        Status = newStatus;
        SetUpdated();
    }

    public void SetPriority(PriorityLevel priority)
    {
        Priority = priority;
        SetUpdated();
    }

    /// <summary>
    /// Problemi silinmiş olarak işaretler (soft delete).
    /// </summary>
    public void Delete()
    {
        MarkAsDeleted();
    }
}
