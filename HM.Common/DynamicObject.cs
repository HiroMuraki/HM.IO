using Newtonsoft.Json.Linq;
using System.Collections;
using System.Reflection;

namespace HM.Common;

public sealed class DynamicObject
{
    public DynamicObject(Object? obj)
    {
        _value = ConvertValue(obj);
    }

    public DynamicObject? this[String propertyName]
    {
        get
        {
            if (_value is null)
            {
                throw new NullReferenceException();
            }

            if (IsDictionaryObject(_value))
            {
                var dict = (IDictionary<String, Object?>)_value;
                String[] propertyRoutes = propertyName.Split('.', StringSplitOptions.RemoveEmptyEntries);
                Object? nextObj = dict;
                for (Int32 i = 0; i < propertyRoutes.Length; i++)
                {
                    String route = propertyRoutes[i];
                    if (IsDictionaryObject(nextObj))
                    {
                        nextObj = ((IDictionary<String, Object?>)nextObj!)[route];
                    }
                    else
                    {
                        throw new KeyNotFoundException($"Can't find property `{propertyName}`");
                    }
                }
                return new DynamicObject(nextObj);
            }
            else
            {
                return new DynamicObject(_value);
            }
        }
    }

    public DynamicObject? this[Int32 index]
    {
        get
        {
            if (_value is null)
            {
                throw new NullReferenceException();
            }
            if (!_value.GetType().IsArray)
            {
                throw new InvalidOperationException();
            }

            return new DynamicObject(((Array)_value).GetValue(index));
        }
    }

    public static DynamicObject? CreateFromJObject(JObject? jsonObject)
    {
        if (jsonObject is null)
        {
            return null;
        }

        return new DynamicObject(JsonObjToDict(jsonObject));

        static Dictionary<String, Object?> JsonObjToDict(JObject jsonObject)
        {
            var result = new Dictionary<String, Object?>();
            foreach (KeyValuePair<String, JToken?> item in jsonObject)
            {
                result[item.Key] = ConvertValue(item.Value);
            }
            return result;

        }
        static Object? ConvertValue(JToken? token)
        {
            if (token is null)
            {
                return null;
            }

            if (token.GetType() == typeof(JValue))
            {
                return ((JValue)token).Value;
            }
            else if (token.GetType() == typeof(JObject))
            {
                return JsonObjToDict((JObject)token);
            }
            else if (token.GetType() == typeof(JArray))
            {
                var jArray = (JArray)token;
                var arr = new Object?[jArray.Count];
                for (Int32 i = 0; i < jArray.Count; i++)
                {
                    arr[i] = ConvertValue(jArray[i]);
                }
                return arr;
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }

    public Int32 IntValue => Int32.Parse(GetStringValue());

    public String StringValue => GetStringValue();

    public Single FloatValue => Single.Parse(GetStringValue());

    public Boolean BooleanValue => Boolean.Parse(GetStringValue());

    public DynamicObject[]? ArrayValue
    {
        get
        {
            if (_value is null)
            {
                return null;
            }

            if (typeof(Array).IsAssignableFrom(_value.GetType()))
            {
                var array = (Array)_value;
                var result = new DynamicObject[array.Length];
                for (Int32 i = 0; i < array.Length; i++)
                {
                    result[i] = new DynamicObject(array.GetValue(i));
                }
                return result;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }

    public IDictionary<String, Object?>? ToDictionary()
    {
        if (IsDictionaryObject(_value))
        {
            return (IDictionary<String, Object?>)_value!;
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public static implicit operator DynamicObject(Int32 value)
    {
        return new DynamicObject(value);
    }

    public static implicit operator DynamicObject(Single value)
    {
        return new DynamicObject(value);
    }

    public static implicit operator DynamicObject(Boolean value)
    {
        return new DynamicObject(value);
    }

    public static implicit operator DynamicObject(String value)
    {
        return new DynamicObject(value);
    }

    #region Non-Public
    private readonly Object? _value;
    private String GetStringValue()
    {
        if (_value is null)
        {
            throw new InvalidOperationException();
        }
        return _value.ToString() ?? String.Empty;
    }
    private static Boolean IsDictionaryObject(Object? value)
    {
        if (value is null)
        {
            return false;
        }

        return typeof(IDictionary<String, Object?>).IsAssignableFrom(value.GetType());
    }
    private static IDictionary<String, Object?> ConvertToDictionary(Object obj)
    {
        Type objType = obj.GetType();
        if (IsDictionaryObject(obj))
        {
            return (IDictionary<String, Object?>)obj;
        }
        else if (typeof(IDictionary).IsAssignableFrom(objType))
        {
            throw new NotSupportedException();
        }

        var dictObj = new Dictionary<String, Object?>();
        PropertyInfo[] props = objType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        foreach (PropertyInfo prop in props)
        {
            Object? value = prop.GetValue(obj);
            dictObj[prop.Name] = ConvertValue(value);
        }

        return dictObj;

    }
    private static Object? ConvertValue(Object? value)
    {
        if (value is null)
        {
            return null;
        }

        if (value.GetType().IsArray)
        {
            var array = (Array)value;
            Int32 arrayLength = array.Length;
            Object?[] values = new Object?[arrayLength];
            for (Int32 i = 0; i < arrayLength; i++)
            {
                values[i] = ConvertValue(array.GetValue(i));
            }
            return values;
        }

        switch (Type.GetTypeCode(value.GetType()))
        {
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Single:
            case TypeCode.String:
            case TypeCode.Boolean:
                return value;
            case TypeCode.Object:
                return ConvertToDictionary(value);
            default:
                throw new NotSupportedException($"");
        }
    }
    #endregion
}