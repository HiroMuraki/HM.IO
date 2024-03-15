#pragma warning disable IDE0049 // 使用框架类型

using HM.IO.Previews;

namespace HM.IO.UnitTest.Helpers;

public class BinaryFileHelper
{
    public static BinaryFileHelper Open(IFileEntry file)
    {
        var item = new BinaryFileHelper(file);

        return item;
    }

    public void ForEachChunk(Action<byte[]> action, int chunkSize = 16)
    {
        foreach (byte[] bytesLine in EnumerateLines(chunkSize))
        {
            action(bytesLine);
        }
    }

    public IEnumerable<byte[]> EnumerateLines(int chunkSize = 16)
    {
        using (IStream fs = _file.Open(StreamMode.ReadOnly))
        {
            while (true)
            {
                byte[] buffer = new byte[chunkSize];
                int readCount = fs.Read(buffer);

                if (readCount <= 0)
                {
                    break;
                }
                else if (readCount <= buffer.Length)
                {
                    yield return buffer.Take(readCount).ToArray();
                }
            }

            yield break;
        }
    }

    #region NonPublic
    private readonly IFileEntry _file;
    private BinaryFileHelper(IFileEntry file)
    {
        _file = file;
    }
    #endregion
}
