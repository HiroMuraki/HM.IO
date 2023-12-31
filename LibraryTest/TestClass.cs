#pragma warning disable IDE0049 // 使用框架类型

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.CompilerServices;

namespace LibraryTest;

public abstract class TestClass
{
    protected static void PrintTimestamp([CallerMemberName] string? caller = null)
    {
        System.Diagnostics.Debug.WriteLine($"[{caller}] Test time: {DateTime.Now}"); // debug output
    }

    public TestClass()
    {
        PrintTimestamp(GetType().Name);
    }
}
