using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.FounderProfiles.Commands;

/// <summary>
/// Kurucu profilini kaydetme / güncelleme komut nesnesi.
/// </summary>
public class SaveFounderProfileCommand
{
    public string Name { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    public string Industries { get; set; } = string.Empty;
    public string Experience { get; set; } = string.Empty;
    public string Network { get; set; } = string.Empty;
    public string Capital { get; set; } = string.Empty;
    public string Interests { get; set; } = string.Empty;
    public string? Bio { get; set; }
}

/// <summary>
/// Kurucu profilini kaydetme komutunu işleyen sınıf.
/// </summary>
public class SaveFounderProfileHandler
{
    private readonly IFounderProfileRepository _repository;

    public SaveFounderProfileHandler(IFounderProfileRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Mevcut bir kurucu profili varsa günceller, yoksa yenisini oluşturup kaydeder.
    /// </summary>
    public async Task<Guid> HandleAsync(SaveFounderProfileCommand command, CancellationToken cancellationToken = default)
    {
        var profile = await _repository.GetAsync(cancellationToken);
        if (profile is null)
        {
            profile = FounderProfile.Create(
                command.Name,
                command.Skills,
                command.Industries,
                command.Experience,
                command.Network,
                command.Capital,
                command.Interests,
                command.Bio
            );
            await _repository.AddAsync(profile, cancellationToken);
        }
        else
        {
            profile.Update(
                command.Name,
                command.Skills,
                command.Industries,
                command.Experience,
                command.Network,
                command.Capital,
                command.Interests,
                command.Bio
            );
            await _repository.UpdateAsync(profile, cancellationToken);
        }

        await _repository.SaveChangesAsync(cancellationToken);
        return profile.Id;
    }
}
