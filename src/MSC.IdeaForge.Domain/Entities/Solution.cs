using System;
using MSC.IdeaForge.Domain.Enums;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Bir probleme sunulan veya önerilen çözüm alternatiflerini temsil eden entity.
/// </summary>
public class Solution : BaseEntity
{
    public Guid ProblemId { get; private set; }
    public SolutionType SolutionType { get; private set; }
    public string Summary { get; private set; } = null!;
    public ComplexityLevel Complexity { get; private set; }
    public int EstimatedDurationMonths { get; private set; }
    public string? EstimatedCost { get; private set; }
    public string? Pros { get; private set; }
    public string? Cons { get; private set; }
    public bool IsRecommended { get; private set; }

    // EF Core için gizli varsayılan yapıcı metot
    private Solution() { }

    /// <summary>
    /// Yeni bir çözüm alternatifi oluşturur.
    /// </summary>
    public static Solution Create(
        Guid problemId,
        SolutionType solutionType,
        string summary,
        ComplexityLevel complexity,
        int estimatedDurationMonths,
        string? estimatedCost,
        string? pros,
        string? cons,
        bool isRecommended)
    {
        if (problemId == Guid.Empty)
        {
            throw new ArgumentException("Problem ID boş olamaz.", nameof(problemId));
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(summary);

        return new Solution
        {
            ProblemId = problemId,
            SolutionType = solutionType,
            Summary = summary,
            Complexity = complexity,
            EstimatedDurationMonths = estimatedDurationMonths,
            EstimatedCost = estimatedCost,
            Pros = pros,
            Cons = cons,
            IsRecommended = isRecommended
        };
    }

    /// <summary>
    /// Çözüm alternatifini günceller.
    /// </summary>
    public void Update(
        SolutionType solutionType,
        string summary,
        ComplexityLevel complexity,
        int estimatedDurationMonths,
        string? estimatedCost,
        string? pros,
        string? cons,
        bool isRecommended)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(summary);

        SolutionType = solutionType;
        Summary = summary;
        Complexity = complexity;
        EstimatedDurationMonths = estimatedDurationMonths;
        EstimatedCost = estimatedCost;
        Pros = pros;
        Cons = cons;
        IsRecommended = isRecommended;
        SetUpdated();
    }
}
