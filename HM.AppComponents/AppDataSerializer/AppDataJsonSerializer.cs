using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace HM.AppComponents.AppDataSerializer;

public class AppDataJsonSerializer : IAsyncAppDataSerializer
{
    public static AppDataJsonSerializer Create(String configFilePath)
    {
        return new AppDataJsonSerializer(configFilePath);
    }

    public async Task<Boolean> CreateDefaultAsync<T>(Func<T> defaultValueGetter)
        where T : class
    {
        if (File.Exists(_configFilePath))
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
            if (File.Exists(_configFilePath))
            {
                File.Delete(_configFilePath);
            }

            using Stream fs = File.OpenWrite(_configFilePath);
            await JsonSerializer.SerializeAsync(fs, data, GetJsonSerializerOptions());
        }
        catch (Exception e)
        {
            throw new AppDataSerializationException($"Unable to serialize data to json file `{_configFilePath}`, because {e.Message}.", e);
        }
    }

    public async Task<T> DeserializeAsync<T>()
        where T : class
    {
        try
        {
            if (!File.Exists(_configFilePath))
            {
                throw new AppDataDeserializationException($"Json file `{_configFilePath}` not found", new FileNotFoundException(null, _configFilePath));
            }

            using Stream fs = File.OpenRead(_configFilePath);
            T? data = await JsonSerializer.DeserializeAsync<T>(fs, GetJsonSerializerOptions())
                ?? throw new JsonException("Deserialized data is null.");

            return data;
        }
        catch (Exception e)
        {
            throw new AppDataDeserializationException($"Unable to deserialize data from json file `{_configFilePath}`, because {e.Message}", e);
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