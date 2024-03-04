namespace HM.CommandLine;

public record class XConsoleArg
{
    public String? Text { get; set; }

    public ConsoleColor TextColor { get; set; } = Console.ForegroundColor;

    public ConsoleColor BackgroundColor { get; set; } = Console.BackgroundColor;

    public XConsoleArg(String? text)
    {
        Text = text;
    }
}
