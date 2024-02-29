#pragma warning disable IDE0049 // 使用框架类型
using HM.IO.Previews.Stream;
using HM.IO.UnitTest.Helpers;

namespace HM.IO.UnitTest.Preview;

[TestClass]
public class INTERFACE_TEST_IStream : TestClassBase
{
    [TestMethod]
    public void PROP_StreamMode()
    {
        SingleFileMultiValues.TestWithSingleFile(file =>
        {
            using (IStream readOnly = file.Open(StreamMode.ReadOnly))
            {
                Assert.AreEqual(readOnly.Mode, StreamMode.ReadOnly);
            }

            using (IStream writeOnly = file.Open(StreamMode.WriteOnly))
            {
                Assert.AreEqual(writeOnly.Mode, StreamMode.WriteOnly);
            }

            using (IStream readWrite = file.Open(StreamMode.ReadWrite))
            {
                Assert.AreEqual(readWrite.Mode, StreamMode.ReadWrite);
            }
        });
    }

    [TestMethod]
    public void PROP_Length()
    {
        SingleFileMultiValues.TestWithSingleFileMultiValues((file, binaryData) =>
        {
            using IStream writeOnly = file.Open(StreamMode.WriteOnly);
            writeOnly.Write(binaryData, 0, binaryData.Length);

            Assert.AreEqual(writeOnly.SizeInBytes, binaryData.Length);
        }, _testBinaryValues);
    }

    [TestMethod]
    public void METHS_Write_Read_WriteAsync_ReadAsync()
    {
        SingleFileMultiValues.TestWithSingleFileMultiValues((file, testValue) =>
        {
            // Write
            using (IStream writeOnly = file.Open(StreamMode.WriteOnly))
            {
                writeOnly.Write(testValue, 0, testValue.Length);
            }

            // Read
            using (IStream readOnly = file.Open(StreamMode.ReadOnly))
            {
                Assert.AreEqual(readOnly.SizeInBytes, testValue.Length);

                byte[] data = new byte[testValue.Length];
                readOnly.Read(data, 0, data.Length);

                CollectionAssert.AreEqual(data, testValue);
            }

            // WriteAsync
            using (IStream writeOnly = file.Open(StreamMode.WriteOnly))
            {
                if (writeOnly is IAsyncStream asyncStream)
                {
                    asyncStream.WriteAsync(testValue, 0, testValue.Length).Wait();
                }
                else
                {
                    throw new InvalidOperationException($"Unable to case steam to {nameof(IAsyncStream)}");
                }
            }

            // ReadAsync
            using (IStream readOnly = file.Open(StreamMode.ReadOnly))
            {
                if (readOnly is IAsyncStream asyncStream)
                {
                    Assert.AreEqual(readOnly.SizeInBytes, testValue.Length);

                    byte[] data = new byte[testValue.Length];
                    asyncStream.ReadAsync(data, 0, data.Length).Wait();

                    CollectionAssert.AreEqual(data, testValue);
                }
                else
                {
                    throw new InvalidOperationException($"Unable to case steam to {nameof(IAsyncStream)}");
                }
            }
        }, _testBinaryValues);
    }

    #region NonPublic
    private readonly List<byte[]> _testBinaryValues = [
            [],
            [0xAA],
            [0xAA, 0xBB, 0xCC],
            [0xAA, 0xBB, 0xCC, 0xAA, 0xBB, 0xCC, 0xAA, 0xBB, 0xCC, 0xAA, 0xBB, 0xCC],
        ];
    #endregion
}