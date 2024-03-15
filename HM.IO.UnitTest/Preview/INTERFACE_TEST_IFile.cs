using HM.IO.Previews;
using HM.IO.UnitTest.Helpers;
using System.Text;
#pragma warning disable IDE0049 // 使用框架类型
namespace HM.IO.UnitTest.Preview;

[TestClass]
public class INTERFACE_TEST_IFile : TestClassBase
{
    [TestInitialize]
    public void Initialize()
    {
        _testTextData = [
                "",
                "Hello World!",
                string.Join('\n', Enumerable.Repeat(new string[] { "Hello World!" }, 10).SelectMany(x => x)),
                string.Join('\n', Enumerable.Repeat(new string[] { "Hello World!" }, 100).SelectMany(x => x)),
                string.Join('\n', Enumerable.Repeat(new string[] { "Hello World!" }, 1000).SelectMany(x => x)),
            ];
        _testBinaryData = _testTextData.Select(Encoding.UTF8.GetBytes).ToList();
    }

    [TestMethod]
    public void PROP_FileName()
    {
        SingleFileMultiValues.TestWithSingleFile(file =>
        {
            Assert.AreEqual(file.Path.StringPath, SingleFileMultiValues.TestFilePath.StringPath);
            Assert.AreEqual(file.Path.FileName, Path.GetFileNameWithoutExtension(SingleFileMultiValues.TestFilePath.StringPath));
            Assert.AreEqual(file.Path.FileExtension, Path.GetExtension(SingleFileMultiValues.TestFilePath.StringPath));
        });
    }

    [TestMethod]
    public void PROP_Path()
    {
        SingleFileMultiValues.TestWithSingleFile(file =>
        {
            Assert.AreEqual(SingleFileMultiValues.TestFilePath, file.Path);
        });
    }

    [TestMethod]
    public void PROP_Exists()
    {
        SingleFileMultiValues.TestWithSingleFile(file =>
        {
            Assert.IsTrue(file.Exists);

            file.Delete();

            Assert.IsFalse(file.Exists);
        });
    }

    [TestMethod]
    public void PROP_SizeInBytes()
    {
        SingleFileMultiValues.TestWithSingleFile(file =>
        {
            foreach (var data in _testBinaryData)
            {
                using (IStream fs = file.Open(StreamMode.WriteOnly))
                {
                    fs.Write(data);
                }

                Assert.AreEqual(data.Length, file.SizeInBytes);
            }
        });
    }

    [TestMethod]
    public void PROP_Timestamps()
    {
        SingleFileMultiValues.TestWithSingleFile(file =>
        {
            var fakeTime = new DateTime(
                year: 2023,
                month: 5,
                day: 2,
                hour: 10,
                minute: 23,
                second: 55);
            var timestamps = new EntryTimestamps
            {
                CreationTime = fakeTime,
                LastWriteTime = fakeTime + TimeSpan.FromDays(15) + TimeSpan.FromHours(2) + TimeSpan.FromMinutes(10) + TimeSpan.FromSeconds(15),
                LastAccessTime = fakeTime + TimeSpan.FromDays(21) + TimeSpan.FromHours(3) + TimeSpan.FromMinutes(22) + TimeSpan.FromSeconds(10),
            };

            file.Timestamps = timestamps;

            Assert.AreEqual(timestamps.CreationTime, file.Timestamps.CreationTime);
            Assert.AreEqual(timestamps.LastWriteTime, file.Timestamps.LastWriteTime);
            Assert.AreEqual(timestamps.LastAccessTime, file.Timestamps.LastAccessTime);
        });
    }

    [TestMethod]
    public void PROP_Attributes()
    {
        SingleFileMultiValues.TestWithSingleFile(file =>
        {
            var fakeAttributes = new EntryAttributes(FileAttributes.Normal | FileAttributes.ReadOnly | FileAttributes.Hidden);

            file.Attributes = fakeAttributes;

            Assert.IsTrue(file.Attributes.HasAttribute(FileAttributes.Normal));
            Assert.IsTrue(file.Attributes.HasAttribute(FileAttributes.ReadOnly));
            Assert.IsTrue(file.Attributes.HasAttribute(FileAttributes.Hidden));
            Assert.AreEqual(fakeAttributes.Value, file.Attributes.Value);
        });
    }

    [TestMethod]
    public void METHS_OpenWrite_OpenRead_with_BinaryData()
    {
        SingleFileMultiValues.TestWithSingleFileMultiValues((file, testValue) =>
        {
            using (IStream fs = file.Open(StreamMode.WriteOnly))
            {
                fs.Write(testValue);
            }

            var byteListFromFile = new List<byte>();
            using (IStream fs = file.Open(StreamMode.ReadOnly))
            {
                while (true)
                {
                    byte[] buffer = new byte[256];
                    int readCount = fs.Read(buffer);

                    if (readCount <= 0)
                    {
                        break;
                    }

                    byteListFromFile.AddRange(buffer.Take(readCount));
                }
            }

            CollectionAssert.AreEqual(testValue, byteListFromFile.ToArray());
        }, _testBinaryData);
    }

    [TestMethod]
    public void METHS_OpenWrite_OpenRead_with_TextData()
    {
        SingleFileMultiValues.TestWithSingleFileMultiValues((file, testValue) =>
        {
            using (IStream fs = file.Open(StreamMode.WriteOnly))
            using (var writer = new StreamWriter(fs.GetBclStream()))
            {
                writer.Write(testValue);
            }

            string valueFromFile;
            using (IStream fs = file.Open(StreamMode.ReadOnly))
            using (var reader = new StreamReader(fs.GetBclStream()))
            {
                valueFromFile = reader.ReadToEnd();
            }

            Assert.AreEqual(testValue, valueFromFile);
        }, _testTextData);
    }

    [TestMethod]
    public void METHS_Delete_Create()
    {
        SingleFileMultiValues.TestWithSingleFile(file =>
        {
            file.Delete();

            Assert.IsFalse(file.Exists);

            file.Create();

            Assert.IsTrue(file.Exists);
        });
    }

    [TestMethod]
    public void TYPE_LocalFile_IEquatable()
    {
        EqualityTestHelper.Assert_Fully(
            equalValues: [
                new LocalFile("path/to/same/file.exe"),
                new LocalFile("path/to/same/file.exe"),
            ],
            notEqualValues: [
                new LocalFile(""),
                new LocalFile("path/to/file.exe"),
                new LocalFile("path/to/diff/file.exe"),
            ]);
    }

    #region NonPublic
    private List<string> _testTextData = null!;
    private List<byte[]> _testBinaryData = null!;
    #endregion
}
