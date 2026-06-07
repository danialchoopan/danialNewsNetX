namespace danialNewsNetX.Domain.Entities;

public class SystemFeature
{
    public Guid Id { get; set; }
    public string Key { get; set; } = null!;
    public bool IsEnabled { get; set; }
    public string? Description { get; set; }
}
