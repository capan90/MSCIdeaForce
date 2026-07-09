using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Application.Knowledge.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Knowledge.Queries;

/// <summary>
/// Tüm bilgi bankası girdilerini listeleyen sorgu işleyicisi.
/// </summary>
public class GetAllKnowledgeHandler
{
    private readonly IKnowledgeRepository _repository;

    public GetAllKnowledgeHandler(IKnowledgeRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Bilgi bankasındaki tüm girdileri DTO listesi olarak döner.
    /// </summary>
    public async Task<IEnumerable<KnowledgeDto>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var entries = await _repository.GetAllAsync(cancellationToken);

        return entries.Select(e => new KnowledgeDto
        {
            Id = e.Id,
            Title = e.Title,
            Content = e.Content,
            Category = e.Category.ToString(),
            Tags = e.Tags,
            ProblemId = e.ProblemId,
            Source = e.Source,
            LearnedAt = e.LearnedAt
        });
    }
}
