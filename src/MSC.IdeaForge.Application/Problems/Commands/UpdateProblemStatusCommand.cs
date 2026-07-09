using MSC.IdeaForge.Domain.Enums;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Problems.Commands;

/// <summary>
/// Problem durumunu güncelleme komut nesnesi.
/// </summary>
public class UpdateProblemStatusCommand
{
    public Guid Id { get; set; }
    public ProblemStatus Status { get; set; }
}

/// <summary>
/// Problem durumunu güncelleme komutunu işleyen sınıf.
/// </summary>
public class UpdateProblemStatusHandler
{
    private readonly IProblemRepository _repository;

    public UpdateProblemStatusHandler(IProblemRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Verilen ID'ye sahip problemi bulur, durumunu günceller ve veritabanına kaydeder.
    /// </summary>
    /// <param name="id">Durumu güncellenecek problemin benzersiz kimliği</param>
    /// <param name="newStatus">Yeni durum değeri</param>
    /// <param name="cancellationToken">İptal belirteci</param>
    /// <exception cref="KeyNotFoundException">Belirtilen ID'ye sahip problem bulunamazsa fırlatılır.</exception>
    public async Task HandleAsync(Guid id, ProblemStatus newStatus, CancellationToken cancellationToken = default)
    {
        // Veritabanından problemi sorguluyoruz.
        var problem = await _repository.GetByIdAsync(id, cancellationToken);
        if (problem is null)
        {
            throw new KeyNotFoundException($"ID'si {id} olan problem bulunamadı.");
        }

        // Domain nesnesi üzerinden durum güncelleme işlemini gerçekleştiriyoruz.
        problem.ChangeStatus(newStatus);

        // Değişiklikleri veritabanına kaydediyoruz.
        await _repository.UpdateAsync(problem, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
