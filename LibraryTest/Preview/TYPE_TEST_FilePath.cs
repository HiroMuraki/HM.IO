#pragma warning disable IDE0049 // 使用框架类型
using HM.IO.Previews.Directory;
using HM.IO.Previews.File;
using LibraryTest.Helpers;

namespace LibraryTest.Preview;

[TestClass]
[TestCategory("Struct Types")]
public class TYPE_TEST_FilePath : TestClassBase
{
    [TestMethod]
    public void CTOR_ALL()
    {
        Assert.ThrowsException<InvalidOperationException>(() => new FilePath());
        Assert.ThrowsException<ArgumentNullException>(() => new FilePath(null!));
    }

    [TestMethod]
    public void PROP_StringPath()
    {
        var path1 = new FilePath(@"");
        Assert.AreEqual(@"", path1.StringPath);

        var path2 = new FilePath(@"C:\Windows\system32\regedit.exe");
        Assert.AreEqual(@"C:\Windows\system32\regedit.exe", path2.StringPath);
    }

    [TestMethod]
    public void PROP_ParentDirectory()
    {
        var path1 = new FilePath(@"");
        Assert.AreEqual(new DirectoryPath(""), path1.ParentDirectory);

        var path2 = new FilePath(@"\root\system32\regedit.exe");
        Assert.AreEqual(new DirectoryPath(@"\root\system32"), path2.ParentDirectory);

        var path3 = new FilePath(@"C:\Windows\system32\regedit.exe");
        Assert.AreEqual(new DirectoryPath(@"C:\Windows\system32"), path3.ParentDirectory);
    }

    [TestMethod]
    public void PROP_FileName()
    {
        var path1 = new FilePath(@"");
        Assert.AreEqual(@"", path1.FileName);

        var path2 = new FilePath(@"C:\Windows\system32\regedit.exe");
        Assert.AreEqual(@"regedit", path2.FileName);

        var path3 = new FilePath(@"C:\Windows\system32\regedit");
        Assert.AreEqual(@"regedit", path3.FileName);

        var path4 = new FilePath(@"C:\Windows\system32\.exe");
        Assert.AreEqual(@"", path4.FileName);
    }

    [TestMethod]
    public void PROP_FileExtension()
    {
        var path1 = new FilePath(@"");
        Assert.AreEqual(@"", path1.FileExtension);

        var path2 = new FilePath(@"C:\Windows\system32\regedit.exe");
        Assert.AreEqual(@".exe", path2.FileExtension);

        var path3 = new FilePath(@"C:\Windows\system32\regedit");
        Assert.AreEqual(@"", path3.FileExtension);

        var path4 = new FilePath(@"C:\Windows\system32\.exe");
        Assert.AreEqual(@".exe", path4.FileExtension);
    }

    [TestMethod]
    public void COMP_EqualityComparison()
    {
        EqualityTestHelper.Assert_Fully(
            equalValues: [
                new FilePath(@"C:\Windows\system32\regedit.exe"),
                new FilePath(@"C:\Windows\system32\regedit.exe"),
            ],
            notEqualValues: [
                new FilePath(@""),
                new FilePath(@"C:\Windows\regedit.exe"),
                new FilePath(@"C:\Windows\system64\regedit.exe"),
                new FilePath(@"C:\Windows\system32\apps\regedit.exe"),
            ]);
    }
}
