using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

public class ProblemRepository : IProblemRepository
{
    private readonly AppDbContext _context;

    public ProblemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Problem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Problems.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<IEnumerable<Problem>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Problems.OrderByDescending(p => p.CreatedAt).ToListAsync(cancellationToken);

    public async Task AddAsync(Problem problem, CancellationToken cancellationToken = default)
        => await _context.Problems.AddAsync(problem, cancellationToken);

    public Task UpdateAsync(Problem problem, CancellationToken cancellationToken = default)
    {
        _context.Problems.Update(problem);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
