#pragma warning disable IDE0049 // 使用框架类型

namespace LibraryTest.Helpers;

public static class ComparisonTestHelper
{
    public static void Assert_Fully<T>(T[] equalValues, T[] lessThanValue, T[] greaterThanValues)
        where T : notnull
    {
        for (int i = 0; i < equalValues.Length; i++)
        {
            for (int j = 0; j < equalValues.Length; j++)
            {
                Assert_Equals(equalValues[j], equalValues[i]);
            }

            for (int j = 0; j < lessThanValue.Length; j++)
            {
                Assert_LessThan(lessThanValue[j], equalValues[i]);
            }

            for (int j = 0; j < greaterThanValues.Length; j++)
            {
                Assert_GreaterThan(greaterThanValues[j], equalValues[i]);
            }
        }
    }

    public static void Assert_Equals<T>(T a, T b)
        where T : notnull
    {
        // IComparable<T>.Compare
        if (typeof(T).IsAssignableTo(typeof(IComparable<T>)))
        {
            Assert.IsTrue(((IComparable<T>)a).CompareTo(b) == 0);
            Assert.IsFalse(((IComparable<T>)a).CompareTo(b) != 0);
        }

        // IComparable.Compare
        if (typeof(T).IsAssignableTo(typeof(IComparable)))
        {
            Assert.IsTrue(((IComparable)a).CompareTo(b) == 0);
            Assert.IsFalse(((IComparable)a).CompareTo(b) != 0);
        }
    }

    public static void Assert_LessThan<T>(T a, T b)
        where T : notnull
    {
        // IComparable<T>.Compare
        if (typeof(T).IsAssignableTo(typeof(IComparable<T>)))
        {
            Assert.IsFalse(((IComparable<T>)a).CompareTo(b) < 0);
        }

        // IComparable.Compare
        if (typeof(T).IsAssignableTo(typeof(IComparable)))
        {
            Assert.IsFalse(((IComparable)a).CompareTo(b) < 0);
        }
    }

    public static void Assert_GreaterThan<T>(T a, T b)
        where T : notnull
    {
        // IComparable<T>.Compare
        if (typeof(T).IsAssignableTo(typeof(IComparable<T>)))
        {
            Assert.IsFalse(((IComparable<T>)a).CompareTo(b) > 0);

        }

        // IComparable.Compare
        if (typeof(T).IsAssignableTo(typeof(IComparable)))
        {
            Assert.IsFalse(((IComparable)a).CompareTo(b) > 0);
        }
    }
}
