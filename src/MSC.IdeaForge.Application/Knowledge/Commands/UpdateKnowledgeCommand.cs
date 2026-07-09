using System;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Domain.Enums;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Knowledge.Commands;

/// <summary>
/// Bilgi bankası girdisi güncelleme komut nesnesi.
/// </summary>
public class UpdateKnowledgeCommand
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public KnowledgeCategory Category { get; set; }
    public string? Tags { get; set; }
    public Guid? ProblemId { get; set; }
    public string? Source { get; set; }
    public DateTime LearnedAt { get; set; }
}

/// <summary>
/// Bilgi bankası girdisi güncellemeyi işleyen sınıf.
/// </summary>
public class UpdateKnowledgeHandler
{
    private readonly IKnowledgeRepository _repository;

    public UpdateKnowledgeHandler(IKnowledgeRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Bilgi bankası girdisini veritabanında günceller ve kaydeder.
    /// </summary>
    public async Task HandleAsync(UpdateKnowledgeCommand command, CancellationToken cancellationToken = default)
    {
        var entries = await _repository.GetAllAsync(cancellationToken);
        var entry = System.Linq.Enumerable.FirstOrDefault(entries, e => e.Id == command.Id);

        if (entry is null)
        {
            throw new KeyNotFoundException($"ID'si {command.Id} olan bilgi bankası girdisi bulunamadı.");
        }

        entry.Update(
            command.Title,
            command.Content,
            command.Category,
            command.Tags,
            command.ProblemId,
            command.Source,
            command.LearnedAt
        );

        await _repository.UpdateAsync(entry, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
