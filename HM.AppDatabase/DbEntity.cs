using System.ComponentModel.DataAnnotations.Schema;

namespace HM.AppDatabase;

public abstract record class DbEntity :
    IDbEntity
{
    [Column("id", Order = 0)]
    public ulong Id { get; init; }

    [Column("create_time", Order = int.MaxValue - 3)]
    public long CreateTime { get; set; }

    [Column("update_time", Order = int.MaxValue - 2)]
    public long UpdateTime { get; set; }

    [Column("is_deleted", Order = int.MaxValue - 1)]
    public bool IsDeleted { get; set; }
}
