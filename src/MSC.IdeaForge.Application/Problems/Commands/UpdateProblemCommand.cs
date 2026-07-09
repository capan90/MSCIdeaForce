using System;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Problems.Commands;

/// <summary>
/// Problem bilgilerini güncelleme komut nesnesi.
/// </summary>
public class UpdateProblemCommand
{
    public Guid ProblemId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Sector { get; set; }
    public string? Source { get; set; }
}

/// <summary>
/// Problem bilgilerini güncellemeyi işleyen sınıf.
/// </summary>
public class UpdateProblemHandler
{
    private readonly IProblemRepository _repository;

    public UpdateProblemHandler(IProblemRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Problem bilgilerini günceller ve kaydeder.
    /// </summary>
    public async Task HandleAsync(UpdateProblemCommand command, CancellationToken cancellationToken = default)
    {
        var problem = await _repository.GetByIdAsync(command.ProblemId, cancellationToken);
        if (problem is null)
        {
            throw new KeyNotFoundException($"ID'si {command.ProblemId} olan problem bulunamadı.");
        }

        problem.Update(
            command.Title,
            command.Description,
            command.Sector,
            problem.Tags, // Mevcut etiketleri koruyoruz
            command.Source
        );

        await _repository.UpdateAsync(problem, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
