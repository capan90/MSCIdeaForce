namespace MSC.IdeaForge.Application.Signals.DTOs;

/// <summary>
/// Sinyal verilerini dışarıya aktarmak için kullanılan veri transfer nesnesi (DTO).
/// </summary>
public class SignalDto
{
    public Guid Id { get; set; }
    public Guid ProblemId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SignalType { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
