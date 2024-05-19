using HM.Common;

namespace HM.AppComponents.AppDataSerializer;

public interface IAsyncAppDataSerializer
{
    Task SaveAsync<T>(T data) where T : class;

    Task<Option<T>> LoadAsync<T>() where T : class;
}
