#pragma warning disable IDE0049 // 使用框架类型

using HM.IO;

namespace LibraryTest;

[TestClass]
public class EntryPathTest : TestClass
{
    [TestMethod]
    public void Create()
    {
        string[] paths =
        {
            @"C:\Windows\System32",
            @"C:\Windows\System32/",
            @"C:\Windows\System32\",
            @"C:\Windows\System32",
        };

        foreach (string path in paths)
        {
            var entryPath = EntryPath.Create(path);
            Assert.IsTrue(entryPath.StringPath == @"C:\Windows\System32");
            Assert.IsTrue(entryPath.Length == 3);
            Assert.IsTrue(entryPath[0] == "C:");
            Assert.IsTrue(entryPath[1] == "Windows");
            Assert.IsTrue(entryPath[2] == "System32");
        }

        string[] excPaths =
        {
            null!,
            "",
            " ",
            "    ",
            "\n",
            "\r",
            "\t",
            " \n\r\t",
            "\t",
        };

        foreach (String path in excPaths)
        {
            Assert.ThrowsException<ArgumentException>(() => EntryPath.Create(path));
        }
    }

    [TestMethod]
    public void Comparer()
    {
        var refPath = EntryPath.Create("C:\\Windows\\System32");
        (EntryPath path, bool assertResult)[] testPaths =
        {
            (EntryPath.Create("C:\\Windows\\System32"), true),
            (EntryPath.Create("C:/Windows/System32"), true),
            (EntryPath.Create("C:/Windows\\System32"), true),
            (EntryPath.Create("C:\\Windows/System32"), true),
            (EntryPath.Create("C:/Windows\\System32\\"), true),

            (EntryPath.Create("c:\\windows/system32"), false),
            (EntryPath.Create("C:\\WINDOWS\\System32/"), false),
            (EntryPath.Create("C:\\wIndows\\sYstem32"), false),
            (EntryPath.Create("C:\\wIndows\\sYstem321"), false),
            (EntryPath.Create("C:\\wIndows1\\sYstem321"), false),
            (EntryPath.Create("C:\\users"), false),
        };

        foreach ((EntryPath path, Boolean assertResult) in testPaths)
        {
#pragma warning disable IDE0047 // 删除不必要的括号
            Assert.IsTrue((refPath == path) == assertResult);
            Assert.IsTrue((refPath.Equals(path)) == assertResult);
            Assert.IsTrue((((object)refPath).Equals(path)) == assertResult);
            if (assertResult)
            {
                Assert.IsTrue(refPath.GetHashCode() == refPath.GetHashCode());
            }
#pragma warning restore IDE0047 // 删除不必要的括号
        }
    }

    [TestMethod]
    public void IsSubPathOf_IsParentPathOf()
    {
        var refPath = EntryPath.Create("C:/User/abc/Programs/Games");

        (EntryPath path, bool assertResult)[] testPaths =
        {
            (EntryPath.Create("C:"), true),
            (EntryPath.Create("C:/User"), true),
            (EntryPath.Create("C:/User/abc"), true),
            (EntryPath.Create("C:/User/abc/Programs"), true),

            (EntryPath.Create("E:"), false),
            (EntryPath.Create("C:/User/abcd"), false),
            (EntryPath.Create("C:/User/abc/Programs/Game"), false),
        };

        foreach ((EntryPath path, Boolean assertResult) in testPaths)
        {
            Assert.AreEqual(assertResult, refPath.IsSubPathOf(path));
            Assert.AreEqual(assertResult, path.IsParentPathOf(refPath));
        }
    }

    [TestMethod]
    public void DirectoryName_EntryName()
    {
        (string ePath, string eDirectoryName, string eEntryName)[] paths =
        {
            (
                @"C:\User\abc\Programs\Games\CS\Launcher.exe",
                @"C:\User\abc\Programs\Games\CS",
                @"Launcher.exe"
            ),
            (
                @"C:",
                @"",
                @"C:"
            ),
            (
                @"Dir\game.exe",
                @"Dir",
                @"game.exe"
            )
        };

        foreach ((String ePath, String eDirectoryName, String eEntryName) in paths)
        {
            var refPath = EntryPath.Create(ePath);

            Assert.AreEqual(eDirectoryName, refPath.DirectoryName.StringPath);

            Assert.AreEqual(eEntryName, refPath.EntryName.StringPath);
        }
    }
}