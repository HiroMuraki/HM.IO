namespace HM.AppComponents.AppDataSerializer;

public interface IAppDataSerializer<T>
    where T : class
{
    void Save(T data);

    T Load();
}