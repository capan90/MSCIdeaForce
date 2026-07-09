using MSC.IdeaForge.Application.BusinessCanvases.DTOs;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.BusinessCanvases.Commands;

/// <summary>
/// Business Model Canvas'ı elle düzenleyip kaydetme komut nesnesi.
/// </summary>
public class SaveBusinessCanvasCommand
{
    public Guid ProblemId { get; set; }
    public string KeyPartners { get; set; } = string.Empty;
    public string KeyActivities { get; set; } = string.Empty;
    public string KeyResources { get; set; } = string.Empty;
    public string ValuePropositions { get; set; } = string.Empty;
    public string CustomerRelationships { get; set; } = string.Empty;
    public string Channels { get; set; } = string.Empty;
    public string CustomerSegments { get; set; } = string.Empty;
    public string CostStructure { get; set; } = string.Empty;
    public string RevenueStreams { get; set; } = string.Empty;
}

/// <summary>
/// Business Model Canvas kaydetme/güncelleme komutunu işleyen sınıf.
/// </summary>
public class SaveBusinessCanvasHandler
{
    private readonly IBusinessCanvasRepository _canvasRepository;

    public SaveBusinessCanvasHandler(IBusinessCanvasRepository canvasRepository)
    {
        _canvasRepository = canvasRepository;
    }

    /// <summary>
    /// Mevcut canvas varsa günceller, yoksa yenisini oluşturur.
    /// </summary>
    public async Task<BusinessCanvasDto> HandleAsync(SaveBusinessCanvasCommand command, CancellationToken cancellationToken = default)
    {
        var canvas = await _canvasRepository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        if (canvas is null)
        {
            canvas = BusinessCanvas.Create(
                command.ProblemId,
                command.KeyPartners, command.KeyActivities, command.KeyResources,
                command.ValuePropositions, command.CustomerRelationships, command.Channels,
                command.CustomerSegments, command.CostStructure, command.RevenueStreams);
            await _canvasRepository.AddAsync(canvas, cancellationToken);
        }
        else
        {
            canvas.Update(
                command.KeyPartners, command.KeyActivities, command.KeyResources,
                command.ValuePropositions, command.CustomerRelationships, command.Channels,
                command.CustomerSegments, command.CostStructure, command.RevenueStreams);
            await _canvasRepository.UpdateAsync(canvas, cancellationToken);
        }

        await _canvasRepository.SaveChangesAsync(cancellationToken);
        return BusinessCanvasMapper.ToDto(canvas);
    }
}

/// <summary>
/// BusinessCanvas entity → DTO dönüşümü için ortak yardımcı.
/// </summary>
internal static class BusinessCanvasMapper
{
    public static BusinessCanvasDto ToDto(BusinessCanvas c) => new()
    {
        Id = c.Id,
        ProblemId = c.ProblemId,
        KeyPartners = c.KeyPartners,
        KeyActivities = c.KeyActivities,
        KeyResources = c.KeyResources,
        ValuePropositions = c.ValuePropositions,
        CustomerRelationships = c.CustomerRelationships,
        Channels = c.Channels,
        CustomerSegments = c.CustomerSegments,
        CostStructure = c.CostStructure,
        RevenueStreams = c.RevenueStreams,
        GeneratedAt = c.GeneratedAt
    };
}
