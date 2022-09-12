namespace BananaChips.Infrastructure.Extensions;

public static class DateTimeExtensions
{
    private static readonly long StartDateTicks = new DateTime(1970, 1, 1).Ticks;

    public static long ToUnixTimeStampMilliseconds(this DateTime dateTime)
    {
        return (dateTime.Ticks - StartDateTicks) / TimeSpan.TicksPerMillisecond;
    }

    public static long ToUnixTimeStampSeconds(this DateTime dateTime)
    {
        return (dateTime.Ticks - StartDateTicks) / TimeSpan.TicksPerSecond;
    }

    public static DateTime TimeFromUnixTimeStampSeconds(this long unixTimestamp)
    {
        var unixTimeStampInTicks = unixTimestamp * TimeSpan.TicksPerSecond;
        return new DateTime(StartDateTicks + unixTimeStampInTicks);
    }

    public static DateTime TimeFromUnixTimeStampMilliseconds(this long unixTimestamp)
    {
        var unixTimeStampInTicks = unixTimestamp * TimeSpan.TicksPerMillisecond;
        return new DateTime(StartDateTicks + unixTimeStampInTicks);
    }
}