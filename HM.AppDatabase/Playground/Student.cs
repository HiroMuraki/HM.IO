using HM.AppDatabase;
using HM.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

[PrimaryKey(nameof(Id))]
record class Student : DbEntity
{
    [Column("student_id", Order = 1)]
    public string StudentId { get; set; } = string.Empty;

    [Column("name", Order = 2)]
    public string Name { get; set; } = string.Empty;

    [Column("age", Order = 3)]
    public int Age { get; set; }

    public override string ToString()
        => ToStringHelper.Build(this, [
            nameof(Id),
            nameof(StudentId),
            nameof(Name),
            nameof(Age),
            nameof(CreateTime),
            nameof(UpdateTime),
            nameof(IsDeleted),
        ]);
}
