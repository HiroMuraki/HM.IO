namespace HM.CommandLine;

public record class XConsoleArg
{
    public String? Text { get; init; }

    public ConsoleColor TextColor { get; init; } = Console.ForegroundColor;

    public ConsoleColor BackgroundColor { get; init; } = Console.BackgroundColor;

    public XConsoleArg(String? text)
    {
        Text = text;
    }

    public XConsoleArg(String? text, ConsoleColor textColor)
    {
        Text = text;
        TextColor = textColor;
    }
}
