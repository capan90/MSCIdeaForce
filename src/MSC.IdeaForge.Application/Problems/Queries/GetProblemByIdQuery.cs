using MSC.IdeaForge.Application.Problems.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Problems.Queries;

/// <summary>
/// Belirtilen benzersiz kimliğe (Id) sahip problemi getiren sorgu işleyicisi.
/// </summary>
public class GetProblemByIdHandler
{
    private readonly IProblemRepository _repository;

    public GetProblemByIdHandler(IProblemRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Problemi veri tabanından sorgular ve DTO formatında döndürür.
    /// </summary>
    /// <param name="id">Problemin benzersiz kimliği</param>
    /// <param name="cancellationToken">İptal belirteci</param>
    /// <returns>Bulunursa ProblemDto nesnesi, bulunamazsa null döndürür.</returns>
    public async Task<ProblemDto?> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Repository üzerinden problemi ID ile getiriyoruz.
        var problem = await _repository.GetByIdAsync(id, cancellationToken);
        if (problem is null)
        {
            return null;
        }

        // Veri tabanı varlığını DTO nesnesine eşliyoruz.
        return new ProblemDto
        {
            Id = problem.Id,
            Title = problem.Title,
            Description = problem.Description,
            Status = problem.Status.ToString(),
            Priority = problem.Priority.ToString(),
            Sector = problem.Sector,
            Tags = problem.Tags,
            Source = problem.Source,
            CreatedAt = problem.CreatedAt
        };
    }
}
