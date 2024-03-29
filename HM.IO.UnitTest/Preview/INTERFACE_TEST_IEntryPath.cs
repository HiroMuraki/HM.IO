﻿#pragma warning disable IDE0049 // 使用框架类型
using HM.IO.Previews;
using HM.IO.UnitTest.Helpers;

namespace HM.IO.UnitTest.Preview;

[TestClass]
public class INTERFACE_TEST_IEntryPath : TestClassBase
{
    [TestMethod]
    public void TYPE_FilePath_EqualityTest()
    {
        EqualityTestHelper.Assert_Fully(
            equalValues: [
                new FilePath(@"C:\MyApps\MyProgram.exe"),
                new FilePath(@"C:\MyApps\MyProgram.exe"),
            ],
            notEqualValues: [
                new FilePath(""),
                new FilePath(@"D:\MyApps\MyProgram.exe"),
                new FilePath(@"C:\ThisAps\MyProgram.exe"),
                new FilePath(@"C:\MyApps\MyProgram.bat"),
                new FilePath(@"C:\MyApps\MyApp.exe"),
                new FilePath(@"C:\MyApps\MyProgram1.exe"),
            ]);
    }

    [TestMethod]
    public void TYPE_DirectoryPath_EqualityTest()
    {
        EqualityTestHelper.Assert_Fully(
            equalValues: [
                new DirectoryPath(@"C:\MyApps\MyProgram"),
                new DirectoryPath(@"C:\MyApps\MyProgram"),
            ],
            notEqualValues: [
                new DirectoryPath(""),
                new DirectoryPath(@"D:\MyApps\MyProgram"),
                new DirectoryPath(@"C:\ThisAps\MyProgram"),
                new DirectoryPath(@"C:\MyApp\MyProgram"),
                new DirectoryPath(@"C:\MyApps\MyApp"),
                new DirectoryPath(@"C:\MyApps\MyProgram1"),
            ]);
    }
}
