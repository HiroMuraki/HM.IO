namespace HM.AppComponents.AppService.Services;

public interface IProcessNotifier
{
    void NotifyProcess(Int32 current, Int32 totalCount, String processDescription);
}
