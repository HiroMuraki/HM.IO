namespace HM.AppDatabase;

public interface IDbEntity
{
    ulong Id { get; init; }

    long CreateTime { get; set; }

    long UpdateTime { get; set; }

    bool IsDeleted { get; set; }
}
