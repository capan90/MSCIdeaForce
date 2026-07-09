using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Enums;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.ProblemNotes.Commands;

/// <summary>
/// Yeni bir problem notu oluşturma komut nesnesi.
/// </summary>
public class CreateProblemNoteCommand
{
    public Guid ProblemId { get; set; }
    public string Content { get; set; } = string.Empty;
    public NoteType NoteType { get; set; }
}

/// <summary>
/// Problem notu oluşturma komutunu işleyen sınıf.
/// </summary>
public class CreateProblemNoteHandler
{
    private readonly IProblemNoteRepository _repository;

    public CreateProblemNoteHandler(IProblemNoteRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Yeni bir problem notu oluşturur ve kaydeder.
    /// </summary>
    public async Task<Guid> HandleAsync(CreateProblemNoteCommand command, CancellationToken cancellationToken = default)
    {
        var note = ProblemNote.Create(command.ProblemId, command.Content, command.NoteType);
        await _repository.AddAsync(note, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return note.Id;
    }
}
