#pragma warning disable IDE0049 // 使用框架类型
using HM.IO.Previews.Entry;
using HM.IO.Previews.Entry.Memory;
using HM.IO.Previews.FileHashComputer;
using HM.IO.Previews.Stream;
using FileSha256Computer = HM.IO.Previews.FileHashComputer.FileSha256Computer;

namespace HM.IO.UnitTest.Preview;

[TestClass]
public class INTERFACE_TEST_FileHashComputer : TestClassBase
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

        static MemoryFile GetMemoryFile()
        {
            MemoryDisk.Clear();

            MemoryFile file = MemoryDisk.CreateFile(new FilePath("path\\to\\file\\test.txt"));

            using (IStream fs = file.Open(StreamMode.WriteOnly))
            using (var writer = new StreamWriter(fs.GetBclStream()))
            {
                writer.Write("Hello World!");
            }


            return file;
        }
    }
    #endregion
}
