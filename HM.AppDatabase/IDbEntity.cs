namespace HM.AppDatabase;

public interface IDbEntity
{
    UInt64 Id { get; init; }

    Int64 CreateTime { get; set; }

    Int64 UpdateTime { get; set; }

    Boolean IsDeleted { get; set; }
}
