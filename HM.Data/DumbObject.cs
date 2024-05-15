using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace HM.Data;

public sealed class DumbObject
{
    public DumbObject(Object? obj)
    {
        _rawObject = obj;
    }

    public Object? RawObject => _rawObject;

    public Int32 PropertyCount => AsDictionaryOrNull()?.Count ?? -1;

    public Boolean IsNull => _rawObject is null;

    public Boolean IsInt32 => _rawObject is Int32;

    public Boolean IsInt64 => _rawObject is Int64;

    public Boolean IsDouble => _rawObject is Double;

    public Boolean IsString => _rawObject is String;

    public Boolean IsBoolean => _rawObject is Boolean;

    public Boolean IsCollection
    {
        get
        {
            if (_rawObject is null)
            {
                return false;
            }
            if (!IsDictionary)
            {
                return false;
            }

            return _rawObject.GetType().IsAssignableTo(typeof(ICollection));
        }
    }

    public Boolean IsDictionary => _rawObject is IDictionary;

    public DumbObject this[String propertyName] => GetProperty(propertyName);

    public Boolean HasProperty(String propertyName)
    {
        if (TryToDictionary(out IReadOnlyDictionary<String, DumbObject>? value))
        {
            return value.ContainsKey(propertyName);
        }
        else
        {
            return false;
        }
    }

    public DumbObject GetProperty(String propertyName)
    {
        IReadOnlyDictionary<String, DumbObject> dictObj = ToDictionary();

        if (dictObj.TryGetValue(propertyName, out DumbObject? value))
        {
            return value;
        }
        else
        {
            throw new KeyNotFoundException($"Can't get property `{propertyName}`");
        }
    }

    public T GetProperty<T>(String propertyName)
    {
        IReadOnlyDictionary<String, DumbObject> dictObj = ToDictionary();

        if (dictObj.TryGetValue(propertyName, out DumbObject? value))
        {
            if (value._rawObject is T result)
            {
                return result;
            }
            else
            {
                throw new InvalidCastException($"Unable to cast {value._rawObject} to {typeof(T)}");
            }
        }
        else
        {
            throw new KeyNotFoundException($"Can't get property `{propertyName}`");
        }
    }

    public Boolean TryGetProperty(String propertyName, [NotNullWhen(true)] out DumbObject? value)
    {
        if (_rawObject is Dictionary<Object, Object> dictObj && dictObj.TryGetValue(propertyName, out Object? result))
        {
            value = new DumbObject(result);
            return true;
        }
        else
        {
            value = null;
            return false;
        }
    }

