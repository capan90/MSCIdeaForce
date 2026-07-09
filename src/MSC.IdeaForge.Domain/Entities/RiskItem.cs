using System;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Bir projeye/probleme ait tek bir risk kaydını temsil eden entity.
/// </summary>
public class RiskItem : BaseEntity
{
    public Guid ProblemId { get; private set; }
    public string RiskName { get; private set; } = null!;
    public string Category { get; private set; } = string.Empty;   // Teknik/Pazar/Finansal/Operasyonel/Hukuki/Rekabet
    public int Probability { get; private set; }                    // 1-5
    public int Impact { get; private set; }                         // 1-5
    public int RiskScore { get; private set; }                      // Probability * Impact
    public string MitigationStrategy { get; private set; } = string.Empty;
    public string Owner { get; private set; } = string.Empty;
    public string Status { get; private set; } = string.Empty;      // Açık/İzleniyor/Azaltıldı/Kapalı

    // EF Core için gizli varsayılan yapıcı metot
    private RiskItem() { }

    /// <summary>
    /// Yeni bir risk kaydı oluşturur. RiskScore olasılık ve etkiden hesaplanır.
    /// </summary>
    public static RiskItem Create(
        Guid problemId,
        string riskName,
        string category,
        int probability,
        int impact,
        string mitigationStrategy,
        string owner,
        string status)
    {
        if (problemId == Guid.Empty)
        {
            throw new ArgumentException("Problem ID boş olamaz.", nameof(problemId));
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(riskName);

        var prob = Math.Clamp(probability, 1, 5);
        var imp = Math.Clamp(impact, 1, 5);

        return new RiskItem
        {
            ProblemId = problemId,
            RiskName = riskName,
            Category = category ?? string.Empty,
            Probability = prob,
            Impact = imp,
            RiskScore = prob * imp,
            MitigationStrategy = mitigationStrategy ?? string.Empty,
            Owner = owner ?? string.Empty,
            Status = status ?? string.Empty
        };
    }

    /// <summary>
    /// Risk kaydını silinmiş olarak işaretler (soft delete).
    /// </summary>
    public void Delete()
    {
        MarkAsDeleted();
    }
}
