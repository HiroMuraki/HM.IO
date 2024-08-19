using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace HM.AppComponents.AppDataSerializer;

public sealed class AppDataJsonSerializer<T> :
    IAppDataSerializer<T>,
    IAsyncAppDataSerializer<T>
    where T : class
{
    public String ConfigFilePath => _configFilePath;

    public Boolean ConfigFileExists => File.Exists(ConfigFilePath);

    public static AppDataJsonSerializer<T> Create(String configFilePath)
    {
        return new AppDataJsonSerializer<T>(configFilePath);
    }

    public async Task CreateDefaultIfNotExistsAsync(Func<T> defaultValueGetter)
    {
        if (ConfigFileExists)
        {
            return;
        }

        await SaveAsync(defaultValueGetter());
    }

    public T Load()
    {
        AppDataDeserializationException.ThrowIfConfigFileNotExists(ConfigFilePath);

        try
        {
            using Stream fs = File.OpenRead(ConfigFilePath);
            T? data = JsonSerializer.Deserialize<T>(fs, GetJsonSerializerOptions());

            AppDataDeserializationException.ThrowIfDataIsNull(data);

            return data;
        }
        catch (Exception e)
        {
            throw new AppDataDeserializationException(e);
        }
    }

    public async Task<T> LoadAsync()
    {
        AppDataDeserializationException.ThrowIfConfigFileNotExists(ConfigFilePath);

        try
        {
            using Stream fs = File.OpenRead(ConfigFilePath);
            T? data = await JsonSerializer.DeserializeAsync<T>(fs, GetJsonSerializerOptions());

            AppDataDeserializationException.ThrowIfDataIsNull(data);

            return data;
        }
        catch (Exception e)
        {
            throw new AppDataDeserializationException(e);
        }
    }

    public void Save(T data)
    {
        try
        {
            if (ConfigFileExists)
            {
                File.Delete(ConfigFilePath);
            }

            using Stream fs = File.OpenWrite(ConfigFilePath);
            JsonSerializer.Serialize(fs, data, GetJsonSerializerOptions());
        }
        catch (Exception e)
        {
            throw new AppDataSerializationException(e);
        }
    }

    public async Task SaveAsync(T data)
    {
        try
        {
            if (ConfigFileExists)
            {
                File.Delete(ConfigFilePath);
            }

            using Stream fs = File.OpenWrite(ConfigFilePath);
            await JsonSerializer.SerializeAsync(fs, data, GetJsonSerializerOptions());
        }
        catch (Exception e)
        {
            throw new AppDataSerializationException(e);
        }
    }

    #region NonPublic
    private readonly String _configFilePath;
    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create([UnicodeRanges.All]),
        };
    }
    private AppDataJsonSerializer(String configFilePath)
    {
        _configFilePath = configFilePath;
    }
    #endregion
}