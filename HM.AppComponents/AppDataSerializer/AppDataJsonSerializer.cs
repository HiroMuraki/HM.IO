using HM.Common;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace HM.AppComponents.AppDataSerializer;

public class AppDataJsonSerializer : IAsyncAppDataSerializer
{
    public String ConfigFilePath => _configFilePath;

    public Boolean ConfigFileExists => File.Exists(ConfigFilePath);

    public static AppDataJsonSerializer Create(String configFilePath)
    {
        return new AppDataJsonSerializer(configFilePath);
    }

    public async Task CreateDefaultIfNotExistsAsync<T>(Func<T> defaultValueGetter)
        where T : class
    {
        if (ConfigFileExists)
        {
            return;
        }

        await SaveAsync(defaultValueGetter());
    }

    public async Task SaveAsync<T>(T data)
        where T : class
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
            throw new AppDataSerializationException($"Unable to serialize data to json file `{ConfigFilePath}`, because {e.Message}.", e);
        }
    }

    public async Task<Option<T>> LoadAsync<T>()
        where T : class
    {
        if (!ConfigFileExists)
        {
            return null;
        }

        try
        {
            using Stream fs = File.OpenRead(ConfigFilePath);
            T? data = await JsonSerializer.DeserializeAsync<T>(fs, GetJsonSerializerOptions());

            return data;
        }
        catch
        {
            return null;
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