#pragma warning disable IDE0049 // 使用框架类型
using HM.IO.Previews;
using LibraryTest.Helpers;

namespace LibraryTest.Preview;

[TestClass]
[TestCategory("Struct Types")]
public class TYPE_TEST_DirectoryPath : TestClassBase
{
    [TestMethod]
    public void CTOR_ALL()
    {
        Assert.ThrowsException<InvalidOperationException>(() => new DirectoryPath());
        Assert.ThrowsException<ArgumentNullException>(() => new DirectoryPath(null!));
    }

    [TestMethod]
    public void PROP_StringPath()
    {
        var path1 = new DirectoryPath(@"");
        Assert.AreEqual(@"", path1.StringPath);

        var path2 = new DirectoryPath(@"C:\Windows\system32");
        Assert.AreEqual(@"C:\Windows\system32", path2.StringPath);
    }

    [TestMethod]
    public void PROP_ParentDirectory()
    {
        var path1 = new DirectoryPath(@"");
        Assert.AreEqual(new DirectoryPath(""), path1.ParentDirectory);

        var path2 = new DirectoryPath(@"\root\user\apps");
        Assert.AreEqual(new DirectoryPath(@"\root\user"), path2.ParentDirectory);

        var path3 = new DirectoryPath(@"C:\Windows\system32\apps");
        Assert.AreEqual(new DirectoryPath(@"C:\Windows\system32"), path3.ParentDirectory);
    }

    [TestMethod]
    public void PROP_DirectoryName()
    {
        var path1 = new DirectoryPath(@"");
        Assert.AreEqual(@"", path1.DirectoryName);

        var path2 = new DirectoryPath(@"C:\Windows\system32");
        Assert.AreEqual(@"system32", path2.DirectoryName);

        var path3 = new DirectoryPath(@"C:\Windows\system32\myapps");
        Assert.AreEqual(@"myapps", path3.DirectoryName);

        var path4 = new DirectoryPath(@"\root\user\data");
        Assert.AreEqual(@"data", path4.DirectoryName);
    }

    [TestMethod]
    public void COMP_EqualityComparison()
    {
        EqualityTestHelper.Test_Fully(
            equalValues: [
                new DirectoryPath(@"C:\Windows\system32"),
                new DirectoryPath(@"C:\Windows\system32"),
            ],
            notEqualValues: [
                new DirectoryPath(@""),
                new DirectoryPath(@"C:\Windows"),
                new DirectoryPath(@"C:\Windows\system64"),
                new DirectoryPath(@"C:\Windows\system32\apps"),
            ]);
    }
}