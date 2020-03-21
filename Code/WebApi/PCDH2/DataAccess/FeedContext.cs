using Microsoft.EntityFrameworkCore;
using PCDH2.Core.Entities;
using System;

namespace DataAccess
{
    public class FeedContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }

        public FeedContext(DbContextOptions<FeedContext> options)
        : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:programare-concurenta.database.windows.net,1433;Initial Catalog=PCDH2;Persist Security Info=False;User ID=haidepepozitie;Password=cumistermisterjuve69!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
}
