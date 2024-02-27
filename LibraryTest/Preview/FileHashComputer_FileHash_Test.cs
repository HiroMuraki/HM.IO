#pragma warning disable IDE0049 // 使用框架类型
using HM.IO.Previews;
using FileSha256Computer = HM.IO.Previews.FileSha256Computer;

namespace LibraryTest.Preview;

[TestClass]
public class FileHashComputer_Test : TestClassBase
{
    [TestMethod]
    public void TYPE_FileSha256Computer()
    {
        TestCoreHelper(
            hashComputer: new FileSha256Computer(),
            asyncHashComputer: new FileSha256Computer(),
            expectedBinaryValue: Convert.FromHexString("7F83B1657FF1FC53B92DC18148A1D65DFC2D4B1FA3D677284ADDD200126D9069"),
            expectedStringValue: "7F83B1657FF1FC53B92DC18148A1D65DFC2D4B1FA3D677284ADDD200126D9069");
    }

    [TestMethod]
    public void TYPE_FileMd5Computer()
    {
        TestCoreHelper(
            hashComputer: new FileMd5Computer(),
            asyncHashComputer: new FileMd5Computer(),
            expectedBinaryValue: Convert.FromHexString("ED076287532E86365E841E92BFC50D8C"),
            expectedStringValue: "ED076287532E86365E841E92BFC50D8C");
    }

    #region NonPublic
    private static MemoryFile GetMemoryFile()
    {
        MemoryDisk.Clear();

        MemoryFile file = MemoryDisk.CreateFile(new FilePath("path\\to\\file\\test.txt"));

        using (IStream fs = file.Open(StreamMode.WriteOnly))
        using (var writer = new StreamWriter(fs.GetBclStream()))
        {
            writer.Write("Hello World!");
        }

        //System.Diagnostics.Debug.WriteLine($"Bytes:"); // debug output
        //System.Diagnostics.Debug.WriteLine($">>>"); // debug output
        //BinaryFileHelper.Open(file).ForEachChunk(bytes =>
        //{
        //    System.Diagnostics.Debug.WriteLine(string.Join(' ', bytes.Select(x => x.ToString("X2"))));
        //});
        //System.Diagnostics.Debug.WriteLine($">>>"); // debug output

        return file;
    }
    private static void TestCoreHelper(
        IFileHashComputer hashComputer,
        IAsyncFileHashComputer asyncHashComputer,
        byte[] expectedBinaryValue,
        string expectedStringValue)
    {
        MemoryFile file = GetMemoryFile();

        FileHash hash1 = hashComputer.ComputeHash(file);
        CollectionAssert.AreEqual(expectedBinaryValue, hash1.BinaryValue);
        Assert.AreEqual(expectedStringValue, hash1.StringValue);

        FileHash hash2 = asyncHashComputer.ComputeHashAsync(file).Result;
        CollectionAssert.AreEqual(expectedBinaryValue, hash2.BinaryValue);
        Assert.AreEqual(expectedStringValue, hash2.StringValue);

        Assert.IsTrue(hash1 == hash2);
        Assert.IsTrue(hash1.GetHashCode() == hash2.GetHashCode());
        Assert.IsFalse(hash1 != hash2);
        Assert.IsTrue(hash1.Equals(hash2));
    }
    #endregion
}
