using System;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Knowledge.Commands;

/// <summary>
/// Bilgi bankası girdisi silme komut nesnesi.
/// </summary>
public class DeleteKnowledgeCommand
{
    public Guid Id { get; set; }
}

/// <summary>
/// Bilgi bankası girdisi silme komut işleyicisi.
/// </summary>
public class DeleteKnowledgeHandler
{
    private readonly IKnowledgeRepository _repository;

    public DeleteKnowledgeHandler(IKnowledgeRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Bilgi bankası girdisini veritabanından siler (soft delete).
    /// </summary>
    public async Task HandleAsync(DeleteKnowledgeCommand command, CancellationToken cancellationToken = default)
    {
        var entries = await _repository.GetAllAsync(cancellationToken);
        var entry = System.Linq.Enumerable.FirstOrDefault(entries, e => e.Id == command.Id);

        if (entry is not null)
        {
            await _repository.DeleteAsync(entry, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
        }
    }
}
