using MSC.IdeaForge.Application.Checklists.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Checklists.Queries;

/// <summary>
/// Belirtilen probleme ait kontrol listesi öğelerini getiren sorgu işleyicisi.
/// </summary>
public class GetChecklistItemsHandler
{
    private readonly IChecklistRepository _repository;

    public GetChecklistItemsHandler(IChecklistRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Belirtilen probleme ait kontrol listesi öğelerini DTO listesi olarak döner.
    /// </summary>
    public async Task<List<ChecklistItemDto>> HandleAsync(Guid? problemId, CancellationToken cancellationToken = default)
    {
        var items = await _repository.GetByProblemIdAsync(problemId, cancellationToken);
        return items.Select(i => new ChecklistItemDto
        {
            Id = i.Id,
            Title = i.Title,
            Description = i.Description,
            Category = i.Category.ToString(),
            IsCompleted = i.IsCompleted,
            CompletedAt = i.CompletedAt,
            ProblemId = i.ProblemId,
            Priority = i.Priority,
            DueDate = i.DueDate
        }).ToList();
    }
}
