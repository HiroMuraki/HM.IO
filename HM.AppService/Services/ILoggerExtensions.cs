namespace HM.AppService.Services;

public static class ILoggerExtensions
{
    public static void WriteLine(this ILogger self)
        => self.WriteLine(String.Empty);
}