using System;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Bir probleme ve önerilen çözüme ait MVP (Minimum Viable Product) plan detaylarını temsil eden entity.
/// </summary>
public class MVPPlan : BaseEntity
{
    public Guid ProblemId { get; private set; }
    public string? Scope { get; private set; }
    public string? UserStories { get; private set; } // JSON array formatında saklanır
    public string? SprintPlan { get; private set; } // JSON array formatında saklanır
    public string? TechStack { get; private set; }
    public string? Risks { get; private set; }
    public string? FirstSalesPlan { get; private set; }

    // EF Core için gizli varsayılan yapıcı metot
    private MVPPlan() { }

    /// <summary>
    /// Yeni bir MVP planı oluşturur.
    /// </summary>
    public static MVPPlan Create(
        Guid problemId,
        string? scope,
        string? userStories,
        string? sprintPlan,
        string? techStack,
        string? risks,
        string? firstSalesPlan)
    {
        if (problemId == Guid.Empty)
        {
            throw new ArgumentException("Problem ID boş olamaz.", nameof(problemId));
        }

        return new MVPPlan
        {
            ProblemId = problemId,
            Scope = scope,
            UserStories = userStories,
            SprintPlan = sprintPlan,
            TechStack = techStack,
            Risks = risks,
            FirstSalesPlan = firstSalesPlan
        };
    }

    /// <summary>
    /// MVP planını günceller.
    /// </summary>
    public void Update(
        string? scope,
        string? userStories,
        string? sprintPlan,
        string? techStack,
        string? risks,
        string? firstSalesPlan)
    {
        Scope = scope;
        UserStories = userStories;
        SprintPlan = sprintPlan;
        TechStack = techStack;
        Risks = risks;
        FirstSalesPlan = firstSalesPlan;
        SetUpdated();
    }
}
