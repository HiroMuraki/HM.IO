namespace AppDataSerializer;

public interface IAsyncAppDataSerializer
{
    Task SerializeAsync<T>(T data) where T : class;

    Task<T?> DeserializeAsync<T>() where T : class;
}
