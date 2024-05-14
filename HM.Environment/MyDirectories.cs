using System.Reflection;

namespace HM.Environment;

public static class MyDirectories
{
    public static MyDirectory UserDataRoot => new(@"R:\UserData");

    public static MyDirectory Document => UserDataRoot.GetPath(@"Documents");

    public static MyDirectory Download => UserDataRoot.GetPath(@"Downloads");

    public static MyDirectory Video => UserDataRoot.GetPath(@"Videos");

    public static MyDirectory Picture => UserDataRoot.GetPath(@"Pictures");

    public static MyDirectory Music => UserDataRoot.GetPath(@"Music");

    public static MyDirectory Desktop => UserDataRoot.GetPath(@"Desktop");

    public static MyDirectory Application => UserDataRoot.GetPath(@"App");

    public static MyDirectory AppConfig => Application.GetPath(@"Config");

    public static MyDirectory AppLog => Application.GetPath(@"Log");

    public static MyDirectory AppDatabase => Application.GetPath(@"DataBase");

    public static MyDirectory AppTempFile => Application.GetPath(@"TempFile");

    public static MyDirectory[] GetAllMyDirectories()
    {
        PropertyInfo[] props = typeof(MyDirectories).GetProperties(BindingFlags.Static | BindingFlags.Public);
        var result = new MyDirectory[props.Length];

        for (Int32 i = 0; i < props.Length; i++)
        {
            PropertyInfo prop = props[i];
            if (prop.PropertyType == typeof(MyDirectory))
            {
                result[i] = (MyDirectory)prop.GetValue(null)!;
            }
        }

        return result;
    }
}
