using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Enums;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Signals.Commands;

/// <summary>
/// Sinyal oluşturma komut nesnesi.
/// </summary>
public class CreateSignalCommand
{
    public Guid ProblemId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SignalType SignalType { get; set; } = SignalType.Manual;
    public SeverityLevel Severity { get; set; } = SeverityLevel.Medium;
}

/// <summary>
/// Sinyal oluşturma komutunu işleyen sınıf.
/// </summary>
public class CreateSignalHandler
{
    private readonly ISignalRepository _repository;

    public CreateSignalHandler(ISignalRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Verilen komut detayları ile yeni bir sinyal kaydeder.
    /// </summary>
    public async Task<Guid> HandleAsync(CreateSignalCommand command, CancellationToken cancellationToken = default)
    {
        // Domain nesnesini oluşturuyoruz.
        var signal = Signal.Create(
            command.ProblemId,
            command.Title,
            command.Description,
            command.SignalType,
            command.Severity
        );

        // Veritabanına kaydediyoruz.
        await _repository.AddAsync(signal, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return signal.Id;
    }
}
