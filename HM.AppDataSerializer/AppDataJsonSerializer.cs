using HM.IO;
using System.Text.Json;

namespace AppDataSerializer;

public class AppDataJsonSerializer : IAsyncAppDataSerializer
{
    public static AppDataJsonSerializer Create(String configFilePath)
        => Create(configFilePath);

    public static AppDataJsonSerializer Create(EntryPath configFilePath)
    {
        return new AppDataJsonSerializer(configFilePath);
    }

    public async Task<Boolean> CreateDefaultAsync<T>(Func<T> defaultValueGetter)
        where T : class
    {
        if (LocalFileIO.Exists(_configFilePath))
        {
            return false;
        }

        await SerializeAsync(defaultValueGetter());
        return true;
    }

    public async Task SerializeAsync<T>(T data)
        where T : class
    {
        try
        {
            if (LocalFileIO.Exists(_configFilePath))
            {
                LocalFileIO.Delete(_configFilePath);
            }

            using Stream fs = LocalFileIO.OpenWrite(_configFilePath);
            await JsonSerializer.SerializeAsync(fs, data, GetJsonSerializerOptions());
        }
        catch (Exception e)
        {
            throw new AppDataSerializationException($"Unable to serialize data to json file `{_configFilePath.StringPath}`, because {e.Message}.", e);
        }
    }

    public async Task<T> DeserializeAsync<T>()
        where T : class
    {
        try
        {
            if (!LocalFileIO.Exists(_configFilePath))
            {
                throw new AppDataDeserializationException($"Json file `{_configFilePath.StringPath}` not found", new FileNotFoundException(null, _configFilePath.StringPath));
            }

            using Stream fs = LocalFileIO.OpenRead(_configFilePath);
            T? data = await JsonSerializer.DeserializeAsync<T>(fs, GetJsonSerializerOptions())
                ?? throw new JsonException("Deserialized data is null.");

            return data;
        }
        catch (Exception e)
        {
            throw new AppDataDeserializationException($"Unable to deserialize data from json file `{_configFilePath.StringPath}`, because {e.Message}", e);
        }
    }

    #region NonPublic
    private readonly EntryPath _configFilePath;
    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };
    }
    private AppDataJsonSerializer(EntryPath configFilePath)
    {
        _configFilePath = configFilePath;
    }
    #endregion
}