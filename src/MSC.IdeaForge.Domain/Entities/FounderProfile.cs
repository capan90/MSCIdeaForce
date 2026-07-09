using System;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Kurucunun yeteneklerini, deneyimlerini ve kaynaklarını temsil eden entity.
/// Fırsat skorlamasındaki "Founder Fit" (Kurucu Uyumu) kriterini beslemek için kullanılır.
/// </summary>
public class FounderProfile : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string Skills { get; private set; } = string.Empty;        // Virgülle ayrılmış yetenekler
    public string Industries { get; private set; } = string.Empty;    // Virgülle ayrılmış sektör deneyimleri
    public string Experience { get; private set; } = string.Empty;
    public string Network { get; private set; } = string.Empty;
    public string Capital { get; private set; } = string.Empty;       // Bootstrap, Düşük, Orta, Yüksek
    public string Interests { get; private set; } = string.Empty;     // Virgülle ayrılmış ilgi alanları
    public string? Bio { get; private set; }

    // EF Core için gizli varsayılan yapıcı metot
    private FounderProfile() { }

    /// <summary>
    /// Yeni bir kurucu profili oluşturur.
    /// </summary>
    public static FounderProfile Create(
        string name,
        string skills,
        string industries,
        string experience,
        string network,
        string capital,
        string interests,
        string? bio)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new FounderProfile
        {
            Name = name,
            Skills = skills ?? string.Empty,
            Industries = industries ?? string.Empty,
            Experience = experience ?? string.Empty,
            Network = network ?? string.Empty,
            Capital = capital ?? string.Empty,
            Interests = interests ?? string.Empty,
            Bio = bio
        };
    }

    /// <summary>
    /// Mevcut kurucu profilini günceller.
    /// </summary>
    public void Update(
        string name,
        string skills,
        string industries,
        string experience,
        string network,
        string capital,
        string interests,
        string? bio)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;
        Skills = skills ?? string.Empty;
        Industries = industries ?? string.Empty;
        Experience = experience ?? string.Empty;
        Network = network ?? string.Empty;
        Capital = capital ?? string.Empty;
        Interests = interests ?? string.Empty;
        Bio = bio;
        SetUpdated();
    }
}
