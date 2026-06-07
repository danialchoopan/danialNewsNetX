using danialNewsNetX.Application.Common.Interfaces;
using MediatR;

namespace danialNewsNetX.Application.Admin.Queries;

public record GetSystemTelemetryQuery : IRequest<TelemetryDto>;

public class TelemetryDto
{
    public int ActiveWebSockets { get; set; }
    public double DbConnectionPoolUsage { get; set; }
    public double RedisCacheHitRate { get; set; }
    public double MemoryUsageMb { get; set; }
}

public class GetSystemTelemetryQueryHandler : IRequestHandler<GetSystemTelemetryQuery, TelemetryDto>
{
    public Task<TelemetryDto> Handle(GetSystemTelemetryQuery request, CancellationToken cancellationToken)
    {
        // Mocking telemetry for dashboard visuals
        var random = new Random();
        return Task.FromResult(new TelemetryDto
        {
            ActiveWebSockets = random.Next(50, 500),
            DbConnectionPoolUsage = random.NextDouble() * 100,
            RedisCacheHitRate = 85.5 + (random.NextDouble() * 10),
            MemoryUsageMb = 256 + random.Next(0, 1024)
        });
    }
}
