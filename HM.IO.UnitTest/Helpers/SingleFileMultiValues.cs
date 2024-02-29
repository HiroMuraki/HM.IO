#pragma warning disable IDE0049 // 使用框架类型

using HM.IO.Previews;
using HM.IO.Previews.File;

namespace HM.IO.UnitTest.Helpers;

public static class SingleFileMultiValues
{
    public static FilePath TestFilePath { get; } = new("path\\to\\file\\test.txt");

    public static void TestWithSingleFile(Action<IFile> action)
    {
        foreach (IFile file in EnumerateTestFiles())
        {
            action(file);
        }
    }

    public static void TestWithSingleFileMultiValues<T>(Action<IFile, T> action, IEnumerable<T> testValues)
    {
        foreach (IFile file in EnumerateTestFiles())
        {
            foreach (T testValue in testValues)
            {
                action(file, testValue);
            }
        }
    }

    #region NonPublic
    private static IEnumerable<IFile> EnumerateTestFiles()
    {
        MemoryDisk.Clear();
        IFile file = MemoryDisk.CreateFile(TestFilePath);

        yield return file;
    }
    #endregion
}
