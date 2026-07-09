using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Checklists.Commands;

/// <summary>
/// Bir kontrol listesi öğesini silme komut nesnesi.
/// </summary>
public class DeleteChecklistItemCommand
{
    public Guid ItemId { get; set; }
}

/// <summary>
/// Kontrol listesi öğesi silme komutunu işleyen sınıf (soft delete).
/// </summary>
public class DeleteChecklistItemHandler
{
    private readonly IChecklistRepository _repository;

    public DeleteChecklistItemHandler(IChecklistRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Belirtilen öğeyi bulur ve silinmiş olarak işaretler.
    /// </summary>
    public async Task HandleAsync(DeleteChecklistItemCommand command, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetByIdAsync(command.ItemId, cancellationToken);
        if (item is null)
        {
            return;
        }

        item.Delete();
        await _repository.UpdateAsync(item, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
