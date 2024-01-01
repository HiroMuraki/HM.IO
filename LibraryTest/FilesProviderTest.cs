#pragma warning disable IDE0049 // 使用框架类型

using HM.IO;
using HM.IO.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.CompilerServices;

namespace LibraryTest;

[TestClass]
public partial class FilesProviderTest : TestClass
{
    public static string TestCasesBaseDirectory => Path.Combine(Environment.CurrentDirectory, "TestCases");

    public FilesProviderTest()
    {
        System.Diagnostics.Debug.WriteLine($"[{GetType()}] TestTime: {DateTime.Now}"); // debug output
    }

    [TestMethod]
    public void MyTestMethod()
    {
        var sdir = new SearchingDirectory()
        {
            Path = EntryPath.CreateFromPath(Path.Combine(TestCasesBaseDirectory, @"B")),
            RecurseSubdirectories = true,
            MaxRecursionDepth = 1
        };

        FilesProvider fp = FilesProvider.Create().IncludeDirectory(sdir);

        foreach (var item in fp.EnumerateFiles())
        {
            System.Diagnostics.Debug.WriteLine($"{item}"); // debug output
        }
    }

    [TestMethod]
    public void SubTest1_IncludedDirecoriesOnly()
    {
        TestHelper(new()
        {
            IncDirs = [
                Path.Combine(TestCasesBaseDirectory, @"A"),
                Path.Combine(TestCasesBaseDirectory, @"B"),
                Path.Combine(TestCasesBaseDirectory, @"C"),
            ],
            ExcDirs = [

            ],
            IncFiles = [

            ],
            ExcFiles = [

            ]
        }, [
            Path.Combine(TestCasesBaseDirectory, @"A\A-FILE_1"),
            Path.Combine(TestCasesBaseDirectory, @"A\A-FILE_2"),
            Path.Combine(TestCasesBaseDirectory, @"A\A-FILE_3"),

            Path.Combine(TestCasesBaseDirectory, @"B\B-FILE_1"),
            Path.Combine(TestCasesBaseDirectory, @"B\B-FILE_2"),
            Path.Combine(TestCasesBaseDirectory, @"B\B-FILE_3"),

            Path.Combine(TestCasesBaseDirectory, @"C\C-FILE_1"),
            Path.Combine(TestCasesBaseDirectory, @"C\C-FILE_2"),
        ]);
    }

    [TestMethod]
    public void SubTest2_IncludedFilesOnly()
    {
        TestHelper(new()
        {
            IncDirs = [

            ],
            ExcDirs = [

            ],
            IncFiles = [
                Path.Combine(TestCasesBaseDirectory, @"a.txt"),
                Path.Combine(TestCasesBaseDirectory, @"b.txt"),
                Path.Combine(TestCasesBaseDirectory, @"A\A-FILE_1"),
                Path.Combine(TestCasesBaseDirectory, @"A\A-FILE_2"),
                Path.Combine(TestCasesBaseDirectory, @"B\B-FILE_1"),
                Path.Combine(TestCasesBaseDirectory, @"C\C-FILE_2)"),
            ],
            ExcFiles = [

            ]
        }, [
            Path.Combine(TestCasesBaseDirectory, @"a.txt"),
            Path.Combine(TestCasesBaseDirectory, @"b.txt"),
            Path.Combine(TestCasesBaseDirectory, @"A\A-FILE_1"),
            Path.Combine(TestCasesBaseDirectory, @"A\A-FILE_2"),
            Path.Combine(TestCasesBaseDirectory, @"B\B-FILE_1"),
            Path.Combine(TestCasesBaseDirectory, @"C\C-FILE_2)"),
        ]);
    }

