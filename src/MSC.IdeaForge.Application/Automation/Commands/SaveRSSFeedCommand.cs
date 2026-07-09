using System;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Automation.Commands;

public class SaveRSSFeedCommand
{
    public Guid? Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

public class SaveRSSFeedHandler
{
    private readonly IRSSFeedRepository _repository;

    public SaveRSSFeedHandler(IRSSFeedRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(SaveRSSFeedCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Id.HasValue && command.Id.Value != Guid.Empty)
        {
            var existing = await _repository.GetByIdAsync(command.Id.Value, cancellationToken);
            if (existing != null)
            {
                existing.Url = command.Url;
                existing.Name = command.Name;
                existing.IsActive = command.IsActive;
                existing.SetUpdated();
                await _repository.UpdateAsync(existing, cancellationToken);
            }
        }
        else
        {
            var newFeed = new RSSFeed(command.Url, command.Name, command.IsActive);
            await _repository.AddAsync(newFeed, cancellationToken);
        }

        await _repository.SaveChangesAsync(cancellationToken);
    }
}
