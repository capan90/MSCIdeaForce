using MSC.IdeaForge.Domain.Enums;
using MSC.IdeaForge.Domain.ValueObjects;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Bir probleme ait değerlendirilen fırsat verisini temsil eden entity.
/// </summary>
public class Opportunity : BaseEntity
{
    public Guid ProblemId { get; private set; }
    public OpportunityScore OpportunityScore { get; private set; } = null!;
    public string? Notes { get; private set; }
    public OpportunityStatus Status { get; private set; } = OpportunityStatus.Draft;

    // EF Core için gizli varsayılan yapıcı metot
    private Opportunity() { }

    /// <summary>
    /// Yeni bir fırsat nesnesi oluşturur.
    /// </summary>
    public static Opportunity Create(Guid problemId, OpportunityScore score, string? notes = null)
    {
        if (problemId == Guid.Empty)
        {
            throw new ArgumentException("Problem ID boş olamaz.", nameof(problemId));
        }
        ArgumentNullException.ThrowIfNull(score);

        return new Opportunity
        {
            ProblemId = problemId,
            OpportunityScore = score,
            Notes = notes,
            Status = OpportunityStatus.Scored
        };
    }

    /// <summary>
    /// Fırsat puanlamasını ve notlarını günceller.
    /// </summary>
    public void UpdateScore(OpportunityScore score, string? notes = null)
    {
        ArgumentNullException.ThrowIfNull(score);
        OpportunityScore = score;
        Notes = notes;
        Status = OpportunityStatus.Scored;
        SetUpdated();
    }

    /// <summary>
    /// Fırsat durumunu günceller.
    /// </summary>
    public void ChangeStatus(OpportunityStatus status)
    {
        Status = status;
        SetUpdated();
    }
}
