using System;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Automation.Commands;

public class DeleteRSSFeedCommand
{
    public Guid Id { get; set; }
}

public class DeleteRSSFeedHandler
{
    private readonly IRSSFeedRepository _repository;

    public DeleteRSSFeedHandler(IRSSFeedRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(DeleteRSSFeedCommand command, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(command.Id, cancellationToken);
        if (existing != null)
        {
            existing.MarkAsDeleted();
            await _repository.UpdateAsync(existing, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
        }
    }
}
