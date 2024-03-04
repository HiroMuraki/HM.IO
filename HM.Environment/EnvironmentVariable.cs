namespace HM.Environment;

public sealed record class EnvironmentVariable
{
    public String Name { get; set; }

    public String DestinationPath { get; set; }

    public EnvironmentVariable() : this(String.Empty, String.Empty)
    {

    }

    public EnvironmentVariable(String name, String destinationPath)
    {
        Name = name;
        DestinationPath = destinationPath;
    }
}
