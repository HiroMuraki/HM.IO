using HM.Common;

namespace HM.AppComponents.AppDataSerializer;

public static class IAsyncAppDataSerializerExtensions
{
    public static async Task LoadAsync<T>(this IAsyncAppDataSerializer self, Action<Option<T>> dataHandler)
        where T : class
    {
        Option<T> data;

        try
        {
            data = await self.LoadAsync<T>();
        }
        catch
        {
            data = null;
        }

        dataHandler(data);
    }

    public static async Task LoadAsync<T>(this IAsyncAppDataSerializer self, Func<Option<T>, Task> asyncDataHandler)
        where T : class
    {
        Option<T> data;

        try
        {
            data = await self.LoadAsync<T>();
        }
        catch
        {
            data = null;
        }

        await asyncDataHandler(data);
    }

    public static void Load<T>(this IAsyncAppDataSerializer self, Action<Option<T>> dataHandler)
        where T : class
    {
        self.LoadAsync(dataHandler).Wait();
    }
}
