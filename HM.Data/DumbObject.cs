namespace HM.Data;

public class DumbObject
{
    public DumbObject(Object? obj)
    {
        _obj = obj;
    }

    public Boolean IsNull => _obj is null;

    public Boolean IsInt32 => _obj is Int32;

    public Boolean IsInt64 => _obj is Int64;

    public Boolean IsDouble => _obj is Double;

    public Boolean IsString => _obj is String;

    public Boolean IsBoolean => _obj is Boolean;

    public Boolean IsArray => _obj is IEnumerable<Object>;

    public Boolean IsDictionary => _obj is Dictionary<Object, Object>;

    public DumbObject this[String propertyName] => GetProperty(propertyName);

    public Boolean HasProperty(String propertyName)
    {
        if (_obj is Dictionary<Object, Object> dictObj)
        {
            return dictObj.ContainsKey(propertyName);
        }
        else
        {
            return false;
        }
    }

    public DumbObject GetProperty(String propertyName)
    {
        Dictionary<String, DumbObject> dictObj = ToDictionary();

        if (dictObj.TryGetValue(propertyName, out DumbObject? value))
        {
            return value;
        }
        else
        {
            throw new KeyNotFoundException($"Can't get property `{propertyName}`");
        }
    }

    public Boolean TryGetProperty(String propertyName, out DumbObject? value)
    {
        if (_obj is Dictionary<Object, Object> dictObj && dictObj.TryGetValue(propertyName, out Object? result))
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

    public Dictionary<String, DumbObject> ToDictionary()
    {
        if (_obj is not Dictionary<Object, Object> dictObj)
        {
            throw new InvalidOperationException($"Unable to cast current Object `{_obj}` to {typeof(Dictionary<,>)}");
        }

        return dictObj.ToDictionary(
            k => k.Key.ToString() ?? throw new ArgumentNullException(),
            v => new DumbObject(v.Value));
    }

    public Dictionary<String, T> ToDictionary<T>()
    {
        if (_obj is not Dictionary<Object, Object> dictObj)
        {
            throw new InvalidOperationException($"Unable to cast current Object `{_obj}` to {typeof(Dictionary<,>)}");
        }

        return dictObj.ToDictionary(
            k => k.Key.ToString() ?? throw new ArgumentNullException(),
            v => (T)v.Value);
    }

    public override String? ToString()
    {
        return _obj?.ToString();
    }

    public Int32 ToInt32()
    {
        ArgumentNullException.ThrowIfNull(_obj);

        return Convert.ToInt32(_obj);
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
        ArgumentNullException.ThrowIfNull(_obj);

        return Convert.ToInt64(_obj);
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
        ArgumentNullException.ThrowIfNull(_obj);

        return Convert.ToDouble(_obj);
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
        ArgumentNullException.ThrowIfNull(_obj);

        return Convert.ToBoolean(_obj);
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

    public DumbObject[] ToArray()
    {
        if (_obj is IEnumerable<Object> enumerable)
        {
            return enumerable.Select(x => new DumbObject(x)).ToArray();
        }
        else
        {
            throw new InvalidOperationException($"Unable to cast current Object `{_obj}` to {typeof(DumbObject[])}");
        }
    }

    public Boolean TryToArray(out DumbObject[]? value)
    {
        if (IsArray)
        {
            value = ToArray();
            return true;
        }
        else
        {
            value = null;
            return false;
        }
    }

    public T[] ToArray<T>()
    {
        if (_obj is IEnumerable<Object> enumerable)
        {
            return enumerable.Cast<T>().ToArray();
        }
        else
        {
            throw new InvalidOperationException($"Unable to cast current Object `{_obj}` to {typeof(DumbObject[])}");
        }
    }

    public static explicit operator String?(DumbObject dumbObject)
    {
        return dumbObject.ToString();
    }

    #region NonPublic
    private readonly Object? _obj;
    #endregion
}