    [TestMethod]
    public void SubTest3_IncludedAndExcludedDirectoriesOnly()
    {
        TestHelper(new()
        {
            IncDirs = [
                Path.Combine(TestCasesBaseDirectory, @"A"),
                Path.Combine(TestCasesBaseDirectory, @"B"),
                Path.Combine(TestCasesBaseDirectory, @"C"),
            ],
            ExcDirs = [
                Path.Combine(TestCasesBaseDirectory, @"A"),
                Path.Combine(TestCasesBaseDirectory, @"C"),
            ],
            IncFiles = [

            ],
            ExcFiles = [

            ]
        }, [
            Path.Combine(TestCasesBaseDirectory, @"B\B-FILE_1"),
            Path.Combine(TestCasesBaseDirectory, @"B\B-FILE_2"),
            Path.Combine(TestCasesBaseDirectory, @"B\B-FILE_3"),
        ]);
    }

    [TestMethod]
    public void SubTest4_IncludedAndExcludedFilesOnly()
    {
        TestHelper(new()
        {
            IncDirs = [

            ],
            ExcDirs = [

            ],
            IncFiles = [
                Path.Combine(TestCasesBaseDirectory, @"a.txt"),
                Path.Combine(TestCasesBaseDirectory, @"b.txt"),
                Path.Combine(TestCasesBaseDirectory, @"A\A-FILE_1"),
                Path.Combine(TestCasesBaseDirectory, @"A\A-FILE_2"),
                Path.Combine(TestCasesBaseDirectory, @"B\B-FILE_1"),
                Path.Combine(TestCasesBaseDirectory, @"C\C-FILE_2)"),
            ],
            ExcFiles = [
                Path.Combine(TestCasesBaseDirectory, @"a.txt"),
                Path.Combine(TestCasesBaseDirectory, @"A\A-FILE_1"),
                Path.Combine(TestCasesBaseDirectory, @"C\C-FILE_2)"),
            ]
        }, [
            Path.Combine(TestCasesBaseDirectory, @"b.txt"),
            Path.Combine(TestCasesBaseDirectory, @"A\A-FILE_2"),
            Path.Combine(TestCasesBaseDirectory, @"B\B-FILE_1"),
        ]);
    }

    [TestMethod]
    public void SubTest5_Full()
    {
        TestHelper(new()
        {
            IncDirs = [
                Path.Combine(TestCasesBaseDirectory, @"A"),
                Path.Combine(TestCasesBaseDirectory, @"B"),
                Path.Combine(TestCasesBaseDirectory, @"C"),
            ],
            ExcDirs = [
                Path.Combine(TestCasesBaseDirectory, @"B"),
            ],
            IncFiles = [
                Path.Combine(TestCasesBaseDirectory, @"a.txt"),
                Path.Combine(TestCasesBaseDirectory, @"b.txt"),
            ],
            ExcFiles = [
                Path.Combine(TestCasesBaseDirectory, @"A\A-FILE_1"),
            ]
        }, [
            Path.Combine(TestCasesBaseDirectory, @"A\A-FILE_2"),
            Path.Combine(TestCasesBaseDirectory, @"A\A-FILE_3"),
            Path.Combine(TestCasesBaseDirectory, @"C\C-FILE_1"),
            Path.Combine(TestCasesBaseDirectory, @"C\C-FILE_2"),
            Path.Combine(TestCasesBaseDirectory, @"a.txt"),
            Path.Combine(TestCasesBaseDirectory, @"b.txt"),
        ]);
    }

