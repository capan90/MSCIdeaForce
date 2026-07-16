using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Enums;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.AI.Commands;

public class SavePromptCommand
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PromptType PromptType { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

public class SavePromptHandler
{
    private readonly IAIPromptRepository _repository;

    public SavePromptHandler(IAIPromptRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(SavePromptCommand command, CancellationToken cancellationToken = default)
    {
        // Eğer bu aktif edilecekse, aynı tipteki diğer aktif prompt'ları pasif et!
        if (command.IsActive)
        {
            var all = await _repository.GetAllAsync(cancellationToken);
            var activePrompts = all.Where(p => p.PromptType == command.PromptType && p.IsActive);
            foreach (var active in activePrompts)
            {
                if (active.Id != command.Id)
                {
                    active.IsActive = false;
                    await _repository.UpdateAsync(active, cancellationToken);
                }
            }
        }

        if (command.Id.HasValue && command.Id.Value != Guid.Empty)
        {
            var existing = await _repository.GetByIdAsync(command.Id.Value, cancellationToken);
            if (existing != null)
            {
                // Eğer içerik değiştiyse versiyonu arttır ve yeni bir Prompt kaydı oluştur! (Prompt versiyon geçmişi için)
                if (existing.Content != command.Content)
                {
                    // Eskisini pasif yap
                    existing.IsActive = false;
                    await _repository.UpdateAsync(existing, cancellationToken);

                    // Yenisini ekle
                    var newPrompt = new AIPrompt(
                        command.Name,
                        command.Description,
                        command.PromptType,
                        command.Content,
                        command.IsActive,
                        existing.Version + 1
                    );
                    await _repository.AddAsync(newPrompt, cancellationToken);
                }
                else
                {
                    existing.Name = command.Name;
                    existing.Description = command.Description;
                    existing.IsActive = command.IsActive;
                    existing.SetUpdated();
                    await _repository.UpdateAsync(existing, cancellationToken);
                }
            }
        }
        else
        {
            var newPrompt = new AIPrompt(
                command.Name,
                command.Description,
                command.PromptType,
                command.Content,
                command.IsActive,
                1
            );
            await _repository.AddAsync(newPrompt, cancellationToken);
        }

        await _repository.SaveChangesAsync(cancellationToken);
    }
}
