namespace Domain.Common;

public static class Extensions
{
    public static bool IsEmpty(this string? str)
    {
        return string.IsNullOrWhiteSpace(str);
    }
}