    [TestMethod]
    public void SubTest6_Recursive()
    {
        TestHelper_Recursive(new()
        {
            IncDirs = [
                new(Path.Combine(TestCasesBaseDirectory, @"A"), true, Int32.MaxValue),
                new(Path.Combine(TestCasesBaseDirectory, @"B"), true, 1),
                new(Path.Combine(TestCasesBaseDirectory, @"C"), true, 3),
            ],
            ExcDirs = [
                new(Path.Combine(TestCasesBaseDirectory, @"A\A1_r"), true, Int32.MaxValue),
                Path.Combine(TestCasesBaseDirectory, @"C\C1\C2"),
            ],
            IncFiles = [
                Path.Combine(TestCasesBaseDirectory, @"a.txt"),
                Path.Combine(TestCasesBaseDirectory, @"c.txt"),
            ],
            ExcFiles = [
                Path.Combine(TestCasesBaseDirectory, @"A\A-FILE_1"),
                Path.Combine(TestCasesBaseDirectory, @"B\B-FILE_2"),
                Path.Combine(TestCasesBaseDirectory, @"C\C1\C-C1-FILE_1"),
            ]
        }, [
            Path.Combine(TestCasesBaseDirectory, @"a.txt"),
            //Path.Combine(TestCasesBaseDirectory, @"b.txt"),
            Path.Combine(TestCasesBaseDirectory, @"c.txt"),

            //Path.Combine(TestCasesBaseDirectory, @"A\A-FILE_1"),
            Path.Combine(TestCasesBaseDirectory, @"A\A-FILE_2"),
            Path.Combine(TestCasesBaseDirectory, @"A\A-FILE_3"),
            Path.Combine(TestCasesBaseDirectory, @"A\A1\A-A1-FILE_1"),
            Path.Combine(TestCasesBaseDirectory, @"A\A1\A-A1-FILE_2"),
            Path.Combine(TestCasesBaseDirectory, @"A\A1\A-A1-FILE_3"),
            Path.Combine(TestCasesBaseDirectory, @"A\A1\A2\A-A1-A2-FILE_1"),
            Path.Combine(TestCasesBaseDirectory, @"A\A1\A2\A-A1-A2-FILE_2"),
            Path.Combine(TestCasesBaseDirectory, @"A\A1\A2\A-A1-A2-FILE_3"),
            Path.Combine(TestCasesBaseDirectory, @"A\A1\A2\A3\A-A1-A2-A3-FILE_1"),
            Path.Combine(TestCasesBaseDirectory, @"A\A1\A2\A3\A-A1-A2-A3-FILE_2"),
            Path.Combine(TestCasesBaseDirectory, @"A\A1\A2\A3\A4\A-A1-A2-A3-A4-FILE_1"),
            Path.Combine(TestCasesBaseDirectory, @"A\A1\A2\A3\A4\A-A1-A2-A3-A4-FILE_2"),
            Path.Combine(TestCasesBaseDirectory, @"A\A1\A2\A3\A4\A5\A-A1-A2-A3-A4-A5-FILE_1"),
            //Path.Combine(TestCasesBaseDirectory, @"A\A1_r\A-A1_r-FILE_1"),
            //Path.Combine(TestCasesBaseDirectory, @"A\A1_r\A-A1_r-FILE_2"),
            //Path.Combine(TestCasesBaseDirectory, @"A\A1_r\A-A1_r-FILE_3"),
            //Path.Combine(TestCasesBaseDirectory, @"A\A1_r\A2_r\A-A1_r-A2_r-FILE_1"),

            Path.Combine(TestCasesBaseDirectory, @"B\B-FILE_1"),
            //Path.Combine(TestCasesBaseDirectory, @"B\B-FILE_2"),
            Path.Combine(TestCasesBaseDirectory, @"B\B-FILE_3"),
            Path.Combine(TestCasesBaseDirectory, @"B\B1\B-B1-FILE_1"),
            Path.Combine(TestCasesBaseDirectory, @"B\B1\B-B1-FILE_2"),
            Path.Combine(TestCasesBaseDirectory, @"B\B1\B-B1-FILE_3"),
            //Path.Combine(TestCasesBaseDirectory, @"B\B1\B2\B-B1-B2-FILE_1"),
            //Path.Combine(TestCasesBaseDirectory, @"B\B1\B2\B-B1-B2-FILE_2"),
            //Path.Combine(TestCasesBaseDirectory, @"B\B1\B2\B-B1-B2-FILE_3"),
            //Path.Combine(TestCasesBaseDirectory, @"B\B1\B2\B3\B-B1-B2-B3-FILE_1"),

            Path.Combine(TestCasesBaseDirectory, @"C\C-FILE_1"),
            Path.Combine(TestCasesBaseDirectory, @"C\C-FILE_2"),
            //Path.Combine(TestCasesBaseDirectory, @"C\C1\C-C1-FILE_1"),
            Path.Combine(TestCasesBaseDirectory, @"C\C1\C-C1-FILE_2"),
            //Path.Combine(TestCasesBaseDirectory, @"C\C1\C2\C-C1-C2-FILE_1"),
            //Path.Combine(TestCasesBaseDirectory, @"C\C1\C2\C-C1-C2-FILE_2"),
            Path.Combine(TestCasesBaseDirectory, @"C\C1\C2\C3\C-C1-C2-C3-FILE_1"),
            Path.Combine(TestCasesBaseDirectory, @"C\C1\C2\C3\C-C1-C2-C3-FILE_2"),
            //Path.Combine(TestCasesBaseDirectory, @"C\C1\C2\C3\C4\C-C1-C2-C3-C4-FILE_1"),
            //Path.Combine(TestCasesBaseDirectory, @"C\C1\C2\C3\C4\C-C1-C2-C3-C4-FILE_2"),
        ]);
    }
}

