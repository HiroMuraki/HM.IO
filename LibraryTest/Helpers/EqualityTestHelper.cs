#pragma warning disable IDE0049 // 使用框架类型

using System.Numerics;

namespace LibraryTest.Helpers;

public static class EqualityTestHelper
{
    public static void Test_IEquatable<T>(T[] equalValues, T[] notEqualValues)
        where T : IEquatable<T>
    {
        for (int i = 0; i < equalValues.Length; i++)
        {
            for (int j = 0; j < equalValues.Length; j++)
            {
                Expect_IEquatableEquals(equalValues[i], equalValues[j]);
            }

            for (int j = 0; j < notEqualValues.Length; j++)
            {
                Expect_IEquatableNotEquals(equalValues[i], notEqualValues[j]);
            }
        }
    }

    public static void Test_Fully<T>(T[] equalValues, T[] notEqualValues)
        where T : IEquatable<T>, IEqualityOperators<T, T, bool>, IComparable<T>
    {
        for (int i = 0; i < equalValues.Length; i++)
        {
            for (int j = 0; j < equalValues.Length; j++)
            {
                Expect_FullyEquals(equalValues[i], equalValues[j]);
            }

            for (int j = 0; j < notEqualValues.Length; j++)
            {
                Expect_FullyNotEquals(equalValues[i], notEqualValues[j]);
            }
        }
    }

    public static void Expect_IEquatableEquals<T>(T a, T b)
        where T : IEquatable<T>
    {
        // object.Equals
        Assert.IsTrue(((object)a).Equals(b));
        Assert.IsTrue(((object)b).Equals(a));

        // IEquatable<T>.Equals
        Assert.IsTrue((a as IEquatable<T>)!.Equals(b));
        Assert.IsTrue((b as IEquatable<T>)!.Equals(a));

        // GetHashCode
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());

        // Assert
        Assert.AreEqual(a, b);
    }

    public static void Expect_FullyEquals<T>(T a, T b)
        where T : IEquatable<T>, IEqualityOperators<T, T, bool>, IComparable<T>
    {
        Expect_IEquatableEquals(a, b);

        // == operator
        Assert.IsTrue(a == b);
        Assert.IsTrue(b == a);

        // != operator
        Assert.IsFalse(a != b);
        Assert.IsFalse(b != a);

        // IComparable<T>.Compare
        Assert.AreEqual(a.CompareTo(b), 0);
        Assert.AreEqual(b.CompareTo(a), 0);

        // Assert
        Assert.AreEqual(a, b);
    }

    public static void Expect_IEquatableNotEquals<T>(T a, T b)
        where T : IEquatable<T>
    {
        // object.Equals
        Assert.IsFalse(((object)a).Equals(b));
        Assert.IsFalse(((object)b).Equals(a));

        // IEquatable<T>.Equals
        Assert.IsFalse((a as IEquatable<T>)!.Equals(b));
        Assert.IsFalse((b as IEquatable<T>)!.Equals(a));

        // GetHashCode
        // While even two different object can still share a same hash value, so, skip this

        // Assert
        Assert.AreNotEqual(a, b);
    }

    public static void Expect_FullyNotEquals<T>(T a, T b)
        where T : IEquatable<T>, IEqualityOperators<T, T, bool>, IComparable<T>
    {
        Expect_IEquatableNotEquals(a, b);

        // == operator
        Assert.IsFalse(a == b);
        Assert.IsFalse(b == a);

        // != operator
        Assert.IsTrue(a != b);
        Assert.IsTrue(b != a);

        // IComparable<T>.Compare
        Assert.AreNotEqual(a.CompareTo(b), 0);
        Assert.AreNotEqual(b.CompareTo(a), 0);

        // Assert
        Assert.AreNotEqual(a, b);
    }
}