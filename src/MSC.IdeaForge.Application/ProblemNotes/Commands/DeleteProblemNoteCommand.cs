using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.ProblemNotes.Commands;

/// <summary>
/// Bir problem notunu silme komut nesnesi.
/// </summary>
public class DeleteProblemNoteCommand
{
    public Guid NoteId { get; set; }
}

/// <summary>
/// Problem notu silme komutunu işleyen sınıf (soft delete).
/// </summary>
public class DeleteProblemNoteHandler
{
    private readonly IProblemNoteRepository _repository;

    public DeleteProblemNoteHandler(IProblemNoteRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Belirtilen notu bulur ve silinmiş olarak işaretler.
    /// </summary>
    public async Task HandleAsync(DeleteProblemNoteCommand command, CancellationToken cancellationToken = default)
    {
        var note = await _repository.GetByIdAsync(command.NoteId, cancellationToken);
        if (note is null)
        {
            return;
        }

        note.Delete();
        await _repository.UpdateAsync(note, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