public partial class FilesProviderTest
{
    static void TestHelper(SearchingOption option, List<string> expectedFiles, [CallerMemberName] string? caller = null)
    {
        System.Diagnostics.Debug.WriteLine(caller); // debug output
#pragma warning disable IDE0200
#pragma warning disable IDE0008 // 使用显式类型
        FilesProvider fp = null!;
        var orderExpectedFiles = expectedFiles.Order().ToList();

        // At once
        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                // SearchingType
                case 0:
                    fp = FilesProvider.Create()
                        .IncludeDirectories(
                            option.IncDirs.Select(x => new SearchingDirectory(EntryPath.CreateFromPath(x)))
                        )
                        .ExcludeDirectories(
                            option.ExcDirs.Select(x => new SearchingDirectory(EntryPath.CreateFromPath(x))
                        ))
                        .IncludeFiles(
                            option.IncFiles.Select(x => new SearchingFile(EntryPath.CreateFromPath(x)))
                        )
                        .ExcludeFiles(
                            option.ExcFiles.Select(x => new SearchingFile(EntryPath.CreateFromPath(x)))
                        );
                    break;
                // EntryPathType
                case 1:
                    fp = FilesProvider.Create()
                       .IncludeDirectories(
                           option.IncDirs.Select(x => EntryPath.CreateFromPath(x))
                       )
                       .ExcludeDirectories(
                           option.ExcDirs.Select(x => EntryPath.CreateFromPath(x)
                       ))
                       .IncludeFiles(
                           option.IncFiles.Select(x => EntryPath.CreateFromPath(x))
                       )
                       .ExcludeFiles(
                           option.ExcFiles.Select(x => EntryPath.CreateFromPath(x))
                       );
                    break;
                // StringType
                case 2:
                    fp = FilesProvider.Create()
                       .IncludeDirectories(
                           option.IncDirs.Select(x => x.Path)
                       )
                       .ExcludeDirectories(
                           option.ExcDirs.Select(x => x.Path)
                       )
                       .IncludeFiles(
                           option.IncFiles.Select(x => x.Path)
                       )
                       .ExcludeFiles(
                           option.ExcFiles.Select(x => x.Path)
                       );
                    break;
            }

