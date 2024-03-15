using HM.Common;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;

namespace HM.Cryptography;

public readonly struct Key :
    IEquatable<Key>,
    IEqualityOperators<Key, Key, Boolean>,
    IComparable<Key>,
    IComparable
{
    public Byte this[Int32 index]
    {
        get
        {
            return _binaryValue[index];
        }
    }

    public Byte[] BinaryValue
    {
        get
        {
            Byte[] copiedKey = new Byte[_binaryValue.Length];
            _binaryValue.CopyTo(copiedKey.AsSpan());
            return copiedKey;
        }
    }

    public Int32 Length
    {
        get
        {
            return _binaryValue.Length;
        }
    }

    public static Key Create(String stringKey, Int32 expectingLength)
    {
        return Create(Encoding.UTF8.GetBytes(stringKey), expectingLength);
    }

    public static Key Create(Byte[] binaryKey, Int32 expectingLength)
    {
        if (binaryKey.Length > expectingLength)
        {
            throw new ArgumentException($"The length of key can't larger than expecting length");
        }

        Byte[] paddedKey = new Byte[expectingLength];
        for (Int32 i = 0; i < binaryKey.Length; i++)
        {
            paddedKey[i] = binaryKey[i];
        }

        return new Key(paddedKey);
    }

    public readonly Boolean Equals(Key other)
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

    public readonly Int32 CompareTo(Key other)
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

    public static Boolean operator ==(Key left, Key right)
    {
        return left.Equals(right);
    }

    public static Boolean operator !=(Key left, Key right)
    {
        return !(left == right);
    }

    public readonly override String ToString()
        => ToStringHelper.Build(this);

    #region NonPublic
    private readonly Byte[] _binaryValue;
    private Key(Byte[] binaryKey)
    {
        _binaryValue = binaryKey;
    }
    #endregion
}
