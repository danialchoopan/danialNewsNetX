using danialNewsNetX.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace danialNewsNetX.Infrastructure.Services;

public class EngagementCalculatorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EngagementCalculatorService> _logger;

    public EngagementCalculatorService(IServiceProvider serviceProvider, ILogger<EngagementCalculatorService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    // Simple Hacker News style algorithm: Score = (P - 1) / (T + 2)^G
                    // Where P = Likes, T = Age in hours, G = Gravity (1.8)
                    var now = DateTime.UtcNow;
                    var posts = await context.Posts.Where(p => p.CreatedAt > now.AddDays(-7)).ToListAsync();

                    foreach (var post in posts)
                    {
                        var hoursAge = (now - post.CreatedAt).TotalHours;
                        post.EngagementScore = (post.LikeCount + (post.ReplyCount * 2)) / Math.Pow(hoursAge + 2, 1.8);
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating engagement scores.");
            }

            await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
        }
    }
}