            var files = fp.EnumerateFiles().Select(x => x.StringPath).Order().ToList();
            Assert.AreEqual(orderExpectedFiles.Count, files.Count);
            Assert.IsTrue(orderExpectedFiles.SequenceEqual(files));
        }

        // By each
        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                // SearchingType
                case 0:
                    fp = FilesProvider.Create();
                    foreach (var item in option.IncDirs.Select(x => new SearchingDirectory(EntryPath.CreateFromPath(x))))
                    {
                        fp.IncludeDirectory(item);
                    }
                    foreach (var item in option.ExcDirs.Select(x => new SearchingDirectory(EntryPath.CreateFromPath(x))))
                    {
                        fp.ExcludeDirectory(item);
                    }
                    foreach (var item in option.IncFiles.Select(x => new SearchingFile(EntryPath.CreateFromPath(x))))
                    {
                        fp.IncludeFile(item);
                    }
                    foreach (var item in option.ExcFiles.Select(x => new SearchingFile(EntryPath.CreateFromPath(x))))
                    {
                        fp.ExcludeFile(item);
                    }
                    break;
                // EntryPathType
                case 1:
                    fp = FilesProvider.Create();
                    foreach (var item in option.IncDirs.Select(x => EntryPath.CreateFromPath(x)))
                    {
                        fp.IncludeDirectory(item);
                    }
                    foreach (var item in option.ExcDirs.Select(x => EntryPath.CreateFromPath(x)))
                    {
                        fp.ExcludeDirectory(item);
                    }
                    foreach (var item in option.IncFiles.Select(x => EntryPath.CreateFromPath(x)))
                    {
                        fp.IncludeFile(item);
                    }
                    foreach (var item in option.ExcFiles.Select(x => EntryPath.CreateFromPath(x)))
                    {
                        fp.ExcludeFile(item);
                    }
                    break;
                // StringType
                case 2:
                    fp = FilesProvider.Create();
                    foreach (var item in option.IncDirs.Select(x => x.Path))
                    {
                        fp.IncludeDirectory(item);
                    }
                    foreach (var item in option.ExcDirs.Select(x => x.Path))
                    {
                        fp.ExcludeDirectory(item);
                    }
                    foreach (var item in option.IncFiles.Select(x => x.Path))
                    {
                        fp.IncludeFile(item);
                    }
                    foreach (var item in option.ExcFiles.Select(x => x.Path))
                    {
                        fp.ExcludeFile(item);
                    }
                    break;
            }

            var files = fp.EnumerateFiles().Select(x => x.StringPath).Order().ToList();
            Assert.AreEqual(orderExpectedFiles.Count, files.Count);
            Assert.IsTrue(orderExpectedFiles.SequenceEqual(files));
        }
#pragma warning restore IDE0200
#pragma warning restore IDE0008 // 使用显式类型
    }

    static void TestHelper_Recursive(SearchingOption option, List<string> expectedFiles, [CallerMemberName] string? caller = null)
    {
        System.Diagnostics.Debug.WriteLine(caller); // debug output
#pragma warning disable IDE0200
#pragma warning disable IDE0008 // 使用显式类型
        FilesProvider fp = null!;
        var orderExpectedFiles = expectedFiles.Order().ToList();

        // By once
        fp = FilesProvider.Create()
            .IncludeDirectories(
                option.IncDirs.Select(x => x.AsSearchingDirectory())
            )
            .ExcludeDirectories(
                option.ExcDirs.Select(x => x.AsSearchingDirectory())
            )
            .IncludeFiles(
                option.IncFiles.Select(x => new SearchingFile(EntryPath.CreateFromPath(x)))
            )
            .ExcludeFiles(
                option.ExcFiles.Select(x => new SearchingFile(EntryPath.CreateFromPath(x)))
            );

        var files = fp.EnumerateFiles().Select(x => x.StringPath).Order().ToList();
        files = fp.EnumerateFiles().Select(x => x.StringPath).Order().ToList();
        foreach (var item in files.ToHashSet().Except(orderExpectedFiles))
        {
            System.Diagnostics.Debug.WriteLine($"?? {item}"); // debug output
        }
        Assert.AreEqual(orderExpectedFiles.Count, files.Count);
        Assert.IsTrue(orderExpectedFiles.SequenceEqual(files));

        // By each
        fp = FilesProvider.Create();
        foreach (var item in option.IncDirs.Select(x => x.AsSearchingDirectory()))
        {
            fp.IncludeDirectory(item);
        }
        foreach (var item in option.ExcDirs.Select(x => x.AsSearchingDirectory()))
        {
            fp.ExcludeDirectory(item);
        }
        foreach (var item in option.IncFiles.Select(x => new SearchingFile(EntryPath.CreateFromPath(x))))
        {
            fp.IncludeFile(item);
        }
        foreach (var item in option.ExcFiles.Select(x => new SearchingFile(EntryPath.CreateFromPath(x))))
        {
            fp.ExcludeFile(item);
        }

        Assert.AreEqual(orderExpectedFiles.Count, files.Count);
        Assert.IsTrue(orderExpectedFiles.SequenceEqual(files));
#pragma warning restore IDE0200
#pragma warning restore IDE0008 // 使用显式类型
    }

    class SearchingOption
    {
        public PathOption[] IncDirs { get; set; } = [];
        public PathOption[] IncFiles { get; set; } = [];
        public PathOption[] ExcDirs { get; set; } = [];
        public PathOption[] ExcFiles { get; set; } = [];
    }

    class PathOption
    {
        public string Path { get; }
        public bool IncSub { get; }
        public int MaxRecDepth { get; }

        public PathOption(string path, bool incSub, int maxRecDepth)
        {
            Path = path;
            IncSub = incSub;
            MaxRecDepth = maxRecDepth;
        }

        public static implicit operator PathOption(string path)
        {
            return new PathOption(path, false, Int32.MinValue);
        }

        public static implicit operator string(PathOption option)
        {
            return option.Path;
        }

        public SearchingDirectory AsSearchingDirectory()
        {
            return new SearchingDirectory
            {
                Path = EntryPath.CreateFromPath(Path),
                RecurseSubdirectories = IncSub,
                MaxRecursionDepth = MaxRecDepth,
            };
        }
    }
}

