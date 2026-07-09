using MSC.IdeaForge.Application.FieldNotes.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.FieldNotes.Queries;

/// <summary>
/// Saha notlarını getiren sorgu işleyicisi.
/// </summary>
public class GetFieldNotesHandler
{
    private readonly IFieldNoteRepository _repository;

    public GetFieldNotesHandler(IFieldNoteRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Tüm saha notlarını (gözlem tarihine göre) DTO listesi olarak döner.
    /// </summary>
    public async Task<List<FieldNoteDto>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var notes = await _repository.GetAllAsync(cancellationToken);
        return notes.Select(n => new FieldNoteDto
        {
            Id = n.Id,
            Title = n.Title,
            Location = n.Location,
            ObservationDate = n.ObservationDate,
            Content = n.Content,
            Source = n.Source.ToString(),
            Tags = n.Tags,
            ProblemId = n.ProblemId,
            HasPhoto = n.HasPhoto
        }).ToList();
    }
}
