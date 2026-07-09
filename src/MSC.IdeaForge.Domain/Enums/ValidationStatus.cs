namespace MSC.IdeaForge.Domain.Enums;

/// <summary>
/// Problem doğrulama aşamalarını temsil eden enum.
/// </summary>
public enum ValidationStatus
{
    NotStarted = 0,
    InProgress = 1,
    Validated = 2,
    Rejected = 3
}
