using System.Reflection;
using System.Text;

namespace HM.Common;

public static class ToStringHelper
{
    public static String Build(Object obj)
    {
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
        PropertyInfo[] propertyInfos = obj.GetType().GetProperties(bindingFlags).ToArray();

        return $"{{ {BuildPropertiesString(obj, propertyInfos)} }}";
    }

    public static String Build(Object obj, String[] propertyNames)
    {
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
        var propertyInfos = new PropertyInfo[propertyNames.Length];
        Type type = obj.GetType();

        for (Int32 i = 0; i < propertyNames.Length; i++)
        {
            propertyInfos[i] = type.GetProperty(propertyNames[i], bindingFlags) ??
                throw new ArgumentException($"Unable to find property `{propertyNames[i]}`");
        }

        return $"{{ {BuildPropertiesString(obj, propertyInfos)} }}";
    }

    public static String Build(Object obj, Func<PropertyInfo, Boolean> propertyPredicate)
    {
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
        PropertyInfo[] propertyInfos = obj.GetType().GetProperties(bindingFlags)
            .Where(propertyPredicate)
            .ToArray();

        return $"{{ {BuildPropertiesString(obj, propertyInfos)} }}";
    }

    public static String Build(Object obj, Func<PropertyInfo, Boolean> propertyPredicate, Func<FieldInfo, Boolean> fieldPredicate)
    {
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
        PropertyInfo[] propertyInfos = obj.GetType().GetProperties(bindingFlags)
            .Where(propertyPredicate)
            .ToArray();
        FieldInfo[] fieldInfos = obj.GetType().GetFields(bindingFlags)
            .Where(fieldPredicate)
            .ToArray();

        return $"{{ Properties = [{BuildPropertiesString(obj, propertyInfos)}], Fields = [{BuildFieldString(obj, fieldInfos)}] }}";
    }

    #region NonPublic
    private static StringBuilder BuildPropertiesString(Object obj, PropertyInfo[] propertyInfos)
    {
        var sb = new StringBuilder();

        for (Int32 i = 0; i < propertyInfos.Length; i++)
        {
            PropertyInfo propertyInfo = propertyInfos[i];
            sb.Append($"{propertyInfo.Name} = {propertyInfo.GetValue(obj, null)}");
            if (i != propertyInfos.Length - 1)
            {
                sb.Append(", ");
            }
        }

        return sb;
    }
    private static StringBuilder BuildFieldString(Object obj, FieldInfo[] fieldInfos)
    {
        var sb = new StringBuilder();

        for (Int32 i = 0; i < fieldInfos.Length; i++)
        {
            FieldInfo fieldInfo = fieldInfos[i];
            sb.Append($"{fieldInfo.Name} = {fieldInfo.GetValue(obj)}");
            if (i != fieldInfos.Length - 1)
            {
                sb.Append(", ");
            }
        }

        return sb;
    }
    #endregion
}
