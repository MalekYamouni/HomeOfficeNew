using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeOffice.Core; // Der Namespace in dem UserModel definiert ist
using System.IO;

namespace HomeOffice.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<HomeOfficeTimeModel> Time { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Erzeugt den Pfad zur Datenbankdatei im HomeOffice.Data Ordner
            // Sollte eigentlich im HomeOffice.Data Ordner erstellt werden
            // noch ändern !!
            string dataFolder = Path.Combine(Directory.GetCurrentDirectory(), "HomeOffice.Data", "UserDatabase.db");
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            };

            string dbPath = Path.Combine(dataFolder, "UserDatabase.db");

            // Verbindungszeichenfolge für SQLite
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
