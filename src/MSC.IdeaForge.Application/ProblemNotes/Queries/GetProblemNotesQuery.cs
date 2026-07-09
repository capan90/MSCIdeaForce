using MSC.IdeaForge.Application.ProblemNotes.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.ProblemNotes.Queries;

/// <summary>
/// Belirtilen probleme ait notları getiren sorgu işleyicisi.
/// </summary>
public class GetProblemNotesHandler
{
    private readonly IProblemNoteRepository _repository;

    public GetProblemNotesHandler(IProblemNoteRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Probleme ait notları (yeniden eskiye) DTO listesi olarak döner.
    /// </summary>
    public async Task<List<ProblemNoteDto>> HandleAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        var notes = await _repository.GetByProblemIdAsync(problemId, cancellationToken);
        return notes.Select(n => new ProblemNoteDto
        {
            Id = n.Id,
            ProblemId = n.ProblemId,
            Content = n.Content,
            NoteType = n.NoteType.ToString(),
            CreatedAt = n.CreatedAt
        }).ToList();
    }
}
