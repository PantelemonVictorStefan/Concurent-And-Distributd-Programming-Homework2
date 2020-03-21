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

            //optionsBuilder.UseSqlServer("");
        }
    }
}
