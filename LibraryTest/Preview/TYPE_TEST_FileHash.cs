#pragma warning disable IDE0049 // 使用框架类型
using HM.IO.Previews;
using LibraryTest.Helpers;

namespace LibraryTest.Preview;

[TestClass]
[TestCategory("Struct Types")]
public class TYPE_TEST_FileHash : TestClassBase
{
    [TestMethod]
    public void COMP_EqualityComparison()
    {
        EqualityTestHelper.Test_Fully(
            equalValues: [
                new FileHash([0xaa, 0xbb, 0xcc, 0xdd, 0xee]),
                new FileHash([0xaa, 0xbb, 0xcc, 0xdd, 0xee]),
            ],
            notEqualValues: [
                new FileHash([0xaa, 0x00, 0xcc, 0x00, 0x00]),
                new FileHash([0xaa, 0x00, 0xcc]),
                new FileHash([]),
            ]);

        var fileHash = new FileHash([0xaa, 0xbb, 0xcc, 0xdd, 0xee]);
        byte[] binaryOfHash1 = fileHash.BinaryValue;
        for (int i = 0; i < binaryOfHash1.Length; i++)
        {
            binaryOfHash1[i] = 0xFF;
        }

        EqualityTestHelper.Expect_FullyEquals(fileHash, new FileHash([0xaa, 0xbb, 0xcc, 0xdd, 0xee]));
    }
}
