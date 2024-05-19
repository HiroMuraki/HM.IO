namespace HM.Common;

public enum CallChainState
{
    SkipElse,
    Continue,
    Stop,
}

public readonly struct CallChain<T>
{
    public CallChain(T value, CallChainState state)
    {
        _value = value;
        _state = state;
    }

    public CallChain<T> Continue(Action action)
    {
        if (SkipContinue())
        {
            return new CallChain<T>(_value, _state);
        }

        action();
        return new CallChain<T>(_value, CallChainState.Continue);
    }

    public CallChain<T> Continue(Func<CallChainState> func)
    {
        if (SkipContinue())
        {
            return new CallChain<T>(_value, _state);
        }

        return new CallChain<T>(_value, func());
    }

    public CallChain<T> Continue(Func<T, CallChainState> func)
    {
        if (SkipContinue())
        {
            return new CallChain<T>(_value, _state);
        }

        return new CallChain<T>(_value, func(_value));
    }

    public CallChain<T> ElseIf(Boolean condition, Action action)
    {
        if (SkipElse())
        {
            return new CallChain<T>(_value, _state);
        }

        if (condition)
        {
            action();
            return new CallChain<T>(_value, CallChainState.Stop);
        }
        else
        {
            return new CallChain<T>(_value, CallChainState.Continue);
        }
    }

    public CallChain<T> ElseIf(Boolean condition, Action<T> action)
    {
        if (SkipElse())
        {
            return new CallChain<T>(_value, _state);
        }

        if (condition)
        {
            action(_value);
            return new CallChain<T>(_value, CallChainState.Stop);
        }
        else
        {
            return new CallChain<T>(_value, CallChainState.Continue);
        }
    }

    public void Else(Action action)
    {
        if (SkipElse())
        {
            return;
        }

        action();
    }

    public void Else(Action<T> action)
    {
        if (SkipElse())
        {
            return;
        }

        action(_value);
    }

    #region NonPublic
    private readonly T _value;
    private readonly CallChainState _state;
    private Boolean SkipContinue()
    {
        return _state == CallChainState.Stop;
    }
    private Boolean SkipElse()
    {
        return _state == CallChainState.Stop || _state == CallChainState.SkipElse;
    }
    #endregion
}
