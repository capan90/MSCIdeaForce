using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Problems.Commands;

public class CreateProblemCommand
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Sector { get; set; }
    public string? Source { get; set; }
}

public class CreateProblemHandler
{
    private readonly IProblemRepository _repository;

    public CreateProblemHandler(IProblemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> HandleAsync(CreateProblemCommand command, CancellationToken cancellationToken = default)
    {
        var problem = Problem.Create(command.Title, command.Description, command.Sector, command.Source);
        await _repository.AddAsync(problem, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return problem.Id;
    }
}
