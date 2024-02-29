#pragma warning disable IDE0049 // 使用框架类型
using HM.IO.Previews.File;
using HM.IO.UnitTest.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace HM.IO.UnitTest.Preview;

[TestClass]
[TestCategory("Struct Types")]
public class TYPE_TEST_FileSize : TestClassBase
{
    [TestMethod]
    public void CTORS()
    {
        Assert.ThrowsException<InvalidOperationException>(() => new FileSize());
    }

    [TestMethod]
    public void PROP_SizeInBytes()
    {
        AssertHelper((size, fileSize) =>
        {
            Assert.AreEqual(size, fileSize.SizeInBytes);
        });
    }

    [TestMethod]
    public void PROP_SizeInKBytes()
    {
        AssertHelper((size, fileSize) =>
        {
            Assert.AreEqual(size / 1024.0, fileSize.SizeInKBytes);
        });
    }

    [TestMethod]
    public void PROP_SizeInMBytes()
    {
        AssertHelper((size, fileSize) =>
        {
            Assert.AreEqual(size / 1024.0 / 1024, fileSize.SizeInMBytes);
        });
    }

    [TestMethod]
    public void PROP_SizeInGBytes()
    {
        AssertHelper((size, fileSize) =>
        {
            Assert.AreEqual(size / 1024.0 / 1024 / 1024, fileSize.SizeInGBytes);
        });
    }

    [TestMethod]
    public void COMP_EqualityComparison()
    {
        EqualityTestHelper.Assert_Fully(
            equalValues: [
                FileSize.FromBytes(64L * 1024 * 1024 * 1024),
                FileSize.FromKBytes(64L * 1024 * 1024),
                FileSize.FromMBytes(64L * 1024),
                FileSize.FromGBytes(64L),
            ],
            notEqualValues: [
                FileSize.FromBytes(0),
                FileSize.FromBytes(64),
                FileSize.FromBytes(114),
                FileSize.FromBytes(65535),
            ]);
    }

    #region NonPublic
    private static void AssertHelper(Action<long, FileSize> asserter)
    {
        long[] sizes = [
            0L,
            64L,
            64L * 1024,
            64L * 1024 * 1024,
            64L * 1024 * 1024 * 1024,
        ];

        foreach (long size in sizes)
        {
            asserter(size, FileSize.FromBytes(size));
        }
    }
    #endregion
}