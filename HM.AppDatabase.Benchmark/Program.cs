using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

BenchmarkRunner.Run<DbSetBenchmark>();

[MemoryDiagnoser]
public class DbSetBenchmark
{
    static DbSetBenchmark()
    {
        _context = new();
        _context.Database.EnsureCreated();
    }

    [Benchmark]
    public Foo? UseCache()
    {
        DbSet<Foo> dbSet = _context.Set<Foo>();
        Foo? value = null;

        for (int i = 0; i < 100_0000; i++)
        {
            value = dbSet.Where(x => true).FirstOrDefault();
        }

        return value;
    }

    [Benchmark]
    public Foo? DirectlyDbSet()
    {
        Foo? value = null;

        for (int i = 0; i < 100_0000; i++)
        {
            value = _context.Set<Foo>().Where(x => true).FirstOrDefault();
        }

        return value;
    }

    private static readonly TestDbContext _context;
    private class TestDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=test.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Foo>().ToTable("foo");
        }
    }

    [PrimaryKey(nameof(Id))]
    public class Foo
    {
        [Column("id")]
        public ulong Id { get; init; }
    }
}