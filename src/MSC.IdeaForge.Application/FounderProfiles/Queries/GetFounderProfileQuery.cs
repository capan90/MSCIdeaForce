using MSC.IdeaForge.Application.FounderProfiles.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.FounderProfiles.Queries;

/// <summary>
/// Mevcut kurucu profilini getiren sorgu işleyicisi.
/// </summary>
public class GetFounderProfileHandler
{
    private readonly IFounderProfileRepository _repository;

    public GetFounderProfileHandler(IFounderProfileRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Kurucu profilini DTO formatında döner. Kayıt yoksa null döner.
    /// </summary>
    public async Task<FounderProfileDto?> HandleAsync(CancellationToken cancellationToken = default)
    {
        var profile = await _repository.GetAsync(cancellationToken);
        if (profile is null)
        {
            return null;
        }

        return new FounderProfileDto
        {
            Id = profile.Id,
            Name = profile.Name,
            Skills = profile.Skills,
            Industries = profile.Industries,
            Experience = profile.Experience,
            Network = profile.Network,
            Capital = profile.Capital,
            Interests = profile.Interests,
            Bio = profile.Bio
        };
    }
}
