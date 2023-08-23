using HM.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Immutable;
using System.Numerics;
using System.Reflection;

namespace LibraryTest;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestCommon()
    {
        var ep1 = EntryPath.CreateFromPath(@"C:\Windows\System32");
        var ep2 = EntryPath.CreateFromPath(@"C:\Windows\System32\etc");
        var ep3 = EntryPath.CreateFromPath(@"C:\Windows\Wows64\etc");

        System.Diagnostics.Debug.WriteLine($"{ep2.IsSubPathOf(ep1)}"); // debug output
        System.Diagnostics.Debug.WriteLine($"{ep3.IsSubPathOf(ep1)}"); // debug output
        //var array1 = new string[] { "AAA", "BBB", "CCC" };
        //var array2 = new string[] { "AAA", "BBB", "CCC" };
        //var a = array1.ToImmutableArray(); 
        //var b = array2.ToImmutableArray();
        //System.Diagnostics.Debug.WriteLine($"{array1.GetHashCode()}"); // debug output
        //System.Diagnostics.Debug.WriteLine($"{array2.GetHashCode()}"); // debug output
        //System.Diagnostics.Debug.WriteLine($""); // debug output
        //System.Diagnostics.Debug.WriteLine($"{a.GetHashCode()}"); // debug output
        //System.Diagnostics.Debug.WriteLine($"{b.GetHashCode()}"); // debug output
        //return;

        //EntryPath[] paths =
        //{
        //    EntryPath.CreateFromPath(@"C:\Users\11717\Downloads\_FP TEST CASES\A\*"),
        //    EntryPath.CreateFromPath(@"C:\Users\11717\Downloads\_FP TEST CASES\B\*"),
        //    EntryPath.CreateFromPath(@"C:\Users\11717\Downloads\_FP TEST CASES\C"),
        //};
        //EntryPath[] eep =
        //{
        //    EntryPath.CreateFromPath(@"C:\Users\11717\Downloads\_FP TEST CASES\C")
        //};
        //bool e20 = EntryPathEqualityComparer.Default.Equals(paths[2], eep[0]);
        //int hashCode3 = EntryPathEqualityComparer.Default.GetHashCode(paths[0]);
        //int hashCode1 = EntryPathEqualityComparer.Default.GetHashCode(paths[2]);
        //int hashCode2 = EntryPathEqualityComparer.Default.GetHashCode(eep[0]);
        //System.Diagnostics.Debug.WriteLine($"{EqualityComparer<EntryPath>.Default.Equals(eep[0], paths[2])}"); // debug output
        //System.Diagnostics.Debug.WriteLine($"{EqualityComparer<EntryPath>.Default.GetHashCode(eep[0])}"); // debug output
        //System.Diagnostics.Debug.WriteLine($"{EqualityComparer<EntryPath>.Default.GetHashCode(paths[2])}"); // debug output
        //var set = new HashSet<EntryPath>(paths, EntryPathEqualityComparer.Default);
        //bool c = set.Contains(eep[0]);
        //return;

        //bool e00 = EntryPathEqualityComparer.Default.Equals(paths[0], eep[0]);
        //bool e10 = EntryPathEqualityComparer.Default.Equals(paths[1], eep[0]);
        //var eps = paths.Except(eep, EntryPathEqualityComparer.Default).ToList();

        //int[] values = { 1, 2, 3, 4, 5 };
        //int[] ev = { 2, 3, 4 };
        //var ints = values.Except(ev).ToList();
        //;

        //string[] strs1 =
        //{
        //    "AAA",
        //    "BBB",
        //    "CCC",
        //    "DDD",
        //};
        //var selected = strs1.Except(new string[] { "BBB", "CCC" });
        //foreach (var item in selected)
        //{
        //    System.Diagnostics.Debug.WriteLine($"{item}"); // debug output
        //}

        //System.Diagnostics.Debug.WriteLine("12345"[0..^1]); // debug output
    }

    [TestMethod]
    public void TestFilesProvider()
    {
        System.Diagnostics.Debug.WriteLine($"TestTime: {DateTime.Now}"); // debug output
        var fp = new FilesProvider()
        {
            IncludingDirectories =
            {
                @"C:\Users\11717\Downloads\_FP TEST CASES\A\*",
                @"C:\Users\11717\Downloads\_FP TEST CASES\B\*",
                @"C:\Users\11717\Downloads\_FP TEST CASES\C",
            },
            ExcludingDirectories =
            {
                @"C:\Users\11717\Downloads\_FP TEST CASES\A\A2",
                @"C:\Users\11717\Downloads\_FP TEST CASES\B\B1",
            },
            IncludingFiles =
            {
                @"C:\Users\11717\Downloads\_FP TEST CASES\a.txt",
                @"C:\Users\11717\Downloads\_FP TEST CASES\b.txt",
            },
            ExcludingFiles =
            {
                @"C:\Users\11717\Downloads\_FP TEST CASES\A\A-FILE_1",
                @"C:\Users\11717\Downloads\_FP TEST CASES\B\B-FILE_2",
            }
        };

        var files = fp.EnumerateFiles();

        // Test: no any excluding included
        Assert.IsFalse(files.Any(x =>
        {
            return x.StringPath.StartsWith(@"C:\Users\11717\Downloads\_FP TEST CASES\A\A2")
                || x.StringPath.StartsWith(@"C:\Users\11717\Downloads\_FP TEST CASES\B\B1")
                || x.StringPath == @"C:\Users\11717\Downloads\_FP TEST CASES\A\A-FILE_1"
                || x.StringPath == @"C:\Users\11717\Downloads\_FP TEST CASES\B\B-FILE_2";
        }));

        foreach (var item in fp.EnumerateItems())
        {
            System.Diagnostics.Debug.WriteLine($"{item}"); // debug output
        }
    }

    [TestMethod]
    public void TestEntryPath()
    {
        var path = EntryPath.CreateFromPath(@"C:\Program Files (x86)\Steam\appcache\httpcache");
        System.Diagnostics.Debug.WriteLine($"{path}"); // debug output
        System.Diagnostics.Debug.WriteLine($"{path.StringPath}"); // debug output
    }

    [TestMethod]
    public void TestEntryPathComparer()
    {
        var target = EntryPath.CreateFromPath("C:\\Windows\\System32");
        (EntryPath, bool)[] paths =
        {
            (EntryPath.CreateFromPath("C:/Windows/System32"), true),
            (EntryPath.CreateFromPath("C:/Windows\\System32"), true),
            (EntryPath.CreateFromPath("C:\\Windows/System32"), true),
            (EntryPath.CreateFromPath("c:\\windows/system32"), true),
            (EntryPath.CreateFromPath("C:\\WINDOWS\\System32/"), true),
            (EntryPath.CreateFromPath("C:/Windows\\System32\\"), true),
            (EntryPath.CreateFromPath("C:\\wIndows\\sYstem32"), true),
            (EntryPath.CreateFromPath("C:\\wIndows\\sYstem321"), false),
            (EntryPath.CreateFromPath("C:\\wIndows1\\sYstem321"), false),
            (EntryPath.CreateFromPath("C:\\users"), false),
        };

        Assert.AreEqual(EntryPathEqualityComparer.Default.IgnoreCase, OperatingSystem.IsWindows());
        for (int i = 0; i < paths.Length; i++)
        {
            bool result = EntryPathEqualityComparer.Default.Equals(target, paths[i].Item1);
            Assert.AreEqual(result, paths[i].Item2);
        }
    }
}