using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Yatırımcı briefing verilerine erişim arayüzü.
/// </summary>
public interface IInvestorBriefRepository
{
    /// <summary>
    /// Belirtilen problem ID'sine ait yatırımcı briefing kaydını getirir.
    /// </summary>
    Task<InvestorBrief?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir yatırımcı briefing kaydı ekler.
    /// </summary>
    Task AddAsync(InvestorBrief investorBrief, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yatırımcı briefing kaydını günceller.
    /// </summary>
    Task UpdateAsync(InvestorBrief investorBrief, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
