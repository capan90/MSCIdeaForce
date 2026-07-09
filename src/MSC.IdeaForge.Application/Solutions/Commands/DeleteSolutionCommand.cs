using System;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Solutions.Commands;

/// <summary>
/// Çözüm önerisi silme komut nesnesi.
/// </summary>
public class DeleteSolutionCommand
{
    public Guid ProblemId { get; set; }
    public Guid SolutionId { get; set; }
}

/// <summary>
/// Çözüm önerisi silme komut işleyicisi.
/// </summary>
public class DeleteSolutionHandler
{
    private readonly ISolutionRepository _repository;

    public DeleteSolutionHandler(ISolutionRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Çözüm önerisini veritabanından siler (soft delete).
    /// </summary>
    public async Task HandleAsync(DeleteSolutionCommand command, CancellationToken cancellationToken = default)
    {
        var solutions = await _repository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        var solution = System.Linq.Enumerable.FirstOrDefault(solutions, s => s.Id == command.SolutionId);

        if (solution is not null)
        {
            await _repository.DeleteAsync(solution, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
        }
    }
}
