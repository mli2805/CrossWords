using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CrossWord
{
    public class DataRepository
    {
        private string DbPath { get; }

        public DataRepository()
        {
            var assembly = GetType().Assembly.Location;
            var folder = Path.GetDirectoryName(assembly) + "\\Db";
            DbPath = Path.Join(folder, @"Words.db");

            using var db = new CrosswordDbContext(DbPath);
            db.Database.EnsureCreated();
        }

        public async Task<int> GetCountAsync()
        {
            await using var db = new CrosswordDbContext(DbPath);
            var count = await db.DbWords.CountAsync();
            return count;
        }

        public async Task<int> AddFromFile(string filename, string code, string sourceDescription, Func<string, string, DbWord> parsingMethod)
        {
            var content = await File.ReadAllLinesAsync(filename);
            await using var db = new CrosswordDbContext(DbPath);
            if (db.WordSources.FirstOrDefault(x => x.Code == code) == null)
                db.WordSources.Add(new WordSource(code, sourceDescription));

            db.DbWords.RemoveRange(db.DbWords.Where(x=>x.Source == code));
            await db.SaveChangesAsync();

            var portion = new List<DbWord>();
            var total = 0;
            foreach (var s in content)
            {
                portion.Add(parsingMethod(s, code));
                if (portion.Count >= 2000)
                {
                    await db.DbWords.AddRangeAsync(portion);
                    total += await db.SaveChangesAsync();
                    portion.Clear();
                }
            }
            await db.DbWords.AddRangeAsync(portion);
            total += await db.SaveChangesAsync();

            return total;
        }
    }
}