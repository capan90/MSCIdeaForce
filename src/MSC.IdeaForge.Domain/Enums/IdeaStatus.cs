namespace MSC.IdeaForge.Domain.Enums;

/// <summary>
/// Bir fikrin/problemin yaşam döngüsündeki durumunu temsil eden enum.
/// Raw → Analyzed → Validated → MVPCandidate → Development → Pilot → Released → Parked → Archived → Killed
/// </summary>
public enum IdeaStatus
{
    Raw = 0,
    Analyzed = 1,
    Validated = 2,
    MVPCandidate = 3,
    Development = 4,
    Pilot = 5,
    Released = 6,
    Parked = 7,
    Archived = 8,
    Killed = 9
}