//[TestMethod]
//public void MyTestMethod()
//{
//    var source = new FilesProvider.CompressedRoutedPath(new int[] { 1, 2, 3, 4, 5 });

//    var equals = new FilesProvider.CompressedEntryPath[]
//    {
//        new(new int[] { 1, 2, 3, 4, 5 }),
//    };

//    var notEquals = new FilesProvider.CompressedEntryPath[]
//    {
//        new(new int[] { 1, 2, 3, 4, 6 }),
//        new(new int[] { 1, 2, 3, 4, 5, 6 }),
//        new(new int[] { 1, 2, 3, 4 }),
//        new(new int[] { }),
//    };

//    Console.WriteLine(source.CompareTo(source));
//    Console.WriteLine(source.CompareTo(equals[0]));
//    Console.WriteLine(source.CompareTo(notEquals[0]));
//    Console.WriteLine(source.CompareTo(notEquals[1]));
//    Console.WriteLine(source.CompareTo(notEquals[2]));
//    Console.WriteLine(source.CompareTo(notEquals[3]));
//    TestEquality(source, source, true);
//    foreach (var equal in equals)
//    {
//        TestEquality(source, equal, true);
//    }
//    foreach (var notEqual in notEquals)
//    {
//        TestEquality(source, notEqual, false);
//    }

//    static void TestEquality(FilesProvider.CompressedEntryPath a, FilesProvider.CompressedEntryPath b, bool equal)
//    {
//        if (equal)
//        {
//            Assert.IsTrue(a == b);
//            Assert.IsTrue(!(a != b));

//            Assert.IsTrue(a.Equals(b));

//            Assert.IsTrue(a.CompareTo(b) == 0);
//            Assert.IsTrue(!(a.CompareTo(b) != 0));

//            Assert.IsTrue(a.GetHashCode() == b.GetHashCode());
//            Assert.IsTrue(!(a.GetHashCode() != b.GetHashCode()));
//        }
//        else
//        {
//            Assert.IsFalse(a == b);
//            Assert.IsFalse(!(a != b));

//            Assert.IsFalse(a.Equals(b));

//            Assert.IsFalse(a.CompareTo(b) == 0);
//            Assert.IsFalse(!(a.CompareTo(b) != 0));
//        }
//    }
//}

//[TestMethod]
//public void TestCommon()
//{
//    PrintTimestamp();

