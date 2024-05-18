namespace AppDataSerializer;

public static class IAsyncAppDataSerializerExtensions
{
    public static async Task DeserializeAsync<T>(this IAsyncAppDataSerializer self, Action<T?> dataHandler)
        where T : class
    {
        T? data = await self.DeserializeAsync<T>();
        dataHandler(data);
    }

    public static async Task<T> DeserializeOrDefaultAsync<T>(this IAsyncAppDataSerializer self, Func<T> defaultValueGetter)
        where T : class
    {
        T? value = await self.DeserializeAsync<T>();
        if (value is not null)
        {
            return value;
        }
        else
        {
            return defaultValueGetter();
        }
    }

    public static T DeserializeOrDefault<T>(this IAsyncAppDataSerializer self, Func<T> defaultValueGetter)
        where T : class
    {
        return DeserializeOrDefaultAsync(self, defaultValueGetter).Result;
    }

    public static async Task DeserializeOrDefaultAsync<T>(this IAsyncAppDataSerializer self, Func<T> defaultValueGetter, Action<T> dataHandler)
        where T : class
    {
        T? data = await self.DeserializeOrDefaultAsync(defaultValueGetter);
        dataHandler(data);
    }

    public static void DeserializeOrDefault<T>(this IAsyncAppDataSerializer self, Func<T> defaultValueGetter, Action<T> dataHandler)
        where T : class
    {
        DeserializeOrDefaultAsync(self, defaultValueGetter, dataHandler).Wait();
    }
}
