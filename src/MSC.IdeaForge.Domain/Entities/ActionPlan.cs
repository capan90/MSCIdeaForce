using System;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Bir probleme ait 30-60-90 günlük aksiyon planını temsil eden entity.
/// Plan içeriği JSON olarak (ActionPlanResult serileştirilmiş hali) saklanır.
/// </summary>
public class ActionPlan : BaseEntity
{
    public Guid ProblemId { get; private set; }
    public string PlanJson { get; private set; } = "{}";
    public DateTime GeneratedAt { get; private set; }

    // EF Core için gizli varsayılan yapıcı metot
    private ActionPlan() { }

    /// <summary>
    /// Yeni bir aksiyon planı kaydı oluşturur.
    /// </summary>
    public static ActionPlan Create(Guid problemId, string planJson)
    {
        if (problemId == Guid.Empty)
        {
            throw new ArgumentException("Problem ID boş olamaz.", nameof(problemId));
        }

        return new ActionPlan
        {
            ProblemId = problemId,
            PlanJson = string.IsNullOrWhiteSpace(planJson) ? "{}" : planJson,
            GeneratedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Mevcut aksiyon planı kaydını günceller.
    /// </summary>
    public void Update(string planJson)
    {
        PlanJson = string.IsNullOrWhiteSpace(planJson) ? "{}" : planJson;
        GeneratedAt = DateTime.UtcNow;
        SetUpdated();
    }
}
