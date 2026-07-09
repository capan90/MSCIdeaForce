using System;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Enums;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Knowledge.Commands;

/// <summary>
/// Bilgi bankası girdisi oluşturma komut nesnesi.
/// </summary>
public class CreateKnowledgeCommand
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public KnowledgeCategory Category { get; set; }
    public string? Tags { get; set; }
    public Guid? ProblemId { get; set; }
    public string? Source { get; set; }
    public DateTime LearnedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Bilgi bankası girdisi oluşturma komut işleyicisi.
/// </summary>
public class CreateKnowledgeHandler
{
    private readonly IKnowledgeRepository _repository;

    public CreateKnowledgeHandler(IKnowledgeRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Yeni bir bilgi bankası girdisi oluşturur ve kaydeder.
    /// </summary>
    public async Task<Guid> HandleAsync(CreateKnowledgeCommand command, CancellationToken cancellationToken = default)
    {
        var entry = KnowledgeEntry.Create(
            command.Title,
            command.Content,
            command.Category,
            command.Tags,
            command.ProblemId,
            command.Source,
            command.LearnedAt
        );

        await _repository.AddAsync(entry, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return entry.Id;
    }
}
