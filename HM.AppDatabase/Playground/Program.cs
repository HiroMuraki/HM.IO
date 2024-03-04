using AppDatabase;
using System.Diagnostics;

DbActions actions = DbActions.None;

using (var sdb = SqliteDatabase<Student>.Create("TEST_Sample.db", "sample_db"))
{
    await sdb.InitializeAsync();

    do
    {
        if (actions == DbActions.None)
        {
            break;
        }

        if (actions.HasFlag(DbActions.Add))
        {
            Console.WriteLine("Add");
            sdb.AddMany([
                new Student
                {
                    StudentId = "A114514",
                    Name = "AA",
                    Age = 22
                },
                new Student
                {
                    StudentId = "C114514",
                    Name = "AA",
                    Age = 15
                },
                new Student
                {
                    StudentId = "B1919810",
                    Name = "BB",
                    Age = 16
                },
            ]);
            await sdb.SaveChangesAsync();
            sdb.QueryMany(x => true).ToList().ForEach(Console.WriteLine);
        }
        if (actions.HasFlag(DbActions.Update))
        {
            Console.WriteLine("Update");
            sdb.UpdateMany(x => x.Name.StartsWith("AA"), item =>
            {
                item.Name = $"{item.Name}({item.Age})";
            });
            await sdb.SaveChangesAsync();
            sdb.QueryMany(x => true).ToList().ForEach(Console.WriteLine);
        }
        if (actions.HasFlag(DbActions.Delete))
        {
            Console.WriteLine("Delete");
            sdb.DeleteMany(x => x.Name == "BB");
            await sdb.SaveChangesAsync();
            var s = sdb.QueryMany(x => true).ToList();
            sdb.QueryMany(x => true).ToList().ForEach(Console.WriteLine);
        }
        if (actions.HasFlag(DbActions.Query))
        {
            Console.WriteLine("Query");
            sdb.QueryMany(x => x.Age > 15).ToList().ForEach(Console.WriteLine);
            await sdb.SaveChangesAsync();
        }
    } while (false);
}

await Task.Delay(TimeSpan.FromSeconds(1));
File.Delete("TEST_Sample.db");
Console.WriteLine("Done");
