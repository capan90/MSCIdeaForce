using MSC.IdeaForge.Application.FounderProfiles.Queries;
using MSC.IdeaForge.Domain.Interfaces;
using MSC.IdeaForge.Domain.ValueObjects;

namespace MSC.IdeaForge.Application.Opportunities.Commands;

/// <summary>
/// Yapay zeka ile fırsat skorlama komut nesnesi.
/// </summary>
public class AIScoreOpportunityCommand
{
    public Guid ProblemId { get; set; }
}

/// <summary>
/// Yapay zeka ile otomatik fırsat skorlamasını işleyen sınıf.
/// </summary>
public class AIScoreOpportunityHandler
{
    private readonly IProblemRepository _problemRepository;
    private readonly IAIProvider _aiProvider;
    private readonly GetFounderProfileHandler _getFounderProfileHandler;

    public AIScoreOpportunityHandler(
        IProblemRepository problemRepository,
        IAIProvider aiProvider,
        GetFounderProfileHandler getFounderProfileHandler)
    {
        _problemRepository = problemRepository;
        _aiProvider = aiProvider;
        _getFounderProfileHandler = getFounderProfileHandler;
    }

    /// <summary>
    /// Problem bilgilerini veritabanından çeker ve IAIProvider aracılığıyla fırsat puanlaması sonucunu döner.
    /// Kurucu profili tanımlıysa Founder Fit skorunun daha isabetli olması için profil bilgisi prompt'a eklenir.
    /// </summary>
    public async Task<OpportunityScoreResult> HandleAsync(AIScoreOpportunityCommand command, CancellationToken cancellationToken = default)
    {
        var problem = await _problemRepository.GetByIdAsync(command.ProblemId, cancellationToken);
        if (problem is null)
        {
            throw new InvalidOperationException("Otomatik puanlama yapılacak problem bulunamadı.");
        }

        // Kurucu profilini çekip (varsa) yapay zekaya bağlam olarak veriyoruz. Profil yoksa mevcut davranış korunur.
        var founderContext = await BuildFounderContextAsync(cancellationToken);

        // Yapay zeka sağlayıcısını çağırıyoruz.
        var result = await _aiProvider.ScoreOpportunityAsync(problem.Title, problem.Description, problem.Sector, founderContext);
        return result;
    }

    /// <summary>
    /// Kurucu profilinden yapay zeka prompt'una eklenecek bağlam metnini oluşturur.
    /// Profil yoksa null döner (mevcut davranış korunur).
    /// </summary>
    private async Task<string?> BuildFounderContextAsync(CancellationToken cancellationToken)
    {
        var founder = await _getFounderProfileHandler.HandleAsync(cancellationToken);
        if (founder is null)
        {
            return null;
        }

        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(founder.Skills)) parts.Add($"Kurucunun yetenekleri: {founder.Skills}");
        if (!string.IsNullOrWhiteSpace(founder.Industries)) parts.Add($"Sektör deneyimi: {founder.Industries}");
        if (!string.IsNullOrWhiteSpace(founder.Experience)) parts.Add($"Deneyim: {founder.Experience}");
        if (!string.IsNullOrWhiteSpace(founder.Network)) parts.Add($"Ağı (network): {founder.Network}");
        if (!string.IsNullOrWhiteSpace(founder.Capital)) parts.Add($"Sermaye durumu: {founder.Capital}");
        if (!string.IsNullOrWhiteSpace(founder.Interests)) parts.Add($"İlgi alanları: {founder.Interests}");

        return parts.Count == 0 ? null : string.Join("\n", parts);
    }
}
