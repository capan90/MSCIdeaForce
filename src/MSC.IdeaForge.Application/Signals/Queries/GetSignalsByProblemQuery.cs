using MSC.IdeaForge.Application.Signals.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Signals.Queries;

/// <summary>
/// Bir probleme ait sinyalleri getiren sorgu işleyicisi.
/// </summary>
public class GetSignalsByProblemHandler
{
    private readonly ISignalRepository _repository;

    public GetSignalsByProblemHandler(ISignalRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Belirtilen problem ID'sine ait sinyalleri DTO formatında döner.
    /// </summary>
    public async Task<IEnumerable<SignalDto>> HandleAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        var signals = await _repository.GetByProblemIdAsync(problemId, cancellationToken);
        return signals.Select(s => new SignalDto
        {
            Id = s.Id,
            ProblemId = s.ProblemId,
            Title = s.Title,
            Description = s.Description,
            SignalType = s.SignalType.ToString(),
            Severity = s.Severity.ToString(),
            CreatedAt = s.CreatedAt
        });
    }
}
