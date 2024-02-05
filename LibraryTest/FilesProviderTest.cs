#pragma warning disable IDE0008 // 使用显式类型
#pragma warning disable IDE0049 // 使用框架类型

using HM.IO;
using HM.IO.Providers;
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
    public void ForFile_IncludedDirecoriesOnly()
    {
        TestHelper_File(new()
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
    public void ForFile_IncludedFilesOnly()
    {
        TestHelper_File(new()
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
    public void ForFile_IncludedAndExcludedDirectoriesOnly()
    {
        TestHelper_File(new()
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
    public void ForFile_IncludedAndExcludedFilesOnly()
    {
        TestHelper_File(new()
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
    public void ForFiles_Full()
    {
        TestHelper_File(new()
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
    public void ForFiles_Recursive()
    {
        TestHelper_File_Recursive(new()
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
    [TestMethod]
    public void ForDirectory_IncludedDirecoriesOnly()
    {
        TestHelper_Directory(new()
        {
            IncDirs =
            [
                TestCasesBaseDirectory,
            ]
        },
        [
            Path.Combine(TestCasesBaseDirectory, "A"),
            Path.Combine(TestCasesBaseDirectory, "B"),
            Path.Combine(TestCasesBaseDirectory, "C"),
        ]);
    }
}

public partial class FilesProviderTest
{
    private static void TestHelper_File(SearchingOption option, List<string> expectedFiles, [CallerMemberName] string? caller = null)
    {
        System.Diagnostics.Debug.WriteLine(caller); // debug output

        EntryPathsProvider fp = null!;
        var orderExpectedFiles = expectedFiles.Order().ToList();

        // At once
        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                // SearchingType
                case 0:
                    fp = EntryPathsProvider.Create()
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
                    fp = EntryPathsProvider.Create()
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
                    fp = EntryPathsProvider.Create()
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

            var files = fp.EnumerateFilePaths().Select(x => x.StringPath).Order().ToList();
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
                    fp = EntryPathsProvider.Create();
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
                    fp = EntryPathsProvider.Create();
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
                    fp = EntryPathsProvider.Create();
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

            var files = fp.EnumerateFilePaths().Select(x => x.StringPath).Order().ToList();
            Assert.AreEqual(orderExpectedFiles.Count, files.Count);
            Assert.IsTrue(orderExpectedFiles.SequenceEqual(files));
        }
    }

    private static void TestHelper_File_Recursive(SearchingOption option, List<string> expectedFiles, [CallerMemberName] string? caller = null)
    {
        System.Diagnostics.Debug.WriteLine(caller); // debug output

        EntryPathsProvider fp = null!;
        var orderExpectedFiles = expectedFiles.Order().ToList();

        // By once
        fp = EntryPathsProvider.Create()
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

        var files = fp.EnumerateFilePaths().Select(x => x.StringPath).Order().ToList();
        files = fp.EnumerateFilePaths().Select(x => x.StringPath).Order().ToList();
        foreach (var item in files.ToHashSet().Except(orderExpectedFiles))
        {
            System.Diagnostics.Debug.WriteLine($"?? {item}"); // debug output
        }
        Assert.AreEqual(orderExpectedFiles.Count, files.Count);
        Assert.IsTrue(orderExpectedFiles.SequenceEqual(files));

        // By each
        fp = EntryPathsProvider.Create();
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
    }

    private static void TestHelper_Directory(SearchingOption option, List<string> expectedFiles, [CallerMemberName] string? caller = null)
    {
        System.Diagnostics.Debug.WriteLine(caller); // debug output

        var ep = EntryPathsProvider.Create()
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

        Assert.IsTrue(ep.EnumerateDirectoryPaths().Order()
            .SequenceEqual(expectedFiles.Select(EntryPath.CreateFromPath).Order()));
    }

    private class SearchingOption
    {
        public PathOption[] IncDirs { get; set; } = [];
        public PathOption[] IncFiles { get; set; } = [];
        public PathOption[] ExcDirs { get; set; } = [];
        public PathOption[] ExcFiles { get; set; } = [];
    }

    private class PathOption
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