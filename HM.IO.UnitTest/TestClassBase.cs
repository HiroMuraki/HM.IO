#pragma warning disable IDE0049 // 使用框架类型

using System.Runtime.CompilerServices;

namespace HM.IO.UnitTest;

public abstract class TestClassBase
{
    [TestInitialize]
    public void PrintTimestamp()
    {
        PrintTimestamp(GetType().Name);
    }

    #region NonPublic
    protected static void PrintTimestamp([CallerMemberName] string? caller = null)
    {
        System.Diagnostics.Debug.WriteLine($"[{caller}] Test time: {DateTime.Now}"); // debug output
    }
    #endregion
}
