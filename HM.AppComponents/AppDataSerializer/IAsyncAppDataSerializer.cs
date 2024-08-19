namespace HM.AppComponents.AppDataSerializer;

public interface IAsyncAppDataSerializer<T>
    where T : class
{
    Task SaveAsync(T data);

    Task<T> LoadAsync();
}
