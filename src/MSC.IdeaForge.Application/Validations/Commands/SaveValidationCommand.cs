using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Enums;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Validations.Commands;

/// <summary>
/// Doğrulama kaydını kaydetme / güncelleme komut nesnesi.
/// </summary>
public class SaveValidationCommand
{
    public Guid ProblemId { get; set; }
    public int InterviewCount { get; set; }
    public int ValidatedUserCount { get; set; }
    public int WillingToPayCount { get; set; }
    public int ConfidenceScore { get; set; }
    public string? Notes { get; set; }
    public string? Risks { get; set; }
    public ValidationStatus Status { get; set; }
}

/// <summary>
/// Doğrulama kaydını kaydetme komutunu işleyen sınıf.
/// </summary>
public class SaveValidationHandler
{
    private readonly IValidationRepository _repository;

    public SaveValidationHandler(IValidationRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Problem için mevcut bir doğrulama kaydı varsa günceller, yoksa yenisini oluşturup kaydeder.
    /// </summary>
    public async Task<Guid> HandleAsync(SaveValidationCommand command, CancellationToken cancellationToken = default)
    {
        var validation = await _repository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        if (validation is null)
        {
            // Yeni doğrulama kaydı oluşturuyoruz.
            validation = Validation.Create(
                command.ProblemId,
                command.InterviewCount,
                command.ValidatedUserCount,
                command.WillingToPayCount,
                command.ConfidenceScore,
                command.Notes,
                command.Risks,
                command.Status
            );
            await _repository.AddAsync(validation, cancellationToken);
        }
        else
        {
            // Mevcut doğrulama kaydını güncelliyoruz.
            validation.Update(
                command.InterviewCount,
                command.ValidatedUserCount,
                command.WillingToPayCount,
                command.ConfidenceScore,
                command.Notes,
                command.Risks,
                command.Status
            );
            await _repository.UpdateAsync(validation, cancellationToken);
        }

        // Değişiklikleri veritabanına yansıtıyoruz.
        await _repository.SaveChangesAsync(cancellationToken);
        return validation.Id;
    }
}
