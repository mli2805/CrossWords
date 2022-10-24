using Microsoft.EntityFrameworkCore;

namespace CrossWord
{
    public sealed class CrosswordDbContext : DbContext
    {
        public DbSet<DbWord> DbWords { get; set; }
        public DbSet<WordSource> WordSources { get; set; }

        private string DbPath { get; }

#pragma warning disable CS8618
        public CrosswordDbContext(string dbPath)
#pragma warning restore CS8618
        {
            DbPath = dbPath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
