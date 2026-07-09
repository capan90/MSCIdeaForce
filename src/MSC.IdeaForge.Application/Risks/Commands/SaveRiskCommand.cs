using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Risks.Commands;

/// <summary>
/// Bir probleme ait risk listesini kaydetme komut nesnesi.
/// Mevcut riskler silinip yerine verilen liste yazılır.
/// </summary>
public class SaveRiskCommand
{
    public Guid ProblemId { get; set; }
    public List<RiskInput> Risks { get; set; } = new();
}

/// <summary>
/// Kaydedilecek tek bir risk girdisi.
/// </summary>
public class RiskInput
{
    public string RiskName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Probability { get; set; }
    public int Impact { get; set; }
    public string MitigationStrategy { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// Risk listesini kaydetme komutunu işleyen sınıf.
/// </summary>
public class SaveRiskHandler
{
    private readonly IRiskRepository _repository;

    public SaveRiskHandler(IRiskRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Probleme ait mevcut riskleri silip verilen yeni risk listesini kaydeder.
    /// </summary>
    public async Task HandleAsync(SaveRiskCommand command, CancellationToken cancellationToken = default)
    {
        // Mevcut riskleri silinmiş olarak işaretliyoruz (temiz bir liste için)
        var existing = await _repository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        foreach (var risk in existing)
        {
            risk.Delete();
            await _repository.UpdateAsync(risk, cancellationToken);
        }

        // Yeni riskleri ekliyoruz
        foreach (var input in command.Risks)
        {
            var risk = RiskItem.Create(
                command.ProblemId,
                input.RiskName,
                input.Category,
                input.Probability,
                input.Impact,
                input.MitigationStrategy,
                input.Owner,
                input.Status
            );
            await _repository.AddAsync(risk, cancellationToken);
        }

        await _repository.SaveChangesAsync(cancellationToken);
    }
}
