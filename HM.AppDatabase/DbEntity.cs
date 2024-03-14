using System.ComponentModel.DataAnnotations.Schema;

namespace HM.AppDatabase;

public abstract record class DbEntity :
    IDbEntity
{
    [Column("id", Order = 0)]
    public UInt64 Id { get; init; }

    [Column("create_time", Order = Int32.MaxValue - 3)]
    public Int64 CreateTime { get; set; }

    [Column("update_time", Order = Int32.MaxValue - 2)]
    public Int64 UpdateTime { get; set; }

    [Column("is_deleted", Order = Int32.MaxValue - 1)]
    public Boolean IsDeleted { get; set; }
}
