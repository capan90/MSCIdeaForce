using System.Text.Json;
using MSC.IdeaForge.Application.Problems.DTOs;
using MSC.IdeaForge.Domain.Enums;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Problems.Commands;

/// <summary>
/// Problemin fikir yaşam döngüsü durumunu (IdeaStatus) güncelleme komut nesnesi.
/// </summary>
public class UpdateIdeaStatusCommand
{
    public Guid ProblemId { get; set; }
    public IdeaStatus IdeaStatus { get; set; }
}

/// <summary>
/// Fikir durumunu güncelleyen ve durum geçmişini tutan işleyici.
/// </summary>
public class UpdateIdeaStatusHandler
{
    private readonly IProblemRepository _repository;

    public UpdateIdeaStatusHandler(IProblemRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Problemin fikir durumunu günceller ve durum geçmişine yeni bir kayıt ekler.
    /// </summary>
    public async Task HandleAsync(UpdateIdeaStatusCommand command, CancellationToken cancellationToken = default)
    {
        var problem = await _repository.GetByIdAsync(command.ProblemId, cancellationToken);
        if (problem is null)
        {
            throw new KeyNotFoundException($"ID'si {command.ProblemId} olan problem bulunamadı.");
        }

        // Aynı duruma tekrar geçiş yapılıyorsa gereksiz güncellemeyi atlıyoruz
        if (problem.IdeaStatus == command.IdeaStatus)
        {
            return;
        }

        // Mevcut geçmişi çözümleyip yeni kaydı ekliyoruz
        var history = ParseHistory(problem.IdeaStatusHistory);
        history.Add(new IdeaStatusHistoryEntry
        {
            Status = command.IdeaStatus.ToString(),
            ChangedAt = DateTime.UtcNow
        });

        var historyJson = JsonSerializer.Serialize(history);
        problem.ChangeIdeaStatus(command.IdeaStatus, historyJson);

        await _repository.UpdateAsync(problem, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    private static List<IdeaStatusHistoryEntry> ParseHistory(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return new();
        }
        try
        {
            return JsonSerializer.Deserialize<List<IdeaStatusHistoryEntry>>(json) ?? new();
        }
        catch
        {
            return new();
        }
    }
}
