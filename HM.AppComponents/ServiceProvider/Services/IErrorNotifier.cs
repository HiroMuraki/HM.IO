namespace HM.AppComponents.AppService.Services;

public interface IErrorNotifier
{
    void NotifyError(Exception exception);
}
