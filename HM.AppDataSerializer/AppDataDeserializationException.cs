namespace AppDataSerializer;

[Serializable]
public class AppDataDeserializationException : Exception
{
    public AppDataDeserializationException() { }
    public AppDataDeserializationException(string message) : base(message) { }
    public AppDataDeserializationException(string message, Exception inner) : base(message, inner) { }
}
