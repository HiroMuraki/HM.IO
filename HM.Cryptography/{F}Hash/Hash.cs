using HM.Common;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace HM.Cryptography;

public readonly struct Hash :
    IEquatable<Hash>,
    IEqualityOperators<Hash, Hash, Boolean>,
    IComparable<Hash>,
    IComparable
{
    public readonly Byte[] BinaryValue
    {
        get
        {
            Byte[] copiedHash = new Byte[_binaryValue.Length];
            _binaryValue.CopyTo(copiedHash, 0);
            return copiedHash;
        }
    }

    public readonly String StringValue => Convert.ToHexString(_binaryValue);

    public readonly Boolean Equals(Hash other)
    {
        if (_binaryValue.Length != other._binaryValue.Length)
        {
            return false;
        }

        for (Int32 i = 0; i < _binaryValue.Length; i++)
        {
            if (_binaryValue[i] != other._binaryValue[i])
            {
                return false;
            }
        }

        return true;
    }

    public readonly override Boolean Equals([NotNullWhen(true)] Object? obj)
        => ComparisonHelper.StructEquals(this, obj);

    public readonly Int32 CompareTo(Hash other)
    {
        Int32 minLength = Int32.Min(_binaryValue.Length, other._binaryValue.Length);

        for (Int32 i = 0; i < minLength; i++)
        {
            Int32 compareResult = _binaryValue[i].CompareTo(other._binaryValue[i]);
            if (compareResult < 0)
            {
                return -1;
            }
            else if (compareResult > 0)
            {
                return 1;
            }
        }

        if (_binaryValue.Length < other._binaryValue.Length)
        {
            return -1;
        }
        else if (_binaryValue.Length > other._binaryValue.Length)
        {
            return 1;
        }

        return 0;
    }

    public readonly Int32 CompareTo(Object? obj)
        => ComparisonHelper.StructCompareTo(this, obj);

    public readonly override Int32 GetHashCode()
    {
        Int32 hashCode = 0;

        for (Int32 i = 0; i < _binaryValue.Length; i++)
        {
            hashCode = HashCode.Combine(hashCode, _binaryValue[i]);
        }

        return hashCode;
    }

    public static Boolean operator ==(Hash left, Hash right)
    {
        return left.Equals(right);
    }

    public static Boolean operator !=(Hash left, Hash right)
    {
        return !(left == right);
    }

    public readonly override String ToString()
        => ToStringHelper.Build(this);

    public Hash()
    {
        ThrowHelper.ThrowUnableToCallDefaultConstructor(GetType());
        _binaryValue = [];
    }

    public Hash(Byte[] hash)
    {
        _binaryValue = new Byte[hash.Length];
        hash.CopyTo(_binaryValue, 0);
    }

    #region NonPublic
    private readonly Byte[] _binaryValue;
    #endregion
}
