using System;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Enums;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Decisions.Commands;

/// <summary>
/// Yeni karar ekleme komut nesnesi.
/// </summary>
public class CreateDecisionCommand
{
    public Guid ProblemId { get; set; }
    public DecisionType DecisionType { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime DecisionDate { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Karar ekleme komutunu işleyen sınıf.
/// </summary>
public class CreateDecisionHandler
{
    private readonly IDecisionRepository _repository;

    public CreateDecisionHandler(IDecisionRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Kararı veritabanına ekler ve değişiklikleri kaydeder.
    /// </summary>
    public async Task<Guid> HandleAsync(CreateDecisionCommand command, CancellationToken cancellationToken = default)
    {
        var decision = Decision.Create(
            command.ProblemId,
            command.DecisionType,
            command.Reason,
            command.DecisionDate
        );

        await _repository.AddAsync(decision, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return decision.Id;
    }
}
