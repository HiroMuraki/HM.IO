using System.Threading;

namespace HM.Common;

public static class ThrowHelper
{
    public static void ThrowUnableToCallDefaultConstructor(Type type)
    {
        throw new InvalidOperationException($"Unable to call the default constructor of type `{type}`.");
    }

    public static void ThrowInvalidOperation(String message)
        => ThrowInvalidOperation(message, null);

    public static void ThrowInvalidOperation(String message, Exception? innerException)
    {
        throw new InvalidOperationException(message, innerException);
    }
}
