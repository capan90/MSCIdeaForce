using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Problems.Commands;

/// <summary>
/// Problem silme komut nesnesi.
/// </summary>
public class DeleteProblemCommand
{
    public Guid Id { get; set; }
}

/// <summary>
/// Problem silme komutunu işleyen sınıf.
/// </summary>
public class DeleteProblemHandler
{
    private readonly IProblemRepository _repository;

    public DeleteProblemHandler(IProblemRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Verilen ID'ye sahip problemi bulur, silindi olarak işaretler ve veritabanına kaydeder.
    /// </summary>
    /// <param name="id">Silinecek problemin benzersiz kimliği</param>
    /// <param name="cancellationToken">İptal belirteci</param>
    /// <exception cref="KeyNotFoundException">Belirtilen ID'ye sahip problem bulunamazsa fırlatılır.</exception>
    public async Task HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Veritabanından problemi sorguluyoruz.
        var problem = await _repository.GetByIdAsync(id, cancellationToken);
        if (problem is null)
        {
            throw new KeyNotFoundException($"ID'si {id} olan problem bulunamadı.");
        }

        // Domain nesnesi üzerinden silme (soft delete) işlemini gerçekleştiriyoruz.
        problem.Delete();

        // Değişiklikleri veritabanına yansıtıyoruz.
        await _repository.UpdateAsync(problem, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
