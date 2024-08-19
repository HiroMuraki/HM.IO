using System.Diagnostics.CodeAnalysis;

namespace HM.AppComponents.AppDataSerializer;

[Serializable]
public sealed class AppDataDeserializationException : Exception
{
    public AppDataDeserializationException() { }

    public AppDataDeserializationException(String message) : base(message) { }

    public AppDataDeserializationException(Exception inner) : this(String.Empty, inner)
    {

    }

    public AppDataDeserializationException(String message, Exception inner) : base(message, inner) { }

    internal static void ThrowIfConfigFileNotExists(String configFilePath)
    {
        if (!File.Exists(configFilePath))
        {
            throw new AppDataDeserializationException($"Config file {configFilePath} doesn't exist");
        }
    }

    internal static void ThrowIfDataIsNull([NotNull] Object? data)
    {
        if (data is null)
        {
            throw new AppDataDeserializationException($"Deserialized data is null");
        }
    }
}
