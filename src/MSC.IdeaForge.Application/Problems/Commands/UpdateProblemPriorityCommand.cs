using System;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Domain.Enums;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Problems.Commands;

/// <summary>
/// Problem öncelik seviyesini güncelleme komut nesnesi.
/// </summary>
public class UpdateProblemPriorityCommand
{
    public Guid ProblemId { get; set; }
    public PriorityLevel Priority { get; set; }
}

/// <summary>
/// Problem öncelik seviyesini güncellemeyi işleyen sınıf.
/// </summary>
public class UpdateProblemPriorityHandler
{
    private readonly IProblemRepository _repository;

    public UpdateProblemPriorityHandler(IProblemRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Problem öncelik seviyesini günceller ve kaydeder.
    /// </summary>
    public async Task HandleAsync(UpdateProblemPriorityCommand command, CancellationToken cancellationToken = default)
    {
        var problem = await _repository.GetByIdAsync(command.ProblemId, cancellationToken);
        if (problem is null)
        {
            throw new KeyNotFoundException($"ID'si {command.ProblemId} olan problem bulunamadı.");
        }

        problem.SetPriority(command.Priority);

        await _repository.UpdateAsync(problem, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
