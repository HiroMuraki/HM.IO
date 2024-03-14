namespace HM.CommandLine;

public static class XConsoleExtensions
{
    public static void Write(this XConsole self)
        => self.WriteLine(String.Empty, Console.ForegroundColor);

    public static void Write(this XConsole self, Object? obj)
        => self.Write(obj, Console.ForegroundColor);

    public static void Write(this XConsole self, Object? obj, ConsoleColor textColor)
        => self.Write(new XConsoleArg(obj?.ToString()) { TextColor = textColor });

    public static void Write(this XConsole self, XConsoleArg[] args)
    {
        foreach (XConsoleArg arg in args)
        {
            self.Write(arg);
        }
    }

    public static void WriteLine(this XConsole self)
        => self.WriteLine(String.Empty, Console.ForegroundColor);

    public static void WriteLine(this XConsole self, Object? obj)
        => self.WriteLine(obj, Console.ForegroundColor);

    public static void WriteLine(this XConsole self, Object? obj, ConsoleColor textColor)
        => self.WriteLine(new XConsoleArg(obj?.ToString()) { TextColor = textColor });

    public static void WriteLine(this XConsole self, XConsoleArg[] args)
    {
        foreach (XConsoleArg arg in args)
        {
            self.Write(arg);
        }
        self.WriteLine();
    }
}
