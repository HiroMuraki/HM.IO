namespace HM.AppComponents.AppDataSerializer;

[Serializable]
public sealed class AppDataSerializationException : Exception
{
    public AppDataSerializationException() { }

    public AppDataSerializationException(String message) : base(message) { }

    public AppDataSerializationException(Exception inner) : base(String.Empty, inner) { }

    public AppDataSerializationException(String message, Exception inner) : base(message, inner) { }
}
