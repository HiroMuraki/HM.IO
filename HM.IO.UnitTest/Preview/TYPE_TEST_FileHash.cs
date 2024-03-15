#pragma warning disable IDE0049 // 使用框架类型
using HM.Cryptography;
using HM.IO.UnitTest.Helpers;

namespace HM.IO.UnitTest.Preview;

[TestClass]
[TestCategory("Struct Types")]
public class TYPE_TEST_Hash : TestClassBase
{
    [TestMethod]
    public void COMP_EqualityComparison()
    {
        EqualityTestHelper.Assert_Fully(
            equalValues: [
                new Hash([0xaa, 0xbb, 0xcc, 0xdd, 0xee]),
                new Hash([0xaa, 0xbb, 0xcc, 0xdd, 0xee]),
            ],
            notEqualValues: [
                new Hash([0xaa, 0x00, 0xcc, 0x00, 0x00]),
                new Hash([0xaa, 0x00, 0xcc]),
                new Hash([]),
            ]);

        var fileHash = new Hash([0xaa, 0xbb, 0xcc, 0xdd, 0xee]);
        byte[] binaryOfHash1 = fileHash.BinaryValue;
        for (int i = 0; i < binaryOfHash1.Length; i++)
        {
            binaryOfHash1[i] = 0xFF;
        }

        EqualityTestHelper.Assert_Equals(fileHash, new Hash([0xaa, 0xbb, 0xcc, 0xdd, 0xee]));
    }
}
