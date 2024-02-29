//#pragma warning disable IDE0049 // 使用框架类型
//using System.Reflection;

//namespace HM.IO.UnitTest;

//[TestClass]
//public class RoutedItemsTest : TestClass
//{
//    [TestMethod]
//    public void TestRoutedInt32()
//    {
//        PrintTimestamp();

//        var source = new RoutedInt32(new int[] { 0, 1, 2, 3, 4, 5 });
//        System.Diagnostics.Debug.WriteLine($"{source.StringPath}"); // debug output
//        var equals = new RoutedInt32[]
//        {
//            new RoutedInt32(new int[] { 0, 1, 2, 3, 4, 5 })
//        };
//        var notEquals = new RoutedInt32?[]
//        {
//            new RoutedInt32(new int[] { 0, 1, 2, 3, 4, 6 }),
//            new RoutedInt32(new int[] { 0, 1, 2, 3, 4 }),
//            new RoutedInt32(new int[] { 0, 1, 2, 3, 4, 5, 6 }),
//            new RoutedInt32(new int[] { 1, 2, 3, 4, 5}),
//            new RoutedInt32(new int[] { }),
//            null,
//        };

//        foreach (var equal in equals)
//        {
//            TestEquality(source, equal, true);
//        }

//        foreach (var notEqual in notEquals)
//        {
//            TestEquality(source, notEqual, false);
//        }

//        static void TestEquality(RoutedInt32 a, RoutedInt32? b, bool equal)
//        {
//            if (equal)
//            {
//                Assert.IsTrue(a == b);
//                Assert.IsTrue(!(a != b));

//                Assert.IsTrue(a.Equals(b));

//                Assert.IsTrue(a.CompareTo(b) == 0);
//                Assert.IsTrue(((IComparable)a).CompareTo(b) == 0);
//                Assert.IsTrue(!(a.CompareTo(b) != 0));
//                Assert.IsTrue(!(((IComparable)a).CompareTo(b) != 0));

//                Assert.IsTrue(a.GetHashCode() == b?.GetHashCode());
//                Assert.IsTrue(!(a.GetHashCode() != b?.GetHashCode()));
//            }
//            else
//            {
//                Assert.IsFalse(a == b);
//                Assert.IsFalse(!(a != b));

//                Assert.IsFalse(a.Equals(b));

//                Assert.IsFalse(a.CompareTo(b) == 0);
//                Assert.IsFalse(((IComparable)a).CompareTo(b) == 0);
//                Assert.IsFalse(!(a.CompareTo(b) != 0));
//                Assert.IsFalse(!(((IComparable)a).CompareTo(b) != 0));
//            }
//        }
//    }
//}

