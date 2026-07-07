using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

public interface IProblemRepository
{
    Task<Problem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Problem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Problem problem, CancellationToken cancellationToken = default);
    Task UpdateAsync(Problem problem, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
