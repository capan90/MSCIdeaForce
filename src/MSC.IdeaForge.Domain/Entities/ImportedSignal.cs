using System;
using MSC.IdeaForge.Domain.Enums;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Dış kaynaklardan otomatik veya yarı-otomatik olarak toplanan ham sinyalleri temsil eden entity.
/// </summary>
public class ImportedSignal : BaseEntity
{
    public ImportSourceType SourceType { get; private set; }
    public string? SourceUrl { get; private set; }
    public string RawContent { get; private set; } = null!;
    public string Title { get; private set; } = null!;
    public string? Summary { get; private set; }
    public DateTime ImportedAt { get; private set; }
    public bool IsProcessed { get; private set; }
    public Guid? ProblemId { get; private set; }

    // EF Core için gizli varsayılan yapıcı metot
    private ImportedSignal() { }

    /// <summary>
    /// Yeni bir içe aktarılmış sinyal kaydı oluşturur.
    /// </summary>
    public static ImportedSignal Create(
        ImportSourceType sourceType,
        string? sourceUrl,
        string rawContent,
        string title,
        string? summary,
        DateTime importedAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rawContent);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        return new ImportedSignal
        {
            SourceType = sourceType,
            SourceUrl = sourceUrl,
            RawContent = rawContent,
            Title = title,
            Summary = summary,
            ImportedAt = importedAt,
            IsProcessed = false,
            ProblemId = null
        };
    }

    /// <summary>
    /// Sinyali bir problem ile ilişkilendirerek işlendi olarak işaretler.
    /// </summary>
    public void MarkAsProcessed(Guid problemId)
    {
        if (problemId == Guid.Empty)
        {
            throw new ArgumentException("İlişkilendirilecek Problem ID boş olamaz.", nameof(problemId));
        }

        ProblemId = problemId;
        IsProcessed = true;
        SetUpdated();
    }
}
