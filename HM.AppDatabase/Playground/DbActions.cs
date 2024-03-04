[Flags]
enum DbActions
{
#pragma warning disable format
    None   = 0b0000_0000,
    Add    = 0b0000_0001,
    Update = 0b0000_0010,
    Delete = 0b0000_0100,
    Query  = 0b0000_1000,
    All = Add | Update | Delete | Query
#pragma warning restore format
}
