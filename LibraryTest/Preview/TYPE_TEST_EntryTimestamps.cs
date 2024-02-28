#pragma warning disable IDE0049 // 使用框架类型
using HM.IO.Previews;
using LibraryTest.Helpers;

namespace LibraryTest.Preview;

[TestClass]
[TestCategory("Struct Types")]
public class TYPE_TEST_EntryTimestamps : TestClassBase
{
    [TestMethod]
    public void PROP_CreationTime()
    {
        DateTime dateTime = DateTime.Now;
        var x = new EntryTimestamps { CreationTime = dateTime };
        var y = new EntryTimestamps { CreationTime = dateTime };

        Assert.AreEqual(dateTime, x.CreationTime);
        Assert.AreEqual(dateTime, y.CreationTime);
        Assert.AreEqual(x.CreationTime, y.CreationTime);
    }

    [TestMethod]
    public void PROP_LastWriteTime()
    {
        DateTime dateTime = DateTime.Now;
        var x = new EntryTimestamps { LastWriteTime = dateTime };
        var y = new EntryTimestamps { LastWriteTime = dateTime };

        Assert.AreEqual(dateTime, x.LastWriteTime);
        Assert.AreEqual(dateTime, y.LastWriteTime);
        Assert.AreEqual(x.LastWriteTime, y.LastWriteTime);
    }

    [TestMethod]
    public void PROP_LastAccessTime()
    {
        DateTime dateTime = DateTime.Now;
        var x = new EntryTimestamps { LastAccessTime = dateTime };
        var y = new EntryTimestamps { LastAccessTime = dateTime };

        Assert.AreEqual(dateTime, x.LastAccessTime);
        Assert.AreEqual(dateTime, y.LastAccessTime);
        Assert.AreEqual(x.LastAccessTime, y.LastAccessTime);
    }

    [TestMethod]
    public void COMP_EqualityComparison()
    {
        DateTime dateTime = DateTime.Now;

        EqualityTestHelper.Assert_Fully(
            equalValues: [
                new EntryTimestamps
                {
                    CreationTime = dateTime,
                    LastWriteTime = dateTime + TimeSpan.FromDays(1),
                    LastAccessTime = dateTime + TimeSpan.FromHours(5)
                },
                new EntryTimestamps
                {
                    CreationTime = dateTime,
                    LastWriteTime = dateTime + TimeSpan.FromDays(1),
                    LastAccessTime = dateTime + TimeSpan.FromHours(5)
                },
            ],
            notEqualValues:
            [
                new EntryTimestamps
                {
                    CreationTime = dateTime,
                },
                new EntryTimestamps
                {
                    LastWriteTime = dateTime + TimeSpan.FromDays(1),
                },
                new EntryTimestamps
                {
                    LastAccessTime = dateTime + TimeSpan.FromHours(5)
                },
                new EntryTimestamps
                {
                    CreationTime = dateTime + TimeSpan.FromDays(2),
                    LastWriteTime = dateTime + TimeSpan.FromDays(1),
                    LastAccessTime = dateTime + TimeSpan.FromHours(5)
                },
                new EntryTimestamps
                {
                    CreationTime = dateTime + TimeSpan.FromDays(2),
                    LastWriteTime = dateTime + TimeSpan.FromDays(2),
                    LastAccessTime = dateTime + TimeSpan.FromHours(5)
                },
                new EntryTimestamps
                {
                    CreationTime = dateTime + TimeSpan.FromDays(2),
                    LastWriteTime = dateTime + TimeSpan.FromDays(2),
                    LastAccessTime = dateTime + TimeSpan.FromHours(2)
                },
            ]);
    }
}