//    var ep1 = EntryPath.CreateFromPath(@"C:\Windows\System32");
//    var ep2 = EntryPath.CreateFromPath(@"C:\Windows\System32\etc");
//    var ep3 = EntryPath.CreateFromPath(@"C:\Windows\Wows64\etc");

//    System.Diagnostics.Debug.WriteLine($"{ep2.IsSubPathOf(ep1)}"); // debug output
//    System.Diagnostics.Debug.WriteLine($"{ep3.IsSubPathOf(ep1)}"); // debug output
//                                                                   //var array1 = new string[] { "AAA", "BBB", "CCC" };
//                                                                   //var array2 = new string[] { "AAA", "BBB", "CCC" };
//                                                                   //var a = array1.ToImmutableArray(); 
//                                                                   //var b = array2.ToImmutableArray();
//                                                                   //System.Diagnostics.Debug.WriteLine($"{array1.GetHashCode()}"); // debug output
//                                                                   //System.Diagnostics.Debug.WriteLine($"{array2.GetHashCode()}"); // debug output
//                                                                   //System.Diagnostics.Debug.WriteLine($""); // debug output
//                                                                   //System.Diagnostics.Debug.WriteLine($"{a.GetHashCode()}"); // debug output
//                                                                   //System.Diagnostics.Debug.WriteLine($"{b.GetHashCode()}"); // debug output
//                                                                   //return;

//    //EntryPath[] paths =
//    //{
//    //    EntryPath.CreateFromPath(@"C:\Users\11717\Downloads\_FP TEST CASES\A\*"),
//    //    EntryPath.CreateFromPath(@"C:\Users\11717\Downloads\_FP TEST CASES\B\*"),
//    //    EntryPath.CreateFromPath(@"C:\Users\11717\Downloads\_FP TEST CASES\C"),
//    //};
//    //EntryPath[] eep =
//    //{
//    //    EntryPath.CreateFromPath(@"C:\Users\11717\Downloads\_FP TEST CASES\C")
//    //};
//    //bool e20 = EntryPathEqualityComparer.Default.Equals(paths[2], eep[0]);
//    //int hashCode3 = EntryPathEqualityComparer.Default.GetHashCode(paths[0]);
//    //int hashCode1 = EntryPathEqualityComparer.Default.GetHashCode(paths[2]);
//    //int hashCode2 = EntryPathEqualityComparer.Default.GetHashCode(eep[0]);
//    //System.Diagnostics.Debug.WriteLine($"{EqualityComparer<EntryPath>.Default.Equals(eep[0], paths[2])}"); // debug output
//    //System.Diagnostics.Debug.WriteLine($"{EqualityComparer<EntryPath>.Default.GetHashCode(eep[0])}"); // debug output
//    //System.Diagnostics.Debug.WriteLine($"{EqualityComparer<EntryPath>.Default.GetHashCode(paths[2])}"); // debug output
//    //var set = new HashSet<EntryPath>(paths, EntryPathEqualityComparer.Default);
//    //bool c = set.Contains(eep[0]);
//    //return;

//    //bool e00 = EntryPathEqualityComparer.Default.Equals(paths[0], eep[0]);
//    //bool e10 = EntryPathEqualityComparer.Default.Equals(paths[1], eep[0]);
//    //var eps = paths.Except(eep, EntryPathEqualityComparer.Default).ToList();

//    //int[] values = { 1, 2, 3, 4, 5 };
//    //int[] ev = { 2, 3, 4 };
//    //var ints = values.Except(ev).ToList();
//    //;

//    //string[] strs1 =
//    //{
//    //    "AAA",
//    //    "BBB",
//    //    "CCC",
//    //    "DDD",
//    //};
//    //var selected = strs1.Except(new string[] { "BBB", "CCC" });
//    //foreach (var item in selected)
//    //{
//    //    System.Diagnostics.Debug.WriteLine($"{item}"); // debug output
//    //}

//    //System.Diagnostics.Debug.WriteLine("12345"[0..^1]); // debug output
//}