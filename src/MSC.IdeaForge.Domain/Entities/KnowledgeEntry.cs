using System;
using MSC.IdeaForge.Domain.Enums;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Proje geliştirme ve fikir doğrulama süreçlerinde edinilen tecrübe, pazar araştırması ve bilgileri tutan entity.
/// </summary>
public class KnowledgeEntry : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string Content { get; private set; } = null!;
    public KnowledgeCategory Category { get; private set; }
    public string? Tags { get; private set; }
    public Guid? ProblemId { get; private set; }
    public string? Source { get; private set; }
    public DateTime LearnedAt { get; private set; }

    // EF Core için gizli varsayılan yapıcı metot
    private KnowledgeEntry() { }

    /// <summary>
    /// Yeni bir bilgi bankası girdisi oluşturur.
    /// </summary>
    public static KnowledgeEntry Create(
        string title,
        string content,
        KnowledgeCategory category,
        string? tags,
        Guid? problemId,
        string? source,
        DateTime learnedAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        return new KnowledgeEntry
        {
            Title = title,
            Content = content,
            Category = category,
            Tags = tags,
            ProblemId = problemId,
            Source = source,
            LearnedAt = learnedAt
        };
    }

    /// <summary>
    /// Bilgi bankası girdisini günceller.
    /// </summary>
    public void Update(
        string title,
        string content,
        KnowledgeCategory category,
        string? tags,
        Guid? problemId,
        string? source,
        DateTime learnedAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        Title = title;
        Content = content;
        Category = category;
        Tags = tags;
        ProblemId = problemId;
        Source = source;
        LearnedAt = learnedAt;
        SetUpdated();
    }
}
