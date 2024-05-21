namespace HM.AppComponents.AppDataSerializer;

public static class IAsyncAppDataSerializerExtensions
{
    public static async Task LoadAsync<T>(this IAsyncAppDataSerializer self, Action<T?> dataHandler)
        where T : class
    {
        T? data = await self.LoadAsync<T>(); ;

        dataHandler(data);
    }

    public static async Task LoadAsync<T>(this IAsyncAppDataSerializer self, Func<T?, Task> asyncDataHandler)
        where T : class
    {
        T? data = await self.LoadAsync<T>();

        await asyncDataHandler(data);
    }

    public static void Load<T>(this IAsyncAppDataSerializer self, Action<T?> dataHandler)
        where T : class
    {
        self.LoadAsync(dataHandler).Wait();
    }
}