    public Boolean TryGetProperty<T>(String propertyName, [NotNullWhen(true)] out T? value)
    {
        if (_rawObject is Dictionary<Object, Object> dictObj && dictObj.TryGetValue(propertyName, out Object? result))
        {
            if (result is T t)
            {
                value = t;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }
        else
        {
            value = default;
            return false;
        }
    }

    public override String? ToString()
    {
        return _rawObject?.ToString();
    }

    public Int32 ToInt32()
    {
        ArgumentNullException.ThrowIfNull(_rawObject);

        return (Int32)_rawObject;
    }

    public Boolean TryToInt32(out Int32 value)
    {
        if (IsInt32)
        {
            value = ToInt32();
            return true;
        }
        else
        {
            value = 0;
            return false;
        }
    }

    public Int64 ToInt64()
    {
        ArgumentNullException.ThrowIfNull(_rawObject);

        return (Int64)_rawObject;
    }

    public Boolean TryToInt64(out Int64 value)
    {
        if (IsInt64)
        {
            value = ToInt64();
            return true;
        }
        else
        {
            value = 0;
            return false;
        }
    }

    public Double ToDouble()
    {
        ArgumentNullException.ThrowIfNull(_rawObject);

        return (Double)_rawObject;
    }

    public Boolean TryToDouble(out Double value)
    {
        if (IsDouble)
        {
            value = ToDouble();
            return true;
        }
        else
        {
            value = 0;
            return false;
        }
    }

    public Boolean ToBoolean()
    {
        ArgumentNullException.ThrowIfNull(_rawObject);

        return (Boolean)_rawObject;
    }

    public Boolean TryToBoolean(out Boolean value)
    {
        if (IsBoolean)
        {
            value = ToBoolean();
            return true;
        }
        else
        {
            value = false;
            return false;
        }
    }

    public IReadOnlyCollection<DumbObject> ToCollection()
    {
        return AsCollectionOrNull()?.Select(x => new DumbObject(x)).ToImmutableArray()
            ?? throw new InvalidOperationException($"Unable to cast current Object `{_rawObject}` to {typeof(DumbObject[])}");
    }

    public Boolean TryToCollection([NotNullWhen(true)] out IReadOnlyCollection<DumbObject>? value)
    {
        value = AsCollectionOrNull()?.Select(x => new DumbObject(x)).ToImmutableArray();
        return value is not null;
    }

    public IReadOnlyCollection<T> ToCollection<T>()
    {
        return AsCollectionOrNull()?.Select(x => (T)x).ToImmutableArray()
            ?? throw new InvalidOperationException($"Unable to cast current Object `{_rawObject}` to {typeof(DumbObject[])}");
    }

    public Boolean TryToCollection<T>([NotNullWhen(true)] out IReadOnlyCollection<T>? value)
    {
        value = AsCollectionOrNull()?.Select(x => (T)x).ToImmutableArray();
        return value is not null;
    }

    public IReadOnlyDictionary<String, DumbObject> ToDictionary()
    {
        IReadOnlyDictionary<Object, Object>? dictObj = AsDictionaryOrNull()
             ?? throw new InvalidOperationException($"Unable to cast current Object `{_rawObject}` to {typeof(Dictionary<,>)}");

        return dictObj.ToImmutableDictionary(
            k => k.Key.ToString() ?? throw new ArgumentNullException(),
            v => new DumbObject(v.Value));
    }

    public Boolean TryToDictionary([NotNullWhen(true)] out IReadOnlyDictionary<String, DumbObject>? value)
    {
        IReadOnlyDictionary<Object, Object>? dictObj = AsDictionaryOrNull();
        if (dictObj is null)
        {
            value = null;
            return false;
        }

        value = dictObj.ToImmutableDictionary(
            k => k.Key.ToString()!,
            v => new DumbObject(v.Value));
        return true;
    }

    public IReadOnlyDictionary<String, T> ToDictionary<T>()
    {
        IReadOnlyDictionary<Object, Object>? dictObj = AsDictionaryOrNull()
             ?? throw new InvalidOperationException($"Unable to cast current Object `{_rawObject}` to {typeof(Dictionary<,>)}");

        return dictObj.ToImmutableDictionary(
            k => k.Key.ToString() ?? throw new ArgumentNullException(),
            v => (T)v.Value);
    }

    public Boolean TryToDictionary<T>([NotNullWhen(true)] out IReadOnlyDictionary<String, T>? value)
    {
        IReadOnlyDictionary<Object, Object>? dictObj = AsDictionaryOrNull();
        if (dictObj is null)
        {
            value = null;
            return false;
        }

        var result = new Dictionary<String, T>();
        foreach ((Object k, Object v) in dictObj)
        {
            String? keyString = k?.ToString();
            if (keyString is null || v is not T valueT)
            {
                value = default;
                return false;
            }
            else
            {
                result[keyString] = valueT;
            }
        }
        value = result.ToImmutableDictionary();
        return true;
    }

    #region NonPublic
    private readonly Object? _rawObject;
    private IReadOnlyDictionary<Object, Object>? AsDictionaryOrNull()
    {
        return _rawObject as IReadOnlyDictionary<Object, Object>;
    }
    private IReadOnlyCollection<Object>? AsCollectionOrNull()
    {
        return _rawObject as IReadOnlyCollection<Object>;
    }
    #endregion
}
