namespace HM.AppComponents.AppDataSerializer;

public static class IAsyncAppDataSerializerExtensions
{
    public static async Task<Boolean> LoadAsync<T>(this IAsyncAppDataSerializer<T> self, Action<T?> dataHandler)
        where T : class
    {
        try
        {
            T data = await self.LoadAsync();
            dataHandler(data);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static async Task<Boolean> LoadAsync<T>(this IAsyncAppDataSerializer<T> self, Func<T?, Task> asyncDataHandler)
        where T : class
    {
        try
        {
            T data = await self.LoadAsync();
            await asyncDataHandler(data);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
