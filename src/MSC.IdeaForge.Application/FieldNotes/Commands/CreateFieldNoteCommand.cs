using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Enums;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.FieldNotes.Commands;

/// <summary>
/// Yeni bir saha notu oluşturma komut nesnesi.
/// </summary>
public class CreateFieldNoteCommand
{
    public string Title { get; set; } = string.Empty;
    public string? Location { get; set; }
    public DateTime ObservationDate { get; set; }
    public string Content { get; set; } = string.Empty;
    public FieldNoteSource Source { get; set; }
    public string? Tags { get; set; }
    public Guid? ProblemId { get; set; }
    public bool HasPhoto { get; set; }
}

/// <summary>
/// Saha notu oluşturma komutunu işleyen sınıf.
/// </summary>
public class CreateFieldNoteHandler
{
    private readonly IFieldNoteRepository _repository;

    public CreateFieldNoteHandler(IFieldNoteRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Yeni bir saha notu oluşturur ve kaydeder.
    /// </summary>
    public async Task<Guid> HandleAsync(CreateFieldNoteCommand command, CancellationToken cancellationToken = default)
    {
        var note = FieldNote.Create(
            command.Title,
            command.Location,
            command.ObservationDate,
            command.Content,
            command.Source,
            command.Tags,
            command.ProblemId,
            command.HasPhoto
        );

        await _repository.AddAsync(note, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return note.Id;
    }
}
