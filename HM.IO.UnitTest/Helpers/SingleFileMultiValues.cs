#pragma warning disable IDE0049 // 使用框架类型

using HM.IO.Previews;
using HM.IO.Previews.Memory;

namespace HM.IO.UnitTest.Helpers;

public static class SingleFileMultiValues
{
    public static FilePath TestFilePath { get; } = new("path\\to\\file\\test.txt");

    public static void TestWithSingleFile(Action<IFileEntry> action)
    {
        foreach (IFileEntry file in EnumerateTestFiles())
        {
            action(file);
        }
    }

    public static void TestWithSingleFileMultiValues<T>(Action<IFileEntry, T> action, IEnumerable<T> testValues)
    {
        foreach (IFileEntry file in EnumerateTestFiles())
        {
            foreach (T testValue in testValues)
            {
                action(file, testValue);
            }
        }
    }

    #region NonPublic
    private static IEnumerable<IFileEntry> EnumerateTestFiles()
    {
        MemoryDisk.Clear();
        IFileEntry file = MemoryDisk.CreateFile(TestFilePath);

        yield return file;
    }
    #endregion
}
