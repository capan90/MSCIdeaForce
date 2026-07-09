using MSC.IdeaForge.Domain.Enums;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Bir problemin doğrulama (validation) sürecini ve istatistiklerini temsil eden entity.
/// </summary>
public class Validation : BaseEntity
{
    public Guid ProblemId { get; private set; }
    public int InterviewCount { get; private set; }
    public int ValidatedUserCount { get; private set; }
    public int WillingToPayCount { get; private set; }
    public int ConfidenceScore { get; private set; }
    public string? Notes { get; private set; }
    public string? Risks { get; private set; }
    public ValidationStatus Status { get; private set; } = ValidationStatus.NotStarted;

    // EF Core için gizli varsayılan yapıcı metot
    private Validation() { }

    /// <summary>
    /// Yeni bir doğrulama kaydı oluşturur.
    /// </summary>
    public static Validation Create(
        Guid problemId,
        int interviewCount,
        int validatedUserCount,
        int willingToPayCount,
        int confidenceScore,
        string? notes,
        string? risks,
        ValidationStatus status)
    {
        if (problemId == Guid.Empty)
        {
            throw new ArgumentException("Problem ID boş olamaz.", nameof(problemId));
        }
        ValidateCounts(interviewCount, validatedUserCount, willingToPayCount, confidenceScore);

        return new Validation
        {
            ProblemId = problemId,
            InterviewCount = interviewCount,
            ValidatedUserCount = validatedUserCount,
            WillingToPayCount = willingToPayCount,
            ConfidenceScore = confidenceScore,
            Notes = notes,
            Risks = risks,
            Status = status
        };
    }

    /// <summary>
    /// Doğrulama verilerini günceller.
    /// </summary>
    public void Update(
        int interviewCount,
        int validatedUserCount,
        int willingToPayCount,
        int confidenceScore,
        string? notes,
        string? risks,
        ValidationStatus status)
    {
        ValidateCounts(interviewCount, validatedUserCount, willingToPayCount, confidenceScore);

        InterviewCount = interviewCount;
        ValidatedUserCount = validatedUserCount;
        WillingToPayCount = willingToPayCount;
        ConfidenceScore = confidenceScore;
        Notes = notes;
        Risks = risks;
        Status = status;
        SetUpdated();
    }

    private static void ValidateCounts(int interviewCount, int validatedUserCount, int willingToPayCount, int confidenceScore)
    {
        if (interviewCount < 0) throw new ArgumentException("Görüşme sayısı negatif olamaz.");
        if (validatedUserCount < 0) throw new ArgumentException("Doğrulayan kişi sayısı negatif olamaz.");
        if (willingToPayCount < 0) throw new ArgumentException("Ödeme isteği sayısı negatif olamaz.");
        if (validatedUserCount > interviewCount) throw new ArgumentException("Doğrulayan kişi sayısı görüşme sayısından fazla olamaz.");
        if (willingToPayCount > validatedUserCount) throw new ArgumentException("Ödeme isteği sayısı doğrulayan kişi sayısından fazla olamaz.");
        if (confidenceScore < 1 || confidenceScore > 100) throw new ArgumentException("Güven skoru 1 ile 100 arasında olmalıdır.");
    }
}
