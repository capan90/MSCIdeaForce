using System;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Enums;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Solutions.Commands;

/// <summary>
/// Çözüm önerisi kaydetme komut nesnesi.
/// </summary>
public class SaveSolutionCommand
{
    public Guid? Id { get; set; }
    public Guid ProblemId { get; set; }
    public SolutionType SolutionType { get; set; }
    public string Summary { get; set; } = string.Empty;
    public ComplexityLevel Complexity { get; set; }
    public int EstimatedDurationMonths { get; set; }
    public string? EstimatedCost { get; set; }
    public string? Pros { get; set; }
    public string? Cons { get; set; }
    public bool IsRecommended { get; set; }
}

/// <summary>
/// Çözüm önerisi kaydetme komut işleyicisi.
/// </summary>
public class SaveSolutionHandler
{
    private readonly ISolutionRepository _repository;

    public SaveSolutionHandler(ISolutionRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Yeni çözüm kaydeder veya mevcut çözümü günceller.
    /// </summary>
    public async Task<Guid> HandleAsync(SaveSolutionCommand command, CancellationToken cancellationToken = default)
    {
        Solution? solution = null;

        if (command.Id.HasValue && command.Id != Guid.Empty)
        {
            var existing = await _repository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
            solution = System.Linq.Enumerable.FirstOrDefault(existing, s => s.Id == command.Id.Value);
        }

        if (solution is null)
        {
            solution = Solution.Create(
                command.ProblemId,
                command.SolutionType,
                command.Summary,
                command.Complexity,
                command.EstimatedDurationMonths,
                command.EstimatedCost,
                command.Pros,
                command.Cons,
                command.IsRecommended
            );
            await _repository.AddAsync(solution, cancellationToken);
        }
        else
        {
            solution.Update(
                command.SolutionType,
                command.Summary,
                command.Complexity,
                command.EstimatedDurationMonths,
                command.EstimatedCost,
                command.Pros,
                command.Cons,
                command.IsRecommended
            );
            await _repository.UpdateAsync(solution, cancellationToken);
        }

        await _repository.SaveChangesAsync(cancellationToken);
        return solution.Id;
    }
}
