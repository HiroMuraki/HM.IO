namespace AppDataSerializer;

[Serializable]
public class AppDataSerializationException : Exception
{
    public AppDataSerializationException() { }
    public AppDataSerializationException(string message) : base(message) { }
    public AppDataSerializationException(string message, Exception inner) : base(message, inner) { }
}
