namespace HM.AppComponents.AppDataSerializer;

[Serializable]
public class AppDataDeserializationException : Exception
{
    public AppDataDeserializationException() { }

    public AppDataDeserializationException(String message) : base(message) { }

    public AppDataDeserializationException(String message, Exception inner) : base(message, inner) { }
}
