namespace HM.AppComponents.AppService.Services;

public interface ILogger
{
    void WriteLine(String message, LogSeverity severity);
}