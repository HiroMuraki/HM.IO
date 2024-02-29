using HM.Common;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace HM.IO.Previews;

public readonly struct EntryAttributes :
    IEquatable<EntryAttributes>,
    IEqualityOperators<EntryAttributes, EntryAttributes, Boolean>
{
    public readonly FileAttributes Value => _value;

    public readonly Boolean HasAttribute(FileAttributes attribute)
    {
        return (Value & attribute) == attribute;
    }

    public Boolean Equals(EntryAttributes other)
    {
        return Value == other.Value;
    }

    public override Boolean Equals([NotNullWhen(true)] Object? obj)
        => ComparisonHelper.StructEquals(this, obj);

    public override Int32 GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static Boolean operator ==(EntryAttributes left, EntryAttributes right)
    {
        return left.Equals(right);
    }

    public static Boolean operator !=(EntryAttributes left, EntryAttributes right)
    {
        return !(left == right);
    }

    public override String ToString()
    {
        return Value.ToString();
    }

    public static EntryAttributes Combine(FileAttributes[] fileAttributes)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(fileAttributes.Length);

        FileAttributes combinedAttributes = fileAttributes[0];

        foreach (FileAttributes fileAttribute in fileAttributes[1..])
        {
            combinedAttributes |= fileAttribute;
        }

        return new EntryAttributes(combinedAttributes);
    }

    public EntryAttributes()
    {
        ThrowHelper.ThrowUnableToCallDefaultConstructor(typeof(EntryAttributes));
    }

    public EntryAttributes(FileAttributes attributes)
    {
        _value = attributes;
    }

    #region NonPublic
    private readonly FileAttributes _value;
    #endregion
}
