using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.AI.Commands;

public class SaveAIProviderSettingCommand
{
    public Guid? Id { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsDefault { get; set; } = false;
}

public class SaveAIProviderSettingHandler
{
    private readonly IAIProviderSettingRepository _repository;

    public SaveAIProviderSettingHandler(IAIProviderSettingRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(SaveAIProviderSettingCommand command, CancellationToken cancellationToken = default)
    {
        // Eğer bu default ise diğerlerini default olmaktan çıkar!
        if (command.IsDefault)
        {
            var all = await _repository.GetAllAsync(cancellationToken);
            foreach (var setting in all)
            {
                if (setting.Id != command.Id)
                {
                    setting.IsDefault = false;
                    await _repository.UpdateAsync(setting, cancellationToken);
                }
            }
        }

        if (command.Id.HasValue && command.Id.Value != Guid.Empty)
        {
            var existing = await _repository.GetByIdAsync(command.Id.Value, cancellationToken);
            if (existing != null)
            {
                existing.ProviderName = command.ProviderName;
                existing.ApiKey = command.ApiKey;
                existing.Model = command.Model;
                existing.IsActive = command.IsActive;
                existing.IsDefault = command.IsDefault;
                existing.SetUpdated();
                await _repository.UpdateAsync(existing, cancellationToken);
            }
        }
        else
        {
            var newSetting = new AIProviderSetting(command.ProviderName, command.ApiKey, command.Model, command.IsActive, command.IsDefault);
            await _repository.AddAsync(newSetting, cancellationToken);
        }

        await _repository.SaveChangesAsync(cancellationToken);
    }
}
