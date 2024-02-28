#pragma warning disable IDE0049 // 使用框架类型
using HM.IO.Previews;
using LibraryTest.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibraryTest.Preview;

[TestClass]
[TestCategory("Struct Types")]
public class TYPE_TEST_EntryAttributes : TestClassBase
{
    [TestMethod]
    public void CTOR_ALL()
    {
        Assert.ThrowsException<InvalidOperationException>(() => new EntryAttributes());
    }

    [TestMethod]
    public void PROP_Value()
    {
        FileAttributes[] attrs = [
            FileAttributes.Normal,
            FileAttributes.Directory | FileAttributes.ReparsePoint,
            FileAttributes.ReadOnly | FileAttributes.Archive | FileAttributes.Compressed,
        ];

        foreach (FileAttributes attr in attrs)
        {
            var attributes = new EntryAttributes(attr);
            Assert.AreEqual(attributes.Value, attr);
        }
    }

    [TestMethod]
    public void METH_HasAttribute()
    {
        FileAttributes attr = FileAttributes.ReadOnly | FileAttributes.Archive | FileAttributes.Compressed;
        var attributes = new EntryAttributes(attr);
        Assert.IsTrue(attributes.HasAttribute(FileAttributes.ReadOnly));
        Assert.IsTrue(attributes.HasAttribute(FileAttributes.Archive));
        Assert.IsTrue(attributes.HasAttribute(FileAttributes.Compressed));
        Assert.IsFalse(attributes.HasAttribute(FileAttributes.Directory));
    }

    [TestMethod]
    public void COMP_EqualityComparison()
    {
        EqualityTestHelper.Assert_Fully(
            equalValues: [
                new EntryAttributes(FileAttributes.Directory | FileAttributes.ReparsePoint),
                new EntryAttributes(FileAttributes.Directory | FileAttributes.ReparsePoint),
            ],
            notEqualValues:
            [
                new EntryAttributes(FileAttributes.None),
                new EntryAttributes(FileAttributes.Directory),
                new EntryAttributes(FileAttributes.ReparsePoint),
                new EntryAttributes(FileAttributes.Hidden | FileAttributes.ReadOnly),
            ]);
    }
}

