using MSC.IdeaForge.Domain.Enums;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Bir probleme ait kaynak sinyal varlığını temsil eder.
/// </summary>
public class Signal : BaseEntity
{
    public Guid ProblemId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public SignalType SignalType { get; private set; } = SignalType.Manual;
    public SeverityLevel Severity { get; private set; } = SeverityLevel.Medium;

    // EF Core için gizli varsayılan yapıcı metot
    private Signal() { }

    /// <summary>
    /// Yeni bir Sinyal nesnesi oluşturur.
    /// </summary>
    public static Signal Create(Guid problemId, string title, string description, SignalType signalType, SeverityLevel severity)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        if (problemId == Guid.Empty)
        {
            throw new ArgumentException("Problem ID boş olamaz.", nameof(problemId));
        }

        return new Signal
        {
            ProblemId = problemId,
            Title = title,
            Description = description,
            SignalType = signalType,
            Severity = severity
        };
    }

    /// <summary>
    /// Sinyal detaylarını günceller.
    /// </summary>
    public void Update(string title, string description, SignalType signalType, SeverityLevel severity)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        Title = title;
        Description = description;
        SignalType = signalType;
        Severity = severity;
        SetUpdated();
    }
}
