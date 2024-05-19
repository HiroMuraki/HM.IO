using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace HM.Common;

public struct Option<T> :
    IEquatable<Option<T>>,
    IEqualityOperators<Option<T>, Option<T>, Boolean>
    where T : class
{
    public Option(T? value)
    {
        _value = value;
    }

    [MemberNotNullWhen(true, nameof(_value))]
    public readonly Boolean HasValue => _value is not null;

    public readonly T GetOr(T orValue)
    {
        if (HasValue)
        {
            return _value;
        }
        else
        {
            return orValue;
        }
    }

    public readonly TValue GetMemberValueOr<TValue>(Func<T, TValue> memberValueGetter, TValue orValue)
    {
        if (HasValue)
        {
            return memberValueGetter(_value);
        }
        else
        {
            return orValue;
        }
    }

    public readonly T GetOr(Func<T> orValueGetter)
    {
        if (HasValue)
        {
            return _value;
        }
        else
        {
            return orValueGetter();
        }
    }

    public readonly CallChain<Option<T>> GetThen(Action<T> func)
    {
        if (HasValue)
        {
            func(_value);
        }

        CallChainState state = HasValue ? CallChainState.SkipElse : CallChainState.Continue;
        return new CallChain<Option<T>>(this, state);
    }

    public readonly async Task<CallChain<Option<T>>> GetThenAsync(Func<T, Task> asyncFunc)
    {
        if (HasValue)
        {
            await asyncFunc(_value);
        }

        CallChainState state = HasValue ? CallChainState.SkipElse : CallChainState.Continue;
        return new CallChain<Option<T>>(this, state);
    }

    public readonly CallChain<Option<T>> GetThen(Func<T, CallChainState> func)
    {
        CallChainState state = HasValue
            ? func(_value)
            : CallChainState.Continue;

        return new CallChain<Option<T>>(this, state);
    }

    public readonly async Task<CallChain<Option<T>>> GetThenAsync(Func<T, Task<CallChainState>> asyncFunc)
    {
        CallChainState state = HasValue
            ? (await asyncFunc(_value))
            : CallChainState.Continue;

        return new CallChain<Option<T>>(this, state);
    }

    public CallChain<Option<T>> WithValue(T value, Action<T> func)
    {
        _value = value;
        func(_value);
        return new CallChain<Option<T>>(this, CallChainState.Continue);
    }

    public CallChain<Option<T>> WithValue(T value, Func<T, CallChainState> func)
    {
        _value = value;
        return new CallChain<Option<T>>(this, func(_value));
    }

    public readonly override Boolean Equals([NotNullWhen(true)] Object? obj)
        => ComparisonHelper.StructEquals(this, obj);

    public readonly Boolean Equals(Option<T> other)
    {
        return CompareValueEquality(_value, other._value);
    }

    public readonly override Int32 GetHashCode()
    {
        return HashCode.Combine(_value);
    }

    public static Boolean operator ==(Option<T> left, Option<T> right)
    {
        return CompareValueEquality(left._value, right._value);
    }

    public static Boolean operator !=(Option<T> left, Option<T> right)
    {
        return !(left == right);
    }

    public static Boolean operator ==(Option<T> left, T? right)
    {
        return CompareValueEquality(left._value, right);
    }

    public static Boolean operator !=(Option<T> left, T right)
    {
        return !(left == right);
    }

    public static Boolean operator ==(T? left, Option<T> right)
    {
        return CompareValueEquality(left, right._value);
    }

    public static Boolean operator !=(T left, Option<T> right)
    {
        return !(left == right);
    }

    public static implicit operator Option<T>(T? value)
    {
        return new Option<T>
        {
            _value = value
        };
    }

    public static implicit operator T?(Option<T> value)
    {
        return value._value;
    }

    #region NonPublic
    private T? _value;
    private static Boolean CompareValueEquality(T? left, T? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }
        if (left == null)
        {
            return right == null;
        }
        if (left is IEquatable<T> equatable)
        {
            return equatable.Equals(right);
        }

        return left.Equals(right);
    }
    #endregion
}
