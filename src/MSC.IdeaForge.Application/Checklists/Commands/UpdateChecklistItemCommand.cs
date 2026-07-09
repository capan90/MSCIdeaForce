using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Checklists.Commands;

/// <summary>
/// Bir kontrol listesi öğesinin tamamlanma durumunu güncelleme komut nesnesi.
/// </summary>
public class UpdateChecklistItemCommand
{
    public Guid ItemId { get; set; }
    public bool IsCompleted { get; set; }
}

/// <summary>
/// Kontrol listesi öğesinin tamamlanma durumunu güncelleyen işleyici.
/// </summary>
public class UpdateChecklistItemHandler
{
    private readonly IChecklistRepository _repository;

    public UpdateChecklistItemHandler(IChecklistRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Öğeyi bulur ve tamamlandı/tamamlanmadı olarak işaretler.
    /// </summary>
    public async Task HandleAsync(UpdateChecklistItemCommand command, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetByIdAsync(command.ItemId, cancellationToken);
        if (item is null)
        {
            return;
        }

        item.SetCompleted(command.IsCompleted);
        await _repository.UpdateAsync(item, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
