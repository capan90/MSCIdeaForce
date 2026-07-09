using MSC.IdeaForge.Application.BusinessCanvases.Commands;
using MSC.IdeaForge.Application.BusinessCanvases.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.BusinessCanvases.Queries;

/// <summary>
/// Belirtilen probleme ait Business Model Canvas verisini getiren sorgu işleyicisi.
/// </summary>
public class GetBusinessCanvasHandler
{
    private readonly IBusinessCanvasRepository _repository;

    public GetBusinessCanvasHandler(IBusinessCanvasRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Probleme ait canvas kaydını DTO formatında döner. Kayıt yoksa null döner.
    /// </summary>
    public async Task<BusinessCanvasDto?> HandleAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        var canvas = await _repository.GetByProblemIdAsync(problemId, cancellationToken);
        return canvas is null ? null : BusinessCanvasMapper.ToDto(canvas);
    }
}
