using System;
using MSC.IdeaForge.Domain.Enums;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Bir problem hakkında alınan karar kaydını temsil eden entity.
/// </summary>
public class Decision : BaseEntity
{
    public Guid ProblemId { get; private set; }
    public DecisionType DecisionType { get; private set; }
    public string Reason { get; private set; } = null!;
    public DateTime DecisionDate { get; private set; }

    // EF Core için gizli varsayılan yapıcı metot
    private Decision() { }

    /// <summary>
    /// Yeni bir karar kaydı oluşturur.
    /// </summary>
    public static Decision Create(Guid problemId, DecisionType decisionType, string reason, DateTime decisionDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);
        if (problemId == Guid.Empty)
        {
            throw new ArgumentException("Problem ID boş olamaz.", nameof(problemId));
        }

        return new Decision
        {
            ProblemId = problemId,
            DecisionType = decisionType,
            Reason = reason,
            DecisionDate = decisionDate
        };
    }
}
