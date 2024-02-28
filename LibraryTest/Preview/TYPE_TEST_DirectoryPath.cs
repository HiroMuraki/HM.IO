#pragma warning disable IDE0049 // 使用框架类型
using HM.IO.Previews;
using LibraryTest.Helpers;

namespace LibraryTest.Preview;

[TestClass]
[TestCategory("Struct Types")]
public class TYPE_TEST_DirectoryPath : TestClassBase
{
    [TestMethod]
    public void EqualityComparison()
    {
        EqualityTestHelper.Test_Fully(
            equalValues: [
                new DirectoryPath(@"C:\Windows\system32"),
                new DirectoryPath(@"C:\Windows\system32"),
            ],
            notEqualValues: [
                new DirectoryPath(@""),
                new DirectoryPath(@"C:\Windows"),
                new DirectoryPath(@"C:\Windows\system64"),
                new DirectoryPath(@"C:\Windows\system32\apps"),
            ]);
    }
}