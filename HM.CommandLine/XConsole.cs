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

    public void Write(XConsoleArg arg)
        => WriteCore(arg);

    public void WriteLine(XConsoleArg arg)
        => WriteCore(arg with { Text = arg.Text + "\n" });

    public void WriteLineSuccess(String message)
    => WriteLine(new XConsoleArg(message) { TextColor = ConsoleColor.Green });

    public void WriteLineWarning(String message)
        => WriteLine(new XConsoleArg(message) { TextColor = ConsoleColor.Yellow });

    public void WriteLineError(String message)
        => WriteLine(new XConsoleArg(message) { TextColor = ConsoleColor.Red });

    public void RegisterCancelEvent(Action action)
    {
        Console.CancelKeyPress += (s, e) =>
        {
            action();
        };
    }

    public void NotifyPressEnterToContinue(String tip = "Press Enter to continue...")
    {
        Write(new XConsoleArg(tip));
        Console.ReadLine();
    }

    public Boolean NotifyConfirmAction(String tip = "Confirm listed actions?")
    {
        Write(new XConsoleArg($"{tip}(Y/N): "));

        String userInput = Console.ReadLine()?.ToUpper() ?? "N";

        return userInput == "Y";
    }

    #region NonPublic
    private void WriteCore(XConsoleArg arg)
    {
        ConsoleColor preForegroundColor = Console.ForegroundColor;
        ConsoleColor preBackgroundColor = Console.BackgroundColor;
        Console.ForegroundColor = arg.TextColor;
        Console.BackgroundColor = arg.BackgroundColor;
        Console.Write(arg.Text);
        Console.ForegroundColor = preForegroundColor;
        Console.BackgroundColor = preBackgroundColor;
    }
    private async Task WriteCoreAsync(String? message, ConsoleColor textColor)
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
