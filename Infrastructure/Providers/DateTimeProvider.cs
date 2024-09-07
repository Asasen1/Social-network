using Domain.Common;

namespace Infrastructure.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;

    public DateTime AddHours(int hours)
    {
        return UtcNow.AddHours(hours);
    }
}