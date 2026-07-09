using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Enums;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Checklists.Commands;

/// <summary>
/// Yeni bir kontrol listesi öğesi oluşturma komut nesnesi.
/// </summary>
public class CreateChecklistItemCommand
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ChecklistCategory Category { get; set; }
    public int Priority { get; set; } = 3;
    public Guid? ProblemId { get; set; }
    public DateTime? DueDate { get; set; }
}

/// <summary>
/// Kontrol listesi öğesi oluşturma komutunu işleyen sınıf.
/// </summary>
public class CreateChecklistItemHandler
{
    private readonly IChecklistRepository _repository;

    public CreateChecklistItemHandler(IChecklistRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Yeni bir kontrol listesi öğesi oluşturur ve kaydeder.
    /// </summary>
    public async Task<Guid> HandleAsync(CreateChecklistItemCommand command, CancellationToken cancellationToken = default)
    {
        var item = ChecklistItem.Create(
            command.Title,
            command.Description,
            command.Category,
            command.Priority,
            command.ProblemId,
            command.DueDate
        );
        await _repository.AddAsync(item, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return item.Id;
    }
}
