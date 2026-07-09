using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Search;

/// <summary>
/// Sistem geneli aramayı gerçekleştiren sorgu işleyicisi.
/// Problemler, bilgi bankası ve saha notlarında arama yapar.
/// </summary>
public class GlobalSearchHandler
{
    private readonly IProblemRepository _problemRepository;
    private readonly IKnowledgeRepository _knowledgeRepository;
    private readonly IFieldNoteRepository _fieldNoteRepository;

    public GlobalSearchHandler(
        IProblemRepository problemRepository,
        IKnowledgeRepository knowledgeRepository,
        IFieldNoteRepository fieldNoteRepository)
    {
        _problemRepository = problemRepository;
        _knowledgeRepository = knowledgeRepository;
        _fieldNoteRepository = fieldNoteRepository;
    }

    /// <summary>
    /// Verilen arama metnini problem başlık/açıklamalarında, bilgi bankası ve saha notu başlıklarında arar.
    /// </summary>
    public async Task<GlobalSearchDto> HandleAsync(string query, CancellationToken cancellationToken = default)
    {
        var result = new GlobalSearchDto();

        // Boş arama için boş sonuç döneriz
        if (string.IsNullOrWhiteSpace(query))
        {
            return result;
        }

        var term = query.Trim();

        // Problemler: başlık ve açıklamada arama
        var problems = await _problemRepository.GetAllAsync(cancellationToken);
        result.Problems = problems
            .Where(p => Contains(p.Title, term) || Contains(p.Description, term))
            .Take(10)
            .Select(p => new SearchResultItem { Id = p.Id, Title = p.Title, Url = $"/problems/{p.Id}" })
            .ToList();

        // Bilgi Bankası: başlıkta arama
        var knowledge = await _knowledgeRepository.GetAllAsync(cancellationToken);
        result.KnowledgeEntries = knowledge
            .Where(k => Contains(k.Title, term))
            .Take(10)
            .Select(k => new SearchResultItem { Id = k.Id, Title = k.Title, Url = "/knowledge" })
            .ToList();

        // Saha Notları: başlıkta arama
        var fieldNotes = await _fieldNoteRepository.GetAllAsync(cancellationToken);
        result.FieldNotes = fieldNotes
            .Where(f => Contains(f.Title, term))
            .Take(10)
            .Select(f => new SearchResultItem { Id = f.Id, Title = f.Title, Url = "/field-intelligence" })
            .ToList();

        return result;
    }

    // Büyük/küçük harf duyarsız içerir kontrolü
    private static bool Contains(string? source, string term)
        => !string.IsNullOrEmpty(source) && source.Contains(term, StringComparison.OrdinalIgnoreCase);
}
