#pragma warning disable IDE0008

using AppDatabase;

namespace HM.AppDatabase.Test;

[TestClass]
public class TYPE_TEST_Database
{
    [TestMethod]
    public void ERR_WhenNotInitialized()
    {
        var db = SqliteDatabase<Student>.Create("TEST_Error.db", "test_table");
        Assert.ThrowsException<InvalidOperationException>(() => db.Query(_ => true));
    }

    [TestMethod]
    public void METHS_Add_AddMany_AddAsync_AddManyAsync()
    {
        List<Student> newItems = [
            new Student
            {
                StudentId = "BA_MG_2",
                Name = "ÌÒ",
                Age = 15,
                Height = 143,
                Birthday = new DateTime(2050, 12, 8).Ticks,
                Hobbies = "ÓÎÏ·",
            },
            new Student
            {
                StudentId = "BA_MG_2",
                Name = "ÂÌ",
                Age = 15,
                Height = 143,
                Birthday = new DateTime(2050, 12, 8).Ticks,
                Hobbies = "»æ»­",
            }
        ];

        var asserter = CreateAsserter("TEST_Add.db", Student.Samples, Student.Samples.Concat(newItems));

        asserter(db => newItems.ForEach(db.Add));
        asserter(db => newItems.ForEach(x => db.AddAsync(x).Wait()));
        asserter(db => db.AddMany(newItems));
        asserter(db => db.AddManyAsync(newItems).Wait());
    }

    [TestMethod]
    public void METHS_Update_UpdateAsync()
    {
        var asserter = CreateAsserter("TEST_Update.db", Student.Samples, Student.Samples.Select(x =>
        {
            if (x.Name == "°®ÀöË¿")
            {
                return x with { Name = "ÌìÍ¯°®ÀöË¿" };
            }
            else
            {
                return x;
            }

        }));

        asserter(db => db.Update(x => x.Name == "°®ÀöË¿", item => item.Name = "ÌìÍ¯°®ÀöË¿"));
        asserter(db => db.UpdateAsync(x => x.Name == "°®ÀöË¿", item => item.Name = "ÌìÍ¯°®ÀöË¿").Wait());
        asserter(db => db.UpdateMany(x => x.Name == "°®ÀöË¿", item => item.Name = "ÌìÍ¯°®ÀöË¿"));
        asserter(db => db.UpdateManyAsync(x => x.Name == "°®ÀöË¿", item => item.Name = "ÌìÍ¯°®ÀöË¿").Wait());
    }

    [TestMethod]
    public void METHS_Delete_DeleteAsync()
    {
        Student[] newItems =
        [
            new Student
            {
                StudentId = "UNKNOW",
                Name = "Î´Öª1",
                Age = -1,
                Height = -1,
                Birthday = 0,
                Hobbies = "",
            },
        ];

        var asserter = CreateAsserter("TEST_Delete.db", Student.Samples.Concat(newItems), Student.Samples);

        asserter(db => db.Delete(x => x.StudentId == "UNKNOW"));
        asserter(db => db.DeleteAsync(x => x.StudentId == "UNKNOW").Wait());
        asserter(db => db.DeleteMany(x => x.StudentId == "UNKNOW"));
        asserter(db => db.DeleteManyAsync(x => x.StudentId == "UNKNOW").Wait());
    }

    [TestMethod]
    public void METHS_Query_QueryAsync()
    {
        using (SqliteDatabase<Student> db = GetTestDb("TEST_Query.db", Student.Samples, removeFile: true))
        {
            AssertRows(db.QueryMany(x => true), Student.Samples);
            AssertRows(db.QueryManyAsync(x => true).ToBlockingEnumerable(), Student.Samples);

            AssertRows(db.QueryMany(x => x.Age > 15), Student.Samples.Where(x => x.Age > 15));
            AssertRows(db.QueryManyAsync(x => x.Age > 15).ToBlockingEnumerable(), Student.Samples.Where(x => x.Age > 15));

            AssertRows(db.QueryMany(x => x.StudentId.StartsWith("BA_M")), Student.Samples.Where(x => x.StudentId.StartsWith("BA_M")));
            AssertRows(db.QueryManyAsync(x => x.StudentId.StartsWith("BA_M")).ToBlockingEnumerable(), Student.Samples.Where(x => x.StudentId.StartsWith("BA_M")));

            AssertRows(db.QueryMany(x => x.Name == "°®ÀöË¿"), Student.Samples.Where(x => x.Name == "°®ÀöË¿"));
            AssertRows(db.QueryManyAsync(x => x.Name == "°®ÀöË¿").ToBlockingEnumerable(), Student.Samples.Where(x => x.Name == "°®ÀöË¿"));

            AssertRows([db.Query(x => x.Name == "°®ÀöË¿")!], [Student.Samples.First(x => x.Name == "°®ÀöË¿")]);
            AssertRows([db.QueryAsync(x => x.Name == "°®ÀöË¿").Result!], [Student.Samples.First(x => x.Name == "°®ÀöË¿")]);

        }
    }

    #region NonPublic
    private static SqliteDatabase<Student> GetTestDb(string testFilePath, IEnumerable<Student>? initData, bool removeFile = false)
    {
        if (removeFile && File.Exists(testFilePath))
        {
            File.Delete(testFilePath);
        }

        var db = SqliteDatabase<Student>.Create(testFilePath, "test_table");
        db.InitializeAsync().Wait();

        if (initData is not null)
        {
            db.AddMany(initData);
        }

        db.SaveChangesAsync().Wait();

        return db;
    }
    private static Action<Action<SqliteDatabase<Student>>> CreateAsserter(string dbFileName, IEnumerable<Student> initData, IEnumerable<Student> expectingValues)
    {
        void asserter(Action<SqliteDatabase<Student>> modifier)
        {
            using (SqliteDatabase<Student> db = GetTestDb(dbFileName, initData, removeFile: true))
            {
                modifier(db);
                db.SaveChangesAsync().Wait();
            }

            using (SqliteDatabase<Student> db = GetTestDb(dbFileName, null, removeFile: false))
            {
                Student[] inDbValues = db.QueryMany(_ => true).ToArray();
                Student[] eValues = expectingValues.ToArray();

                AssertRows(inDbValues, eValues);
            }
        }

        return asserter;
    }
    private static void AssertRows(IEnumerable<Student> inDbValues, IEnumerable<Student> expectingValues)
    {
        Student[] inDbValueArray = inDbValues.ToArray();
        Student[] expectingValueArray = expectingValues.ToArray();

        Assert.AreEqual(expectingValueArray.Length, inDbValueArray.Length);

        for (int i = 0; i < expectingValueArray.Length; i++)
        {
            Student inDbVal = inDbValueArray[i];
            Student eVal = expectingValueArray[i];

            bool r = inDbVal.StudentId == eVal.StudentId
                && inDbVal.Name == eVal.Name
                && inDbVal.Age == eVal.Age
                && inDbVal.Height == eVal.Height
                && inDbVal.Birthday == eVal.Birthday
                && inDbVal.Hobbies == eVal.Hobbies;

            Assert.IsTrue(r);
        }
    }
    #endregion
}
