namespace HM.AppComponents.AppDataSerializer;

public static class IAppDataSerializerExtensions
{
    public static Boolean Load<T>(this IAppDataSerializer<T> self, Action<T> dataHandler)
        where T : class
    {
        try
        {
            T data = self.Load();
            dataHandler(data);
            return true;
        }
        catch
        {
            return false;
        }
    }
}