using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Researches.Commands;

/// <summary>
/// Araştırma verilerini manuel olarak kaydetmek / güncellemek için kullanılan komut nesnesi.
/// </summary>
public class SaveResearchCommand
{
    public Guid ProblemId { get; set; }
    public string? MarketAnalysis { get; set; }
    public string? CompetitorSummary { get; set; }
    public string? TechnologyNotes { get; set; }
    public string? TrendNotes { get; set; }
    public string? Sources { get; set; }
}

/// <summary>
/// Manuel araştırma kaydetme komutunu işleyen sınıf.
/// </summary>
public class SaveResearchHandler
{
    private readonly IResearchRepository _repository;

    public SaveResearchHandler(IResearchRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Probleme ait mevcut araştırma kaydı varsa günceller, yoksa yeni kayıt oluşturur.
    /// </summary>
    public async Task HandleAsync(SaveResearchCommand command, CancellationToken cancellationToken = default)
    {
        var research = await _repository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        if (research is null)
        {
            research = Research.Create(
                command.ProblemId,
                command.MarketAnalysis,
                command.CompetitorSummary,
                command.TechnologyNotes,
                command.TrendNotes,
                command.Sources
            );
            await _repository.AddAsync(research, cancellationToken);
        }
        else
        {
            research.Update(
                command.MarketAnalysis,
                command.CompetitorSummary,
                command.TechnologyNotes,
                command.TrendNotes,
                command.Sources
            );
            await _repository.UpdateAsync(research, cancellationToken);
        }

        await _repository.SaveChangesAsync(cancellationToken);
    }
}
