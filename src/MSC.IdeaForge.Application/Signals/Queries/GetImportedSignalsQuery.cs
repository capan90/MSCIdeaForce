using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Application.Signals.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Signals.Queries;

/// <summary>
/// İçe aktarılmış ham sinyalleri sorgulamak ve listelemek için kullanılan sorgu işleyicisi.
/// </summary>
public class GetImportedSignalsHandler
{
    private readonly IImportedSignalRepository _repository;

    public GetImportedSignalsHandler(IImportedSignalRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Tüm içe aktarılmış sinyalleri getirir ve DTO listesi olarak döner.
    /// </summary>
    public async Task<IEnumerable<ImportedSignalDto>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var signals = await _repository.GetAllAsync(cancellationToken);

        return signals.Select(s => new ImportedSignalDto
        {
            Id = s.Id,
            SourceType = s.SourceType.ToString(),
            SourceUrl = s.SourceUrl,
            RawContent = s.RawContent,
            Title = s.Title,
            Summary = s.Summary,
            ImportedAt = s.ImportedAt,
            IsProcessed = s.IsProcessed,
            ProblemId = s.ProblemId
        });
    }
}
