using System.CommandLine;
using System.CommandLine.IO;

namespace HM.CommandLine;

public class XConsole : IConsole
{
    public static XConsole Instance { get; } = new();

    public IStandardStreamWriter Out { get; } = new StandardStreamWriter(Console.Out);

    public Boolean IsOutputRedirected => Console.IsOutputRedirected;

    public IStandardStreamWriter Error { get; } = new StandardStreamWriter(Console.Error);

    public Boolean IsErrorRedirected => Console.IsErrorRedirected;

    public Boolean IsInputRedirected => Console.IsInputRedirected;

    public static void Write(XConsoleArg arg)
        => WriteCore(arg);

    public static void Write(XConsoleArg[] args)
    {
        foreach (XConsoleArg arg in args)
        {
            Write(arg);
        }
    }

    public static void Write()
        => Write(String.Empty, Console.ForegroundColor);

    public static void Write(String message)
        => Write(message, Console.ForegroundColor);

    public static void Write(String message, ConsoleColor textColor)
        => WriteCore(new XConsoleArg(message, textColor));

    public static void WriteLine()
        => WriteLine(String.Empty, Console.ForegroundColor);

    public static void WriteLine(String message)
        => WriteLine(message, Console.ForegroundColor);

    public static void WriteLine(String message, ConsoleColor textColor)
        => WriteCore(new XConsoleArg(message + "\n", textColor));

    public static void WriteLineSuccess(String message)
        => WriteLine(message, ConsoleColor.Green);

    public static void WriteLineWarning(String message)
        => WriteLine(message, ConsoleColor.Yellow);

    public static void WriteLineError(String message)
        => WriteLine(message, ConsoleColor.Red);

    public static void WriteLine(XConsoleArg arg)
        => WriteCore(arg with { Text = arg.Text + "\n" });

    public static void WriteLine(XConsoleArg[] args)
    {
        foreach (XConsoleArg arg in args)
        {
            Write(arg);
        }
        WriteLine("");
    }

    public static void RegisterCancelEvent(Action action)
    {
        Console.CancelKeyPress += (s, e) =>
        {
            action();
        };
    }

    public static void NotifyPressEnterToContinue(String tip = "Press Enter to continue...")
    {
        Write(new XConsoleArg(tip));
        Console.ReadLine();
    }

    public static Boolean NotifyConfirmAction(String tip = "Confirm listed actions?")
    {
        Write(new XConsoleArg($"{tip}(Y/N): "));

        String userInput = Console.ReadLine()?.ToUpper() ?? "N";

        return userInput == "Y";
    }

    public static void Wait(Int32 seconds)
    {
        for (Int32 i = 0; i < seconds; i++)
        {
            WriteLine($"Wait {seconds - i} seconds...");
            Thread.Sleep(1000);
        }
    }

    #region NonPublic
    private static void WriteCore(XConsoleArg arg)
    {
        ConsoleColor preForegroundColor = Console.ForegroundColor;
        ConsoleColor preBackgroundColor = Console.BackgroundColor;
        Console.ForegroundColor = arg.TextColor;
        Console.BackgroundColor = arg.BackgroundColor;
        Console.Write(arg.Text);
        Console.ForegroundColor = preForegroundColor;
        Console.BackgroundColor = preBackgroundColor;
    }
    private static async Task WriteCoreAsync(String? message, ConsoleColor textColor)
    {
        ConsoleColor preColor = Console.ForegroundColor;
        Console.ForegroundColor = textColor;
        await Console.Out.WriteAsync(message);
        Console.ForegroundColor = preColor;
    }
    private XConsole()
    {

    }
    private class StandardStreamWriter : IStandardStreamWriter
    {
        public void Write(String? value)
        {
            _textWriter.Write(value);
        }

        public StandardStreamWriter(TextWriter textWriter)
        {
            _textWriter = textWriter;
        }

        #region NonPublic
        private readonly TextWriter _textWriter;
        #endregion
    }
    #endregion
}
