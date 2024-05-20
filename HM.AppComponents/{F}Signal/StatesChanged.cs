namespace HM.AppComponents;

public sealed class StatesChanged<T>
    where T : class
{
    public StatesChanged(T sender)
    {
        Sender = sender;
    }

    public T Sender { get; init; }
}
