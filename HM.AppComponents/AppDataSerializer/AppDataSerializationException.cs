namespace HM.AppComponents.AppDataSerializer;

[Serializable]
public class AppDataSerializationException : Exception
{
    public AppDataSerializationException() { }

    public AppDataSerializationException(String message) : base(message) { }

    public AppDataSerializationException(String message, Exception inner) : base(message, inner) { }
}
