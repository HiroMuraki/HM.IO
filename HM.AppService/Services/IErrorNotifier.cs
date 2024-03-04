namespace HM.AppService.Services;

public interface IErrorNotifier
{
    void NotifyError(string message);

    void NotifyError(Exception exception);
}