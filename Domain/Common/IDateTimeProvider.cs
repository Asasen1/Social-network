namespace Domain.Common;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
    public DateTime AddHours(int hours);
}