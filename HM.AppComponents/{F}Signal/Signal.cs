namespace HM.AppComponents;

public sealed class Signal<TSignalArg>
{
    public void Register(ISignalReceiver<TSignalArg> signalReceiver)
    {
        lock (_locker)
        {
            _handlers.Add(new WeakReference<ISignalReceiver<TSignalArg>?>(signalReceiver));
        }
    }

    public void Unregister(ISignalReceiver<TSignalArg> signalReceiver)
    {
        lock (_locker)
        {
            WeakReference<ISignalReceiver<TSignalArg>?>? targetHandler = _handlers.FirstOrDefault(x =>
            {
                return x.TryGetTarget(out ISignalReceiver<TSignalArg>? result) && result.Equals(signalReceiver);
            });

            if (targetHandler is not null)
            {
                _handlers.Remove(targetHandler);
            }
        }
    }

    public void Emit(TSignalArg signalArg)
    {
        lock (_locker)
        {
            for (Int32 i = 0; i < _handlers.Count; i++)
            {
                if (_handlers[i].TryGetTarget(out ISignalReceiver<TSignalArg>? signalReceiver))
                {
                    signalReceiver.Receive(signalArg);
                }
                else
                {
                    _handlers.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    #region NonPublic
    private readonly Object _locker = new();
    private readonly List<WeakReference<ISignalReceiver<TSignalArg>?>> _handlers = [];
    #endregion
}
