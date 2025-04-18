using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FinanceTracker.Infrastructure.Extensions;

public static class TypeConverters
{
    public class NullableDateTimeAsUtcValueConverter()
        : ValueConverter<DateTime?, DateTime?>(
            v => !v.HasValue ? v : ToUtc(v.Value),
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v
        )
    {
        private static DateTime? ToUtc(DateTime v) =>
            v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime();
    }

    public class DateTimeAsUtcValueConverter()
        : ValueConverter<DateTime, DateTime>(
            v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
        );
}
