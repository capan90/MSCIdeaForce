namespace MSC.IdeaForge.Application.Validations.DTOs;

/// <summary>
/// Doğrulama verilerini dışarıya aktarmak için kullanılan veri transfer nesnesi (DTO).
/// </summary>
public class ValidationDto
{
    public Guid Id { get; set; }
    public Guid ProblemId { get; set; }
    public int InterviewCount { get; set; }
    public int ValidatedUserCount { get; set; }
    public int WillingToPayCount { get; set; }
    public int ConfidenceScore { get; set; }
    public string? Notes { get; set; }
    public string? Risks { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    // Yapay zeka tarafından üretilen doğrulama sorularının JSON serileştirilmiş hali.
    public string? ValidationQuestions { get; set; }
}
