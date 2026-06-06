using danialNewsNetX.Application.Common.Interfaces;
using danialNewsNetX.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace danialNewsNetX.Infrastructure.Services;

public class StoryArchiverService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public StoryArchiverService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                var expiredStories = await context.Stories
                    .Where(s => !s.IsArchived && s.ExpiresAt <= DateTime.UtcNow)
                    .ToListAsync(stoppingToken);

                if (expiredStories.Any())
                {
                    foreach (var story in expiredStories)
                    {
                        story.IsArchived = true;
                    }

                    await context.SaveChangesAsync(stoppingToken);
                }
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
