using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;
using MSC.IdeaForge.Domain.ValueObjects;

namespace MSC.IdeaForge.Application.Opportunities.Commands;

/// <summary>
/// Fırsat puanlama komut nesnesi.
/// </summary>
public class ScoreOpportunityCommand
{
    public Guid ProblemId { get; set; }
    public int ProblemSeverity { get; set; }
    public int MarketSize { get; set; }
    public int FounderFit { get; set; }
    public int RiskScore { get; set; }
    public int TechnicalFeasibility { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Fırsat puanlama komutunu işleyen sınıf.
/// </summary>
public class ScoreOpportunityHandler
{
    private readonly IOpportunityRepository _repository;

    public ScoreOpportunityHandler(IOpportunityRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Kriter puanlarını alıp fırsat oluşturur veya mevcut olanı güncelleyip veritabanına kaydeder.
    /// </summary>
    public async Task<Guid> HandleAsync(ScoreOpportunityCommand command, CancellationToken cancellationToken = default)
    {
        // Kriter puanlarından değer nesnesini oluşturuyoruz.
        var score = new OpportunityScore(
            command.ProblemSeverity,
            command.MarketSize,
            command.FounderFit,
            command.RiskScore,
            command.TechnicalFeasibility
        );

        // Problem için daha önce oluşturulmuş bir fırsat olup olmadığını kontrol ediyoruz.
        var opportunity = await _repository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        if (opportunity is null)
        {
            // Yeni fırsat kaydı oluşturuluyor.
            opportunity = Opportunity.Create(command.ProblemId, score, command.Notes);
            await _repository.AddAsync(opportunity, cancellationToken);
        }
        else
        {
            // Mevcut fırsat kaydı puanları güncelleniyor.
            opportunity.UpdateScore(score, command.Notes);
            await _repository.UpdateAsync(opportunity, cancellationToken);
        }

        // Değişiklikleri veritabanına yansıtıyoruz.
        await _repository.SaveChangesAsync(cancellationToken);

        return opportunity.Id;
    }
}
