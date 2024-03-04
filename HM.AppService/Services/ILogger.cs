namespace HM.AppService.Services;

public interface ILogger
{
    void WriteLine(string? message);
}

public static class ILoggerExtensions
{
    public static void WriteLine(this ILogger self)
        => self.WriteLine(string.Empty);
}