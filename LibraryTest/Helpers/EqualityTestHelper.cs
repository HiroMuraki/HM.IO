#pragma warning disable IDE0049 // 使用框架类型

namespace LibraryTest.Helpers;

public static class EqualityTestHelper
{
    public static void Assert_Fully<T>(T[] equalValues, T[] notEqualValues)
        where T : notnull
    {
        for (int i = 0; i < equalValues.Length; i++)
        {
            for (int j = 0; j < equalValues.Length; j++)
            {
                Assert_Equals(equalValues[i], equalValues[j]);
            }

            for (int j = 0; j < notEqualValues.Length; j++)
            {
                Assert_NotEquals(equalValues[i], notEqualValues[j]);
            }
        }
    }

    public static void Assert_Equals<T>(T x, T y)
        where T : notnull
    {
        // Object.Equals
        Assert.IsTrue(x.Equals(y));
        Assert.IsFalse(x.Equals(null));

        // IEquatable<T>.Equals
        if (typeof(T).IsAssignableTo(typeof(IEquatable<T>)))
        {
            Assert.IsTrue(((IEquatable<T>)x).Equals(y));
        }

        // GetHashCode
        Assert.AreEqual(x.GetHashCode(), y.GetHashCode());

        // IComparable<T>.Compare
        if (typeof(T).IsAssignableTo(typeof(IComparable<T>)))
        {
            Assert.AreEqual(((IComparable<T>)x).CompareTo(y), 0);
        }

        // IComparable.Compare
        if (typeof(T).IsAssignableTo(typeof(IComparable)))
        {
            Assert.AreEqual(((IComparable)x).CompareTo(y), 0);
        }

        // Assert
        Assert.AreEqual(x, y);

        // // == operator
        // Assert.IsTrue(a == b);
        // Assert.IsTrue(b == a);

        // // != operator
        // Assert.IsFalse(a != b);
        // Assert.IsFalse(b != a);
    }

    public static void Assert_NotEquals<T>(T x, T y)
        where T : notnull
    {
        // Object.Equals
        Assert.IsFalse(x.Equals(y));
        Assert.IsFalse(x.Equals(null));

        // IEquatable<T>.Equals
        if (typeof(T).IsAssignableTo(typeof(IEquatable<T>)))
        {
            Assert.IsFalse(((IEquatable<T>)x).Equals(y));
        }

        // IComparable<T>.Compare
        if (typeof(T).IsAssignableTo(typeof(IComparable<T>)))
        {
            Assert.AreNotEqual(((IComparable<T>)x).CompareTo(y), 0);
        }

        // IComparable.Compare
        if (typeof(T).IsAssignableTo(typeof(IComparable)))
        {
            Assert.AreNotEqual(((IComparable)x).CompareTo(y), 0);
        }

        // Assert
        Assert.AreNotEqual(x, y);

        // // == operator
        // Assert.IsTrue(a == b);
        // Assert.IsTrue(b == a);

        // // != operator
        // Assert.IsFalse(a != b);
        // Assert.IsFalse(b != a);
    }
}