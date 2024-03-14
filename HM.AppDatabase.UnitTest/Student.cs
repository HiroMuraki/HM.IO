using HM.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HM.AppDatabase.UnitTest;

[PrimaryKey(nameof(Id))]
internal record class Student : DbEntity
{
    private readonly static (String StudentId, String Name, Int32 Age, Int32 Heigth, String Birthday, String Hobbies)[] _sampleRows =
        [
            ("BA_68_1", "爱露", 16, 160, "3.12", "学习经营"),
            ("BA_GD_1", "日奈", 17, 142, "2.19", "睡眠、休息"),
            ("BA_MS_1", "优香", 16, 156, "3.14", "计算"),
            ("BA_MG_4", "爱丽丝", -1, 152, "3.25", "游戏（特别是RPG)"),
            ("BA_MT_3", "真纪", 15, 149, "8.1", "涂鸦、游戏"),
            ("BA_TT_2", "小春", 15, 148, "4.16", "空想、妄想、收集R18杂志"),
        ];

    public static Student[] Samples => _sampleRows.Select(x => new Student
    {
        StudentId = x.StudentId,
        Name = x.Name,
        Age = x.Age,
        Height = x.Heigth,
        Birthday = new DateTime(2025, Convert.ToInt32(x.Birthday.Split('.')[0]), Convert.ToInt32(x.Birthday.Split('.')[1])).Ticks,
        Hobbies = x.Hobbies
    }).ToArray();

    [Column("student_id", Order = 1)]
    public String StudentId { get; set; } = String.Empty;

    [Column("name", Order = 2)]
    public String Name { get; set; } = String.Empty;

    [Column("age", Order = 3)]
    public Int32 Age { get; set; }

    [Column("birthday", Order = 4)]
    public Int64 Birthday { get; set; }

    [Column("height", Order = 5)]
    public Int32 Height { get; set; }

    [Column("hobbies", Order = 6)]
    public String Hobbies { get; set; } = String.Empty;

    public override String ToString()
        => ToStringHelper.Build(this, [
            nameof(Id),
            nameof(StudentId),
            nameof(Name),
            nameof(Age),
            nameof(Birthday),
            nameof(Height),
            nameof(Hobbies),
            nameof(CreateTime),
            nameof(UpdateTime),
            nameof(IsDeleted),
        ]);
}
